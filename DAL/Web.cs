using System;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DAL
{
    public class Web
    {      

        public string GetWebsiteMenuLink(string menuCode)
        {
            SqlCommand oCmd = new SqlCommand();
            string url = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetWebsiteMenuLink";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMenuCode", menuCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pURL", "");
                DBUtility.Execute(oCmd);
                url = oCmd.Parameters["@pURL"].Value.ToString();                

            }
            catch
            {
                url = string.Empty;
            }
            finally
            {
                oCmd = null;
            }

            return url;
        }

        public string GetPdfView(string menuCode)
        {
            SqlCommand oCmd = new SqlCommand();
            string url = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPdfView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMenuCode", menuCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pURL", "");
                DBUtility.Execute(oCmd);
                url = oCmd.Parameters["@pURL"].Value.ToString();

            }
            catch
            {
                url = string.Empty;
            }
            finally
            {
                oCmd = null;
            }

            return url;
        }
    }
}
