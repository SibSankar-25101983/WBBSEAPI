using System;
using System.Web;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Data;
using WBBSE.Areas.Admin.Models;
using WBBSE.Models;
using Common;

namespace WBBSE.Reporting.Page
{
    public partial class MstPreSchool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("~/Error/Unexpected.html");
                    }
                    else if (Session[SessionNames.Role] == null || Session[SessionNames.Role].ToString() != UserType.ADMIN)
                    {
                        Response.Redirect("~/Error/Unexpected.html");
                    }
                    else
                    {
                        Warning[] warnings;
                        string[] streamIds;
                        string contentType = string.Empty, encoding = string.Empty, extension = string.Empty;
                        int total = 0;

                        DataTable dt = new MMstPreSchool().downloadMstPreSchoolList(1, MaxFileSize.RecordCount, null, null, string.Empty, ref total, SearchType.PreSchool.SchoolName, SearchType.SchoolProfile.All, Convert.ToInt32(ReportFormat.Pdf));

                        if (dt != null)
                        {
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/RDLC/RptMstPreSchool.rdlc");
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ReportingDateTime", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt")));
                            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("RecordCount", total.ToString()));
                            ReportDataSource rdc = new ReportDataSource("DsMstPreSchool", dt);
                            ReportViewer1.LocalReport.DataSources.Add(rdc);
                            //Export the RDLC Report to Byte Array.
                            byte[] bytes = ReportViewer1.LocalReport.Render("pdf", null, out contentType, out encoding, out extension, out streamIds, out warnings);
                            //Download the RDLC Report in Word, Excel, PDF and Image formats.
                            Response.Clear();
                            Response.Buffer = true;
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = contentType;
                            Response.AppendHeader("Content-Disposition", "attachment; filename=Junior School List." + extension);
                            Response.BinaryWrite(bytes);
                            Response.Flush();
                            Response.End();
                        }
                        else
                        {
                            ReportViewer1.Visible = false;
                            message.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MCommon.saveExceptionLog(ex.Message, "Page_Load", "Reporting/MstPreSchool");
                }
            }
        }
    }
}