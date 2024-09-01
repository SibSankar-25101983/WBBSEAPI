using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace DAL
{
    public class Block
    {
        public DataTable getMstBlockListDropDown()
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstBlockListDropDown";
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
        
        public int saveMstBlock(string mode, int blockId, string blockName,  string ipAddress, int createdBy,
                                    ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int error = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveMstBlock";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pBlockId", blockId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBlockName", blockName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                error = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }
            finally
            {
                oCmd = null;
            }
            return error;
        }

        public DataTable getMstBlockList(string searchString, int? pageNo, int? pageSize, ref int totalCount)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstBlockList";
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

        public DataTable downloadMstBlockList(string searchString, int? pageNo, int? pageSize, ref int totalCount, int reportFormat)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DownloadMstBlockList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pMode", reportFormat);
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
