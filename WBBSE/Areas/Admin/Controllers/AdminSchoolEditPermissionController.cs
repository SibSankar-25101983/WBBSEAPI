using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net.Http;
using System.Web.Helpers;
using ViewModel;
using WBBSE.Models;
using WBBSE.Areas.Admin.Models;
using Microsoft.Security.Application;
using Common;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminSchoolEditPermissionController : Controller
    {
        /****************************************************************************
            * THIS CONTROLLER IS USED FOR ADMIN USER (WBBSE) EDIT PERMISSION. SUPPER ADMIN CAN SET UN-LOCK FOR EDIT PERMISSION OF ADMIN USER (WBBSE).
            * IF EDIT PERMISSION IS UN-LOCKED FOR ADMIN USER (WBBSE) THEN IT WILL BE LOCKED EDIT PERMISSION FOR SCHOOL USER/ SCHOOL ADMIN
            * makeData()-- IT IS USED FOR ROLE BASED PERMISSION LIKE ADD/EDIT/DELETE AND VIEW PERMISSION. IT IS ALSO USED DEFINE GRID VIEW COLUMNS. 
         ****************************************************************************/        
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

                    /******************************************************
                     * SET COLUMN NAME IN GRID VIEW
                     * columnList array information :-
                     * ----------------------------
                     * 0 : Database Field Name
                     * 1 : Column Name To Display In Grid
                     * 2 : Hidden : true/false
                    ******************************************************/
                    string columnList = string.Empty;
                    string[,] columns = new string[,] {
                        {"SlNo", "Sl No", "false", "80"},
                        {"SchoolId", "SchoolId", "true", "0"},
                        {"SchoolName", "School Name", "false", "550"},
                        {"Index", "Index", "true", "0"},
                        {"EditPermissionYN", "Edit Permission", "true", "0"},
                    };

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
                            columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "Y", "Y");

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.GridColumns] = columnList;

                            status = 1;
                        }
                    }
                    else
                    {
                        ViewData[ViewDataNames.AddYN] = "visible";
                        //columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "N");
                        columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "Y", "Y");
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminSchoolEditPermissionController");
                status = -1;
            }

            return status;
        }

        //CONFIGURED SUCCESS/ DELETE/ FAILD MESSAGE
        [HttpGet]
        public ActionResult SchoolEditPermission(string x)
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
                        ViewData[ViewDataNames.SaveInfo] = Message.SaveMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveDelete)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.DeleteMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
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

                    Session[SessionNames.URL] = x;
                    //MENU LINK IS ACTIVE THROUGH ROLEID
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
                MCommon.saveExceptionLog(ex.Message, "SchoolEditPermission/View", "AdminSchoolEditPermissionController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
        
        //GETTING SCHOOL LIST FROM THIS METHOD/FUNCTION
        [HttpGet]
        public JsonResult GetSchoolList(int? page, int? limit, string sortBy, string direction, string searchString = null, string searchType = null)
        {
            /********************             
            SEARCH TYPE
            -----------
            * N : BY SCHOOL NAME
            * I : BY INDEX NO            
            ********************/
            searchType = string.IsNullOrEmpty(searchType) ? SearchType.School.SchoolName : searchType;

            int total = 0;

            var records = new MMstSchool().getMstSchoolList(page, limit, sortBy, direction, searchString, ref total, searchType, SearchType.SchoolProfile.All); //All--> not locked/not UnLocked (NOT FILTERED BY LOCKED/UN-LOCKED FIELD)

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        //SHOWING/VIEW SCHOOL DETAILS BY PARTICULAR SCHOOL ID
        [HttpGet]
        public JsonResult GetSchoolView(string s)
        {            
            string View = string.Empty;
            try
            {
                View = new MMstSchool().getMstSchoolView(GblFunctions.Base64Decode(s)); /*SCHOOL ID DECRYPTED THROUGH BASE 64 DECODE METHOD*/
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolView", "AdminSchoolEditPermission");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        //SUPPER ADMIN CAN SET UN-LOCK EDIT PERMISSION FOR ADMIN USER OF WBBSE AND RETURN THE RESULT
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SchoolEditPermission(VMSchoolEditPermission data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                string errDesc = string.Empty;

                if (ModelState.IsValid)
                {
                    /*
                        * SUPPER ADMIN CAN UN-LOCK EDIT PERMISSION FOR ADMIN USER (WBBSE).
                        * AT THAT TIME, SCHOOL EDIT PERMISSION IS LOCKED. 
                        * SCHOOL PROFILE DATA CAN NOT MODIFY BY SCHOOL USER.
                        * SCHOOL PROFILE DATA CAN ONLY MODIFY BY WBBSE ADMIN USER.
                        * THIS IS PERFMORMED FOR A PARTICULAR SCHHOL AT A TIME (USING SCHHOL ID). 
                     */
                    err = new MMstSchool().unLoackPermission(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
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
                    return RedirectToAction("SchoolEditPermission", "AdminSchoolEditPermission", rvd);
                }
                else
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";

                    status = makeData(Session[SessionNames.URL].ToString());

                    if (status == 1)
                    {
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
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SchoolEditPermission/Un-Lock", "AdminSchoolEditPermissionController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //CHECK USER SUPPER ADMIN(WBBSE Authority) IS ALLOWED OR NOT FOR UN-LOCK PERMISSION (FOR SCHOOL PROFILE EDIT)
        [HttpGet]
        public JsonResult ChkEdit(string s)
        {
            string d = string.Empty;

            try
            {
                Int64 schoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(s).Trim()));
                
                if (Convert.ToInt16(Session[SessionNames.GroupId]) == 1) //User AdminNIC is all time allowed for UnLock edit permission of school profile
                {
                    d = "Y";
                }
                else
                {
                    d = new MMstSchool().chkSchoolEditPermission(schoolId, SchoolProfileEditFor.SuperAdmin); //Check User Supper Admin(WBBSE Authority) is allowed or not for UnLock permission
                }
                
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ChkEdit", "AdminSchoolEditPermissionController");
            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
    }
}
