using System;
using System.Web;
using System.Data;
using DAL;
using Common;
using WBBSE.Models;

namespace WBBSE.Areas.Candidate.Models
{
    public class MCandidateHome
    {
        public string getMenuDetails(string forSite, int groupId)
        {
            string data = string.Empty, dashboardData = string.Empty;
            Menu m = new Menu();
            DataTable dt = new DataTable();

            try
            {
                dt = m.GetMenuDetails(forSite, groupId, ref data, ref dashboardData);

                HttpContext.Current.Session[SessionNames.RoleDetails] = dt;
                HttpContext.Current.Session[SessionNames.MenuDetails] = data;
            }
            catch (Exception ex)
            {
                data = "error";
                MCommon.saveExceptionLog(ex.Message, "Home/getMenuDetails(MCandidateHome)", "CandidateHomeController");
            }
            finally
            {
                dt = null;
                m = null;
            }

            return dashboardData;
        }

        public int getCandidatePPRPPSDetails(ref string candidateData, ref string resultData, ref string eligibility)
        {
            int err = 0;
            string postPublicationType = string.Empty, paymentPageLink = string.Empty;
            float postPublicationPrice = 0;
            DataTable dt = new DataTable();

            try
            {
                string rollNo = HttpContext.Current.Session[SessionNames.UserId].ToString();

                dt = new Home().GetCandidatePPRPPSDetails(rollNo);

                if (dt != null && dt.Rows.Count > 0)
                {
                    err = Convert.ToInt32(dt.Rows[0]["Err"]);
                    candidateData = dt.Rows[0]["ScrutityData"].ToString();
                    resultData = dt.Rows[0]["ResultData"].ToString();
                    eligibility = dt.Rows[0]["Eligibility"].ToString();
                    postPublicationType = dt.Rows[0]["PostPublicationType"].ToString();
                    postPublicationPrice = Convert.ToSingle(dt.Rows[0]["PostPublicationPrice"]);
                    paymentPageLink = dt.Rows[0]["PaymentPageLink"].ToString();

                    HttpContext.Current.Session[SessionNames.PostPublicationType] = postPublicationType;
                    HttpContext.Current.Session[SessionNames.PostPublicationPrice] = postPublicationPrice;
                    HttpContext.Current.Session[SessionNames.PostPublicationPaymentPageLink] = paymentPageLink;

                }
                else
                {
                    err = 1;
                    candidateData = string.Empty;
                    resultData = string.Empty;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                candidateData = string.Empty;
                MCommon.saveExceptionLog(ex.Message, "GetCandidateDetails/getCandidatePPRPPSDetails(MCandidateHome)", "CandidateHomeController");
            }
            finally
            {
                dt = null;
            }

            return err;
        }
    }
}
