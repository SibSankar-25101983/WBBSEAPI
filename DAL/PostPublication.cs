using System;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DAL
{
    public class PostPublication
    {
        public int ChkCandidatePPRPPSModuleAvailability(string rollNo, int mode, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkCandidatePPRPPSModuleAvailability";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRollNo", rollNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch(Exception ex)
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

        public DataTable GetCandidatePPRPPSApplicationDetails(string rollNo)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCandidatePPRPPSApplicationDetails";
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

        public int SaveCandidatePPRPPSDetails(string rollNo, string postPublicationType, int totalSubject, float totalPrice, string applicationDetails,  string ipAddress, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveCandidatePPRPPSDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRollNo", rollNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPostPublicationType", postPublicationType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pTotalSubject", totalSubject);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pTotalPrice", totalPrice);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, applicationDetails.Length + 1, "@pApplicationDetails", applicationDetails);
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

        public int GetCandidatePPRPPSApplicationFormPrintData(string rollNo, ref string data)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCandidatePPRPPSApplicationFormPrintData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRollNo", rollNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }
            finally
            {
                oCmd = null;
            }

            return err;
        }

        public int GetCandidatePPRPPSApplicationPaymentPrintData(string rollNo, ref string data)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCandidatePPRPPSApplicationPaymentPrintData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRollNo", rollNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }
            finally
            {
                oCmd = null;
            }

            return err;
        }

        public DataTable GetAdminCandididatePPRPPSList(string searchString, int? pageNo, int? pageSize, ref int totalCount, int searchType)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminCandididatePPRPPSList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSearchType", searchType);
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

        public int GetAdminCandididatePPRPPSView(string rollNo, ref string data)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminCandididatePPRPPSView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRollNo", rollNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }
            finally
            {
                oCmd = null;
            }

            return err;
        }

        public DataTable RptAdminPPRPPS(DateTime? fromDate, DateTime? toDate, int scrutinyTypeId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "RptAdminPPRPPS";
                if (fromDate == null)
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDate", DBNull.Value);
                }
                else
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDate", fromDate);
                }
                if(toDate == null)
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", DBNull.Value);
                }
                else
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", toDate);
                }
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pScrutinyTypeId", scrutinyTypeId);
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

        public DataTable GetScrutinyTypeListDropDown()
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetScrutinyTypeListDropDown";
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
