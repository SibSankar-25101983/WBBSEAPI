using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ViewModel;
using Common;
using Microsoft.Security.Application;

namespace DAL
{
    /*******************************************************************************
     * A BRIEF HISTORY OF DATA ACCESS LAYER(DAL) School.
     * HANDELS SCHOOL RELATED DATA ACCESS & ESTABLISH COMMUNICATION WITH BOTH MODEL AND DATABASE.  
     * CONTAINS FUNCTIONS FOR PULLING/PUSHING DATA FROM DATA BASE.
     * DAL: DATA ACCESS LAYER.
        * DBUtility: THIS CALSS HANDELS ALL DATABASE CONNECTIONS RELATED OBJECTS LIKE EXECUTE QUERY, EXECUTE SELECT QUERY, EXECUTE SP, ADDING SQL PARAMETER ETC.
     * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS.
     * ViewModel: COMMON VIEW MODEL CLASS FOR COMMON DATABASE ENTITIES.
     ******************************************************************************/
    public class School
    {
        /* PULLING SCHOOL LIST FROM DATABASE ACCORDING TO THE SEARCH PARAMETER & RETURN AS DATATABLE
         * PARAM1: searchString = SEARCH CRITERIA AS STRING
         * PARAM2: pageNo= INT PAGE NO.
         * PARAM2: pageSize= INT PAGE SIZE
         * PARAM3: totalCount= INT TOTAL ROW COUNT AS REFERENCE STRING (OUT PUT PARAMETER)
         * PARAM4: searchType= N : BY SCHOOL NAME, I : BY INDEX NO, S : SCHOOL ID         
         */
        public DataTable GetMstSchoolList(string searchString, int? pageNo, int? pageSize, ref int totalCount, string searchType, string lockType )
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                /*SP GetMstSchoolList CALLING WITH AFORESAID PARAMETERS*/
                oCmd.CommandText = "GetMstSchoolList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSearchType", searchType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pLockedYN", lockType);
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

        /* CHECK DUPLICATE ENTRY EMAIL/PHONE NO./MOBILE NO. FROM DATABASE ACCORDING TO THE SEARCH PARAMETER & RETURN AS INT
         * PARAM1: emailId = EMAIL ID. AS USER DATA
         * PARAM2: phoneNo= PHONE NO. AS USER DATA
         * PARAM2: mobileNo= MOBILE NO. AS USER DATA
         * PARAM3: schoolId= SCHOOL UNIQUE ID
         * PARAM4: entType= I: INSERT, E: EDIT, D: DELETE
         * PARAM5: errDesc= OUTPUT, REFERENCE PARAMETER FOR CATCHING SQL TRANSACTIONAL ERROR 
         * PARAM6: diseCode= SCHOOL UNIQUE DISE CODE
         * PARAM7: indexNo= SCHOOL UNIQUE INDEX NO.
         * PARAM8: subDivisionId= UNIQUE SUBDIVISION ID
         */
        public int ChkDuplicateContactMstSchool(string emailId, string phoneNo, string mobileNo, string schoolId, string entType, ref string errDesc, string diseCode, string indexNo, int subDivisionId)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                /*SP ChkDuplicateContactMstSchool CALLING WITH AFORESAID PARAMETERS*/
                oCmd.CommandText = "ChkDuplicateContactMstSchool";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pEmailId", emailId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pPhoneNo", phoneNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMobileNo", mobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", Convert.ToInt64(schoolId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pEntType", entType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pDISECode", diseCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pIndexNo", indexNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSubDivisionId", subDivisionId);
                DBUtility.Execute(oCmd);

                /*GETTING ERROR CODE AS 0/1, 0: SUCCESSFULL TRANSACTION & 1: FAILED TRANSACTION*/
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

                /*OUTPUT, REFERENCE PARAMETER FOR CATCHING SQL TRANSACTIONAL ERROR
                 NOTE: WE CAN VIEW THIS TRANSACTIONAL ERROR IN DEBUGGING MODE IF REQUIRED.
                 */
                errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch(Exception ex) // HANDELING THE EXCEPTION
            {
                /*SETTING ERR=1, MEANS SOME ERROR OCCURED DURIND THE EXECUTION OF TRY BLOCK*/
                err = 1;
                errDesc = "Operation Error. Details : " + ex.Message; //SETTING ERROR MESSAGE FOR VIEW
            }
            finally
            {
                oCmd = null;
            }
            return err;
        }

        /* PULLING DATA BY UNIQUE SCHOOL ID. FROM DATABASE & RETURN AS HTML DATA
        * PARAM1: schoolId = UNIQUE SCHOOL ID. AS USER DATA    
        * PARAM5: data= OUTPUT, REFERENCE PARAMETER FOR CATCHING HTML DATA         
        */
        public int GetMstSchoolView(string schoolId, ref string data)
        {
            /*SETTING ERR=0, MEANS INITIALIALLY NO ERROR OCCURED*/
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                /*SP GetMstSchoolView CALLING WITH AFORESAID PARAMETERS*/
                oCmd.CommandText = "GetMstSchoolView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", Convert.ToInt64(schoolId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);

                /*GETTING ERROR CODE AS 0/1, 0: SUCCESSFULL TRANSACTION & 1: FAILED TRANSACTION*/
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

                /*OUTPUT, REFERENCE PARAMETER FOR CATCHING HTML DATA*/
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                /*SETTING ERR=1, MEANS SOME ERROR OCCURED DURIND THE EXECUTION OF TRY BLOCK*/
                err = 1;
                data = ex.Message; //SETTING ERROR MESSAGE FOR VIEW
            }

            return err;
        }

        /* PULLING SCHOOL DETAILS DATA BY UNIQUE SCHOOL ID. FROM DATABASE & RETURN AS DATATABLE
        * PARAM1: schoolId = UNIQUE SCHOOL ID. AS USER DATA    
        */
        public DataTable GetMstSchoolDetails(string schoolId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                /*SP GetMstSchoolDetails CALLING WITH AFORESAID PARAMETERS*/
                oCmd.CommandText = "GetMstSchoolDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
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

        /* SAVE ROUTINE FOR SAVING SCHOOL MASTER DATA TO DATABASE & RETURN SUCCESS/ERROR AS INT
        * PARAM1: VMMstSchool = SCHOOL USER DATA INPUT AS VIEW MODEL LIKE SchoolId, DISECode, SchoolName ETC.
        * PARAM2: userTypeId= 1:ADMIN/2:SCHOOL/3:PRE-SCHOOL
        * PARAM2: mode= ADD/EDIT/DELETE/UNLOCK ETC. THIS IS A ADD OPERATION. 
        * PARAM3: ipAddress= USER IP ADDRESS FOR HISTORY KEEPING
        * PARAM4: createdBy= LOGGED IN UNIQUE USER ID FOR HISTORY KEEPING
        * PARAM5: errDesc= OUTPUT, REFERENCE PARAMETER FOR CATCHING SQL TRANSACTIONAL ERROR         
        * PARAM7: indexNo= SCHOOL UNIQUE INDEX NO.       
        */
        public int SaveMstSchool(VMMstSchool data, int userTypeId, string mode, string ipAddress, int createdBy, ref string errDesc, string indexNo)
        {
            SqlCommand oCmd = new SqlCommand();
            /*SETTING ERR=0, MEANS INITIALIALLY NO ERROR OCCURED*/
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;

                /*SP SaveMstSchool CALLING WITH AFORESAID PARAMETERS*/
                oCmd.CommandText = "SaveMstSchool";

                /*ADDING REQUIRED PARAMETERS*/
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.SchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pDISECode", (Sanitizer.GetSafeHtmlFragment(data.DISECode) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pSchoolName", (Sanitizer.GetSafeHtmlFragment(data.SchoolName) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSubDivisionId", Convert.ToInt32(GblFunctions.Base64Decode((data.SubDivisionId ?? DefaultSetting.DefaultValEnc).Trim())));
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pPreSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.PreSchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIndexNo", indexNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pOrderNo", (Sanitizer.GetSafeHtmlFragment(data.OrderNo) ?? DefaultSetting.EmptyVal).Trim());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pOrderDate", (GblFunctions.setDate(Sanitizer.GetSafeHtmlFragment(data.OrderDate).Trim()) ?? (DateTime?)null));
                DBUtility.Execute(oCmd);

                /*GETTING ERROR CODE AS 0/1, 0: SUCCESSFULL TRANSACTION & 1: FAILED TRANSACTION*/
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

                /*OUTPUT, REFERENCE PARAMETER FOR CATCHING SQL TRANSACTIONAL ERROR
                 NOTE: WE CAN VIEW THIS TRANSACTIONAL ERROR IN DEBUGGING MODE IF REQUIRED.
                 */
                errDesc = oCmd.Parameters["@pErrDesc"].Value.ToString();
            }
            catch (Exception ex)
            {
                /*SETTING ERR=1, MEANS SOME ERROR OCCURED DURIND THE EXECUTION OF TRY BLOCK*/
                err = 1;
                //SETTING ERROR MESSAGE FOR VIEW
                errDesc = ex.Message;
            }
            finally
            {
                oCmd = null;
            }
            return err;
        }

        /* PULLING SCHOOL LIST FROM DATABASE ACCORDING TO THE SEARCH PARAMETER & RETURN AS DATATABLE
        * PARAM1: searchString = SEARCH CRITERIA AS STRING
        * PARAM2: pageNo= INT PAGE NO.
        * PARAM2: pageSize= INT PAGE SIZE
        * PARAM3: totalCount= INT TOTAL ROW COUNT AS REFERENCE STRING (OUT PUT PARAMETER)
        * PARAM4: searchType= N : BY SCHOOL NAME, I : BY INDEX NO, S : SCHOOL ID  
        * PARAM5: subdivisionId= UNIQUE SUB-DIVISION ID   
        */
        public DataTable GetSchoolDirectory(string searchString, int subdivisionId, int? pageNo, int? pageSize, ref int totalCount, string searchType)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSchoolDirectory";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearchString", searchString);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSubDivisionId", subdivisionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageNo", pageNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pPageSize", pageSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pTotalCount", totalCount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSearchType", searchType);
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

        /* PULLING DATA BY UNIQUE SCHOOL ID. FROM DATABASE & RETURN AS HTML DATA. IT IS USED FOR SCHOOL DIRECTORY
        * PARAM1: schoolId = UNIQUE SCHOOL ID. AS USER DATA    
        * PARAM5: data= OUTPUT, REFERENCE PARAMETER FOR CATCHING HTML DATA         
        */
        public int GetSchoolView(string schoolId, ref string data)
        {
            /*SETTING ERR=0, MEANS INITIALIALLY NO ERROR OCCURED*/
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                /*SP GetMstSchoolView CALLING WITH AFORESAID PARAMETERS*/
                //oCmd.CommandText = "GetMstSchoolView";
                oCmd.CommandText = "GetSchoolView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", Convert.ToInt64(schoolId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 1, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.Execute(oCmd);

                /*GETTING ERROR CODE AS 0/1, 0: SUCCESSFULL TRANSACTION & 1: FAILED TRANSACTION*/
                err = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

                /*OUTPUT, REFERENCE PARAMETER FOR CATCHING HTML DATA*/
                data = oCmd.Parameters["@pData"].Value.ToString();
            }
            catch (Exception ex)
            {
                /*SETTING ERR=1, MEANS SOME ERROR OCCURED DURIND THE EXECUTION OF TRY BLOCK*/
                err = 1;
                data = ex.Message; //SETTING ERROR MESSAGE FOR VIEW
            }

            return err;
        }

        public DataTable GetMstSchoolDetailsByPreSchoolId(Int64 preSchoolId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();


            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstSchoolDetailsByPreSchoolId";
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

        /*FETCHING REQUIRED DATA IN HTML FORMAT FOR POPULATION OF SCHOOL ADMIN DASHBOARD ACCORDIN TO SPECIFIC SCHOOL AND USER
         * PARAM1: UNIQUE SCHOOL ID
         * PARAM2: UNIQUE USER ID
         */
        public string GetSchoolProfileDashBoardData(Int64 schoolId,int userId)
        {
            SqlCommand oCmd = new SqlCommand();
            string data = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSchoolProfileDashBoardData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 8000, "@pData", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pUserId", userId);
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

        public int UpdateSchoolProfile(VMSchoolProfile data, int userTypeId, string mode, string ipAddress, int createdBy, ref string errDesc, string indexNo)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateSchoolProfile";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.SchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
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

        public string ChkSchoolEditPermission(Int64 schoolId, string editFor)
        {
            SqlCommand oCmd = new SqlCommand();
            string data = string.Empty;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkSchoolEditPermission";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
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

        public int saveSchoolEditPermission(VMSchoolEditPermission data, string ipAddress, int createdBy, ref string errDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int err = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveSchoolEditPermission";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.SchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
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

        public DataTable downloadMstSchoolList(string searchString, int? pageNo, int? pageSize, ref int totalCount, string searchType, string lockType, int reportFormat)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                /*SP GetMstSchoolList CALLING WITH AFORESAID PARAMETERS*/
                oCmd.CommandText = "DownloadMstSchoolList";
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

        public DataTable GetMstSchoolListAutoComplete(string searchString)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMstSchoolListAutoComplete";
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

        public int GetSchoolTransferView(Int64 schoolId, string deleteYN, ref string data)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSchoolTransferView";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", schoolId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDeleteYN", deleteYN);
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

        public int SaveSchoolTransfer(VMSchoolTransfer data, string mode, string ipAddress, int createdBy, ref string errDesc)
        {
            int err = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveSchoolTransfer";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 20, "@pSchoolId", Convert.ToInt64(GblFunctions.Base64Decode((data.SchoolId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pSubDivisionId", Convert.ToInt32(GblFunctions.Base64Decode((data.SubDivisionId ?? DefaultSetting.DefaultValEnc).Trim())));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", mode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIpAddress", ipAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", createdBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 10, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
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
