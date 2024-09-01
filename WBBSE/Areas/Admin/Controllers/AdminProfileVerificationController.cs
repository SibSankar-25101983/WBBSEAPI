using System;
using System.Web;
using System.Web.Mvc;
using WBBSE.Areas.Admin.Models;
using WBBSE.Models;
using ViewModel;
using Common;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminProfileVerificationController : Controller
    {
        public ActionResult AdminProfileVerification()
        {
            try
            {
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "Y" && Session[SessionNames.EmailVerifiedYN].ToString() == "Y" && Session[SessionNames.NewPasswordChangedYN].ToString() == "Y")
                {
                    return RedirectToActionPermanent("AdminHome", "AdminHome");
                }
                string resendEmailOTPYN = string.Empty, resendContactOTPYN = string.Empty, showHomeLinkYN = string.Empty; //, emailId = string.Empty, contactNo = string.Empty
                int userId = (Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.UserId]);
                int userTypeId = Convert.ToInt32(Session[SessionNames.UserTypeId]);
                int err = 0;

                VMProfileVerification data = new MProfileVerification().getProfileVerificationData(ref err, userId, ref resendEmailOTPYN, ref resendContactOTPYN, ref showHomeLinkYN, userTypeId, (TempData[TempDataNames.ProfileVerificationDataFetchMode] == null) ? ProfileVerificationDataFetchMode.New : TempData[TempDataNames.ProfileVerificationDataFetchMode].ToString());

                //int error = new MProfileVerification().getProfileVerificationData(userId, userTypeId, ref emailId, ref contactNo, ref resendEmailOTPYN, ref resendContactOTPYN, ref showHomeLinkYN);

                if (err == 0)
                {
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = resendEmailOTPYN;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = resendContactOTPYN;
                    ViewData[ViewDataNames.ShowHomeLinksYN] = showHomeLinkYN;
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
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEG")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SER")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEC")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCG")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSent;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCR")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSent;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCC")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FP")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FEG")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FER")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FEC")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCG")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCR")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCC")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationErrorMsg;
                }
                else
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                }

                //show home button link
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "Y" && (Session[SessionNames.EmailVerifiedYN].ToString() == "Y" || Session[SessionNames.ContactNoVerifiedYN].ToString() == "Y"))
                {
                    ViewData[ViewDataNames.ShowHomeLinksYN] = "Y";
                }
                return View(data);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminProfileVerification/View", "AdminProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult ChkDuplicateContact(string e, string m)
        {
            int err = 0;
            string errDesc = string.Empty;
            int userId = 0;

            try
            {
                userId = Convert.ToInt32(Session[SessionNames.UserId]);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ChkDuplicateContact", "AdminProfileVerificationController");
            }

            err = new MMstUser().chkDuplicateContactMstUser((e ?? string.Empty), (m ?? string.Empty), userId.ToString(), EntType.EDIT, ref errDesc);

            return Json(new { err, errDesc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AdminProfileVerification(VMProfileVerification vmpv)
        {
            try
            {
                string mode = vmpv.OperationType;
                /****VALIDATIONS****/
                if (string.IsNullOrEmpty(mode))
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                    return View("~/Areas/Admin/Views/AdminProfileVerification/AdminProfileVerification.cshtml");
                }
                /*
                 * Modes
                 * -----
                 * P : New Password Change
                 * EG : Email Get OTP
                 * ER : Email Resend OTP
                 * EC : Email Confirmation
                 * CG : Contact Get OTP
                 * CR : Contact Resend OTP
                 * CC : Contact Confirmation
                */
                if (mode != "P" && mode != "EG" && mode != "ER" && mode != "EC" && mode != "CG" && mode != "CR" && mode != "CC")
                {
                    //ViewData[ViewDataNames.ErrorVisibility] = "";
                    //ViewData[ViewDataNames.SucessVisibility] = "none";
                    //ViewData[ViewDataNames.SaveInfo] = Message.InvalidRequest;
                    //return View("~/Areas/Admin/Views/AdminProfileVerification/AdminProfileVerification.cshtml");
                    return Redirect("~/Error/Unexpected.html");
                }
                /*******************/
                string errDesc = string.Empty;
                MProfileVerification mpv = new MProfileVerification();
                vmpv.VerificationFor = UserType.ADMIN;

                int err = mpv.saveProfileVerificationData(vmpv, ref errDesc);

                if (err == 0)
                {
                    TempData[TempDataNames.ProfileVerificationDataFetchMode] = ProfileVerificationDataFetchMode.Session;

                    if (mode == "P")
                    {
                        TempData[TempDataNames.SaveStatus] = "SP";
                    }
                    else if (mode == "EG")
                    {
                        TempData[TempDataNames.SaveStatus] = "SEG";
                    }
                    else if (mode == "ER")
                    {
                        TempData[TempDataNames.SaveStatus] = "SER";
                    }
                    else if (mode == "EC")
                    {
                        TempData[TempDataNames.SaveStatus] = "SEC";
                    }
                    else if (mode == "CG")
                    {
                        TempData[TempDataNames.SaveStatus] = "SCG";
                    }
                    else if (mode == "CR")
                    {
                        TempData[TempDataNames.SaveStatus] = "SCR";
                    }
                    else if (mode == "CC")
                    {
                        TempData[TempDataNames.SaveStatus] = "SCC";
                    }
                }
                else if (err == -1)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                    return View("~/Areas/Admin/Views/AdminProfileVerification/AdminProfileVerification.cshtml");
                }
                else if (err == -2)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = errDesc;
                    return View("~/Areas/Admin/Views/AdminProfileVerification/AdminProfileVerification.cshtml");
                }
                else
                {
                    if (mode == "P")
                    {
                        TempData[TempDataNames.SaveStatus] = "FP";
                    }
                    else if (mode == "EG")
                    {
                        TempData[TempDataNames.SaveStatus] = "FEG";
                    }
                    else if (mode == "ER")
                    {
                        TempData[TempDataNames.SaveStatus] = "FER";
                    }
                    else if (mode == "EC")
                    {
                        TempData[TempDataNames.SaveStatus] = "FEC";
                    }
                    else if (mode == "CG")
                    {
                        TempData[TempDataNames.SaveStatus] = "FCG";
                    }
                    else if (mode == "CR")
                    {
                        TempData[TempDataNames.SaveStatus] = "FCR";
                    }
                    else if (mode == "CC")
                    {
                        TempData[TempDataNames.SaveStatus] = "FCC";
                    }
                }
                return RedirectToAction("AdminProfileVerification", "AdminProfileVerification");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminProfileVerification/Save", "AdminProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
                //return View("~/Areas/Admin/Views/AdminProfileVerification/AdminProfileVerification.cshtml");
            }
        }
    }
}
