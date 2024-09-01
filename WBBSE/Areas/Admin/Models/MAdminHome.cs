using System;
using System.Web;
using System.Data;
using DAL;
using Common;
using WBBSE.Models;

namespace WBBSE.Areas.Admin.Models
{
    public class MAdminHome
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
            catch(Exception ex)
            {
                data = "error";
                MCommon.saveExceptionLog(ex.Message, "AdminHome/getMenuDetails(MAdminHome)", "AdminHomeController");
            }
            finally
            {
                dt = null;
                m = null;
            }

            return dashboardData;
        }

        public DataTable getAdminWebsiteDashboardRegistrationStatistics(ref int err)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new Statistics().GetAdminWebsiteDashboardRegistrationStatistics(ref err);

                if (dt == null || err > 0)
                {
                    MCommon.saveExceptionLog("Data Not Returned", "GetStat1View/getAdminWebsiteDashboardRegistrationStatistics(MAdminHome)", "AdminHomeController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetStat1View/getAdminWebsiteDashboardRegistrationStatistics(MAdminHome)", "AdminHomeController");
            }

            return dt;
        }

        public string getAdminWebsiteDashboardStatistics()
        {
            string StatViewData = string.Empty;
            Statistics st = new Statistics();
            //int err = 0;

            try
            {
                StatViewData = st.GetAdminWebsiteDashboardStatistics();              
            }
            catch (Exception ex)
            {
                //err = 1;
                MCommon.saveExceptionLog(ex.Message, "GetStat2View/getAdminWebsiteDashboardStatistics(MAdminHome)", "AdminHomeController");
            }
            finally
            {
                st = null;
            }

            return StatViewData;
        }
    }
}