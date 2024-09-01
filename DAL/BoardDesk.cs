using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ViewModel;
using Common;
using Microsoft.Security.Application;

namespace DAL
{
    public class BoardDesk
    {
        public DataTable GetBoardDeskList(string menuCode, int? pageNo, int? pageSize, ref int totalCount)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBoardDeskList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 2, "@pMenuCode", menuCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.ExecuteForSelect(oCmd, dt);
                totalCount = Convert.ToInt32(oCmd.Parameters["@pTotalCount"].Value);
            }
            catch
            {
                dt = null;
            }
            finally
            {
                oCmd = null;
            }
            return dt;
        }
    }
}
