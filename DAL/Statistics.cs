using System;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DAL
{
    public class Statistics
    {
        public DataTable GetAdminWebsiteDashboardRegistrationStatistics(ref int err)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminWebsiteDashboardRegistrationStatistics";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.ExecuteForSelect(oCmd, dt);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
            }
            catch
            {
                err = 1;
            }
            finally
            {
                oCmd = null;
            }
            return dt;
        }

        public string GetAdminWebsiteDashboardStatistics()
        {
            string Stat1ViewData = string.Empty;
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminWebsiteDashboardStatistics";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pSection1", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                Stat1ViewData = oCmd.Parameters["@pSection1"].Value.ToString();               
            }
            catch
            {
                err = 1;                
                Stat1ViewData = Message.OperationError;
            }
            finally
            {
                oCmd = null;
            }
            return Stat1ViewData;
        }

        public DataTable GetSchoolDashboardRegistrationStatistics(Int64 schoolId, ref int err)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSchoolDashboardRegistrationStatistics";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.ExecuteForSelect(oCmd, dt);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
            }
            catch
            {
                err = 1;
            }
            finally
            {
                oCmd = null;
            }
            return dt;
        }
    }
}
