using System;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace DAL
{
    public class MAC
    {
        public int ValidateMAC(string decmac, string detokenid, string successyn, int createdBy, string ipAddress)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ValidateMAC";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, decmac.Length + 1, "@pMACList", decmac);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, detokenid.Length + 1, "@pTokenId", detokenid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pSuccessYN", successyn);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                string errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch
            {
                err = 1;
            }
            finally
            {
                oCmd = null;
            }
            return err;
        }

        public string ChkMACAuthorizationActiveYN()
        {
            string status = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkMACAuthorizationActiveYN";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Char, 1, "@pSwitchedYN", "");
                DBUtility.Execute(oCmd);
                status = Convert.ToString(oCmd.Parameters["@pSwitchedYN"].Value);
            }
            catch
            {
                status = "N";
            }
            finally
            {
                oCmd = null;
            }
            return status;
        }

        public DataTable loadMACList(DateTime? fromdate, DateTime? todate, string searchtype, string searchvalue)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "LoadMACList";
                if (fromdate == null)
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pFromDate", DBNull.Value);
                }
                else
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pFromDate", fromdate);
                }
                if (todate == null)
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pToDate", DBNull.Value);
                }
                else
                {
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pToDate", todate);
                }
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSearchType", searchtype);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, searchvalue.Length + 1, "@pSearchValue", searchvalue);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch
            {
                dt = null;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }

        public int SaveMachineDetails(string computername, string machineid, string authorizedYN, int tableId
                                    , string mode, string ipAddress, int createdby, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveMachineDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 20, "@pTblId", tableId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, computername.Length + 1, "@pComputerName", computername);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, machineid.Length + 1, "@pMachineId", machineid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAuthorizedYN", authorizedYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdby);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                errDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message.Replace("'", "");
            }
            finally
            {
                oCmd.Dispose();
            }
            return err;
        }

        public DataTable loadMACDetails(string machineid)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "LoadMACDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pMachineId", machineid);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch
            {
                dt = null;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }

        public int SaveMacAuthorizationData(string xmldata, string ipAddress, int createdby, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveMacAuthorizationData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmldata.Length + 1, "@pXmlData", xmldata);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdby);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                errDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
            return err;
        }

        public int ValidateMACRequestSource(string machineid, ref string tokenid, string ipAddress, ref DateTime timeStamp)
        {
            int status = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ValidateMACRequestSource";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, machineid.Length + 1, "@pMACList", machineid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pStatus", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pStatusDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.DateTime, 10, "@pTimeStamp", DateTime.Now);
                DBUtility.Execute(oCmd);
                status = Convert.ToInt32(oCmd.Parameters["@pStatus"].Value);
                tokenid = oCmd.Parameters["@pStatusDesc"].Value.ToString();
                timeStamp = Convert.ToDateTime(oCmd.Parameters["@pTimeStamp"].Value);
            }
            catch
            {
                status = 0;
            }
            return status;
        }

        public string ChkMACAuthorizationRequestPermision(string machineId)
        {
            string data = string.Empty;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkMACAuthorizationRequestPermision";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, machineId.Length + 1, "@pMACList", machineId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Char, 1, "@pRequestPermittedYN", "");
                DBUtility.Execute(oCmd);
                data = oCmd.Parameters["@pRequestPermittedYN"].Value.ToString();
            }
            catch
            {
                data = "N";
            }

            return data;
        }

        public string ChkMACSoftwareDownloadPermission(int userId)
        {
            string data = string.Empty;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkMACSoftwareDownloadPermission";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pUserId", userId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Char, 1, "@pDownloadPermittedYN", "");
                DBUtility.Execute(oCmd);
                data = oCmd.Parameters["@pDownloadPermittedYN"].Value.ToString();
            }
            catch
            {
                data = "N";
            }

            return data;
        }

        public int SaveMACSoftwareDownloadDetails(int userId, string downloadType, string ipAddress, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int error = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveMACSoftwareDownloadDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 20, "@pUserId", userId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pDownloadType", downloadType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10, "@pErr", 0);
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

            return error;
        }

        public int DeleteMACToken(ref string errDesc)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DeleteMACToken";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10, "@pErr", 0);
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

            return err;
        }
    }
}
