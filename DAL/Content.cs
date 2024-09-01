using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ViewModel;
using Common;
using Microsoft.Security.Application;

namespace DAL
{
    public class Content
    {
        public DataTable GetContentEditListDropDown()
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetContentEditListDropDown";
                DBUtility.ExecuteForSelect(oCmd, dt);
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

        public DataTable GetContentList(string searchString, string menuCode, int? pageNo, int? pageSize, ref int totalCount, string archiveYN)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetContentList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 2, "@pMenuCode", menuCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pArchiveYN", archiveYN);
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

        public DataTable GetRTIList(string searchString, int? pageNo, int? pageSize, ref int totalCount)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetRTIList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);                
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

        public string GetDisclaimer()
        {
            SqlCommand oCmd = new SqlCommand();
            string data = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDisclaimer";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch
            {
                data = string.Empty;
            }
            finally
            {
                oCmd = null;
            }

            return data;
        }

        public string GetBoardDeskDetails(string menuCode, ref string  headerText, ref string image)
        {
            SqlCommand oCmd = new SqlCommand();
            string bodyText = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBoardDeskDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pMenuCode", menuCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pHeaderText", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pBodyText", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pImage", "");
                DBUtility.Execute(oCmd);
                headerText = oCmd.Parameters["@pHeaderText"].Value.ToString();
                bodyText = oCmd.Parameters["@pBodyText"].Value.ToString();
                image = oCmd.Parameters["@pImage"].Value.ToString();

            }
            catch
            {
                bodyText = string.Empty;
            }
            finally
            {
                oCmd = null;
            }

            return bodyText;
        }
    }
}
