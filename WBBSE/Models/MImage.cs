using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.Security.Application;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Models
{
    public class MImage
    {
        Image i = new Image();

        public string getImageList(string imageFor, string editYN, string deleteYN, ref int err)
        {
            string data = string.Empty;

            try
            {
                data = i.GetImageList(imageFor, editYN, deleteYN, ref err);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "SliderHome/getImageList(MImage)", "AdminImageController");
                    data = Message.OperationError;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SliderHome/getImageList(MImage)", "AdminImageController");
            }

            return data;
        }

        public int saveImage(HttpPostedFileBase[] postedFiles, VMImage data, string imageFor, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            string mode = string.Empty, ipAddress = string.Empty, imagePath = string.Empty, imageFileExtension = string.Empty, dbImagePath = string.Empty;

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }

                //check in delete mode, whether file name is present or not.
                if (mode == Mode.DELETE && string.IsNullOrEmpty(data.ImageName))
                {
                    MCommon.saveExceptionLog("Image Name Not Found while Deleting Image", "saveImage(MImage)", "AdminImageController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                if (imageFor == MenuCode.ImageSlider)
                {
                    imagePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/SliderImages/");
                    dbImagePath = ImagePath.ImageSlider;
                }
                else if (imageFor == MenuCode.PhotoGallery)
                {
                    imagePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/PhotoGallery/");
                    dbImagePath = ImagePath.PhotoGallery;
                }

                foreach (HttpPostedFileBase postedFile in postedFiles)
                {
                    //validations
                    if (mode != Mode.DELETE)
                    {
                        //check image extension
                        imageFileExtension = Path.GetExtension(postedFile.FileName);

                        if (imageFileExtension.ToLower() != ImageExtension.JPG && imageFileExtension.ToLower() != ImageExtension.JPEG && imageFileExtension.ToLower() != ImageExtension.PNG)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.InvalidImageExt;
                            break;
                        }

                        /*check header data to ensure file type*/
                        string mimeType = string.Empty;

                        //get mime type first
                        Stream inputStream = postedFile.InputStream;
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        mimeType = MimeSniffer.GetMime(memoryStream.ToArray());

                        if (mimeType.ToLower() != MIMETypes.JPG && mimeType.ToLower() != MIMETypes.JPEG && mimeType.ToLower() != MIMETypes.PJPEG && mimeType.ToLower() != MIMETypes.PNG)
                        {
                            err = 2;
                            errDesc = Message.FileUpload.InvalidImageExt;
                            break;
                        }
                        /***************************************/
                    }

                    err = i.SaveImage(data.ImageId, imageFor, dbImagePath, imageFileExtension, mode, createdBy, ipAddress, ref errDesc);

                    if (err == 0 && mode != Mode.DELETE)
                    {
                        if (mode == Mode.EDIT)
                        {
                            //try to delete previous image
                            try
                            {
                                string prevImageName = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.ImageName));

                                if (prevImageName != errDesc)
                                {
                                    i.deleteImages(imagePath, prevImageName, ref prevImageName);
                                }
                            }
                            catch
                            {
                                //nothing to do. previous image will not be deleted
                            }
                        }
                        postedFile.SaveAs(imagePath + errDesc);
                    }
                    else if (err == 0 && mode == Mode.DELETE)
                    {

                        err = i.deleteImages(imagePath, GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.ImageName)), ref errDesc);

                        //error handling during image delete
                        if (err == 1)
                        {
                            MCommon.saveExceptionLog(errDesc, "saveImage(MImage)", "AdminImageController");
                            errDesc = Message.OperationError;
                            break;
                        }
                    }
                    else
                    {
                        MCommon.saveExceptionLog(errDesc, "saveImage(MImage)", "AdminImageController");
                        err = 1;
                        errDesc = Message.OperationError;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "saveImage(MImage)", "AdminImageController");
            }

            return err;
        }

        public string getPhotogalleryList(string imageFor, ref int err, ref string pageLink)
        {
            string data = string.Empty;

            try
            {
                data = i.GetPhotogalleryList(imageFor, ref err, ref pageLink);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "PhotoGallery/GetPhotogalleryList(MImage)", "WebController");
                    data = Message.OperationError;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PhotoGallery/GetPhotogalleryList(MImage)", "WebController");
            }

            return data;
        }
    }
}
