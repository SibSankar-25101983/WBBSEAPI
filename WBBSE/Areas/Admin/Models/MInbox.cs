using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Areas.Admin.Models
{
    public class MInbox
    {
        public List<VMInbox> getAdminSchoolInboxList(int? page, int? limit, string sortBy, string direction, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMInbox> result = new List<VMInbox>();

            try
            {
                dt = new Inbox().GetAdminSchoolInboxList(searchString, page, limit, ref total, archiveYN, 0);

                var records = (from data in dt.AsEnumerable()
                               select new VMInbox
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   InboxId = data.Field<string>("InboxId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   BodyTextUnread = data.Field<string>("BodyTextUnread"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   ReadYN = data.Field<string>("ReadYN"),
                                   SchoolCount = data.Field<string>("SchoolCount"),
                                   LinkTypeId = data.Field<string>("LinkTypeId"),
                                   SchoolIdList = data.Field<string>("SchoolIdList")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxList/getAdminSchoolInboxList(MInbox)", "AdminSchoolInboxController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveAdminSchoolInbox(HttpPostedFileBase[] postedFiles, VMInbox data, ref string errDesc)
        {
            int err = 0, createdBy = 0, linkTypeId = 0;
            Int64 inboxId = 0;
            string mode = string.Empty, ipAddress = string.Empty, filePath = string.Empty, fileName = string.Empty, fileExtension = string.Empty, dbFilePath = string.Empty;
            string bodyText = string.Empty, URL = string.Empty, newYN = string.Empty, archiveYN = string.Empty;
            string allYN = string.Empty, schoolIdList = string.Empty, schoolIdListXml = string.Empty;

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                inboxId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.InboxId.Trim())));

                if (mode != Mode.DELETE)
                {
                    linkTypeId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.LinkTypeId.Trim())));
                    //adjust link type. if not selected, make link type = content
                    if (linkTypeId == -1)
                    {
                        linkTypeId = Convert.ToInt32(LinkTypes.CONTENT);
                    }
                    bodyText = Sanitizer.GetSafeHtmlFragment((data.BodyTextOriginal ?? string.Empty).Trim());
                    URL = Sanitizer.GetSafeHtmlFragment((data.URL ?? string.Empty).Trim());
                    newYN = (Sanitizer.GetSafeHtmlFragment(data.NewYN) ?? DefaultSetting.DefaultValN).Trim();
                    if (newYN == string.Empty)
                    {
                        newYN = "N";
                    }
                    archiveYN = (Sanitizer.GetSafeHtmlFragment(data.ArchiveYN) ?? DefaultSetting.DefaultValN).Trim();
                    if (archiveYN == string.Empty)
                    {
                        archiveYN = "N";
                    }
                    if (string.IsNullOrEmpty(bodyText))
                    {
                        err = 2;
                        errDesc = Message.Inbox.InboxIdRequired;
                        return err;
                    }
                    if (!GblFunctions.chkDataFormat(RegexType.Alpha, bodyText))
                    {
                        err = 2;
                        errDesc = Message.Inbox.InvalidContent;
                        return err;
                    }
                    if (mode == Mode.ADD && linkTypeId.ToString() == LinkTypes.PDF && postedFiles[0] == null)
                    {
                        err = 2;
                        errDesc = Message.FileUpload.PdfFileRequired;
                        return err;
                    }
                    if (linkTypeId.ToString() == LinkTypes.URL)
                    {
                        if (URL == string.Empty)
                        {
                            err = 2;
                            errDesc = Message.Inbox.URLRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, URL))
                        {
                            err = 2;
                            errDesc = Message.Inbox.InvalidURL;
                            return err;
                        }
                    }
                    //if from no mode to pdf/url mode is selected, check whether proper data is available or not
                    if (mode == Mode.EDIT && (linkTypeId.ToString() == LinkTypes.PDF || linkTypeId.ToString() == LinkTypes.URL) && data.URL == null && data.PdfFilePath == null)
                    {
                        if (linkTypeId.ToString() == LinkTypes.PDF && postedFiles[0] == null)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.PdfFileRequired;
                            return err;
                        }
                        if (linkTypeId.ToString() == LinkTypes.URL)
                        {
                            err = 2;
                            errDesc = Message.Inbox.URLRequired;
                            return err;
                        }
                    }
                    if (postedFiles[0] != null)
                    {
                        //file size checking
                        if (postedFiles[0].ContentLength > MaxFileSize.PDF)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.MaxFileSizePdf;
                            return err;
                        }

                        fileExtension = Path.GetExtension(postedFiles[0].FileName);
                        fileName = DateTime.Now.ToFileTime().ToString() + fileExtension;

                        //check file extension
                        if (fileExtension.ToLower() != FileExtension.PDF)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.InvalidContentTypePdf;
                            return err;
                        }

                        /*check header data to ensure file type*/
                        string mimeType = string.Empty;

                        //get mime type first
                        Stream inputStream = postedFiles[0].InputStream;
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        mimeType = MimeSniffer.GetMime(memoryStream.ToArray());

                        if (mimeType.ToLower() != MIMETypes.PDF)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.InvalidContentTypePdf;
                            return err;
                        }
                        /***************************************/

                        dbFilePath = FilePath.Notice;
                    }

                    //get school list
                    schoolIdList = (Sanitizer.GetSafeHtmlFragment(data.SchoolIdList) ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(schoolIdList))
                    {
                        err = 2;
                        errDesc = Message.Inbox.SchoolListRequired;
                        return err;
                    }
                    try
                    {
                        schoolIdList = GblFunctions.Base64Decode(schoolIdList);
                    }
                    catch
                    {
                        //all school not selected
                    }
                    if (schoolIdList == InboxOptions.Mode.All)
                    {
                        allYN = "Y";
                    }
                    else
                    {
                        allYN = "N";

                        //make xml
                        schoolIdListXml = "<SchoolList>";
                        string[] schoolIdListAry = schoolIdList.Split(',');
                        for (int i = 0; i < schoolIdListAry.Length; i++)
                        {
                            schoolIdListXml += "<School>";
                            schoolIdListXml += "<SchoolId>" + GblFunctions.Base64Decode(schoolIdListAry[i]) + "</SchoolId>";
                            schoolIdListXml += "</School>";
                        }
                        schoolIdListXml += "</SchoolList>";
                    }
                }

                filePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/Notice/");
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new Inbox().SaveAdminSchoolInbox(inboxId, linkTypeId, string.Empty, bodyText, string.Empty, URL, fileName
                                                        , dbFilePath, mode, ipAddress, createdBy, ref errDesc, newYN, archiveYN, allYN
                                                        , schoolIdListXml, InboxOptions.Sender.Admin, InboxOptions.Receiver.School);

                if (err == 0 && mode != Mode.DELETE)
                {
                    if (mode == Mode.EDIT && (postedFiles[0] != null || linkTypeId.ToString() != LinkTypes.PDF))
                    {
                        //try to delete previous file
                        try
                        {
                            string prevFileName = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.PdfFilePath.Trim()));
                            prevFileName = filePath + prevFileName.Substring(prevFileName.LastIndexOf('/') + 1).Trim();

                            if (File.Exists(prevFileName))
                            {
                                File.Delete(prevFileName);
                            }
                        }
                        catch
                        {
                            //nothing to do. previous image will not be deleted
                        }
                    }
                    if (postedFiles[0] != null)
                    {
                        postedFiles[0].SaveAs(filePath + fileName);
                    }
                }
                else if (err == 0 && mode == Mode.DELETE)
                {
                    string prevFileName = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment((data.PdfFilePath ?? string.Empty).Trim()));
                    prevFileName = filePath + prevFileName.Substring(prevFileName.LastIndexOf('/') + 1).Trim();

                    if (File.Exists(prevFileName))
                    {
                        File.Delete(prevFileName);
                    }
                }
                else
                {
                    MCommon.saveExceptionLog(errDesc, "AdminInbox/saveAdminSchoolInbox(MInbox)", "AdminInboxController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "AdminInbox/saveAdminSchoolInbox(MInbox)", "AdminInboxController");
            }

            return err;
        }

        public string getAdminSchoolInboxView(string inboxId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = new Inbox().GetAdminSchoolInboxView(Convert.ToInt64(GblFunctions.Base64Decode(inboxId)), ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetSchoolInboxView/getAdminSchoolInboxView(MInbox)", "AdminSchoolInboxController");
                    data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxView/getAdminSchoolInboxView(MInbox)", "AdminSchoolInboxController");
            }

            return data;
        }

        public string getAdminSchoolInboxEdit(string inboxId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = new Inbox().GetAdminSchoolInboxEdit(Convert.ToInt64(GblFunctions.Base64Decode(inboxId)), ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetSchoolInboxEdit/getAdminSchoolInboxEdit(MInbox)", "AdminSchoolInboxController");
                    data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxEdit/getAdminSchoolInboxEdit(MInbox)", "AdminSchoolInboxController");
            }

            return data;
        }

        public List<VMInbox> getAdminPreSchoolInboxList(int? page, int? limit, string sortBy, string direction, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMInbox> result = new List<VMInbox>();

            try
            {
                dt = new Inbox().GetAdminPreSchoolInboxList(searchString, page, limit, ref total, archiveYN, 0);

                var records = (from data in dt.AsEnumerable()
                               select new VMInbox
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   InboxId = data.Field<string>("InboxId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   BodyTextUnread = data.Field<string>("BodyTextUnread"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   ReadYN = data.Field<string>("ReadYN"),
                                   SchoolCount = data.Field<string>("SchoolCount"),
                                   LinkTypeId = data.Field<string>("LinkTypeId"),
                                   SchoolIdList = data.Field<string>("SchoolIdList")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxList/getAdminPreSchoolInboxList(MInbox)", "AdminPreSchoolInboxController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveAdminPreSchoolInbox(HttpPostedFileBase[] postedFiles, VMInbox data, ref string errDesc)
        {
            int err = 0, createdBy = 0, linkTypeId = 0;
            Int64 inboxId = 0;
            string mode = string.Empty, ipAddress = string.Empty, filePath = string.Empty, fileName = string.Empty, fileExtension = string.Empty, dbFilePath = string.Empty;
            string bodyText = string.Empty, URL = string.Empty, newYN = string.Empty, archiveYN = string.Empty;
            string allYN = string.Empty, schoolIdList = string.Empty, schoolIdListXml = string.Empty;

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                inboxId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.InboxId.Trim())));

                if (mode != Mode.DELETE)
                {
                    linkTypeId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.LinkTypeId.Trim())));
                    //adjust link type. if not selected, make link type = content
                    if (linkTypeId == -1)
                    {
                        linkTypeId = Convert.ToInt32(LinkTypes.CONTENT);
                    }
                    bodyText = Sanitizer.GetSafeHtmlFragment((data.BodyTextOriginal ?? string.Empty).Trim());
                    URL = Sanitizer.GetSafeHtmlFragment((data.URL ?? string.Empty).Trim());
                    newYN = (Sanitizer.GetSafeHtmlFragment(data.NewYN) ?? DefaultSetting.DefaultValN).Trim();
                    if (newYN == string.Empty)
                    {
                        newYN = "N";
                    }
                    archiveYN = (Sanitizer.GetSafeHtmlFragment(data.ArchiveYN) ?? DefaultSetting.DefaultValN).Trim();
                    if (archiveYN == string.Empty)
                    {
                        archiveYN = "N";
                    }
                    if (string.IsNullOrEmpty(bodyText))
                    {
                        err = 2;
                        errDesc = Message.Inbox.InboxIdRequired;
                        return err;
                    }
                    if (!GblFunctions.chkDataFormat(RegexType.Alpha, bodyText))
                    {
                        err = 2;
                        errDesc = Message.Inbox.InvalidContent;
                        return err;
                    }
                    if (mode == Mode.ADD && linkTypeId.ToString() == LinkTypes.PDF && postedFiles[0] == null)
                    {
                        err = 2;
                        errDesc = Message.FileUpload.PdfFileRequired;
                        return err;
                    }
                    if (linkTypeId.ToString() == LinkTypes.URL)
                    {
                        if (URL == string.Empty)
                        {
                            err = 2;
                            errDesc = Message.Inbox.URLRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, URL))
                        {
                            err = 2;
                            errDesc = Message.Inbox.InvalidURL;
                            return err;
                        }
                    }
                    //if from no mode to pdf/url mode is selected, check whether proper data is available or not
                    if (mode == Mode.EDIT && (linkTypeId.ToString() == LinkTypes.PDF || linkTypeId.ToString() == LinkTypes.URL) && data.URL == null && data.PdfFilePath == null)
                    {
                        if (linkTypeId.ToString() == LinkTypes.PDF && postedFiles[0] == null)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.PdfFileRequired;
                            return err;
                        }
                        if (linkTypeId.ToString() == LinkTypes.URL)
                        {
                            err = 2;
                            errDesc = Message.Inbox.URLRequired;
                            return err;
                        }
                    }
                    if (postedFiles[0] != null)
                    {
                        //file size checking
                        if (postedFiles[0].ContentLength > MaxFileSize.PDF)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.MaxFileSizePdf;
                            return err;
                        }

                        fileExtension = Path.GetExtension(postedFiles[0].FileName);
                        fileName = DateTime.Now.ToFileTime().ToString() + fileExtension;

                        //check file extension
                        if (fileExtension.ToLower() != FileExtension.PDF)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.InvalidContentTypePdf;
                            return err;
                        }

                        /*check header data to ensure file type*/
                        string mimeType = string.Empty;

                        //get mime type first
                        Stream inputStream = postedFiles[0].InputStream;
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        mimeType = MimeSniffer.GetMime(memoryStream.ToArray());

                        if (mimeType.ToLower() != MIMETypes.PDF)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.InvalidContentTypePdf;
                            return err;
                        }
                        /***************************************/

                        dbFilePath = FilePath.Notice;
                    }

                    //get school list
                    schoolIdList = (Sanitizer.GetSafeHtmlFragment(data.SchoolIdList) ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(schoolIdList))
                    {
                        err = 2;
                        errDesc = Message.Inbox.SchoolListRequired;
                        return err;
                    }
                    try
                    {
                        if (GblFunctions.Base64Decode(schoolIdList) == InboxOptions.Mode.All)
                        {
                            allYN = "Y";
                            schoolIdList = InboxOptions.Mode.All;
                        }
                    }
                    catch
                    {
                        //all school not selected
                    }
                    if (schoolIdList != InboxOptions.Mode.All)
                    {
                        allYN = "N";

                        //make xml
                        schoolIdListXml = "<SchoolList>";
                        string[] schoolIdListAry = schoolIdList.Split(',');
                        for (int i = 0; i < schoolIdListAry.Length; i++)
                        {
                            schoolIdListXml += "<School>";
                            schoolIdListXml += "<SchoolId>" + GblFunctions.Base64Decode(schoolIdListAry[i]) + "</SchoolId>";
                            schoolIdListXml += "</School>";
                        }
                        schoolIdListXml += "</SchoolList>";
                    }
                }

                filePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/Notice/");

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new Inbox().SaveAdminPreSchoolInbox(inboxId, linkTypeId, string.Empty, bodyText, string.Empty, URL, fileName
                                                        , dbFilePath, mode, ipAddress, createdBy, ref errDesc, newYN, archiveYN, allYN
                                                        , schoolIdListXml, InboxOptions.Sender.Admin, InboxOptions.Receiver.PreSchool);

                if (err == 0 && mode != Mode.DELETE)
                {
                    if (mode == Mode.EDIT && (postedFiles[0] != null || linkTypeId.ToString() != LinkTypes.PDF))
                    {
                        //try to delete previous file
                        try
                        {
                            string prevFileName = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.PdfFilePath.Trim()));
                            prevFileName = filePath + prevFileName.Substring(prevFileName.LastIndexOf('/') + 1).Trim();

                            if (File.Exists(prevFileName))
                            {
                                File.Delete(prevFileName);
                            }
                        }
                        catch
                        {
                            //nothing to do. previous image will not be deleted
                        }
                    }
                    if (postedFiles[0] != null)
                    {
                        postedFiles[0].SaveAs(filePath + fileName);
                    }
                }
                else if (err == 0 && mode == Mode.DELETE)
                {
                    string prevFileName = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment((data.PdfFilePath ?? string.Empty).Trim()));
                    prevFileName = filePath + prevFileName.Substring(prevFileName.LastIndexOf('/') + 1).Trim();

                    if (File.Exists(prevFileName))
                    {
                        File.Delete(prevFileName);
                    }
                }
                else
                {
                    MCommon.saveExceptionLog(errDesc, "AdminInbox/saveAdminPreSchoolInbox(MInbox)", "AdminPreSchoolInboxController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "AdminInbox/saveAdminPreSchoolInbox(MInbox)", "AdminPreSchoolInboxController");
            }

            return err;
        }

        public string getAdminPreSchoolInboxView(string inboxId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = new Inbox().GetAdminPreSchoolInboxView(Convert.ToInt64(GblFunctions.Base64Decode(inboxId)), ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetSchoolInboxView/getAdminPreSchoolInboxView(MInbox)", "AdminPreSchoolInboxController");
                    data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxView/getAdminPreSchoolInboxView(MInbox)", "AdminPreSchoolInboxController");
            }

            return data;
        }

        public string getAdminPreSchoolInboxEdit(string inboxId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = new Inbox().GetAdminPreSchoolInboxEdit(Convert.ToInt64(GblFunctions.Base64Decode(inboxId)), ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetSchoolInboxEdit/getAdminPreSchoolInboxEdit(MInbox)", "AdminPreSchoolInboxController");
                    data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxEdit/getAdminPreSchoolInboxEdit(MInbox)", "AdminPreSchoolInboxController");
            }

            return data;
        }
    }
}
