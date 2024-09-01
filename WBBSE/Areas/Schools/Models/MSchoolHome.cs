using System;
using System.Web;
using System.Data;
using DAL;
using Common;
using WBBSE.Models;

namespace WBBSE.Areas.Schools.Models
{
    /*******************************************************************************
      * A BRIEF HISTORY OF MSchoolHome.
      * CONTAINS SCHOOL DASHBOARD PAGE/HOMEPAGE POPULATION LOGIC 
      * DAL: DATA ACCESS LAYER
      * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
      * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
      ******************************************************************************/
    public class MSchoolHome
    {
        /* FETCH THE MENUDETAILS FORM DATABASE AND POPULATING THE SCHOOL ADMINISTRATIVE MENU
         * @forSite= ADMIN SITE/CANDIDATE WEBSITE
         * @groupId= USER UNIQUE GROUP ID 
         */
        public string getMenuDetails(string forSite, int groupId)
        {
            string data = string.Empty, dashboardData = string.Empty;
            /*DATA ACCESS CLASS OF MENU*/
            Menu m = new Menu();
            DataTable dt = new DataTable();

            try
            {
                /*FETCHING SCHOOL ADMIN MENU*/
                dt = m.GetMenuDetails(forSite, groupId, ref data, ref dashboardData);

                /*STORING ROLE & MENU DETAILS IN SESSION*/
                HttpContext.Current.Session[SessionNames.RoleDetails] = dt;
                HttpContext.Current.Session[SessionNames.MenuDetails] = data;
            }
            catch (Exception ex) /*HANDELING THE EXCEPTION DURING EXECUTION*/
            {
                /*SETTING ERROR IF EXCEPTION OCCURE*/
                data = "error";
                /*SAVING EXCEPTION LOG IN DATABASE FOR THIS SPECIFIC JOB [PARAM1: EX MESSAGE, PARAM2:MODELCLASS & FUNCTION NAME, PARAM3:CONTROLLER NAME]*/
                MCommon.saveExceptionLog(ex.Message, "SchoolHome/getMenuDetails(MSchoolHome)", "SchoolHomeController");
            }
            finally
            {
                dt = null;
                m = null;
            }

            return dashboardData;
        }

        /*FETCHING REQUIRED DATA IN HTML FORMAT FOR POPULATION OF SCHOOL ADMIN DASHBOARD ACCORDIN TO SPECIFIC SCHOOL AND USER*/       
        public string getSchoolProfileDashBoardData()
        {
            string data = string.Empty;

            try
            {
                /*FETCHING REQUIRED DATA USING DAL
                 * PARAM1: UNIQUE SCHOOL ID
                 * PARAM2: UNIQUE USER ID
                 */
                Int64 schoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                int userId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);              
                data = new School().GetSchoolProfileDashBoardData(schoolId, userId);
            }
            catch (Exception ex) /*HANDELING THE EXCEPTION DURING EXECUTION*/
            {
                data = string.Empty;
                /*SAVING EXCEPTION LOG IN DATABASE FOR THIS SPECIFIC JOB [PARAM1: EX MESSAGE, PARAM2:MODELCLASS & FUNCTION NAME, PARAM3:CONTROLLER NAME]*/
                MCommon.saveExceptionLog(ex.Message, "GetSchoolProfileDashBoardData/getSchoolProfileDashBoardData(MSchoolHome)", "SchoolHomeController");
            }

            return data;
        }

        public DataTable getSchoolDashboardRegistrationStatistics(ref int err)
        {
            DataTable dt = new DataTable();

            try
            {
                Int64 schoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);

                dt = new Statistics().GetSchoolDashboardRegistrationStatistics(schoolId, ref err);

                if (dt == null || err > 0)
                {
                    MCommon.saveExceptionLog("Data Not Returned", "GetStat2View/getSchoolDashboardRegistrationStatistics(MSchoolHome)", "SchoolHomeController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetStat2View/getSchoolDashboardRegistrationStatistics(MSchoolHome)", "SchoolHomeController");
            }

            return dt;
        }
    }
}
