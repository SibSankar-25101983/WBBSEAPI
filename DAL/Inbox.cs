using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ViewModel;
using Common;
using Microsoft.Security.Application;

namespace DAL
{
    public class Inbox
    {
        public DataTable GetAdminSchoolInboxList(string searchString, int? pageNo, int? pageSize, ref int totalCount, string archiveYN, Int64 schoolId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminSchoolInboxList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pArchiveYN", archiveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
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

        public int SaveAdminSchoolInbox(Int64 InboxId, int linkTypeId, string HeaderText, string BodyText, string FooterText, string URL, string PDFName, string PdfFilePath
                                    , string mode, string ipAddress, int createdBy, ref string errDesc, string newYN, string archiveYN, string allYN
                                    , string schoolIdList, string sender, string receiver)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveAdminSchoolInbox";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pInboxId", InboxId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pLinkTypeId", linkTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, HeaderText.Length + 1, "@pHeaderText", HeaderText);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, BodyText.Length + 1, "@pBodyText", BodyText);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, FooterText.Length + 1, "@pFooterText", FooterText);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pURL", URL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPDFName", PDFName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pPdfFilePath", PdfFilePath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pNewYN", newYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pArchiveYN", archiveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pAllYN", allYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, schoolIdList.Length + 1, "@pSchoolIdList", schoolIdList);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pSender", sender);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pReceiver", receiver);
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

        public int GetAdminSchoolInboxView(Int64 inboxId, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminSchoolInboxView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pInboxId", inboxId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }

            return err;
        }

        public string GetInboxLink(Int64 inboxId, int linkType, ref int err, ref string headerData)
        {
            string data = string.Empty;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetInboxLink";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pInboxId", inboxId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pLinkType", linkType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pHeader", "");
                DBUtility.Execute(oCmd);
                data = oCmd.Parameters["@pData"].Value.ToString();
                headerData = oCmd.Parameters["@pHeader"].Value.ToString();
                err = 0;
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }

            return data;
        }

        public int GetAdminSchoolInboxEdit(Int64 inboxId, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminSchoolInboxEdit";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pInboxId", inboxId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }

            return err;
        }

        public int GetSchoolUnreadNoticeList(Int64 schoolId, int groupId, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSchoolUnreadNoticeList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pGroupId", groupId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }

            return err;
        }

        public int UpdateAdminSchoolReadStatus(Int64 schoolId, ref string errDesc)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateAdminSchoolReadStatus";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
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

        public DataTable GetAdminPreSchoolInboxList(string searchString, int? pageNo, int? pageSize, ref int totalCount, string archiveYN, Int64 preSchoolId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminPreSchoolInboxList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pArchiveYN", archiveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", preSchoolId);
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

        public int GetAdminPreSchoolInboxView(Int64 inboxId, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminPreSchoolInboxView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pInboxId", inboxId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }

            return err;
        }

        public int GetAdminPreSchoolInboxEdit(Int64 inboxId, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAdminPreSchoolInboxEdit";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pInboxId", inboxId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }

            return err;
        }

        public int SaveAdminPreSchoolInbox(Int64 InboxId, int linkTypeId, string HeaderText, string BodyText, string FooterText, string URL, string PDFName, string PdfFilePath
                                    , string mode, string ipAddress, int createdBy, ref string errDesc, string newYN, string archiveYN, string allYN
                                    , string schoolIdList, string sender, string receiver)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveAdminPreSchoolInbox";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pInboxId", InboxId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pLinkTypeId", linkTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, HeaderText.Length + 1, "@pHeaderText", HeaderText);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, BodyText.Length + 1, "@pBodyText", BodyText);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, FooterText.Length + 1, "@pFooterText", FooterText);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pURL", URL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPDFName", PDFName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pPdfFilePath", PdfFilePath);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pNewYN", newYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pArchiveYN", archiveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pAllYN", allYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, schoolIdList.Length + 1, "@pSchoolIdList", schoolIdList);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pSender", sender);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pReceiver", receiver);
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

        public int GetPreSchoolUnreadNoticeList(Int64 preSchoolId, int groupId, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPreSchoolUnreadNoticeList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", preSchoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pGroupId", groupId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                data = ex.Message;
            }

            return err;
        }

        public int UpdateAdminPreSchoolReadStatus(Int64 preSchoolId, ref string errDesc)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateAdminPreSchoolReadStatus";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", preSchoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
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
