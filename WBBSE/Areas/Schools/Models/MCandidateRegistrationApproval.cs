using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel;
using DAL;
using Common;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WBBSE.Models;
using Microsoft.Security.Application;

namespace WBBSE.Areas.Schools.Models
{
    /*******************************************************************************
      * A BRIEF HISTORY OF MCandidateRegistrationApproval.
      * CONTAINS CANDIDATE REGISTRATION APPROVAL  
      * DAL: DATA ACCESS LAYER
      * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
      * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
      ******************************************************************************/
    public class MCandidateRegistrationApproval
    {
        /* saveCandidateRegistration-:THIS METHOD IS USED FOR SAVE APPROVAL PERMISSION.
         * PARAMETER LIST ARE
         * studentId-:STUDENT REGISTRATION ID(THIS IS UNIQUE ID,USED FOR UNIQUELY IDENTIFY).
         * approvalBy-:AUTO GENERATED ID(THIS IS UNIQUE ID,USED FOR UNIQULEY IDENTIFY APPROVAL TABLE)
         * updatedBy-:BY PASS USERID WHO APPROVE THE PERMISSION 
         * ipAddress-:USER IP ADDRESS
               */
        public int saveCandidateRegistration(VMCandidateRegistration cr, ref string errDesc)
        {
            /* error IS AN INTEGER VARIABLE.IT IS USED FOR SAVE STATUS.
             * IF error RETURN 1 MEANS EXCEPTION OCCURED.
             * IF error RETURN 0 MEANS SUCCESSFULLY EXECUTION.
              */
            int error = 0;

            try
            {
                int studentId = 0, updatedBy = 0;
                string approvalBy = string.Empty,ipAddress = string.Empty;

                    approvalBy = StudentRegistrationApproval.Others;
                    studentId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(cr.StudentId)));
                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    updatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                    /* CandidateRegistration IS AN DAL OBJECT(DATA ACCESS LAYER).
                     * DAL.SaveStudentRegistrationApproval-:THIS METHOD IS USED FOR SAVE APPROVAL PERMISSION.IT CONTAINTS 5 PARAMETERS,
                     * LIKE studentId, approvalBy, ipAddress, updatedBy, errDesc.
                       */
                    CandidateRegistration oCr = new CandidateRegistration();
                    error = oCr.SaveStudentRegistrationApproval(studentId, approvalBy, ipAddress, updatedBy, ref errDesc);
                    if (error == 1)
                    {
                        MCommon.saveExceptionLog(errDesc, "CandidateRegistrationApproval/saveCandidateRegistration(MCandidateRegistrationApproval)", "SchoolCandidateRegistrationApprovalController");
                        errDesc = Message.OperationError;
                    }
                
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = Message.OperationError;
                MCommon.saveExceptionLog(ex.Message, "CandidateRegistrationApproval/saveCandidateRegistration(MCandidateRegistrationApproval)", "SchoolCandidateRegistrationApprovalController");

            }

            return error;
        }

        public List<VMCandidateRegistration> GetStudentRegistrationApprovalDetailsList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                dt = cr.GetStudentRegistrationApprovalDetailsList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, HttpContext.Current.Session[SessionNames.OrganizationCode].ToString());

                var records = (from data in dt.AsEnumerable()
                               select new VMCandidateRegistration
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   StudentId = data.Field<string>("StudentId"),
                                   RegistrationYear = data.Field<string>("RegistrationYear"),
                                   MadhyamikParikshaYear = data.Field<string>("MadhyamikParikshaYear"),
                                   SchoolIndex = data.Field<string>("SchoolIndex"),
                                   FormNo = data.Field<string>("FormNo"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   StudentName = data.Field<string>("StudentName"),
                                   FathersName = data.Field<string>("FathersName"),
                                   MothersName = data.Field<string>("MothersName"),
                                   GuardiansName = data.Field<string>("GuardiansName"),
                                   Address = data.Field<string>("Address"),
                                   Pin = data.Field<string>("Pin"),
                                   ContactNo = data.Field<string>("ContactNo"),
                                   NationalityCode = data.Field<string>("NationalityCode"),
                                   ReligionCode = data.Field<string>("ReligionCode"),
                                   CasteCode = data.Field<string>("CasteCode"),
                                   PhysicallychallengedCode = data.Field<string>("PhysicallychallengedCode"),
                                   SexCode = data.Field<string>("SexCode"),
                                   DateOfBirth = data.Field<string>("DateOfBirth"),
                                   FirstLanguageId = data.Field<string>("FirstLanguageId"),
                                   FirstLanguageSymbol = data.Field<string>("FirstLanguageSymbol"),
                                   FirstLanguageCode = data.Field<string>("FirstLanguageCode"),
                                   SecondLanguageId = data.Field<string>("SecondLanguageId"),
                                   SecondLanguageSymbol = data.Field<string>("SecondLanguageSymbol"),
                                   SecondLanguageCode = data.Field<string>("SecondLanguageCode"),
                                   OptionalElectiveId = data.Field<string>("OptionalElectiveId"),
                                   OptionalElectiveSymbol = data.Field<string>("OptionalElectiveSymbol"),
                                   OptionalElectiveCode = data.Field<string>("OptionalElectiveCode"),
                                   PupilAdmitted = data.Field<string>("PupilAdmitted"),
                                   DateOfAdmission = data.Field<string>("DateOfAdmission"),
                                   //StudentImageName = data.Field<string>("StudentImageName"),
                                   //StudentImagePath = data.Field<string>("StudentImagePath"),
                                   //StudentSignatureName = data.Field<string>("StudentSignatureName"),
                                   //StudentSignaturePath = data.Field<string>("StudentSignaturePath")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            finally
            {
                cr = null;
                dt = null;
            }
            return result;
        }

        /* getStudentRegistrationApprovalView-:THIS METHOD IS USED FOR VIEW CANDIDATE DETAILS.IT REQUIRED TWO PARAMETERS LIKE studentId AND ViewApprovalId.
         * studentId-:STUDENT REGISTRATION ID(THIS IS UNIQUE ID,USED FOR UNIQUELY IDENTIFY).
         * ViewApprovalId-:VIEW WITH APPROVAL PERMISSION OR ONLY VIEW PERMISSION
               */
        public string getStudentRegistrationApprovalView(string studentId, string ViewApprovalId)
        {
            int err = 0;
            string data = string.Empty;
      
            try
            {
                /* CandidateRegistration IS AN DAL OBJECT(DATA ACCESS LAYER).
                 * DAL.GetStudentRegistrationApprovalView-:THIS METHOD IS USED FOR VIEW CANDIDATE DETAILS.
                   */
                err = new CandidateRegistration().GetStudentRegistrationApprovalView(studentId.ToString(),ViewApprovalId, ref data);
                if (err == 1)
                {
                    /*SAVING EXCEPTION LOG IN DATABASE FOR THIS SPECIFIC JOB [PARAM1: EX MESSAGE, PARAM2:MODELCLASS & FUNCTION NAME, PARAM3:CONTROLLER NAME]*/
                    MCommon.saveExceptionLog(data, "GetStudentRegistrationApprovalView/getStudentRegistrationApprovalView(MCandidateRegistrationApproval)", "SchoolCandidateRegistrationApprovalController");
                }
            }
            catch (Exception ex)
            {
                /*SETTING ERROR IF EXCEPTION OCCURE*/
                /*SAVING EXCEPTION LOG IN DATABASE FOR THIS SPECIFIC JOB [PARAM1: EX MESSAGE, PARAM2:MODELCLASS & FUNCTION NAME, PARAM3:CONTROLLER NAME]*/
                MCommon.saveExceptionLog(ex.Message, "GetStudentRegistrationApprovalView/getStudentRegistrationApprovalView(MCandidateRegistrationApproval)", "SchoolCandidateRegistrationApprovalController");
            }

            return data;
        }
    }
}