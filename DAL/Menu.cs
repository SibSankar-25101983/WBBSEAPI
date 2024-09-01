using System;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DAL
{
    public class Menu
    {
        public DataTable GetMenuDetails(string forSite, int groupId, ref string data, ref string dashboardData)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMenuDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pForSite", forSite);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pGroupId", groupId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pMenuDetails", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pDashBoardData", "");
                DBUtility.ExecuteForSelect(oCmd, dt);
                data = oCmd.Parameters["@pMenuDetails"].Value.ToString();
                dashboardData = oCmd.Parameters["@pDashBoardData"].Value.ToString();
            }
            catch
            {
                dt = null;
                data = "error";
            }
            finally
            {
                oCmd = null;
            }
            return dt;
        }

        public int GetWebsiteHeader(ref string headerData, ref string menuDetails)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetWebsiteHeader";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pHeaderData", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pMenuDetails", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                headerData = oCmd.Parameters["@pHeaderData"].Value.ToString();
                menuDetails = oCmd.Parameters["@pMenuDetails"].Value.ToString();
            }
            catch(Exception ex)
            {
                err = 1;
                headerData = ex.Message;
                menuDetails = Message.OperationError;
            }
            finally
            {
                oCmd = null;
            }
            return err;
        }
    }
}
