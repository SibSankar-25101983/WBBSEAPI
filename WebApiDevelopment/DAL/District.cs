using DAL;
using System.Data.SqlClient;
using System.Data;
using System;
using WebApiDevelopment.ViewModel;

namespace WebApiDevelopment.DAL
{
    public class District
    {
        public DataTable getMstDistrictListDropDown()
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstDistrictListDropDown";
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

        public DataTable getMstDistrictList(string searchString, int? pageNo, int? pageSize, ref int totalCount)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstDistrictList";
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
        public int saveMstDistrict(VMMstDistrict data, string mode, string ipAddress, int createdBy, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int error = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveMstDistrict";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pDistrictId", Convert.ToInt64(data.DistrictId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pDistrictName", data.DistrictName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pZoneId", Convert.ToInt64(data.ZoneId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pInitialIndex", data.IndexInitial);
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

    }
}
