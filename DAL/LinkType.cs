using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class LinkType
    {
        public DataTable GetMstLinkTypeListDropDown()
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstLinkTypeListDropDown";
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
    }
}
