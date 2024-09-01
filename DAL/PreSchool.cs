using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ViewModel;
using Common;
using Microsoft.Security.Application;

namespace DAL
{
    public class PreSchool
    {
        public DataTable GetMstPreSchoolListDropDown()
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstPreSchoolListDropDown";
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

        public DataTable GetMstPreSchoolListAutoComplete(string searchString)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstPreSchoolListAutoComplete";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
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

        public DataTable GetMstPreSchoolList(string searchString, int? pageNo, int? pageSize, ref int totalCount, string searchType, string lockType)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstPreSchoolList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSearchType", searchType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pLockedYN", lockType);
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

        public int GetMstPreSchoolView(string preSchoolId, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstPreSchoolView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", Convert.ToInt64(preSchoolId));
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

        public DataTable GetMstPreSchoolDetails(string preSchoolId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();


            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstPreSchoolDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", preSchoolId);
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

        public int ChkDuplicateContactMstPreSchool(string emailId, string phoneNo, string mobileNo, string preSchoolId, string entType, ref string errDesc, string diseCode)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDuplicateContactMstPreSchool";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pEmailId", emailId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pPhoneNo", phoneNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMobileNo", mobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", Convert.ToInt64(preSchoolId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pEntType", entType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pDISECode", diseCode);
                DBUtility.Execute(oCmd);
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = "Operation Error. Details : " + ex.Message;
            }
            finally
            {
                oCmd = null;
            }
            return err;
        }

        public int saveMstPreSchool(VMMstPreSchool data, int userTypeId, string mode, string ipAddress, int createdBy, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                //oCmd.CommandText = "SaveMstSchool";
                oCmd.CommandText = "SaveMstPreSchool";                
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.PreSchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pDISECode", (Sanitizer.GetSafeHtmlFragment(data.DISECode) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pSchoolName", (Sanitizer.GetSafeHtmlFragment(data.SchoolName) ?? DefaultSetting.EmptyVal).Trim());
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSubDivisionId", Convert.ToInt32(GblFunctions.Base64Decode((data.SubDivisionId ?? DefaultSetting.DefaultValEnc).Trim())));
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCircleId", Convert.ToInt32(GblFunctions.Base64Decode((data.CircleId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCircleId", -1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolTypeId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolTypeId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolCategoryId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolCategoryId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolStatusId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolStatusId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolMediumId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolMediumId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolRecognitionId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolRecognitionId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolManagementId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolManagementId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pDesignationId", Convert.ToInt32(GblFunctions.Base64Decode((data.DesignationId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAddressLine1", (Sanitizer.GetSafeHtmlFragment(data.AddressLine1) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAddressLine2", (Sanitizer.GetSafeHtmlFragment(data.AddressLine2) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pGramPanchayet", (Sanitizer.GetSafeHtmlFragment(data.GramPanchayet) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pStdCode", (Sanitizer.GetSafeHtmlFragment(data.StdCode) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPostOffice", (Sanitizer.GetSafeHtmlFragment(data.PostOffice) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPoliceStation", (Sanitizer.GetSafeHtmlFragment(data.PoliceStation) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCity", (Sanitizer.GetSafeHtmlFragment(data.City) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pDistrictId", Convert.ToInt32(GblFunctions.Base64Decode((data.DistrictId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPinCode", (Sanitizer.GetSafeHtmlFragment(data.PinCode) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pPhoneNo", (Sanitizer.GetSafeHtmlFragment(data.PhoneNo) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMobileNo", (Sanitizer.GetSafeHtmlFragment(data.MobileNo) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 14, "@pFaxNo", (Sanitizer.GetSafeHtmlFragment(data.FaxNo) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWebsite", (Sanitizer.GetSafeHtmlFragment(data.Website) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmailId", (Sanitizer.GetSafeHtmlFragment(data.EmailId) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSalutationId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolHeadSalutationId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pSHFirstName", (Sanitizer.GetSafeHtmlFragment(data.SchoolHeadFirstName) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pSHMiddleName", (Sanitizer.GetSafeHtmlFragment(data.SchoolHeadMiddleName) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pSHLastName", (Sanitizer.GetSafeHtmlFragment(data.SchoolHeadLastName) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLoginPwd", DefaultSetting.DefaultPwdHash);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pUserTypeId", userTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
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

        public string GetPreSchoolProfileDashBoardData(Int64 preSchoolId, int userId)
        {
            SqlCommand oCmd = new SqlCommand();
            string data = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPreSchoolProfileDashBoardData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", preSchoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pUserId", userId);
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

        public int UpdatePreSchoolProfile(VMPreSchoolProfile data, int userTypeId, string mode, string ipAddress, int createdBy, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;                
                oCmd.CommandText = "UpdatePreSchoolProfile";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.PreSchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pSchoolName", (Sanitizer.GetSafeHtmlFragment(data.SchoolName) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pDISECode", (Sanitizer.GetSafeHtmlFragment(data.DISECode) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolTypeId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolTypeId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolCategoryId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolCategoryId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolStatusId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolStatusId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolMediumId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolMediumId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolRecognitionId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolRecognitionId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSchoolManagementId", Convert.ToInt32(GblFunctions.Base64Decode((data.SchoolManagementId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pDesignationId", Convert.ToInt32(GblFunctions.Base64Decode((data.DesignationId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAddressLine1", (Sanitizer.GetSafeHtmlFragment(data.AddressLine1) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAddressLine2", (Sanitizer.GetSafeHtmlFragment(data.AddressLine2) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pGramPanchayet", (Sanitizer.GetSafeHtmlFragment(data.GramPanchayet) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pStdCode", (Sanitizer.GetSafeHtmlFragment(data.StdCode) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPostOffice", (Sanitizer.GetSafeHtmlFragment(data.PostOffice) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPoliceStation", (Sanitizer.GetSafeHtmlFragment(data.PoliceStation) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCity", (Sanitizer.GetSafeHtmlFragment(data.City) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pDistrictId", Convert.ToInt32(GblFunctions.Base64Decode((data.DistrictId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPinCode", (Sanitizer.GetSafeHtmlFragment(data.PinCode) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pPhoneNo", (Sanitizer.GetSafeHtmlFragment(data.PhoneNo) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMobileNo", (Sanitizer.GetSafeHtmlFragment(data.MobileNo) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 14, "@pFaxNo", (Sanitizer.GetSafeHtmlFragment(data.FaxNo) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWebsite", (Sanitizer.GetSafeHtmlFragment(data.Website) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmailId", (Sanitizer.GetSafeHtmlFragment(data.EmailId) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
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

        public string ChkPreSchoolEditPermission(Int64 preSchoolId, string editFor)
        {
            SqlCommand oCmd = new SqlCommand();
            string data = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkPreSchoolEditPermission";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", preSchoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pEditFor", editFor);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Char, 1, "@pPermittedYN", "");
                DBUtility.Execute(oCmd);
                data = oCmd.Parameters["@pPermittedYN"].Value.ToString();
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

        public int savePreSchoolEditPermission(VMPreSchoolEditPermission data, string ipAddress, int createdBy, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePreSchoolEditPermission";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.PreSchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
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

        public DataTable downloadMstPreSchoolList(string searchString, int? pageNo, int? pageSize, ref int totalCount, string searchType, string lockType, int reportFormat)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                /*SP GetMstSchoolList CALLING WITH AFORESAID PARAMETERS*/
                oCmd.CommandText = "DownloadMstPreSchoolList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSearchType", searchType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pLockedYN", lockType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pMode", reportFormat);
                DBUtility.ExecuteForSelect(oCmd, dt);

                /*CATCHING OUTPUT PARAMETER VALUE AFTER EXECUTION OF AFORESAID SP*/
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
