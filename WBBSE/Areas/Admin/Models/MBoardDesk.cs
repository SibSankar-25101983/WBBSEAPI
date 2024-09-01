using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;
using System.IO;

namespace WBBSE.Areas.Admin.Models
{
    public class MBoardDesk
    {
        BoardDesk bd = new BoardDesk();

        public List<VMBoardDesk> getBoardDeskList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total)
        {
            DataTable dt = new DataTable();
            List<VMBoardDesk> result = new List<VMBoardDesk>();

            try
            {
                dt = bd.GetBoardDeskList(menuCode, page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMBoardDesk
                               {
                                   ContentId = data.Field<string>("ContentId"),
                                   ImagePath = data.Field<string>("ImagePath"),
                                   BodyTextPartial = data.Field<string>("BodyTextPartial"),
                                   BodyText = data.Field<string>("BodyText")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetPresidentDeskList/getBoardDeskList(MBoardDesk)", "AdminBoardDeskController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveBoardDesk(HttpPostedFileBase[] postedFiles, VMBoardDesk data, string menuCode, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            string mode = string.Empty, ipAddress = string.Empty, imagePath = string.Empty, imageFileExtension = string.Empty, dbImagePath = string.Empty;

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                string bodyText = Sanitizer.GetSafeHtmlFragment((data.BodyText ?? string.Empty).Trim());
                bodyText = bodyText.Replace(System.Environment.NewLine, "");

                if (bodyText == string.Empty)
                {
                    err = 2;
                    errDesc = Message.Content.ContentRequired;
                    return err;
                }
                if (!GblFunctions.chkDataFormat(RegexType.ContentEdit, bodyText))
                {
                    err = 2;
                    errDesc = Message.Content.InvalidContent;
                    return err;
                }

                if (mode == Mode.ADD && postedFiles[0] == null)
                {
                    err = 2;
                    errDesc = Message.FileUpload.ImageRequired;
                    return err;
                }

                if ((mode == Mode.DELETE || mode == Mode.EDIT) && string.IsNullOrEmpty(data.ImagePath))
                {
                    MCommon.saveExceptionLog("Image Name Not Found while Deleting Image", "saveBoardDesk(MBoardDesk)", "AdminBoardDeskController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }

                if (mode != Mode.DELETE && postedFiles[0] != null)
                {
                    imageFileExtension = Path.GetExtension(postedFiles[0].FileName);

                    //check image extension
                    if (imageFileExtension.ToLower() != ImageExtension.JPG && imageFileExtension.ToLower() != ImageExtension.JPEG && imageFileExtension.ToLower() != ImageExtension.PNG)
                    {
                        err = 2;
                        errDesc = Message.FileUpload.InvalidImageExt;
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

                    if (mimeType.ToLower() != MIMETypes.JPG && mimeType.ToLower() != MIMETypes.JPEG && mimeType.ToLower() != MIMETypes.PNG)
                    {
                        err = 2;
                        errDesc = Message.FileUpload.InvalidImageExt;
                    }
                    /***************************************/

                    dbImagePath = ImagePath.BoardDesk + ((menuCode == MenuCode.PresidentDesk) ? "President" : "Secretary") + imageFileExtension;
                }

                imagePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/BoardDesk/");
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new DAL.Common().SaveContentDetails(Convert.ToInt64(GblFunctions.Base64Decode(data.ContentId)), menuCode, Convert.ToInt32(LinkTypes.URL)
                                                        , data.HeaderText, bodyText, string.Empty, dbImagePath, string.Empty, string.Empty, mode
                                                        , ipAddress, createdBy, ref errDesc);

                if (err == 0 && mode != Mode.DELETE)
                {
                    if (mode == Mode.EDIT && postedFiles[0] != null)
                    {
                        //try to delete previous image
                        try
                        {
                            string prevImageName = Sanitizer.GetSafeHtmlFragment(data.ImagePath.Trim());
                            prevImageName = imagePath + prevImageName.Substring(prevImageName.LastIndexOf('/') + 1).Trim();

                            if (File.Exists(prevImageName))
                            {
                                File.Delete(prevImageName);
                            }
                        }
                        catch
                        {
                            //nothing to do. previous image will not be deleted
                        }
                    }
                    if (postedFiles[0] != null)
                    {
                        postedFiles[0].SaveAs(imagePath + ((menuCode == MenuCode.PresidentDesk) ? "President" : "Secretary") + imageFileExtension);
                    }
                }
                else if (err == 0 && mode == Mode.DELETE)
                {
                    string prevImageName = Sanitizer.GetSafeHtmlFragment(data.ImagePath.Trim());
                    prevImageName = imagePath + prevImageName.Substring(prevImageName.LastIndexOf('/') + 1).Trim();

                    if (prevImageName != errDesc)
                    {
                        if (File.Exists(prevImageName))
                        {
                            File.Delete(prevImageName);
                        }
                    }
                }
                else
                {
                    MCommon.saveExceptionLog(errDesc, "PresidentDesk/saveBoardDesk(MBoardDesk)", "AdminBoardDeskController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "PresidentDesk/saveBoardDesk(MBoardDesk)", "AdminBoardDeskController");
            }

            return err;
        }
    }
}
