using System;
using System.Web;
using System.Web.Mvc;
using WBBSE.Models;
using WBBSE.Areas.Admin.Models;
using WBBSE.Areas.PreSchools.Models;
using ViewModel;
using Common;
using System.Web.Routing;

namespace WBBSE.Areas.PreSchools.Controllers
{
    /*
        * THIS CONTROLLER IS USED FOR PRE-SCHOOL PROFILE VRIFICATION.
        * SET PRE-SCHOOL USER DETAILS LIKE SALUTATION,NAME
        * SET EMAIL-ID, MOBILE NO, PHONE NO   
        * USER CAN RESET PASSWARD
        * USER CAN GENERATE/RESEND OTP FOR EMAIL/MOBILE VERIFICATION
        * USER CAN VERIFIED EMAIL-ID THROUGH OTP
        * makeData()-- IT IS USED FOR ROLE BASED PERMISSION LIKE ADD/EDIT/DELETE AND VIEW PERMISSION.
     */
    [Authorize(Roles = UserType.PRESCHOOL), NoCache]
    public class PreSchoolProfileVerificationController : Controller
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

                    if (groupId != 1 && groupId != 2) //IF NOT SUREP USER (1 & 2) THEN EXECUTE BELOW CODE BLOCK
                    {
                        MUserPermission mu = new MUserPermission();

                        mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                        if (ViewYN == "N")
                        {
                            status = 0; //configure for un-authenticated user access
                        }
                        else
                        {
                            status = 1;
                        }
                    }
                    else
                    {
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "PreSchoolProfileVerificationController");
                status = -1;
            }

            return status;
        }

        //CONFIGURED SUCCESS/ DELETE/ FAILD MESSAGE
        public ActionResult PreSchoolProfileVerification()
        {
            try
            {
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "Y")
                {
                    return RedirectToActionPermanent("PreSchoolHome", "PreSchoolHome");
                }
                
                int err = 0;
                string resendEmailOTPYN = string.Empty, resendContactOTPYN = string.Empty, showHomeLinkYN = string.Empty;
                int userId = (Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.UserId]); //USER UNIQUE ID
                int userTypeId = Convert.ToInt32(Session[SessionNames.UserTypeId]); //USER TYPE UNIQUE ID
                int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]); // USER GROUP UNIQUE ID
                Int64 organizationId = Convert.ToInt64(Session[SessionNames.OrganizationId]); //PRE-SCHOOL UNIQUE ID
                

                VMProfileVerification data = new VMProfileVerification();

                data = new MPreSchoolProfileVerification().getOrganizationProfileVerificationData(ref err, userId, ref resendEmailOTPYN, ref resendContactOTPYN, ref showHomeLinkYN, userTypeId, organizationId, groupId, (TempData[TempDataNames.ProfileVerificationDataFetchMode] == null) ? ProfileVerificationDataFetchMode.New : TempData[TempDataNames.ProfileVerificationDataFetchMode].ToString()); //email id, contact no are of Pre-School

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
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SP") //password save success
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.SchoolProfileVerificationDataSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEG") //email send OTP success
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SER") //email resend OTP success
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEC") //email OTP confirmation success
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCG") //contact send OTP success
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSent;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCR") //contact resend OTP success
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSent;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCC") //contact OTP confirmation success
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FP") //password save failure
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FEG") //email send OTP failure
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FER") //email resend OTP failure
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FEC") //email OTP confirmation failure
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCG") //contact send OTP failure
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCR") //contact resend OTP failure
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCC") //contact OTP confirmation failure
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationErrorMsg;
                }
                //show home button link
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "Y" && (Session[SessionNames.EmailVerifiedYN].ToString() == "Y" || Session[SessionNames.ContactNoVerifiedYN].ToString() == "Y"))
                {
                    ViewData[ViewDataNames.ShowHomeLinksYN] = "Y";
                }
                return View("~/Areas/PreSchools/Views/PreSchoolProfileVerification/PreSchoolProfileVerification.cshtml", data);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PreSchoolProfileVerification/View", "PreSchoolProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //POPULATE SALUTATION LIST (SRI, MR, MRS, MISS ETC ) FROM THIS METHOD/FUNCTION
        [HttpGet]
        public JsonResult GetSalutationList()
        {
            var records = new MCommon().getMstSalutationListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        /*
            * DUPLICATE EMAIL-ID/PHONE NO/ MOBILE NO CHECKING.
            * WHETHER EMAIL-ID,PHONE NO AND MOBILE NO IS EXITS WITH ANOTHER PRE-SCHOOL. CONTACTS NO MUST UNIQUE.
         */
        [HttpGet]
        public JsonResult ChkDuplicateContact(string e, string p, string m) //AJAX CALL FOR DUPLICATE ENTRY (e=EMAIL, p=PHONE NO., m=MOBILE NO.) CHECKING
        {
            int err = 0;
            string errDesc = string.Empty;
            Int64 preSchoolId = 0;

            try
            {
                preSchoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ChkDuplicateContact", "PreSchoolProfileVerificationController");
            }

            /*MODEL CALLING FOR CHECK*/
            err = new MMstPreSchool().chkDuplicateContactMstPreSchool((e ?? string.Empty), (p ?? string.Empty), (m ?? string.Empty), preSchoolId.ToString(), EntType.EDIT, ref errDesc, string.Empty);

            return Json(new { err, errDesc }, JsonRequestBehavior.AllowGet);
        }

        //UPDATE PRE-SCHOOL USER PROFILE DATA AND PASSWORD
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PreSchoolProfileVerification(VMProfileVerification vmpv)
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
                    return View("~/Areas/PreSchools/Views/PreSchoolProfileVerification/PreSchoolProfileVerification.cshtml");
                }
                /*
                 * Modes
                 * -----
                 * P : New Password Change                 
                */
                if (mode != "P")
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.InvalidRequest;
                    return View("~/Areas/PreSchools/Views/PreSchoolProfileVerification/PreSchoolProfileVerification.cshtml");
                }
                /*******************/
                string errDesc = string.Empty;
                MProfileVerification mpv = new MProfileVerification();
                vmpv.VerificationFor = UserType.PRESCHOOL;

                int err = mpv.saveProfileVerificationData(vmpv, ref errDesc); //UPDATE PRE-SCHOOL USER PROFILE AND RELEVANT DATA

                if (err == 0)
                {
                    if (mode == "P")
                    {
                        RouteValueDictionary rvd = new RouteValueDictionary();
                        rvd.Add("m", SchoolHomeMessage.ProfileVerficationSuccess);

                        //TempData[TempDataNames.SaveStatus] = "SP";
                        return RedirectToAction("PreSchoolHome", "PreSchoolHome", new { m = GblFunctions.encryptPassword(SchoolHomeMessage.ProfileVerficationSuccess), area = "PreSchools" });
                    }
                }
                else if (err == -1)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                    return View("~/Areas/PreSchools/Views/PreSchoolProfileVerification/PreSchoolProfileVerification.cshtml");
                }
                else if (err == -2)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = errDesc;
                    return View("~/Areas/PreSchools/Views/PreSchoolProfileVerification/PreSchoolProfileVerification.cshtml");
                }
                else
                {
                    if (mode == "P")
                    {
                        TempData[TempDataNames.SaveStatus] = "FP";
                    }
                }
                return RedirectToAction("PreSchoolProfileVerification", "PreSchoolProfileVerification");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PreSchoolProfileVerification/Save", "PreSchoolProfileVerificationController");
                return View("~/Areas/PreSchools/Views/PreSchoolProfileVerification/PreSchoolProfileVerification.cshtml");
            }
        }        

        public ActionResult EmailVerification(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    string resendEmailOTPYN = string.Empty, resendContactOTPYN = string.Empty, showHomeLinkYN = string.Empty;
                    int userTypeId = Convert.ToInt32(Session[SessionNames.UserTypeId]);
                    int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]);
                    Int64 organizationId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
                    int userId = (Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.UserId]);
                    int err = 0;

                    var data = new MPreSchoolProfileVerification().getOrganizationProfileVerificationData(ref err, userId, ref resendEmailOTPYN, ref resendContactOTPYN, ref showHomeLinkYN, userTypeId, organizationId, groupId, (TempData[TempDataNames.ProfileVerificationDataFetchMode] == null) ? ProfileVerificationDataFetchMode.New : TempData[TempDataNames.ProfileVerificationDataFetchMode].ToString());

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
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "SEG") //email send otp success
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                        ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "SER") //email resend otp success
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                        ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "SEC") //email otp confirmation success
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationSaveMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "FEG") //email send otp failure
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "FER") //email resend otp failure
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "FEC") //email otp confirmation failure
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationErrorMsg;
                    }

                    //CONFIGURATION FOR ACTIVE LINK HIGHLIGHT
                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString(); // A REPRESENT THE ANCHOR LINK
                    return View(data);
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
                MCommon.saveExceptionLog(ex.Message, "EmailVerification/View", "PreSchoolProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        /*
            * EMAIL-ID VERIFICATION WITH EMAIL OTP
            * GENERATE OTP AND SEND TO PRE-SCHOOL REGISTERED EMAIL-ID
            * USER CAN RESEND OTP (IF NOT GENERATED PROPERLY)
            * EMAIL-ID CONFIRM AND VERIFIED 
         */
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EmailVerification(VMProfileVerification data)
        {
            try
            {
                int status = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                string mode = data.OperationType;
                /*
                 * Modes
                 * -----                 
                 * EG : Email Get OTP
                 * ER : Email Resend OTP
                 * EC : Email Confirmation
                 * CG : Contact Get OTP
                 * CR : Contact Resend OTP
                 * CC : Contact Confirmation
                */
                if (mode != "EG" && mode != "ER" && mode != "EC")
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                string errDesc = string.Empty;
                data.VerificationFor = UserType.PRESCHOOL;

                int err = new MProfileVerification().saveProfileVerificationData(data, ref errDesc);

                //operation successful
                if (err == 0)
                {
                    TempData[TempDataNames.ProfileVerificationDataFetchMode] = ProfileVerificationDataFetchMode.Session;
                    if (mode == "EG")
                    {
                        TempData[TempDataNames.SaveStatus] = "SEG"; //EMAIL SEND OTP SUCCESS
                    }
                    else if (mode == "ER")
                    {
                        TempData[TempDataNames.SaveStatus] = "SER"; //EMAIL RESEND OTP SUCCESS
                    }
                    else if (mode == "EC")
                    {
                        TempData[TempDataNames.SaveStatus] = "SEC"; //EMAIL OTP CONFIRMATION SUCCESS
                    }
                    else
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }
                else if (err == -1) //operation error
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;

                    status = makeData(Session[SessionNames.URL].ToString());

                    if (status == 1) //SUCCESS FLAG AND SHOWING VIEW
                    {
                        return View();                        
                    }
                    else if (status == -1) //OPERATION ERROR
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                    else //CONFIGURE FOR NO VIEW PERMISSION
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }
                else if (err == -2) //validation falied
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = errDesc;

                    status = makeData(Session[SessionNames.URL].ToString());

                    if (status == 1) //SUCCESS FLAG AND SHOWING VIEW
                    {
                        return View();                        
                    }
                    else if (status == -1) //OPERATION ERROR
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                    else //CONFIGURE FOR NO VIEW PERMISSION
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }
                else //operation failed
                {
                    if (mode == "EG")
                    {
                        TempData[TempDataNames.SaveStatus] = "FEG"; //EMAIL SEND OTP FAILURE
                    }
                    else if (mode == "ER")
                    {
                        TempData[TempDataNames.SaveStatus] = "FER"; //EMAIL RESEND OTP FAILURE
                    }
                    else if (mode == "EC")
                    {
                        TempData[TempDataNames.SaveStatus] = "FEC"; //EMAIL OTP CONFIRMATION FAILURE
                    }
                    else
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }
                return RedirectToAction("EmailVerification", "PreSchoolProfileVerification", rvd);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "EmailVerification/Save", "PreSchoolProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

    }
}
