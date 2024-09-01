using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;
using System.Text.RegularExpressions;

namespace WBBSE.Areas.Admin.Models
{
    public class MAdminPostPublication
    {
        public List<VMPostPublicationApplicationData> getAdminCandididatePPRPPSList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, string searchType)
        {
            DataTable dt = new DataTable();
            List<VMPostPublicationApplicationData> result = new List<VMPostPublicationApplicationData>();

            try
            {
                dt = new PostPublication().GetAdminCandididatePPRPPSList((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, page, limit, ref total, Convert.ToInt32(searchType));

                var records = (from data in dt.AsEnumerable()
                               select new VMPostPublicationApplicationData
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   RollNo = data.Field<string>("RollNo"),
                                   StudentName = data.Field<string>("StudentName"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   ScrutinyType = data.Field<string>("ScrutinyType"),
                                   TotalSubject = data.Field<string>("TotalSubject"),
                                   TotalPrice = data.Field<string>("TotalPrice")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetApplicationData/getAdminCandididatePPRPPSList(MAdminPostPublication)", "AdminPostPublicationApplicationController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getAdminCandididatePPRPPSView(string rollNo)
        {
            string data = string.Empty;
            int err = 0;

            try
            {
                err = new PostPublication().GetAdminCandididatePPRPPSView(Sanitizer.GetSafeHtmlFragment(rollNo), ref data);

                if (err == 1)
                {
                    data = "<div class='row d-print-none'><div class='col-md-8 mx-auto'><div class='alert alert-danger m-0'><strong>" + Message.OperationError + "</strong></div></div></div>";
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetApplicationDetails/getAdminCandididatePPRPPSView(MAdminPostPublication)", "AdminPostPublicationApplicationController");
                data = "<div class='row d-print-none'><div class='col-md-8 mx-auto'><div class='alert alert-danger m-0'><strong>" + Message.OperationError + "</strong></div></div></div>";
            }

            return data;
        }

        public DataTable rptAdminPPRPPS(VMReporting data, ref int err, ref string errDesc)
        {
            DataTable dt = new DataTable();

            try
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;
                string fd = Sanitizer.GetSafeHtmlFragment((data.FromDate ?? string.Empty).Trim());
                string td = Sanitizer.GetSafeHtmlFragment((data.ToDate ?? string.Empty).Trim());
                int scrutinyTypeId = 0;
                if (!string.IsNullOrEmpty(fd))
                {
                    try
                    {
                        fromDate = GblFunctions.setDate(fd);
                    }
                    catch
                    {
                        err = 2;
                        errDesc = Message.Reporting.InvalidFromDate;
                        return dt;
                    }
                }
                if (!string.IsNullOrEmpty(td))
                {
                    try
                    {
                        toDate = GblFunctions.setDate(td);
                    }
                    catch
                    {
                        err = 2;
                        errDesc = Message.Reporting.InvalidToDate;
                        return dt;
                    }
                }

                try
                {
                    scrutinyTypeId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.Param1)));
                }
                catch
                {
                    err = 2;
                    errDesc = Message.Reporting.PostPublicationReport.InvalidScrutinyType;
                    return dt;
                }

                dt = new PostPublication().RptAdminPPRPPS(fromDate, toDate, scrutinyTypeId);
            }
            catch (Exception ex)
            {
                err = 2;
                errDesc = Message.OperationError;
                dt = null;
                MCommon.saveExceptionLog(ex.Message, "DownloadReport/rptAdminPPRPPS(MAdminPostPublication)", "AdminPostPublicationApplicationController");
            }

            return dt;
        }

        public List<VMMstScrutiny> getScrutinyTypeListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstScrutiny> result = new List<VMMstScrutiny>();

            try
            {
                dt = new PostPublication().GetScrutinyTypeListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstScrutiny
                               {
                                   ScrutinyTypeId = data.Field<string>("ScrutinyTypeId"),
                                   ScrutinyType = data.Field<string>("ScrutinyType")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetScrutinyType/getScrutinyTypeListDropDown(MAdminPostPublication)", "AdminPostPublicationApplicationController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
    }
}
