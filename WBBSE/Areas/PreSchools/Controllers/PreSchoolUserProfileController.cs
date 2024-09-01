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

namespace WBBSE.Areas.PreSchools.Controllers
{
    [Authorize(Roles = UserType.PRESCHOOL), NoCache]
    public class PreSchoolUserProfileController : Controller
    {
        public ActionResult UserProfile()
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
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEP") //EDIT PROFILE MESSAGE
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
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FVEP") //FAILED EDIT PROFILE MESSAGE
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
                MCommon.saveExceptionLog(ex.Message, "UserProfile/View", "PreSchoolUserProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UserProfile(VMProfileData vmpd)
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
                return RedirectToAction("UserProfile", "PreSchoolUserProfile");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "UserProfile/Save", "PreSchoolUserProfileController");
                return View("~/Areas/PreSchools/Views/PreSchoolUserProfile/PreUserProfile.cshtml");
            }
        }

        [HttpGet]
        public JsonResult GetSalutationList()
        {
            var records = new MCommon().getMstSalutationListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PreSchoolUserEditProfile(VMProfileData vmpd)
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
                return RedirectToAction("UserProfile", "PreSchoolUserProfile");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PreSchoolUserEditProfile/Edit", "PreSchoolUserProfileController");
                return View("~/Areas/PreSchools/Views/PreSchoolUserProfile/UserProfile.cshtml");
            }
        }

    }
}
