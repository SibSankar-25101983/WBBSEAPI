using System;
using System.Web;
using System.Web.Mvc;
using WBBSE.Areas.Admin.Models;
using System.Web.Routing;
using Microsoft.Security.Application;
using System.Text;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using WBBSE.Models;
using ViewModel;
using Common;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminReportsController : Controller
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
                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.ReportYN] = (ReportYN == "Y") ? "visible" : "hidden";
                            
                            status = 1;
                        }
                    }
                    else
                    {
                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.ReportYN] = "visible";
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminReportsController");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult PostPublicationReport(string x)
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
                MCommon.saveExceptionLog(ex.Message, "PostPublicationReport/View", "AdminReportsController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PostPublicationReport(VMReporting data)
        {
            try
            {
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                int err = 0;
                string errDesc = string.Empty;

                string reportFormat = Sanitizer.GetSafeHtmlFragment(data.ReportType);
                if (reportFormat != ReportFormat.Excel && reportFormat != ReportFormat.Pdf)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                    TempData[TempDataNames.ErrDesc] = Message.Reporting.InvalidFromDate;
                    return RedirectToAction("PostPublicationReport", "AdminReports", rvd);
                }

                if (reportFormat == ReportFormat.Excel)
                {
                    DataTable dt = new MAdminPostPublication().rptAdminPPRPPS(data, ref err, ref errDesc);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            int i = 1;
                            var ws = wb.Worksheets.Add("PP Report");
                            ws.Cell(1, 1).Value = "Post Publication Applications(From Date : " + (string.IsNullOrEmpty(Sanitizer.GetSafeHtmlFragment(data.FromDate)) ? "NA" : Sanitizer.GetSafeHtmlFragment(data.FromDate)) + ", To Date : " + (string.IsNullOrEmpty(Sanitizer.GetSafeHtmlFragment(data.ToDate)) ? "NA" : Sanitizer.GetSafeHtmlFragment(data.ToDate)) + ")";
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                            ws.Cell(2, 1).Value = "Total Records : " + dt.Rows.Count.ToString();
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

                            // add a cookie with the name 'dlc' and the value from the postback
                            //ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("dlc", Sanitizer.GetSafeHtmlFragment(data.CookieValue)));

                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb.SaveAs(stream);
                                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Post Publication Applications Report(Generated On-" + DateTime.Now.ToString() + ").xlsx");
                            }
                        }
                    }
                    else if (err == 2)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = errDesc;
                        return RedirectToAction("PostPublicationReport", "AdminReports", rvd);
                    }
                    else if (dt.Rows.Count == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = Message.Reporting.ReportGenerationFailed;
                        return RedirectToAction("PostPublicationReport", "AdminReports", rvd);
                    }
                    else
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }
                else if (reportFormat == ReportFormat.Pdf)
                {
                    return RedirectToAction("PostPublicationReport", "AdminReports", rvd);
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PostPublicationReport", "AdminReportsController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
