using System;
using System.Web;
using System.Net;
using System.Data;
using System.Web.UI;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Routing;
using WBBSE.Models;
using ViewModel;
using System.Text.RegularExpressions;
using Common;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminProfileController : Controller
    {
        public ActionResult AdminProfile()
        {
            try
            {
                int userId = (Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.UserId]);

                if (userId == 0)
                {
                    return Redirect("~/Error/Unexpected.html");
                }

                VMProfileData vpd = new MProfileVerification().getProfileData(userId);

                if (vpd == null)
                {
                    return Redirect("~/Error/Unexpected.html");
                }

                if (TempData[TempDataNames.SaveStatus] == null)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SP")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.PasswordSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEP")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.SchoolProfileVerificationDataSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SI")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.FileUpload.ImageUploadSuccessful;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FVP")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = TempData[TempDataNames.ErrDesc];
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FVEP")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = TempData[TempDataNames.ErrDesc];
                }
                    
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FVI")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = TempData[TempDataNames.ErrDesc];
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FP")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FEP")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FI")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
                }
                else
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                }

                return View(vpd);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminProfile/View", "AdminProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AdminProfile(VMProfileData vmpd)
        {
            try
            {
                string errDesc = string.Empty;
                MProfileVerification mpv = new MProfileVerification();

                int err = mpv.saveProfileData(vmpd, ref errDesc);

                if (err == 0)
                {
                    TempData[TempDataNames.SaveStatus] = "SP";
                }
                else if (err == -1)
                {
                    TempData[TempDataNames.SaveStatus] = "FP";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                }
                else if (err == -2)
                {
                    TempData[TempDataNames.SaveStatus] = "FVP";
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    TempData[TempDataNames.ErrDesc] = errDesc;
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = "FP";
                }
                return RedirectToAction("AdminProfile", "AdminProfile");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminProfile/Save", "AdminProfileController");
                return View("~/Areas/Admin/Views/AdminProfile/AdminProfile.cshtml");
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AdminEditProfile(VMProfileData vmpd)
        {
            try
            {
                string errDesc = string.Empty;
                MProfileVerification mpv = new MProfileVerification();

                int err = mpv.saveEditProfileData(vmpd, ref errDesc);

                if (err == 0)
                {
                    TempData[TempDataNames.SaveStatus] = "SEP";
                }
                else if (err == -1)
                {
                    TempData[TempDataNames.SaveStatus] = "FEP";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                }
                else if (err == -2)
                {
                    TempData[TempDataNames.SaveStatus] = "FVEP";
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    TempData[TempDataNames.ErrDesc] = errDesc;
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = "FP";
                }
                return RedirectToAction("AdminProfile", "AdminProfile");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminProfile/Save", "AdminProfileController");
                return View("~/Areas/Admin/Views/AdminProfile/AdminProfile.cshtml");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AdminProfileUploadImage(HttpPostedFileBase[] postedFile)
        {
            try
            {
                int err = 0;
                string errDesc = string.Empty;

                err = new MProfileVerification().saveProfileImage(postedFile, ref errDesc);

                if (err == 0)
                {
                    Session[SessionNames.ProfileImage] = errDesc + "?" + DateTime.Now.ToFileTime();
                    TempData[TempDataNames.SaveStatus] = "SI";
                }
                else if (err == -1)
                {
                    TempData[TempDataNames.SaveStatus] = "FVI";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = "FI";
                }
                return RedirectToAction("AdminProfile", "AdminProfile");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminProfileUploadImage", "AdminProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
