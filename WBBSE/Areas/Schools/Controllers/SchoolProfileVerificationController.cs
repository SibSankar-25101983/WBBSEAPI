using System;
using System.Web;
using System.Web.Mvc;
using WBBSE.Models;
using WBBSE.Areas.Admin.Models;
using WBBSE.Areas.Schools.Models;
using ViewModel;
using Common;
using System.Web.Routing;

namespace WBBSE.Areas.Schools.Controllers
{
    /*******************************************************************************
    * A BRIEF HISTORY OF SchoolProfileVerificationController.
    * CONTAINS SCHOOL PROFILE VERIFICATION DATA RELATED ACTIONS 
    * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
    * ViewModel: THIS IS A PROJECT WHERE ALL VIEW MODEL CLASSES ARE WRITTEN AS CLASS FILE.
    * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
    * WBBSE.AREAS.SCHOOLS.MODELS: MODEL CLASS FOR SchoolHome RELATED BUSINESS LOGICS & FUNCTIONS. 
    ******************************************************************************/

    /* ROLE BASED ASP.NET MVC AUTHORIZATION [ONLY SCHOOL ROLE IS ALLOWED FOR ACCESSING THIS CONTROLLER]
     * NoCache: PREVENT CACHING IN MVC
     */
    [Authorize(Roles = UserType.SCHOOL), NoCache]
    public class SchoolProfileVerificationController : Controller
    {        
        public ActionResult SchoolProfileVerification() //DEFAULT ACTION OF THIS CONTROLLER
        {
            try
            {
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "Y") //IF ALREADY CHANGED THE PASSWORD THEN REDIRECT TO SCHOOL HOME PAGE VIEW
                {
                    return RedirectToActionPermanent("SchoolHome", "SchoolHome");
                }

                int err = 0;
                string resendEmailOTPYN = string.Empty, resendContactOTPYN = string.Empty, showHomeLinkYN = string.Empty;
                VMProfileVerification data = new VMProfileVerification();
                int userId = (Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.UserId]); //USER UNIQUE ID
                int userTypeId = Convert.ToInt32(Session[SessionNames.UserTypeId]); //USER TYPE UNIQUE ID
                int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]); // USER GROUP UNIQUE ID
                Int64 organizationId = Convert.ToInt64(Session[SessionNames.OrganizationId]); //SCHOOL UNIQUE ID

                /*CALLING THE MODEL FOR SCHOOL PROFILE VERIFICATION DATA*/
                data = new MSchoolProfileVerification().getOrganizationProfileVerificationData(ref err, userId, ref resendEmailOTPYN, ref resendContactOTPYN, ref showHomeLinkYN, userTypeId, organizationId, groupId, (TempData[TempDataNames.ProfileVerificationDataFetchMode] == null) ? ProfileVerificationDataFetchMode.New : TempData[TempDataNames.ProfileVerificationDataFetchMode].ToString());

                if (err == 0) //SUCCESS AND SETTING THE RESPECTIVE VIEW DATA
                {
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = resendEmailOTPYN;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = resendContactOTPYN;
                    ViewData[ViewDataNames.ShowHomeLinksYN] = showHomeLinkYN;
                }
                if (TempData[TempDataNames.SaveStatus] == null) //CONFIGURATION FOR ERROR & SUCCESS MESSAGE VISIBILITY
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SP") //PASSWORD SAVE SUCCESS
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.SchoolProfileVerificationDataSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEG") //EMAIL SEND OTP SUCCESS
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SER") //EMAIL RESEND OTP SUCCESS
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                    ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SEC") //EMAIL OTP CONFIRMATION SUCCESS
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCG") //CONTACT SEND OTP SUCCESS
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSent;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCR") //CONTACT RESEND OTP SUCCESS
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSent;
                    ViewData[ViewDataNames.ContactOTPVisibleYN] = "Y";
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "SCC") //CONTACT OTP CONFIRMATION SUCCESS
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationSaveMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FP") //PASSWORD SAVE FAILURE
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FEG") //EMAIL SEND OTP FAILURE
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FER") //EMAIL RESEND OTP FAILURE
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FEC") //EMAIL OTP CONFIRMATION FAILURE
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationErrorMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCG") //CONTACT SEND OTP FAILURE
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCR") //CONTACT RESEND OTP FAILURE
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationOTPSentFailed;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == "FCC") //CONTACT OTP CONFIRMATION FAILURE
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.ContactNoVerificationErrorMsg;
                }
                //SHOW HOME BUTTON LINK
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "Y" && (Session[SessionNames.EmailVerifiedYN].ToString() == "Y" || Session[SessionNames.ContactNoVerifiedYN].ToString() == "Y"))
                {
                    ViewData[ViewDataNames.ShowHomeLinksYN] = "Y";
                }
                return View(data);
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "SchoolProfileVerification/View", "SchoolProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSalutationList() //AJAX CALL FOR SALUTATION LIST GENERATION
        {
            var records = new MCommon().getMstSalutationListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChkDuplicateContact(string e, string p, string m) //AJAX CALL FOR DUPLICATE ENTRY (e=EMAIL, p=PHONE NO., m=MOBILE NO.) CHECKING
        {
            int err = 0;
            string errDesc = string.Empty;
            Int64 schoolId = 0;

            try
            {
                schoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "ChkDuplicateContact", "SchoolProfileVerificationController");
            }

            /*MODEL CALLING FOR CHECK*/
            err = new MMstSchool().chkDuplicateContactMstSchool((e ?? string.Empty), (p ?? string.Empty), (m ?? string.Empty), schoolId.ToString(), EntType.EDIT, ref errDesc, string.Empty, string.Empty, DefaultSetting.DefaultValEnc);

            return Json(new { err, errDesc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SchoolProfileVerification(VMProfileVerification vmpv)
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
                    return View("~/Areas/Schools/Views/SchoolProfileVerification/SchoolProfileVerification.cshtml");
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
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.InvalidRequest;
                    return View("~/Areas/Schools/Views/SchoolProfileVerification/SchoolProfileVerification.cshtml");
                }
                /*******************/
                string errDesc = string.Empty;
                MProfileVerification mpv = new MProfileVerification();
                vmpv.VerificationFor = UserType.SCHOOL;

                int err = mpv.saveProfileVerificationData(vmpv, ref errDesc);

                if (err == 0)
                {
                    if (mode == "P")
                    {
                        RouteValueDictionary rvd = new RouteValueDictionary();
                        rvd.Add("m",SchoolHomeMessage.ProfileVerficationSuccess);

                        //TempData[TempDataNames.SaveStatus] = "SP";
                        return RedirectToAction("SchoolHome", "SchoolHome", new { m = GblFunctions.encryptPassword(SchoolHomeMessage.ProfileVerficationSuccess), area = "Schools" });
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
                    return View("~/Areas/Schools/Views/SchoolProfileVerification/SchoolProfileVerification.cshtml");
                }
                else if (err == -2)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = errDesc;
                    return View("~/Areas/Schools/Views/SchoolProfileVerification/SchoolProfileVerification.cshtml");
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
                return RedirectToAction("SchoolProfileVerification", "SchoolProfileVerification");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SchoolProfileVerification/Save", "SchoolProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        /* makeData()-- IT IS USED FOR ROLE BASED PERMISSION LIKE ADD/EDIT/DELETE AND VIEW PERMISSION. IT IS ALSO USED DEFINE GRID VIEW COLUMNS. 
        * PARAM1: encRoleId= UNIQUE ROLE ID (IN ENCRYPTED FORM) OF MASTER ROLE TABLE.         
        */
        private int makeData(string encRoleId)
        {
            int status = 0;

            try
            {
                int groupId = (Session[SessionNames.GroupId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.GroupId]);
                if (string.IsNullOrEmpty(encRoleId))
                {
                    status = 0; //CONFIGURE FOR UN-AUTHENTICATED USER ACCESS
                }
                else
                {
                    int roleId = Convert.ToInt32(GblFunctions.decryptPassword(encRoleId));
                    Session[SessionNames.RoleId] = roleId;

                    string ViewYN = string.Empty, AddYN = string.Empty, EditYN = string.Empty, DeleteYN = string.Empty, ReportYN = string.Empty, SystemYN = string.Empty;

                    if (groupId != 1 && groupId != 2) //IF NOT SUREP USER (1 & 2) THEN EXECUTE BELOW CODE BLOCK 
                    {
                        MUserPermission mu = new MUserPermission();

                        // ROLE BASED PERMISSION CHECKING FOR SPECIFIC ROLE ID
                        mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                        if (ViewYN == "N")
                        {
                            status = 0; //CONFIGURE FOR UN-AUTHENTICATED USER ACCESS
                        }
                        else
                        {
                            status = 1; //SUCCESS FLAG 1
                        }
                    }
                    else
                    {
                        status = 1; //SUCCESS FLAG 1
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "SchoolProfileVerificationController");
                status = -1;
            }

            return status;
        }

        public ActionResult EmailVerification(string x) //FUNCTION FOR EMAIL VERIFICATION, x=ENCRYPTED ROLE ID
        {
            try
            {
                int status = makeData(x); //PERMISSION CHECK AND GETTING DATA

                if (status == 1) //1 MEANS SUCCESS RETURN FROM MAKEDATA FUCTION
                {
                    string resendEmailOTPYN = string.Empty, resendContactOTPYN = string.Empty, showHomeLinkYN = string.Empty;
                    int userTypeId = Convert.ToInt32(Session[SessionNames.UserTypeId]);
                    int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]);
                    Int64 organizationId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
                    int userId = (Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.UserId]);
                    int err = 0;

                    //MODEL DATA FETCHING FOR PROFILE VERIFICATION
                    var data = new MSchoolProfileVerification().getOrganizationProfileVerificationData(ref err, userId, ref resendEmailOTPYN, ref resendContactOTPYN, ref showHomeLinkYN, userTypeId, organizationId, groupId, (TempData[TempDataNames.ProfileVerificationDataFetchMode] == null) ? ProfileVerificationDataFetchMode.New : TempData[TempDataNames.ProfileVerificationDataFetchMode].ToString());

                    if (err == 0) //0 MEANS RETURN SUCCESS FROM MODEL & SETTING VIEW DATA FOR PAGE VIEW
                    {

                        ViewData[ViewDataNames.EmailOTPVisibleYN] = resendEmailOTPYN;
                        ViewData[ViewDataNames.ContactOTPVisibleYN] = resendContactOTPYN;
                        ViewData[ViewDataNames.ShowHomeLinksYN] = showHomeLinkYN;
                    }
                    if (TempData[TempDataNames.SaveStatus] == null) //MESSAGE CONFIGURATION IF NO MESSAGE TO DISPLAY, "null"
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "SEG") //EMAIL SEND OTP SUCCESS
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                        ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "SER") //EMAIL RESEND OTP SUCCESS
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSent;
                        ViewData[ViewDataNames.EmailOTPVisibleYN] = "Y";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "SEC") //EMAIL OTP CONFIRMATION SUCCESS
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationSaveMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "FEG") //EMAIL SEND OTP FAILURE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "FER") //EMAIL RESEND OTP FAILURE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ProfileVerification.EmailVerificationOTPSentFailed;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == "FEC") //EMAIL OTP CONFIRMATION FAILURE
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
                else if (status == -1) //-1 MEANS ERROR GENERATED
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                else //CONFIGURATION FOR NO VIEW PERMISSION
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "EmailVerification/View", "SchoolProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EmailVerification(VMProfileVerification data) //FUNCTION FOR EMAIL VERIFICATION DATA POST, data=VIEW MODEL FOR PROFILE VERIFICATION DATA
        {
            try
            {
                int status = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]); //ROUTE VALUE SETTING, x=ENCRYPTED ROLE ID
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString(); //ACTIVE ANCHOR LINK SETTING
                string mode = data.OperationType;
                /*
                 * MODES
                 * -----
                 * P : NEW PASSWORD CHANGE
                 * EG : EMAIL GET OTP
                 * ER : EMAIL RESEND OTP
                 * EC : EMAIL CONFIRMATION
                 * CG : CONTACT GET OTP
                 * CR : CONTACT RESEND OTP
                 * CC : CONTACT CONFIRMATION
                */
                if (mode != "EG" && mode != "ER" && mode != "EC")
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                string errDesc = string.Empty;
                data.VerificationFor = UserType.SCHOOL;

                //MODEL CALLING FOR SAVING THE PROFILE VERIFICATION DATA
                int err = new MProfileVerification().saveProfileVerificationData(data, ref errDesc);

                //OPERATION SUCCESSFUL
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
                        return Redirect("~/Error/Unexpected.html"); //ERROR PAGE
                    }
                }
                else if (err == -1) //OPERATION ERROR
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.OperationError;

                    status = makeData(Session[SessionNames.URL].ToString());

                    if (status == 1) //SUCCESS FLAG AND SHOWING VIEW
                    {
                        return View();
                        //return View("~/Areas/Schools/Views/SchoolProfileVerification/EmailVerification.cshtml");
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
                else if (err == -2) //VALIDATION FALIED
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = errDesc;

                    status = makeData(Session[SessionNames.URL].ToString());

                    if (status == 1) //SUCCESS FLAG AND SHOWING VIEW
                    {
                        return View();
                        //return View("~/Areas/Schools/Views/SchoolProfileVerification/EmailVerification.cshtml");
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
                else //OPERATION FAILED
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
                return RedirectToAction("EmailVerification", "SchoolProfileVerification", rvd);
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "EmailVerification/Save", "SchoolProfileVerificationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
