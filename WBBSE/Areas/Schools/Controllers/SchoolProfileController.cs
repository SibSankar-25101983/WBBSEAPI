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
using WBBSE.Areas.Admin.Models;
using WBBSE.Areas.Schools.Models;
using ViewModel;
using System.Text.RegularExpressions;
using Common;
using Microsoft.Security.Application;

namespace WBBSE.Areas.Schools.Controllers
{
    /*******************************************************************************
     * A BRIEF HISTORY OF SchoolProfileController.
     * CONTAINS SCHOOL PROFILE DATA RELATED ACTIONS 
     * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
     * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
     * WBBSE.AREAS.SCHOOLS.MODELS: MODEL CLASS FOR SchoolHome RELATED BUSINESS LOGICS & FUNCTIONS. 
     ******************************************************************************/

    /* ROLE BASED ASP.NET MVC AUTHORIZATION [ONLY SCHOOL ROLE IS ALLOWED FOR ACCESSING THIS CONTROLLER]
     * NoCache: PREVENT CACHING IN MVC
     */
    [Authorize(Roles = UserType.SCHOOL), NoCache]
    public class SchoolProfileController : Controller
    {
        /* makeData()-- IT IS USED FOR ROLE BASED PERMISSION LIKE ADD/EDIT/DELETE AND VIEW PERMISSION. IT IS ALSO USED DEFINE GRID VIEW COLUMNS. 
        * PARAM1: encRoleId= UNIQUE ROLE ID (IN ENCRYPTED FORM) OF MASTER ROLE TABLE.         
        */
        private int makeData(string encRoleId)
        {
            int status = 0;

            try
            {
                int groupId = (Session[SessionNames.GroupId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.GroupId]); // groupId::USER GROUP UNIQUE ID
                if (string.IsNullOrEmpty(encRoleId))
                {
                    status = 0;
                }
                else
                {
                    int roleId = Convert.ToInt32(GblFunctions.decryptPassword(encRoleId));
                    Session[SessionNames.RoleId] = roleId;

                    string ViewYN = string.Empty, AddYN = string.Empty, EditYN = string.Empty, DeleteYN = string.Empty, ReportYN = string.Empty, SystemYN = string.Empty;

                    /******************************************************
                     * COLUMNLIST ARRAY INFORMATION :-
                     * ----------------------------
                     * 0 : DATABASE FIELD NAME
                     * 1 : COLUMN NAME TO DISPLAY IN GRID
                     * 2 : HIDDEN : TRUE/FALSE
                    ******************************************************/
                    string columnList = string.Empty;
                    string[,] columns = new string[,] {
                        {"SchoolId", "SchoolId", "true", "0"},
                        {"SchoolName", "School Name", "false", "400"},
                        {"SubDivisionId", "SubDivisionId", "true", "0"},
                        {"SubDivisionName", "Sub-Division", "false", "0"},
                        {"DistrictName", "District", "false", "0"},
                        {"ZoneName", "Zone", "false", "0"},
                        {"SchoolEditPermissionStatus", "Status", "false", "0"},
                    };

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
                            //CHECK WHETHER SCHOOL EDIT PERMISSION IS LOCKED OR NOT
                            if (EditYN == "Y")
                            {
                                Int64 schoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
                                string editFor = SchoolProfileEditFor.Others;

                                
                                EditYN = new MSchoolProfile().chkSchoolEditPermission(schoolId, editFor);
                            }

                            columnList = new GblFunctions().makeGridColumns(columns, EditYN, "N", "Y", "Y"); // FOR SCHOOL DATA GRID

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden"; //ADD NEW BUTTON VISIBILITY CONFIGURATION
                            ViewData[ViewDataNames.GridColumns] = columnList;

                            status = 1;
                        }
                    }
                    else
                    {
                        columnList = new GblFunctions().makeGridColumns(columns, "Y", "N", "Y", "Y"); // FOR SCHOOL DATA GRID

                        ViewData[ViewDataNames.AddYN] = "visible"; //ADD NEW BUTTON VISIBILITY CONFIGURATION
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "makeData", "SchoolProfileController");
                status = -1;
            }

            return status;
        }

        public ActionResult SchoolProfile(string x) //DEFAULT ACTION, x=ENCRYPTED ROLE ID
        {
            try
            {
                int status = makeData(x);

                if (status == 1) //IF SUCCESS STATUS=1
                {
                    if (TempData[TempDataNames.SaveStatus] == null) //NO MESSAGE PASSING SO ERROR IS NOT VISIBLE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveSuccess) //SHOWING SAVE SUCCESS MESSAGE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.SaveMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveDelete) //SHOWING DELETE SUCCESS MESSAGE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.DeleteMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed) //SHOWING FAILED MESSAGE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
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

                    //SETTING ACTIVE LINK AT LEFT HAND SIDE MENU HIGHLIGHT
                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    return View();
                }
                else if (status == -1) //NO VIEW WHEN ERROR IS GENERATED
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
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "SchoolProfile/View", "SchoolProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSchool(int? page, int? limit, string sortBy, string direction) //AJAX CALL FOR SCHOOL LIST GENERATION
        {
            int total = 0;
            Int64 schoolId = 0;

            try
            {
                schoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]); //SCHOOL UNIQUE ID
            }
            catch(Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "GetSchoolList", "SchoolProfileController");
            }

            var records = new MMstSchool().getMstSchoolList(page, limit, sortBy, direction, schoolId.ToString(), ref total, SearchType.School.SchoolId, SearchType.SchoolProfile.All); //All--> not locked/not UnLocked (NOT FILTERED BY LOCKED/UN-LOCKED FIELD)

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolCategoryList() //AJAX CALL FOR SCHOOL CATEGORY LIST GENERATION
        {
            var records = new MSchoolParameters().geMstSchoolCategoryListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolTypeList() //AJAX CALL FOR SCHOOL TYPE LIST GENERATION
        {
            var records = new MSchoolParameters().getMstSchoolTypeListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolStatusList() //AJAX CALL FOR SCHOOL STATUS LIST GENERATION
        {
            var records = new MSchoolParameters().getMstSchoolStatusListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolMediumList() //AJAX CALL FOR SCHOOL MEDIUM LIST GENERATION
        {
            var records = new MSchoolParameters().getMstSchoolMediumListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolRecognitionList() //AJAX CALL FOR SCHOOL RECOGNITION LIST GENERATION
        {
            var records = new MSchoolParameters().getMstSchoolRecognitionListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolManagementList() //AJAX CALL FOR SCHOOL MANAGEMENT LIST GENERATION
        {
            var records = new MSchoolParameters().getMstSchoolManagementListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDesignationList() //AJAX CALL FOR DESIGNATION MASTER LIST GENERATION
        {
            var records = new MCommon().getMstDesignationListDropDown(UserType.SCHOOL);

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChkDuplicateContact(string e, string p, string m, string dc) //AJAX CALL FOR DUPLICATE ENTRY (e=EMAIL, p=PHONE NO., m=MOBILE NO., dc=DISE CODE) CHECKING
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
                MCommon.saveExceptionLog(ex.Message, "ChkDuplicateContact", "SchoolProfileController");
            }

            /*MODEL CALLING FOR CHECK*/
            err = new MMstSchool().chkDuplicateContactMstSchool((e ?? string.Empty), (p ?? string.Empty), (m ?? string.Empty), schoolId.ToString(), EntType.EDIT, ref errDesc, (dc ?? string.Empty), string.Empty, DefaultSetting.DefaultValEnc);

            return Json(new { err, errDesc }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolView(string s) //AJAX CALL FOR SCHOOL VIEW GENERATION, s=BASE 64 ENCODED SCHOOL UNIQUE ID
        {
            string View = string.Empty;

            try
            {
                /*CALLING MODEL WITH BASE 64 DECODED SCHOOL UNIQUE ID */
                View = new MMstSchool().getMstSchoolView(GblFunctions.Base64Decode(s));
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "GetSchoolView", "AdminSchoolController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolDetails(string s) //AJAX CALL FOR SCHOOL DETAILS GENERATION, s=BASE 64 ENCODED SCHOOL UNIQUE ID
        {
            /*CALLING MODEL WITH BASE 64 DECODED SCHOOL UNIQUE ID */
            var Records = new MMstSchool().getMstSchoolDetails(GblFunctions.Base64Decode(s));

            return Json(new { Records }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SchoolProfile(VMSchoolProfile data) //HTTP POST FOR SAVING SCHOOL PROFILE DATA, data= VIEW MODEL OF SCHOOL PROFILE
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]); //PASSING ROUTE VALUE, UNIQUE ENCRYPTED ROLE ID
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString(); //ACTIVE LINK HIGHLIGHT SETTING
                string errDesc = string.Empty;
                string mode = Sanitizer.GetSafeHtmlFragment(data.EntType ?? Mode.ERROR).Trim(); //((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode != EntType.ADD && mode != EntType.EDIT && mode != EntType.DELETE && mode != EntType.LOCK) //CONFIGURATION FOR SECURITY ERROR HANDLING AND SEND TO ERROR PAGE
                {
                    mode = Mode.ERROR;
                }
                if (mode == Mode.ERROR) //SEND TO ERROR VIEW
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    return RedirectToAction("SchoolProfile", "SchoolProfile", rvd);
                }

                if (ModelState.IsValid) //MODEL VALIDATION AS PER VIEW MODEL DATA VALIDATION
                {
                    Int64 schoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
                    data.SchoolId = GblFunctions.Base64Encode(schoolId.ToString());

                    err = new MSchoolProfile().updateSchoolProfile(data, ref errDesc);

                    if (err == 0) //0 MEANS NO ERROR
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess; //SUCCESS MESSAGE CONFIGURATION
                    }
                    else if (err == 2) //2 MEANS CUSTOM ERROR HANDELING FOR DATA VALIDATION RELATED ERROR
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = errDesc;
                    }
                    else //FAILED TO PERFORM THE ACTION
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }
                    return RedirectToAction("SchoolProfile", "SchoolProfile", rvd); //REDIRECTION TO VIEW
                }
                else
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";

                    status = makeData(Session[SessionNames.URL].ToString()); //PERMISSION CHECKING AND DATA MAKING

                    if (status == 1) //1 MEANS SUCCESS AND RETURN TO VIEW
                    {
                        return View();
                    }
                    else if (status == -1) //-1 MEANS ERROR
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                    else //CONFIGURE FOR NO VIEW PERMISSION
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "SchoolProfile/Save", "SchoolProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
