using System;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DAL
{
    public class Home
    {
        public int GetWebsiteHomePageContent(ref string section1, ref string section2, ref string section3, ref string section4, ref string section5, ref string section6)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetWebsiteHomePageContent";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pSection1", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pSection2", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pSection3", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pSection4", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pSection5", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pSection6", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                section1 = oCmd.Parameters["@pSection1"].Value.ToString();
                section2 = oCmd.Parameters["@pSection2"].Value.ToString();
                section3 = oCmd.Parameters["@pSection3"].Value.ToString();
                section4 = oCmd.Parameters["@pSection4"].Value.ToString();
                section5 = oCmd.Parameters["@pSection5"].Value.ToString();
                section6 = oCmd.Parameters["@pSection6"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                section1 = ex.Message;
            }
            finally
            {
                oCmd = null;
            }
            return err;
        }

        public DataTable GetCandidatePPRPPSDetails(string rollNo)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCandidatePPRPPSDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRollNo", rollNo);
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
