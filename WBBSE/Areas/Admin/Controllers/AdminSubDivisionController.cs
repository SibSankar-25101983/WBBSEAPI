using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using WBBSE.Models;
using ViewModel;
using Common;
using Microsoft.Security.Application;
using WBBSE.Areas.Admin.Models;
using System.Net.Http;
using System.Web.Helpers;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminSubDivisionController : Controller
    {
        /*
         * THIS CONTROLLER IS USED TO ADD/EDIT/DELETE SUB-DIVISION NAME (MASTER DATA)
         * USER CAN SEARCH PARTICULAR SUB-DIVISION NAME THROUGH SEARCH CONTROL
         * makeData()-- IT IS USED FOR ROLE BASED PERMISSION LIKE ADD/EDIT/DELETE AND VIEW PERMISSION. IT IS ALSO USED TO DEFINE GRID VIEW COLUMNS. HERE ALSO LOAD SUB-DIVISION NAME
         */
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
                        {"SlNo", "Sl No", "false", "90"},
                        {"SubDivisionId", "SubDivision Id", "true", "0"},
                        {"SubDivisionName", "SubDivision Name", "false", "300"},
                        {"DistrictId", "District Id", "true", "0"},
                        {"DistrictName", "District Name", "false", "300"},
                        {"IndexInitial", "Index Initial", "false", "130"},
                        {"MaxIndexNo", "Max Index No", "false", "150"},
                        {"MigYN", "MigYN", "true", "0"}
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
                            columnList = new GblFunctions().makeGridColumns(columns, EditYN, DeleteYN);

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.ReportYN] = (ReportYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.GridColumns] = columnList;
                            status = 1;
                        }
                    }
                    else
                    {
                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.ReportYN] = "visible";
                        columnList = new GblFunctions().makeGridColumns(columns, "Y", "Y", "Y");
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminSubDivisionController");
                status = -1;
            }

            return status;
        }

        //CONFIGURED SUCCESS/ DELETE/ FAILD MESSAGE
        public ActionResult SubDivisions(string x)
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
                MCommon.saveExceptionLog(ex.Message, "SubDivisions/View", "AdminSubDivisionController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //POPULATE DROPDOWN LIST FOR DISTRICT NAME
        [HttpGet]
        public JsonResult GetDistrictList()
        {
            var records = new MMstDistrict().getMstDistrictListDropDown();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        //GETTING SUB-DIVISION LIST FROM THIS METHOD/FUNCTION. POPULATE IN GRIDVIEW (SUB-DIVISION LIST IN GRIDVIEW)
        public JsonResult GetSubDivisionList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;

            var records = new MMstSubDivision().getMstSubDivisionList(page, limit, sortBy, direction, searchString, ref total);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        //DATA ADD/EDIT/DELETE METHOD
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SubDivisions(VMMstSubDivision data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                string errDesc = string.Empty;
                string mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    return RedirectToAction("SubDivisions", "AdminSubDivision", rvd);
                }

                if (mode == Mode.DELETE) //if mode = delete, bypass model state checking
                {
                    string tableName = "MstSchool";
                    string masterTableName = "MstSubDivision";
                    string fieldName = "SubDivisionId";
                    string fieldValue = GblFunctions.Base64Decode(data.SubDivisionId);

                    //Check Sub-Division name already mapped with School or not? If Sub-Division name is already mapped with School then user can not deleted the particular Sub-Division
                    err = new MCommon().chkDelete(tableName, fieldName, fieldValue);

                    if (err > 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = Message.SubDivision.SubDivisionDeleteNotPermitted;
                        return RedirectToAction("SubDivisions", "AdminSubDivision", rvd);
                    }

                    //migration check
                    err = new MCommon().chkMigration(masterTableName, fieldName, fieldValue);

                    if (err > 0) //If block name is mapped with Circle then delete not permitted
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = Message.SubDivision.SubDivisionDeleteNotPermittedMigration;
                        return RedirectToAction("SubDivisions", "AdminSubDivision", rvd);
                    }

                    err = new MMstSubDivision().saveMstSubDivision(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }
                    return RedirectToAction("SubDivisions", "AdminSubDivision", rvd);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        err = new MMstSubDivision().saveMstSubDivision(data, ref errDesc);

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
                        return RedirectToAction("SubDivisions", "AdminSubDivision", rvd);
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
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SubDivisions/Save", "AdminSubDivisionController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        /*AT SUB-DIVISION DELETING TIME, IT CHECK SUB-DIVISION NAME ALREADY MAPPED WITH SCHOOL OR NOT? IF SUB-DIVISION NAME IS MAPPED WITH SCHOOL THEN USER CAN NOT DELETED THE PARTICULAR SUB-DIVISION
        * THIS METHOD IS CALLED FROM CLIENT SIDE
        */
        [HttpGet]
        public JsonResult ChkSubDivisionDelete(string i)
        {
            int err = 0;
            try
            {
                string tableName = "MstSchool";
                string fieldName = "SubDivisionId";
                string fieldValue = GblFunctions.Base64Decode(i);

                err = new MCommon().chkDelete(tableName, fieldName, fieldValue);
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "ChkSubDivisionDelete", "AdminSubDivisionController");
            }
            return Json(err, JsonRequestBehavior.AllowGet);
        }

        //IF SUB-DIVISION IS TRANSFERED THEN USER CAN NOT CHANGED/MODIFID THE DISTRICT NAME.
        [HttpGet]
        public JsonResult ChkTransfer(string i, string cd, string od)
        {
            int err = 0;
            try
            {
                string tableName = "SubDivisionTransfer";
                string fieldName = "SubDivisionId";
                string fieldValue = GblFunctions.Base64Decode(i);
                string changedDistrictId = GblFunctions.Base64Decode(cd); //New District Id
                string oldDistrictId = GblFunctions.Base64Decode(od); //Old District Id
                if (changedDistrictId != oldDistrictId)
                {
                    err = new MCommon().chkTransfer(tableName, fieldName, fieldValue);
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "ChkTransfer", "AdminSubDivisionController");
            }
            return Json(err, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DownloadReport()
        {
            try
            {
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);

                int total = 0;
                string reportFormat = Request.Form["ReportType"].ToString();
                if (reportFormat != ReportFormat.Excel && reportFormat != ReportFormat.Pdf)
                {
                    return Redirect("~/Error/Unexpected.html");
                }

                if (reportFormat == ReportFormat.Excel)
                {
                    DataTable dt = new MMstSubDivision().DownloadMstSubDivisionList(1, MaxFileSize.RecordCount, null, null, string.Empty, ref total, Convert.ToInt32(reportFormat));

                    if (dt != null)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            int i = 1;
                            var ws = wb.Worksheets.Add("SubDivision List");
                            ws.Cell(1, 1).Value = "SubDivision List (Report Generated on : " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt") + ")";
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                            ws.Cell(2, 1).Value = "Total Records : " + total.ToString();
                            ws.Cell(2, 1).Style.Font.Bold = true;
                            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Range(ws.Cell(2, 1).Address, ws.Cell(2, dt.Columns.Count).Address).Row(1).Merge();

                            foreach (DataColumn dtcol in dt.Columns)
                            {
                                ws.Cell(3, i).Value = dtcol.ColumnName;
                                i++;
                            }
                            ws.Range(ws.Cell(3, 1).Address, ws.Cell(3, dt.Columns.Count).Address).SetAutoFilter();
                            ws.Range(ws.Cell(3, 1).Address, ws.Cell(3, dt.Columns.Count).Address).Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1);
                            ws.Range(ws.Cell(3, 1).Address, ws.Cell(3, dt.Columns.Count).Address).Style.Font.FontColor = XLColor.White;
                            i = 4;
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    ws.Cell(i, j + 1).Value = dtrow[j];
                                }
                                i++;
                            }

                            ws.SheetView.FreezeRows(3); //freeze rows
                            ws.Columns().AdjustToContents(); //adjust column according to data
                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb.SaveAs(stream);
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SubDivision List.xlsx");
                            }
                        }
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ReportGenerationFailed;

                        int status = makeData(Session[SessionNames.URL].ToString());

                        if (status == 1)
                        {
                            return View("~/Areas/Admin/Views/AdminSubDivision/SubDivisions.cshtml");
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
                else if (reportFormat == ReportFormat.Pdf) //report will be downloaded from a different page
                {
                    return RedirectToAction("SubDivisions", "AdminSubDivision", rvd);
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "DownloadReport", "AdminSubDivisionController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

    }
}
