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
    public class MResultsAbstract
    {
        public List<VMContentDetails> getResultsAbstractList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new Result().GetResultsAbstractList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetResultsAbstractList/getResultsAbstractList(MResultsAbstract)", "AdminResultsAbstractController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveResultsAbstract(HttpPostedFileBase[] postedFiles, VMContentDetails data, ref string errDesc)
        {
            int err = 0, createdBy = 0, linkTypeId = 0;
            Int64 ContentId = 0;
            string mode = string.Empty, ipAddress = string.Empty, filePath = string.Empty, fileName = string.Empty, fileExtension = string.Empty, dbFilePath = string.Empty;
            string bodyText = string.Empty, URL = string.Empty, newYN = string.Empty, archiveYN = string.Empty;

            try
            {
                mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                ContentId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.ContentId.Trim())));

                if (mode != Mode.DELETE)
                {
                    linkTypeId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.LinkTypeId.Trim())));
                    bodyText = Sanitizer.GetSafeHtmlFragment((data.BodyTextOriginal ?? string.Empty).Trim());
                    URL = Sanitizer.GetSafeHtmlFragment((data.URL ?? string.Empty).Trim());
                    newYN = "N";
                    archiveYN = "N";

                    if (bodyText == string.Empty)
                    {
                        err = 2;
                        errDesc = Message.Content.ContentRequired;
                        return err;
                    }

                    if (mode == Mode.ADD && linkTypeId.ToString() == LinkTypes.PDF && postedFiles[0] == null)
                    {
                        err = 2;
                        errDesc = Message.FileUpload.PdfFileRequired;
                        return err;
                    }
                    if (linkTypeId.ToString() == LinkTypes.URL && URL == string.Empty)
                    {
                        err = 2;
                        errDesc = Message.Content.URLRequired;
                        return err;
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

                        dbFilePath = FilePath.ResultsAbstract;
                    }
                }

                filePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/ResultsAbstract/");
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new DAL.Common().SaveContentDetails(ContentId, MenuCode.ResultsAbstract, linkTypeId, string.Empty, bodyText, string.Empty
                                                        , URL, fileName, dbFilePath, mode, ipAddress, createdBy, ref errDesc, newYN, archiveYN);

                if (err == 0 && mode != Mode.DELETE)
                {
                    if (mode == Mode.EDIT && postedFiles[0] != null)
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
                    MCommon.saveExceptionLog(errDesc, "ResultsAbstract/saveResultsAbstract(MResultsAbstract)", "AdminResultsAbstractController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "ResultsAbstract/saveResultsAbstract(MResultsAbstract)", "AdminResultsAbstractController");
            }

            return err;
        }
    }
}
