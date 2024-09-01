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

namespace WBBSE.Areas.Schools.Controllers
{
    /*******************************************************************************
   * A BRIEF HISTORY OF SchoolUserProfileController.
   * CONTAINS SCHOOL USER PROFILE DATA RELATED ACTIONS 
   * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
   * ViewModel: THIS IS A PROJECT WHERE ALL VIEW MODEL CLASSES ARE WRITTEN AS CLASS FILE.
   * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
   * WBBSE.AREAS.SCHOOLS.MODELS: MODEL CLASS FOR SchoolHome RELATED BUSINESS LOGICS & FUNCTIONS. 
   ******************************************************************************/

    /* ROLE BASED ASP.NET MVC AUTHORIZATION [ONLY SCHOOL ROLE IS ALLOWED FOR ACCESSING THIS CONTROLLER]
     * NoCache: PREVENT CACHING IN MVC
     */
    [Authorize(Roles = UserType.SCHOOL), NoCache]
    public class SchoolUserProfileController : Controller
    {
        public ActionResult UserProfile() //DEFAULT ACTION OF THIS CONTROLLER
        {
            try
            {
                int userId = (Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.UserId]);

                if (userId == 0)// 0 MEANS INVALID SESSION & ERROR PAGE REDIRECTION
                {
                    return Redirect("~/Error/Unexpected.html");
                }

                VMProfileData vpd = new MProfileVerification().getProfileData(userId); //MODEL DATA FETCHING

                if (vpd == null) //NO MODEL DATA & ERROR PAGE REDIRECTION
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

                return View(vpd); //RETURNING VIEW PAGE WITH PROFILE DATA
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "UserProfile/View", "SchoolUserProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UserProfile(VMProfileData vmpd) //ACTION FOR POSTING PROFILE VERIFICATION DATA TO SERVER, vmpd=VIEW MODEL OF PROFILE DATA
        {
            try
            {
                string errDesc = string.Empty;
                MProfileVerification mpv = new MProfileVerification();

                //POST PROFILE DATA TO MODEL
                int err = mpv.saveProfileData(vmpd, ref errDesc);

                if (err == 0) //0 MEANS SUCCESS FROM MODEL
                {
                    TempData[TempDataNames.SaveStatus] = "SP"; //SAVE SUCCESS MESSAGE
                }
                else if (err == -1)//-1 MEANS OPERATION ERROR FROM MODEL
                {
                    TempData[TempDataNames.SaveStatus] = "FP"; //FAILED PROFILE MESSAGE
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                }
                else if (err == -2) //-2 MEANS DATA VALIDATION ERROR FROM MODEL
                {
                    TempData[TempDataNames.SaveStatus] = "FVP";
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    TempData[TempDataNames.ErrDesc] = errDesc;
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = "FP"; //FAILED PROFILE MESSAGE
                }
                return RedirectToAction("UserProfile", "SchoolUserProfile");
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "UserProfile/Save", "SchoolUserProfileController");
                return View("~/Areas/Schools/Views/SchoolUserProfile/UserProfile.cshtml");
            }
        }

        [HttpGet]
        public JsonResult GetSalutationList()
        {
            var records = new MCommon().getMstSalutationListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SchoolUserEditProfile(VMProfileData vmpd)
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
                    TempData[TempDataNames.ErrDesc] = errDesc;
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = "FP";
                }
                return RedirectToAction("UserProfile", "SchoolUserProfile");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SchoolUserEditProfile/Edit", "SchoolUserProfileController");
                return View("~/Areas/School/Views/SchoolUserProfile/UserProfile.cshtml");
            }
        }

    }
}
