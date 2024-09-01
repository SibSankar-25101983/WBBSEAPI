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
    public class MExamSchedule
    {
        public int saveExamSchedule(HttpPostedFileBase[] postedFiles, VMContentDetails data, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            Int64 ContentId = 0;
            string mode = string.Empty, ipAddress = string.Empty, filePath = string.Empty, fileName = string.Empty, fileExtension = string.Empty, dbFilePath = string.Empty;
            string newYN = string.Empty, archiveYN = "N";

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                ContentId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.ContentId.Trim())));

                if (mode != Mode.DELETE)
                {
                    if (mode == Mode.ADD && postedFiles[0] == null)
                    {
                        err = 2;
                        errDesc = Message.FileUpload.PdfFileRequired;
                        return err;
                    }

                    newYN = (Sanitizer.GetSafeHtmlFragment(data.NewYN) ?? DefaultSetting.DefaultValN).Trim();

                    if (newYN == string.Empty)
                    {
                        newYN = "N";
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

                        //file extension checking
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

                        dbFilePath = FilePath.ExamSchedule;
                    }
                }

                filePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/ExamSchedule/");
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new DAL.Common().SaveContentDetails(ContentId, MenuCode.ExamSchedule, Convert.ToInt32(LinkTypes.PDF), string.Empty, string.Empty, string.Empty
                                                        , string.Empty, fileName, dbFilePath, mode, ipAddress, createdBy, ref errDesc, newYN, archiveYN, MenuType.MainMenu);

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
                    MCommon.saveExceptionLog(errDesc, "Examination/saveExamSchedule(MExamSchedule)", "AdminExamScheduleController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "Examination/saveExamSchedule(MExamSchedule)", "AdminExamScheduleController");
            }

            return err;
        }
    }
}