using System;
using System.Web.Mvc;
using WBBSE.Areas.Admin.Models;
using System.Web.Routing;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using System.Text;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminPostPublicationApplicationController : Controller
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
                        {"SlNo", "Sl No", "false", "100"},
                        {"RollNo", "Roll No", "false", "200"},
                        {"StudentName", "Name", "false", "600"},
                        {"ScrutinyType", "Type", "false", "120"}
                    };

                    if (groupId != 1 && groupId != 2)
                    {
                        MUserPermission mu = new MUserPermission();

                        mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                        if (ViewYN == "N")
                        {
                            status = 0;
                        }
                        else
                        {
                            columnList = new GblFunctions().makeGridColumns(columns, EditYN, DeleteYN, "Y", "Y");

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.ReportYN] = (ReportYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.GridColumns] = columnList;

                            status = 1;
                        }
                    }
                    else
                    {
                        if (groupId == 1)
                        {
                            columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "Y", "Y");
                        }
                        else
                        {
                            columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "Y", "Y");
                        }

                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.ReportYN] = "visible";
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminPostPublicationApplicationController");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult FormData(string x)
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
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    return View();
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "FormData/View", "AdminPostPublicationApplicationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetApplicationData(int? page, int? limit, string sortBy, string direction, string searchString = null, string searchType = null)
        {
            /********************
            SEARCH TYPE
            -----------
            * 1 : BY ROLL NO
            * 2 : BY CANDIDATE NAME
            * 3 : BY SCHOOL NAME
            ********************/
            int total = 0;

            searchType = string.IsNullOrEmpty(searchType) ? SearchType.PostPublication.RollNo : searchType;

            var records = new MAdminPostPublication().getAdminCandididatePPRPPSList(page, limit, sortBy, direction, searchString, ref total, searchType);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolList(string s)
        {
            var records = new MMstSchool().getMstSchoolListAutoComplete(Sanitizer.GetSafeHtmlFragment(s));

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetApplicationDetails(string r)
        {
            var View = new MAdminPostPublication().getAdminCandididatePPRPPSView(r);

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetScrutinyType()
        {
            var records = new MAdminPostPublication().getScrutinyTypeListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }
    }
}
