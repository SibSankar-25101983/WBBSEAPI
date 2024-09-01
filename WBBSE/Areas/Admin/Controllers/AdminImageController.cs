using System;
using System.Web.Mvc;
using WBBSE.Areas.Admin.Models;
using System.Web.Routing;
using Microsoft.Security.Application;
using System.Web;
using WBBSE.Models;
using ViewModel;
using Common;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminImageController : Controller
    {
        private int makeData(string encRoleId)
        {
            int status = 0;

            try
            {
                int groupId = (Session[SessionNames.GroupId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.GroupId]);
                if (string.IsNullOrEmpty(encRoleId))
                {
                    status = 0;
                }
                else
                {
                    int roleId = Convert.ToInt32(GblFunctions.decryptPassword(encRoleId));
                    Session[SessionNames.RoleId] = roleId;

                    string ViewYN = string.Empty, AddYN = string.Empty, EditYN = string.Empty, DeleteYN = string.Empty, ReportYN = string.Empty, SystemYN = string.Empty;

                    if (groupId != 1 && groupId != 2)
                    {
                        MUserPermission mu = new MUserPermission();

                        mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                        if (ViewYN == "N")
                        {
                            status = 0; //configure for un-authenticated user access
                        }
                        else
                        {
                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.EditYN] = EditYN;
                            ViewData[ViewDataNames.DeleteYN] = DeleteYN;
                            status = 1;
                        }
                    }
                    else
                    {
                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.EditYN] = DefaultSetting.DefaultValY;
                        ViewData[ViewDataNames.DeleteYN] = DefaultSetting.DefaultValY;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminImageController");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult SliderHome(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    if (TempData[TempDataNames.SaveStatus] == null)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveSuccess)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.FileUpload.ImageUploadSuccessful;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveDelete)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.FileUpload.ImageDeleteSuccessful;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.ValidationFailed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = TempData[TempDataNames.ErrDesc];
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }

                    //get images
                    int err = 0;

                    string data = new MImage().getImageList(MenuCode.ImageSlider, ViewData[ViewDataNames.EditYN].ToString(), ViewData[ViewDataNames.DeleteYN].ToString(), ref err);

                    if (err == 1)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.AddYN] = "hidden";
                        ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                        ViewData[ViewDataNames.RawData] = string.Empty;
                    }
                    else
                    {
                        ViewData[ViewDataNames.RawData] = data;
                    }

                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    return View();
                }
                else if (status == -1)
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                else //CONFIGURE FOR NO VIEW PERMISSION
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SliderHome/View", "AdminImageController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SliderHome(HttpPostedFileBase[] postedFiles, VMImage data)
        {
            try
            {
                int err = 0;
                string errDesc = string.Empty;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);

                err = new MImage().saveImage(postedFiles, data, MenuCode.ImageSlider, ref errDesc);

                if (err == 0)
                {
                    if (data.EntType == EntType.DELETE)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
                    }
                }
                else if (err == 2)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                    TempData[TempDataNames.ErrDesc] = errDesc;
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                }
                return RedirectToAction("SliderHome", "AdminImage", rvd);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SliderHome/Save", "AdminImageController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult PhotoGallery(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    if (TempData[TempDataNames.SaveStatus] == null)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveSuccess)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.FileUpload.ImageUploadSuccessful;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveDelete)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.FileUpload.ImageDeleteSuccessful;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.ValidationFailed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = TempData[TempDataNames.ErrDesc];
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }

                    //get images
                    int err = 0;

                    string data = new MImage().getImageList(MenuCode.PhotoGallery, ViewData[ViewDataNames.EditYN].ToString(), ViewData[ViewDataNames.DeleteYN].ToString(), ref err);

                    if (err == 1)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.AddYN] = "hidden";
                        ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                        ViewData[ViewDataNames.RawData] = string.Empty;
                    }
                    else
                    {
                        ViewData[ViewDataNames.RawData] = data;
                    }

                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    return View();
                }
                else if (status == -1)
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                else //CONFIGURE FOR NO VIEW PERMISSION
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PhotoGallery/View", "AdminImageController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PhotoGallery(HttpPostedFileBase[] postedFiles, VMImage data)
        {
            try
            {
                int err = 0;
                string errDesc = string.Empty;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);

                err = new MImage().saveImage(postedFiles, data, MenuCode.PhotoGallery, ref errDesc);

                if (err == 0)
                {
                    if (data.EntType == EntType.DELETE)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
                    }
                }
                else if (err == 2)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                    TempData[TempDataNames.ErrDesc] = errDesc;
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                }
                return RedirectToAction("PhotoGallery", "AdminImage", rvd);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PhotoGallery/Save", "AdminImageController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
