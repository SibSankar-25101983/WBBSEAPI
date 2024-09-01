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
    public class AdminSchoolController : Controller
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
                        {"SlNo", "Sl No", "false", "0"},
                        {"SchoolId", "SchoolId", "true", "0"},
                        {"SchoolName", "School Name", "false", "400"},
                        {"SubDivisionId", "SubDivisionId", "true", "0"},
                        {"SubDivisionName", "Sub-Division", "false", "0"},
                        {"DistrictName", "District", "false", "0"},
                        {"ZoneName", "Zone", "false", "0"},
                        {"SchoolEditPermissionStatus", "Status", "false", "0"},
                        {"DeletePermissionCount", "DeletePermissionCount", "true", "0"},
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
                            columnList = new GblFunctions().makeGridColumns(columns, "Y", "Y", "Y", "Y");
                        }
                        else
                        {
                            columnList = new GblFunctions().makeGridColumns(columns, "Y", "N", "Y", "Y");
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
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminSchoolController");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult Schools(string x)
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
                MCommon.saveExceptionLog(ex.Message, "Schools/View", "AdminSchoolController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSchoolList(int? page, int? limit, string sortBy, string direction, string searchString = null, string searchType = null, string lockType = null)
        {
            /********************
            SEARCH TYPE
            -----------
            * N : BY SCHOOL NAME
            * I : BY INDEX NO
            ********************/
            searchType = string.IsNullOrEmpty(searchType) ? SearchType.School.SchoolName : searchType;

            int total = 0;
            if (lockType == "Y")
            {
                lockType = SearchType.SchoolProfile.Locked;
            }
            else if (lockType == "N")
            {
                lockType = SearchType.SchoolProfile.UnLocked;
            }
            else
            {
                lockType = SearchType.SchoolProfile.All;
            }

            var records = new MMstSchool().getMstSchoolList(page, limit, sortBy, direction, searchString, ref total, searchType, lockType);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPreSchoolList(string s)
        {
            var records = new MMstPreSchool().getMstPreSchoolListAutoComplete(s);

            //return new JsonResult()
            //{
            //    Data = records,
            //    ContentType = "application/json",
            //    ContentEncoding = Encoding.UTF8,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            //    MaxJsonLength = Int32.MaxValue
            //};

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPreSchoolDetails(string p)
        {
            Int64 preSchoolId = 0;

            try
            {
                preSchoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(p)));
            }
            catch(Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetPreSchoolDetails", "AdminSchoolController");
            }

            var Records = new MMstSchool().getMstSchoolDetailsByPreSchoolId(preSchoolId);

            return Json(new { Records }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubDivisionList()
        {
            var records = new MMstSubDivision().getMstSubDivisionListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCircleList()
        {
            var records = new MMstCircle().getMstCircleListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetDistrictList()
        {
            var records = new MSchoolParameters().getMstGeographicalDistrictListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDesignationList()
        {
            var records = new MCommon().getMstDesignationListDropDown(UserType.SCHOOL);

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSalutationList()
        {
            var records = new MCommon().getMstSalutationListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetZoneDistrict(string s)
        {
            string zoneName = string.Empty, districtName = string.Empty, indexInitial = string.Empty;

            try
            {
                new MSchoolParameters().getZoneDistrictBySubDivisionId(Convert.ToInt32(GblFunctions.Base64Decode(s)), ref zoneName, ref districtName, ref indexInitial);
            }
            catch(Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetZoneDistrict", "AdminSchoolController");
            }

            return Json(new { zoneName, districtName, indexInitial }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBlock(string c)
        {
            string block = string.Empty;

            try
            {
                block = new MSchoolParameters().getBlockByCircleId(Convert.ToInt32(GblFunctions.Base64Decode(c)));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetBlock", "AdminSchoolController");
            }

            return Json(new { block }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChkDuplicateContact(string e, string p, string m, string s, string et, string dc, string ino, string sdi)
        {
            int err = 0;
            string errDesc = string.Empty;

            err = new MMstSchool().chkDuplicateContactMstSchool((e ?? string.Empty), (p ?? string.Empty), (m ?? string.Empty), GblFunctions.Base64Decode(s ?? DefaultSetting.DefaultValEnc), et, ref errDesc, (dc ?? string.Empty), (ino ?? string.Empty), (sdi ?? string.Empty));

            return Json(new { err, errDesc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Schools(VMMstSchool data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                string errDesc = string.Empty;
                string mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : ((data.EntType == EntType.UNLOCK) ? Mode.UNLOCK : Mode.ERROR))));

                if (mode == Mode.ERROR)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    return RedirectToAction("Schools", "AdminSchool", rvd);
                }

                if (mode == Mode.DELETE) //if mode = delete, bypass model state checking
                {
                    err = new MMstSchool().saveMstSchool(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
                    }
                    else if (err == 2)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = errDesc;
                        return RedirectToAction("Schools", "AdminSchool", rvd);
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }
                    return RedirectToAction("Schools", "AdminSchool", rvd);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        err = new MMstSchool().saveMstSchool(data, ref errDesc);

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
                        return RedirectToAction("Schools", "AdminSchool", rvd);
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
                MCommon.saveExceptionLog(ex.Message, "Schools/Save", "AdminSchoolController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSchoolView(string s)
        {
            string View = string.Empty;

            try
            {
                View = new MMstSchool().getMstSchoolView(GblFunctions.Base64Decode(s));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolView", "AdminSchoolController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolDetails(string s)
        {
            var Records = new MMstSchool().getMstSchoolDetails(GblFunctions.Base64Decode(s));

            return Json(new { Records }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChkEdit(string s)
        {
            string d = string.Empty;

            try
            {
                Int64 schoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(s).Trim()));

                d = new MMstSchool().chkSchoolEditPermission(schoolId, SchoolProfileEditFor.Admin);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ChkEdit", "AdminSchoolController");
            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
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
                    DataTable dt = new MMstSchool().downloadMstSchoolList(1, MaxFileSize.RecordCount, null, null, string.Empty, ref total, SearchType.School.SchoolName, SearchType.SchoolProfile.All, Convert.ToInt32(reportFormat));

                    if (dt != null)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            int i = 1;
                            var ws = wb.Worksheets.Add("School List");
                            ws.Cell(1, 1).Value = "School List (Report Generated on : " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt") + ")";
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
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "School List.xlsx");
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
                            return View("~/Areas/Admin/Views/AdminSchool/Schools.cshtml");
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
                    return RedirectToAction("Schools", "AdminSchool", rvd);
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "DownloadReport", "AdminSchoolController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult ChkDelete(string s)
        {
            int err = 0;

            try
            {
                string tableName = "StudentRegistrationDetails";
                string fieldName = "SchoolId";
                string fieldValue = GblFunctions.Base64Decode(s);

                err = new MCommon().chkDelete(tableName, fieldName, fieldValue);
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "ChkDelete", "AdminSchoolController");
            }
            return Json(new { err }, JsonRequestBehavior.AllowGet);
        }
    }
}
