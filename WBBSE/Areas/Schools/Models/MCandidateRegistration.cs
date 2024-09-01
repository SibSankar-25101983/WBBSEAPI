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
using WBBSE.Models;
using System.Xml.Serialization;
using Microsoft.Security.Application;



namespace WBBSE.Areas.Schools.Models
{
    /*******************************************************************************
      * A BRIEF HISTORY OF MCandidateRegistration.
      * CONTAINS STUDENT/CANDIDATE REGISTRATION RELATED LOGIC 
      * DAL: DATA ACCESS LAYER
      * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
      * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
      ******************************************************************************/
    public class MCandidateRegistration
    {
       
        public List<VMCandidateRegistration> GetNationalityList() //FETCHING NATIONALITY LIST FROM DATABASE
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                //DAL CALLING FOR DATA
                dt = cr.GetNationalityList();

                //LIST CONVERSION FROM DATATABLE
                var records = (from data in dt.AsEnumerable()
                               select new VMCandidateRegistration
                               {
                                   NationalityCode = data.Field<string>("NationalityCode"),
                                   NationalityName = data.Field<string>("NationalityName")
                               });

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public List<VMCandidateRegistration> GetReligionList() //FETCHING RELIGION LIST FROM DATABASE
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                //DAL CALLING FOR DATA
                dt = cr.GetReligionList();

                //LIST CONVERSION FROM DATATABLE
                var records = (from data in dt.AsEnumerable()
                               select new VMCandidateRegistration
                               {
                                   ReligionCode = data.Field<string>("ReligionCode"),
                                   ReligionName = data.Field<string>("ReligionName")
                               });

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public List<VMCandidateRegistration> GetCasteList() //FETCHING CASTE LIST FROM DATABASE
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                //DAL CALLING FOR DATA
                dt = cr.GetCasteList();

                //LIST CONVERSION FROM DATATABLE
                var records = (from data in dt.AsEnumerable()
                               select new VMCandidateRegistration
                               {
                                   CasteCode = data.Field<string>("CasteCode"),
                                   CasteName = data.Field<string>("CasteName")
                               });

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public List<VMCandidateRegistration> GetPhysicallyChallengedList() //FETCHING PHYSICALLY CHALLENGED LIST FROM DATABASE
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                //DAL CALLING FOR DATA
                dt = cr.GetPhysicallyChallengedList();

                //LIST CONVERSION FROM DATATABLE
                var records = (from data in dt.AsEnumerable()
                               select new VMCandidateRegistration
                               {
                                   PhysicallychallengedCode = data.Field<string>("PhysicallychallengedCode"),
                                   PhysicallychallengedName = data.Field<string>("PhysicallychallengedName")
                               });

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public List<VMCandidateRegistration> GetSexList() //FETCHING DATA LIST FROM DATABASE
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                //DAL CALLING FOR DATA
                dt = cr.GetSexList();

                //LIST CONVERSION FROM DATATABLE
                var records = (from data in dt.AsEnumerable()
                               select new VMCandidateRegistration
                               {
                                   SexCode = data.Field<string>("SexCode"),
                                   SexName = data.Field<string>("SexName")
                               });

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public List<VMCandidateRegistration> GetSubjectListFilter(int subID) //FETCHING DATA LIST FROM DATABASE WITH subID(SUBJECT UNIQUE ID)
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                //DAL CALLING FOR DATA
                dt = cr.GetSubjectListFilter(subID);

                //LIST CONVERSION FROM DATATABLE
                var records = (from data in dt.AsEnumerable()
                               select new VMCandidateRegistration
                               {
                                   SubjectId = data.Field<string>("SubjectId"),
                                   SubjectName = data.Field<string>("SubjectName")
                               });

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            return result;
        }

        //public List<VMCandidateRegistration> GetSubjectSymbolCode(int subID)
        //{
        //    DataTable dt = new DataTable();
        //    CandidateRegistration cr = new CandidateRegistration();
        //    List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

        //    try
        //    {
        //        dt = cr.GetSubjectSymbolCode(subID);

        //        var records = (from data in dt.AsEnumerable()
        //                       select new VMCandidateRegistration
        //                       {
        //                           SubjectCode = data.Field<string>("SubjectCode"),
        //                           SubjectSymbol = data.Field<string>("SubjectSymbol")
        //                       });

        //        result = records.ToList();
        //    }
        //    catch
        //    {
        //        result = null;
        //    }
        //    return result;
        //}


        /* FETCHING DATA LIST FROM DATABASE WITH subID(SUBJECT UNIQUE ID)
         * REFERENCE VALUE firstLanguageSymbol
         * REFERENCE VALUE firstLanguageCode
         */
        public void GetSubjectSymbolCode(int subID, ref string firstLanguageSymbol, ref string firstLanguageCode) 
        {
            DataTable dt = new DataTable();

            try
            {
                CandidateRegistration cr = new CandidateRegistration();

                //DAL CALLING FOR DATA
                dt = cr.GetSubjectSymbolCode(subID);

                if (dt != null && dt.Rows.Count == 1)
                {
                    //SETTING VALUES FROM DATATABLE
                    firstLanguageSymbol = dt.Rows[0]["SubjectSymbol"].ToString();
                    firstLanguageCode = dt.Rows[0]["SubjectCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }


        /* SAVE ROUTINE OF CANDIDATE REGISTRATION PROCESS
         * PARAM1: CANDIDATE IMAGE FILE AS HTTP POSTED FILE
         * PARAM2: VIEW MODEL OBJECT OF CANDIDATE REGISTRATION
         * PARAM3: REFERENCE PARAMETER FOR CATCHING THE ERROR         
         */
        public int saveCandidateRegistration(HttpPostedFileBase[] postedFiles, VMCandidateRegistration cr, ref string errDesc)
        {
            int error = 0;

            try
            {

                int studentId = 0, registrationYear = 0, madhyamikParikshaYear = 0, pin = 0, nationalityCode = 0, religionCode = 0, casteCode = 0,
                     physicallychallengedCode = 0, sexCode = 0, firstLanguageId = 0, firstLanguageCode = 0, secondLanguageId = 0, secondLanguageCode = 0,
                     optionalElectiveId = 0, optionalElectiveCode = 0, createdBy = 0;

                string schoolIndex = string.Empty, formNo = string.Empty, schoolName = string.Empty, studentName = string.Empty, fathersName = string.Empty,
                       mothersName = string.Empty, guardiansName = string.Empty, address = string.Empty, contactNo = string.Empty, dateOfBirth = string.Empty,
                       firstLanguageSymbol = string.Empty, secondLanguageSymbol = string.Empty, optionalElectiveSymbol = string.Empty, pupilAdmitted = string.Empty,
                       dateOfAdmission = string.Empty, studentImageExtention = string.Empty, studentImageName = string.Empty, dbStudentImagePath = string.Empty, studentSignatureName = string.Empty,
                       studentSignaturePath = string.Empty, ipAddress = string.Empty, imagePath = string.Empty;


                if (postedFiles[0] != null)
                {
                    //CANDIDATE IMAGE FILE SIZE CHECKING
                    if (postedFiles[0].ContentLength > MaxFileSize.RegImgMax && postedFiles[0].ContentLength < MaxFileSize.RegImgMin)
                    {
                        error = 2; //GENERATING THE ERROR
                        errDesc = Message.FileUpload.StudentRegImageMaxMinSize; //ERROR MESSAGE CONFIGURATION
                        return error;
                    }

                    studentImageExtention = Path.GetExtension(postedFiles[0].FileName); //CANDIDATE IMAGE EXTENTION SETTING
                    studentImageName = DateTime.Now.ToFileTime().ToString() + studentImageExtention; //CANDIDATE IMAGE NAME SETTING

                    //CHECK IMAGE FILE EXTENSION (ONLY JPEG/JPG FILE IS ALLOWED)
                    if (studentImageExtention.ToLower() != ImageExtension.JPEG && studentImageExtention.ToLower() != ImageExtension.JPG)
                    {
                        error = 2; //GENERATING THE ERROR
                        errDesc = Message.FileUpload.InvalidContentTypeImage; //ERROR MESSAGE CONFIGURATION
                        return error;
                    }
                    dbStudentImagePath = ImagePath.StudentRegImage + System.DateTime.Now.Year.ToString() + "/" + HttpContext.Current.Session[SessionNames.SubDivisionId] + "/"; //IMAGE PATH STORED IN DB
                }

                //filePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/StudentRegImage/");
                string mode = ((cr.EntType == "I") ? "ADD" : ((cr.EntType == "E") ? "EDIT" : ((cr.EntType == "D") ? "DELETE" : "ERROR")));
                if (mode == "ERROR")
                {
                    error = 1;
                }
                else
                {
                    if (mode != "ADD")
                    {
                        studentId = Convert.ToInt32(GblFunctions.Base64Decode(cr.StudentId));
                    }
                    if (mode != "DELETE")
                    {



                        schoolIndex = (string.IsNullOrEmpty(cr.SchoolIndex) == true) ? "" : cr.SchoolIndex.Trim();
                        formNo = (string.IsNullOrEmpty(cr.FormNo) == true) ? "" : cr.FormNo.Trim();
                        schoolName = (string.IsNullOrEmpty(cr.SchoolName) == true) ? "" : cr.SchoolName.Trim();
                        studentName = (string.IsNullOrEmpty(cr.StudentName) == true) ? "" : cr.StudentName.Trim();
                        fathersName = (string.IsNullOrEmpty(cr.FathersName) == true) ? "" : cr.FathersName.Trim();
                        mothersName = (string.IsNullOrEmpty(cr.MothersName) == true) ? "" : cr.MothersName.Trim();
                        guardiansName = (string.IsNullOrEmpty(cr.GuardiansName) == true) ? "" : cr.GuardiansName.Trim();
                        address = (string.IsNullOrEmpty(cr.Address) == true) ? "" : cr.Address.Trim();
                        contactNo = (string.IsNullOrEmpty(cr.ContactNo) == true) ? "" : cr.ContactNo.Trim();
                        dateOfBirth = (string.IsNullOrEmpty(cr.DateOfBirth) == true) ? "" : cr.DateOfBirth.Trim();
                        firstLanguageSymbol = (string.IsNullOrEmpty(cr.FirstLanguageSymbol) == true) ? "" : cr.FirstLanguageSymbol.Trim();
                        secondLanguageSymbol = (string.IsNullOrEmpty(cr.SecondLanguageSymbol) == true) ? "" : cr.SecondLanguageSymbol.Trim();
                        optionalElectiveSymbol = (string.IsNullOrEmpty(cr.OptionalElectiveCode) == true) ? "" : cr.OptionalElectiveSymbol.Trim();
                        pupilAdmitted = (string.IsNullOrEmpty(cr.PupilAdmitted) == true) ? "" : cr.PupilAdmitted.Trim();
                        dateOfAdmission = (string.IsNullOrEmpty(cr.DateOfAdmission) == true) ? "" : cr.DateOfAdmission.Trim();
                        //studentImageName = (string.IsNullOrEmpty(cr.StudentImageName) == true) ? "" : cr.StudentImageName.Trim();
                        //studentImagePath = (string.IsNullOrEmpty(cr.StudentImagePath) == true) ? "" : cr.StudentImagePath.Trim();
                        //studentSignatureName = (string.IsNullOrEmpty(cr.StudentSignatureName) == true) ? "" : cr.StudentSignatureName.Trim();
                        //studentSignaturePath = (string.IsNullOrEmpty(cr.StudentSignaturePath) == true) ? "" : cr.StudentSignaturePath.Trim();

                        //studentImageName = "";
                        //studentImagePath = "";
                        //studentSignatureName = "";
                        //studentSignaturePath = "";

                        registrationYear = Convert.ToInt32(cr.RegistrationYear);
                        madhyamikParikshaYear = Convert.ToInt32(cr.MadhyamikParikshaYear);
                        pin = Convert.ToInt32(cr.Pin);
                        nationalityCode = Convert.ToInt32(cr.NationalityCode);
                        religionCode = Convert.ToInt32(cr.ReligionCode);
                        casteCode = Convert.ToInt32(cr.CasteCode);
                        physicallychallengedCode = Convert.ToInt32(cr.PhysicallychallengedCode);
                        sexCode = Convert.ToInt32(cr.SexCode);
                        firstLanguageId = Convert.ToInt32(cr.FirstLanguageId);
                        firstLanguageCode = Convert.ToInt32(cr.FirstLanguageCode);
                        secondLanguageId = Convert.ToInt32(cr.SecondLanguageId);
                        secondLanguageCode = Convert.ToInt32(cr.SecondLanguageCode);
                        optionalElectiveId = Convert.ToInt32(cr.OptionalElectiveId);
                        optionalElectiveCode = Convert.ToInt32(cr.OptionalElectiveCode);
                    }

                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);
                    schoolIndex = HttpContext.Current.Session[SessionNames.OrganizationCode].ToString();


                    CandidateRegistration oCr = new CandidateRegistration();
                    error = oCr.SaveStudentRegistrationDetails(studentId, registrationYear, madhyamikParikshaYear, schoolIndex, formNo, schoolName, studentName
                                                   , fathersName, mothersName, guardiansName, address, pin, contactNo, nationalityCode,
                                                    religionCode, casteCode, physicallychallengedCode, sexCode, dateOfBirth, firstLanguageId, firstLanguageSymbol,
                                                    firstLanguageCode, secondLanguageId, secondLanguageSymbol, secondLanguageCode, optionalElectiveId, optionalElectiveSymbol, optionalElectiveCode,
                                                    pupilAdmitted, dateOfAdmission, studentImageName, dbStudentImagePath, studentSignatureName,
                                                    studentSignaturePath, ipAddress, mode, createdBy, ref errDesc);
                }

                if (error == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "CandidateRegistration/saveCandidateRegistration(MCandidateRegistration)", "SchoolCandidateRegistrationController");
                    errDesc = Message.OperationError;
                }

                // -------------------CANDIDATE IMAGE RELATED JOB------------------------------------------------------
                imagePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/StudentRegImage/");
                //if (mode == Mode.EDIT && postedFiles[0] != null)
                //{
                //    //try to delete previous file
                //    try
                //    {
                //        string prevFileName = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(cr.StudentImagePath.Trim()));
                //        prevFileName = filePath + prevFileName.Substring(prevFileName.LastIndexOf('/') + 1).Trim();

                //        if (File.Exists(prevFileName))
                //        {
                //            File.Delete(prevFileName);
                //        }
                //    }
                //    catch
                //    {
                //        //nothing to do. previous image will not be deleted
                //    }
                //}
                //else if (error == 0 && mode == Mode.DELETE)
                //{
                //    string prevFileName = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment((cr.StudentImagePath ?? string.Empty).Trim()));
                //    prevFileName = filePath + prevFileName.Substring(prevFileName.LastIndexOf('/') + 1).Trim();

                //    if (File.Exists(prevFileName))
                //    {
                //        File.Delete(prevFileName);
                //    }
                //}

                //SAVING IMAGE
                if (postedFiles[0] != null)
                {
                    if (Directory.Exists(imagePath + System.DateTime.Now.Year.ToString() + "/" + HttpContext.Current.Session[SessionNames.SubDivisionId] + "/"))
                    {
                        postedFiles[0].SaveAs(imagePath + System.DateTime.Now.Year.ToString() + "/" + HttpContext.Current.Session[SessionNames.SubDivisionId] + "/" + studentImageName);
                    }
                    else
                    {
                        Directory.CreateDirectory(imagePath + System.DateTime.Now.Year.ToString() + "/" + HttpContext.Current.Session[SessionNames.SubDivisionId] + "/");
                        postedFiles[0].SaveAs(imagePath + System.DateTime.Now.Year.ToString() + "/" + HttpContext.Current.Session[SessionNames.SubDivisionId] + "/" + studentImageName);

                    }
                    //postedFiles[0].SaveAs(filePath + studentImageName);

                }

                // -------------------CANDIDATE IMAGE RELATED JOB------------------------------------------------------
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = Message.OperationError;
                MCommon.saveExceptionLog(ex.Message, "CandidateRegistration/saveCandidateRegistration(MCandidateRegistration)", "SchoolCandidateRegistrationController");                
            }

            return error;
        }

        public List<VMCandidateRegistration> GetStudentRegistrationList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            CandidateRegistration cr = new CandidateRegistration();
            List<VMCandidateRegistration> result = new List<VMCandidateRegistration>();

            try
            {
                dt = cr.GetStudentRegistrationList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, HttpContext.Current.Session[SessionNames.OrganizationCode].ToString());

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

        public string getStudentRegistrationApprovalView(string studentId, string ViewApprovalId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = new CandidateRegistration().GetStudentRegistrationApprovalView(studentId.ToString(), ViewApprovalId, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetStudentRegistrationApprovalView/getStudentRegistrationApprovalView(MCandidateRegistration)", "SchoolCandidateRegistrationApprovalController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetStudentRegistrationApprovalView/getStudentRegistrationApprovalView(MCandidateRegistration)", "SchoolCandidateRegistrationApprovalController");
            }

            return data;
        }
    }
}