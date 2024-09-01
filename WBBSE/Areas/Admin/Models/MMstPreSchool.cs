using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Areas.Admin.Models
{
    public class MMstPreSchool
    {
        public List<VMMstPreSchool> getMstPreSchoolListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstPreSchool> result = new List<VMMstPreSchool>();

            try
            {
                dt = new PreSchool().GetMstPreSchoolListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstPreSchool
                               {
                                   PreSchoolId = data.Field<string>("PreSchoolId"),
                                   SchoolName = data.Field<string>("SchoolName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetPreSchoolList/getMstPreSchoolListDropDown(MMstPreSchool)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstPreSchool> getMstPreSchoolListAutoComplete(string searchString)
        {
            DataTable dt = new DataTable();
            List<VMMstPreSchool> result = new List<VMMstPreSchool>();

            try
            {
                dt = new PreSchool().GetMstPreSchoolListAutoComplete(searchString);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstPreSchool
                               {
                                   PreSchoolId = data.Field<string>("PreSchoolId"),
                                   SchoolName = data.Field<string>("SchoolName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetPreSchoolList/getMstPreSchoolListAutoComplete(MMstPreSchool)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstPreSchool> getMstPreSchoolList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, string searchType, string lockType)
        {
            //int err = 0;
            DataTable dt = new DataTable();
            List<VMMstPreSchool> result = new List<VMMstPreSchool>();

            try
            {                
                dt = new PreSchool().GetMstPreSchoolList((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, page, limit, ref total, searchType, lockType);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstPreSchool
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   PreSchoolId = data.Field<string>("PreSchoolId"),
                                   SchoolCode = data.Field<string>("SchoolCode"),
                                   DISECode = data.Field<string>("DISECode"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   //SubDivisionId = data.Field<string>("SubDivisionId"),
                                   //SubDivisionName = data.Field<string>("SubDivisionName"),
                                   //DistrictName = data.Field<string>("DistrictName"),
                                   ZoneName = data.Field<string>("ZoneName"),
                                   Designation = data.Field<string>("Designation"),
                                   PhoneNo = data.Field<string>("ContactNo"),
                                   PreSchoolEditPermissionStatus = data.Field<string>("SchoolEditPermissionStatus"),
                                   MigYN = data.Field<string>("MigYN"),
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetMstPreSchoolList/getMstPreSchoolList(MMstPreSchool)", "AdminPreSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getMstPreSchoolView(string preSchoolId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = new PreSchool().GetMstPreSchoolView(preSchoolId, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetMstPreSchoolView/getMstSchoolView(MMstPreSchool)", "AdminPreSchoolController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetMstPreSchoolView/getMstSchoolView(MMstPreSchool)", "AdminPreSchoolController");
            }

            return data;
        }

        public List<VMMstPreSchool> getMstPreSchoolDetails(string preSchoolId)
        {
            DataTable dt = new DataTable();
            List<VMMstPreSchool> result = new List<VMMstPreSchool>();

            try
            {
                dt = new PreSchool().GetMstPreSchoolDetails(preSchoolId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstPreSchool
                               {
                                   PreSchoolId = data.Field<string>("PreSchoolId"),
                                   SchoolCode = data.Field<string>("SchoolCode"),
                                   //IndexNo = data.Field<string>("IndexNo"),
                                   DISECode = data.Field<string>("DISECode"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   //SubDivisionId = data.Field<string>("SubDivisionId"),
                                   //CircleId = data.Field<string>("CircleId"),
                                   SchoolTypeId = data.Field<string>("SchoolTypeId"),
                                   SchoolCategoryId = data.Field<string>("SchoolCategoryId"),
                                   SchoolStatusId = data.Field<string>("SchoolStatusId"),
                                   SchoolMediumId = data.Field<string>("SchoolMediumId"),
                                   SchoolRecognitionId = data.Field<string>("SchoolRecognitionId"),
                                   SchoolManagementId = data.Field<string>("SchoolManagementId"),
                                   DesignationId = data.Field<string>("DesignationId"),
                                   AddressLine1 = data.Field<string>("AddressLine1"),
                                   AddressLine2 = data.Field<string>("AddressLine2"),
                                   GramPanchayet = data.Field<string>("GramPanchayet"),
                                   StdCode = data.Field<string>("StdCode"),
                                   PostOffice = data.Field<string>("PostOffice"),
                                   PoliceStation = data.Field<string>("PoliceStation"),
                                   City = data.Field<string>("City"),
                                   DistrictId = data.Field<string>("DistrictId"),
                                   PinCode = data.Field<string>("PinCode"),
                                   PhoneNo = data.Field<string>("PhoneNo"),
                                   MobileNo = data.Field<string>("MobileNo"),
                                   FaxNo = data.Field<string>("FaxNo"),
                                   Website = data.Field<string>("Website"),
                                   EmailId = data.Field<string>("EmailId"),
                                   SchoolHeadSalutationId = data.Field<string>("SalutationId"),
                                   SchoolHeadFirstName = data.Field<string>("FirstName"),
                                   SchoolHeadMiddleName = data.Field<string>("MiddleName"),
                                   SchoolHeadLastName = data.Field<string>("LastName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetMstPreSchoolDetails/getMstPreSchoolDetails(MMstPreSchool)", "AdminPreSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int chkDuplicateContactMstPreSchool(string emailId, string phoneNo, string mobileNo, string preSchoolId, string entType, ref string errDesc, string diseCode)
        {
            int err = 0;
            PreSchool ps = new PreSchool();
            try
            {
                if (preSchoolId == DefaultSetting.DefaultValEnc)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                }
                else
                {
                    err = ps.ChkDuplicateContactMstPreSchool(emailId, phoneNo, mobileNo, preSchoolId, entType, ref errDesc, diseCode);
                }

                errDesc = HttpContext.Current.Server.HtmlDecode(errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "PreSchools/chkDuplicateContactMstPreSchool(MMstPreSchool)", "AdminPreSchoolController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PreSchools/chkDuplicateContactMstPreSchool(MMstPreSchool)", "AdminPreSchoolController");
            }

            return err;
        }

        public int saveMstPreSchool(VMMstPreSchool data, ref string errDesc)
        {
            int err = 0, createdBy = 0, userTypeid = 0;
            string mode = string.Empty, schoolName = string.Empty, diseCode = string.Empty, addressLine1 = string.Empty, addressLine2 = string.Empty, city = string.Empty, pinCode = string.Empty, mobileNo = string.Empty, emailId = string.Empty, 
                ipAddress = string.Empty;
            Int64 preSchoolId = 0;
            PreSchool ps = new PreSchool();
            try
            {
                mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : ((data.EntType == EntType.UNLOCK) ? Mode.UNLOCK : Mode.ERROR))));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                }
                #region VALIDATIONS
                if (mode != Mode.ADD)
                {
                    //check whether pre-school id is present in proper format or not. if not, runtime error will be created.
                    preSchoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.PreSchoolId))); //PreSchoolId is base64 encrypted.
                }
                //check whether admin has edit/delete rights or not
                if (mode == Mode.EDIT) //|| mode == Mode.DELETE
                {
                    string editYN = chkPreSchoolEditPermission(preSchoolId, SchoolProfileEditFor.Admin);

                    if (editYN == "N")
                    {
                        err = 2;
                        errDesc = Message.School.EditNotPermitted;
                        return err;
                    }
                }
                if (mode != Mode.DELETE && mode != Mode.UNLOCK)
                {
                    //in case of add, pre-school id, index no must be present
                    if (mode == Mode.ADD)
                    {
                        try
                        {
                            preSchoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.PreSchoolId)));

                            if (preSchoolId <= 0)
                            {
                                err = 2;
                                errDesc = Message.School.PreSchoolIdRequired;
                                return err;
                            }
                        }
                        catch
                        {
                            err = 2;
                            errDesc = Message.School.PreSchoolIdRequired;
                            return err;
                        }                        
                    }

                    //school name
                    schoolName = Sanitizer.GetSafeHtmlFragment((data.SchoolName ?? "").Trim());
                    if (string.IsNullOrEmpty(schoolName))
                    {
                        err = 2;
                        errDesc = Message.School.SchoolNameRequired;
                        return err;
                    }
                    if (!GblFunctions.chkDataFormat(RegexType.Alpha, schoolName))
                    {
                        err = 2;
                        errDesc = Message.School.InvalidSchoolName;
                        return err;
                    }

                    //dise code
                    diseCode = Sanitizer.GetSafeHtmlFragment((data.DISECode ?? "").Trim());
                    if (string.IsNullOrEmpty(diseCode))
                    {
                        err = 2;
                        errDesc = Message.School.DISECodeRequired;
                        return err;
                    }
                    if (!string.IsNullOrEmpty(diseCode))
                    {
                        Match match = Regex.Match(diseCode, @"^([0-9]{11})$", RegexOptions.IgnoreCase);
                        if (!match.Success)
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidDISECode;
                            return err;
                        }
                    }

                    ////subdivision
                    //try
                    //{
                    //    int subDivisionId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SubDivisionId)));
                    //    if (subDivisionId <= 0)
                    //    {
                    //        err = 2;
                    //        errDesc = Message.School.SubDivisionRequired;
                    //        return err;
                    //    }
                    //}
                    //catch
                    //{
                    //    err = 2;
                    //    errDesc = Message.School.SubDivisionRequired;
                    //    return err;
                    //}

                    addressLine1 = Sanitizer.GetSafeHtmlFragment((data.AddressLine1 ?? "").Trim());
                    addressLine2 = Sanitizer.GetSafeHtmlFragment((data.AddressLine2 ?? "").Trim());
                    city = Sanitizer.GetSafeHtmlFragment((data.City ?? "").Trim());
                    pinCode = Sanitizer.GetSafeHtmlFragment((data.PinCode ?? "").Trim());
                    mobileNo = Sanitizer.GetSafeHtmlFragment((data.MobileNo ?? "").Trim());
                    emailId = Sanitizer.GetSafeHtmlFragment((data.EmailId ?? "").Trim());
                    //In Edit Operation following fields are mendatory.
                    if (mode == Mode.EDIT)
                    {
                        //Street required checking
                        if (string.IsNullOrEmpty(addressLine1))
                        {
                            err = 2;
                            errDesc = Message.School.StreetNameRequired;
                            return err;
                        }
                        //Area required checking
                        if (string.IsNullOrEmpty(addressLine2))
                        {
                            err = 2;
                            errDesc = Message.School.AreaNameRequired;
                            return err;
                        }
                        //city required checking
                        if (string.IsNullOrEmpty(city))
                        {
                            err = 2;
                            errDesc = Message.School.CityRequired;
                            return err;
                        }
                        //pin code required checking
                        if (string.IsNullOrEmpty(pinCode))
                        {
                            err = 2;
                            errDesc = Message.School.PinCodeRequired;
                            return err;
                        }
                        //mobile required checking 
                        if (string.IsNullOrEmpty(mobileNo))
                        {
                            err = 2;
                            errDesc = Message.School.MobileNoRequired;
                            return err;
                        }
                        //email id required checking                       
                        if (string.IsNullOrEmpty(emailId))
                        {
                            err = 2;
                            errDesc = Message.School.EmailIdRequired;
                            return err;
                        }
                        //school type required checking
                        try
                        {
                            int schoolTypeId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolTypeId)));
                            if (schoolTypeId <= 0)
                            {
                                err = 2;
                                errDesc = Message.School.SchoolTypeRequired;
                                return err;
                            }
                        }
                        catch
                        {
                            err = 2;
                            errDesc = Message.School.SchoolTypeRequired;
                            return err;
                        }

                        //school category required
                        try
                        {
                            int schoolCategoryId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolCategoryId)));
                            if (schoolCategoryId <= 0)
                            {
                                err = 2;
                                errDesc = Message.School.SchoolCategoryRequired;
                                return err;
                            }
                        }
                        catch
                        {
                            err = 2;
                            errDesc = Message.School.SchoolCategoryRequired;
                            return err;
                        }

                        //school status required
                        try
                        {
                            int schoolStatusId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolStatusId)));
                            if (schoolStatusId <= 0)
                            {
                                err = 2;
                                errDesc = Message.School.SchoolStatusRequired;
                                return err;
                            }
                        }
                        catch
                        {
                            err = 2;
                            errDesc = Message.School.SchoolStatusRequired;
                            return err;
                        }

                        //school medium required
                        try
                        {
                            int schoolMediumId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolMediumId)));
                            if (schoolMediumId <= 0)
                            {
                                err = 2;
                                errDesc = Message.School.SchoolMediumRequired;
                                return err;
                            }
                        }
                        catch
                        {
                            err = 2;
                            errDesc = Message.School.SchoolMediumRequired;
                            return err;
                        }

                        //school recognization required
                        try
                        {
                            int schoolRecognitionId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolRecognitionId)));
                            if (schoolRecognitionId <= 0)
                            {
                                err = 2;
                                errDesc = Message.School.SchoolRecognizationRequired;
                                return err;
                            }
                        }
                        catch
                        {
                            err = 2;
                            errDesc = Message.School.SchoolRecognizationRequired;
                            return err;
                        }

                        //school management required
                        try
                        {
                            int schoolManagementId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolManagementId)));
                            if (schoolManagementId <= 0)
                            {
                                err = 2;
                                errDesc = Message.School.SchoolManagementRequired;
                                return err;
                            }
                        }
                        catch
                        {
                            err = 2;
                            errDesc = Message.School.SchoolManagementRequired;
                            return err;
                        }
                    }

                    //address1 (street): Special character checking
                    if (!string.IsNullOrEmpty(addressLine1))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, addressLine1))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidStreetName;
                            return err;
                        }
                    }
                    //address2 (Area): Special character checking
                    if (!string.IsNullOrEmpty(addressLine2))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, addressLine2))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidAreaName;
                            return err;
                        }
                    }

                    //gramPanchayet: Special character checking (if not blank or empty)
                    string gramPanchayet = Sanitizer.GetSafeHtmlFragment((data.GramPanchayet ?? "").Trim());
                    if (!string.IsNullOrEmpty(gramPanchayet))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, gramPanchayet))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidGramPanchayetName;
                            return err;
                        }
                    }

                    //postOffice: Special character checking (if not blank or empty)
                    string postOffice = Sanitizer.GetSafeHtmlFragment((data.PostOffice ?? "").Trim());
                    if (!string.IsNullOrEmpty(postOffice))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, postOffice))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidPOName;
                            return err;
                        }
                    }
                    //policeStation: Special character checking (if not blank or empty)
                    string policeStation = Sanitizer.GetSafeHtmlFragment((data.PoliceStation ?? "").Trim());
                    if (!string.IsNullOrEmpty(policeStation))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, policeStation))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidPSName;
                            return err;
                        }
                    }
                    //city: Special character checking (if not blank or empty)
                    if (!string.IsNullOrEmpty(city))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, city))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidCityName;
                            return err;
                        }
                    }
                    //pinCode: Special character checking (if not blank or empty)
                    if (!string.IsNullOrEmpty(pinCode))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.PinCode, pinCode))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidPinCode;
                            return err;
                        }
                    }
                    //std code: Special character checking (if not blank or empty)
                    string stdCode = Sanitizer.GetSafeHtmlFragment((data.StdCode ?? "").Trim());
                    if (!string.IsNullOrEmpty(stdCode))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.StdCode, stdCode))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidSTDCode;
                            return err;
                        }
                    }
                    //phone no: Special character checking (if not blank or empty)
                    string phoneNo = Sanitizer.GetSafeHtmlFragment((data.PhoneNo ?? "").Trim());
                    if (!string.IsNullOrEmpty(phoneNo))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.PhoneNo, phoneNo))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidContactNo;
                            return err;
                        }
                    }
                    //mobile no: Special character checking (if not blank or empty)
                    if (!string.IsNullOrEmpty(mobileNo))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.MobileNo, mobileNo))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidMobileNo;
                            return err;
                        }
                    }
                    //fax no: Numeric checking (if not blank or empty)
                    string faxNo = Sanitizer.GetSafeHtmlFragment((data.FaxNo ?? "").Trim());
                    if (!string.IsNullOrEmpty(faxNo))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Numeric, faxNo))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidFaxNo;
                            return err;
                        }
                    }

                    //if std code present, either phone no or fax no required
                    if (!string.IsNullOrEmpty(stdCode))
                    {
                        if (string.IsNullOrEmpty(phoneNo) && string.IsNullOrEmpty(faxNo))
                        {
                            err = 2;
                            errDesc = Message.School.PhoneNoOrFaxNoRequired;
                            return err;
                        }
                    }

                    //if phone no present, std code required
                    if (phoneNo != "" && stdCode == "")
                    {
                        err = 2;
                        errDesc = Message.School.STDCodePhoneNoRequired;
                        return err;
                    }

                    //if fax no present, std code required
                    if (faxNo != "" && stdCode == "")
                    {
                        err = 2;
                        errDesc = Message.School.STDCodeFaxNoRequired;
                        return err;
                    }

                    //emailId: Format checking (if not blank or empty)
                    if (!string.IsNullOrEmpty(emailId))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.EmailId, emailId))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidEmailId;
                            return err;
                        }
                    }
                    //website: Format checking (if not blank or empty)
                    string website = Sanitizer.GetSafeHtmlFragment((data.Website ?? "").Trim());
                    if (!string.IsNullOrEmpty(website))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.URL, website))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidURL;
                            return err;
                        }
                    }

                    //school head name checking
                    string firstName = Sanitizer.GetSafeHtmlFragment((data.SchoolHeadFirstName ?? "").Trim());
                    if (!string.IsNullOrEmpty(firstName))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, firstName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidFirstName;
                            return err;
                        }
                    }
                    string middleName = Sanitizer.GetSafeHtmlFragment((data.SchoolHeadMiddleName ?? "").Trim());
                    if (!string.IsNullOrEmpty(middleName))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, middleName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidMiddleName;
                            return err;
                        }
                    }
                    string lastName = Sanitizer.GetSafeHtmlFragment((data.SchoolHeadLastName ?? "").Trim());
                    if (!string.IsNullOrEmpty(lastName))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, lastName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidLastName;
                            return err;
                        }
                    }

                    //designation checking
                    try
                    {
                        int designationId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.DesignationId)));
                        if (designationId <= 0)
                        {
                            err = 2;
                            errDesc = Message.School.DesignationRequired;
                            return err;
                        }
                    }
                    catch
                    {
                        err = 2;
                        errDesc = Message.School.DesignationRequired;
                        return err;
                    }
                    
                    //check duplicate contact details
                    err = chkDuplicateContactMstPreSchool((Sanitizer.GetSafeHtmlFragment(data.EmailId) ?? "").Trim(), (Sanitizer.GetSafeHtmlFragment(data.PhoneNo) ?? "").Trim(), (Sanitizer.GetSafeHtmlFragment(data.MobileNo) ?? "").Trim()
                                                        , GblFunctions.Base64Decode((data.PreSchoolId ?? DefaultSetting.DefaultValEnc).Trim())
                                                        , (data.EntType ?? "").Trim(), ref errDesc, (Sanitizer.GetSafeHtmlFragment(data.DISECode) ?? "").Trim());

                    if (err > 0)
                    {
                        err = 2;
                        return err;
                    }
                }

                //check whether delete is permitted or not
                if (mode == Mode.DELETE)
                {
                    string masterTableName = "MstPreSchool";
                    string fieldName = "PreSchoolId";

                    //check whether school is migrated or not
                    err = new MCommon().chkMigration(masterTableName, fieldName, preSchoolId.ToString());

                    if (err > 0)
                    {
                        err = 2;
                        errDesc = Message.School.DeleteNotPermittedMigration;
                        return err;
                    }
                }
                #endregion

                userTypeid = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserTypeId]);
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = ps.saveMstPreSchool(data, userTypeid, mode, ipAddress, createdBy, ref errDesc);

            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }

            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "PreSchools/saveMstPreSchool(MMstPreSchool)", "AdminPreSchoolController");
            }

            return err;
        }

        public string chkPreSchoolEditPermission(Int64 preSchoolId, string editFor)
        {
            string data = string.Empty;

            try
            {
                data = new PreSchool().ChkPreSchoolEditPermission(preSchoolId, editFor);
            }
            catch (Exception ex)
            {
                data = "N";
                MCommon.saveExceptionLog(ex.Message, "ChkEdit/chkPreSchoolEditPermission(MMstPreSchool)", "AdminPreSchoolController");
            }

            return data;
        }

        public int unLockPermission(VMPreSchoolEditPermission data, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            string preSchoolId = string.Empty, schoolName = string.Empty, ipAddress = string.Empty;

            try
            {
                //school Id               
                preSchoolId = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment((data.PreSchoolId ?? "").Trim()));
                if (string.IsNullOrEmpty(preSchoolId))
                {
                    err = 2;
                    errDesc = Message.School.SchoolIdRequired;
                    return err;
                }
                //Check User Supper Admin(WBBSE Authority) is allowed or not for UnLock permission
                if (Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]) != 1)
                {
                    string d = chkPreSchoolEditPermission(Convert.ToInt64(preSchoolId), SchoolProfileEditFor.SuperAdmin);

                    if (d == "N")
                    {
                        err = 2;
                        errDesc = Message.School.UnLockNotPermitted;
                        return err;
                    }
                }

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new PreSchool().savePreSchoolEditPermission(data, ipAddress, createdBy, ref errDesc);

            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }

            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "PreSchoolEditPermission/unLockPermission(MMstPreSchool)", "PreSchoolEditPermissionController");
            }

            return err;
        }

        public DataTable downloadMstPreSchoolList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, string searchType, string lockType, int reportFormat)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new PreSchool().downloadMstPreSchoolList((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, page, limit, ref total, searchType, lockType, reportFormat);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "DownloadReport/downloadMstPreSchoolList(MMstPreSchool)", "AdminPreSchoolController");
            }

            return dt;
        }
    }
}
