using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Books
    {
        public DataTable GetMstBookList(string searchString, int? pageNo, int? pageSize, ref int totalCount)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstBookList";
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

        public int SaveMstBook(int bookId, string bookName, string bookCode, string subjectClass, int mediumId, float bookPrice, string mode, string ipAddress, int createdBy, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveMstBook";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pBookId", bookId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBookName", bookName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBookCode", bookCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pClass", subjectClass);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pMediumId", mediumId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pBookPrice", bookPrice);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }
            finally
            {
                oCmd = null;
            }

            return err;
        }
    }
}
