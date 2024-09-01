using System;
using System.Web;
using System.Data;
using DAL;
using Common;
using WBBSE.Models;

namespace WBBSE.Areas.PreSchools.Models
{
    public class MPreSchoolHome
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
                MCommon.saveExceptionLog(ex.Message, "PreSchoolHome/getMenuDetails(MPreSchoolHome)", "PreSchoolHomeController");
            }
            finally
            {
                dt = null;
                m = null;
            }

            return dashboardData;
        }

        public string getPreSchoolProfileDashBoardData()
        {
            string data = string.Empty;
            Int64 preSchoolId = 0;

            try
            {
                preSchoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                int userId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                data = new PreSchool().GetPreSchoolProfileDashBoardData(preSchoolId, userId);
            }
            catch (Exception ex)
            {
                data = string.Empty;
                MCommon.saveExceptionLog(ex.Message, "GetPreSchoolProfileDashBoardData/getPreSchoolProfileDashBoardData(MPreSchoolHome)", "PreSchoolHomeController");
            }

            return data;
        }
    }
}