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
using WBBSE.Areas.PreSchools.Models;
using ViewModel;
using System.Text.RegularExpressions;
using Common;
using Microsoft.Security.Application;

namespace WBBSE.Areas.PreSchools.Controllers
{
    [Authorize(Roles = UserType.PRESCHOOL), NoCache]
    public class PreSchoolProfileController : Controller
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

                    /******************************************************
                     * columnList array information :-
                     * ----------------------------
                     * 0 : Database Field Name
                     * 1 : Column Name To Display In Grid
                     * 2 : Hidden : true/false
                    ******************************************************/
                    string columnList = string.Empty;
                    string[,] columns = new string[,] {
                        {"PreSchoolId", "PreSchoolId", "true", "0"},
                        {"SchoolCode", "SchoolCode", "true", "0"},
                        {"DISECode", "DISE Code", "false", "123"},
                        {"SchoolName", "Junior School Name", "false", "480"},
                        //{"SubDivisionId", "SubDivisionId", "true", "0"},
                        //{"SubDivisionName", "Sub-Division", "false", "0"},
                        //{"DistrictName", "District", "false", "0"},
                        {"ZoneName", "Zone", "false", "0"},
                        {"Designation", "Designation", "true", "0"},
                        {"PhoneNo", "Contact No", "false", "0"},
                        {"PreSchoolEditPermissionStatus", "Status", "false", "0"},
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
                            //check whether pre-school edit permission is locked or not
                            if (EditYN == "Y")
                            {
                                Int64 preSchoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
                                string editFor = SchoolProfileEditFor.Others;

                                EditYN = new MPreSchoolProfile().chkPreSchoolEditPermission(preSchoolId, editFor);
                            }

                            columnList = new GblFunctions().makeGridColumns(columns, EditYN, "N", "Y", "Y"); // FOR PRE-SCHOOL DATA GRID

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden"; //ADD NEW BUTTON VISIBILITY CONFIGURATION
                            ViewData[ViewDataNames.GridColumns] = columnList;

                            status = 1;
                        }
                    }
                    else
                    {
                        columnList = new GblFunctions().makeGridColumns(columns, "Y", "N", "Y", "Y");

                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "PreSchoolProfileController");
                status = -1;
            }

            return status;
        }

        public ActionResult PreSchoolProfile(string x)
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
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "PreSchoolProfile/View", "PreSchoolProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetPreSchool(int? page, int? limit, string sortBy, string direction)
        {
            int total = 0;
            Int64 preSchoolId = 0;

            try
            {
                preSchoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetPreSchoolList", "PreSchoolProfileController");
            }

            var records = new MMstPreSchool().getMstPreSchoolList(page, limit, sortBy, direction, preSchoolId.ToString(), ref total, SearchType.PreSchool.PreSchoolId, SearchType.SchoolProfile.All); //All--> not locked/not UnLocked (NOT FILTERED BY LOCKED/UN-LOCKED FIELD)

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolCategoryList()
        {
            var records = new MSchoolParameters().geMstSchoolCategoryListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolTypeList()
        {
            var records = new MSchoolParameters().getMstSchoolTypeListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolStatusList()
        {
            var records = new MSchoolParameters().getMstSchoolStatusListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolMediumList()
        {
            var records = new MSchoolParameters().getMstSchoolMediumListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolRecognitionList()
        {
            var records = new MSchoolParameters().getMstSchoolRecognitionListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolManagementList()
        {
            var records = new MSchoolParameters().getMstSchoolManagementListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDesignationList()
        {
            var records = new MCommon().getMstDesignationListDropDown(UserType.SCHOOL);

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChkDuplicateContact(string e, string p, string m, string dc) //AJAX CALL FOR DUPLICATE ENTRY (e=EMAIL, p=PHONE NO., m=MOBILE NO., dc=DISE CODE) CHECKING
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
                MCommon.saveExceptionLog(ex.Message, "ChkDuplicateContact", "PreSchoolProfileController");
            }

            /*MODEL CALLING FOR CHECK*/
            err = new MMstPreSchool().chkDuplicateContactMstPreSchool((e ?? string.Empty), (p ?? string.Empty), (m ?? string.Empty), preSchoolId.ToString(), EntType.EDIT, ref errDesc, (dc ?? string.Empty));

            return Json(new { err, errDesc }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPreSchoolView(string s)
        {
            string View = string.Empty;

            try
            {
                View = new MMstPreSchool().getMstPreSchoolView(GblFunctions.Base64Decode(s));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetPreSchoolView", "PreSchoolProfileController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPreSchoolDetails(string s)
        {
            var Records = new MMstPreSchool().getMstPreSchoolDetails(GblFunctions.Base64Decode(s));

            return Json(new { Records }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PreSchoolProfile(VMPreSchoolProfile data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                string errDesc = string.Empty;

                string mode = Sanitizer.GetSafeHtmlFragment(data.EntType ?? Mode.ERROR).Trim();                
                if (mode != EntType.ADD && mode != EntType.EDIT && mode != EntType.DELETE && mode != EntType.LOCK)
                {
                    mode = Mode.ERROR;
                }

                if (mode == Mode.ERROR)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    return RedirectToAction("PreSchoolProfile", "PreSchoolProfile", rvd);
                }

                if (ModelState.IsValid)
                {
                    err = new MPreSchoolProfile().updatePreSchoolProfile(data, ref errDesc);

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
                    return RedirectToAction("PreSchoolProfile", "PreSchoolProfile", rvd);
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
                MCommon.saveExceptionLog(ex.Message, "PreSchoolProfile/Save", "PreSchoolProfileController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
