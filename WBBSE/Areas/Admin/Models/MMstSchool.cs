using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;
using System.Text.RegularExpressions;

namespace WBBSE.Areas.Admin.Models
{
    public class MMstSchool
    {
        School s = new School();

        public List<VMMstSchool> getMstSchoolList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, string searchType, string lockType)
        {
            DataTable dt = new DataTable();
            List<VMMstSchool> result = new List<VMMstSchool>();

            try
            {
                dt = s.GetMstSchoolList((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, page, limit, ref total, searchType, lockType);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchool
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   SchoolId = data.Field<string>("SchoolId"),
                                   IndexNo = data.Field<string>("IndexNo"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   SubDivisionId = data.Field<string>("SubDivisionId"),
                                   SubDivisionName = data.Field<string>("SubDivisionName"),
                                   DistrictName = data.Field<string>("DistrictName"),
                                   ZoneName = data.Field<string>("ZoneName"),
                                   Designation = data.Field<string>("Designation"),
                                   PhoneNo = data.Field<string>("ContactNo"),
                                   SchoolEditPermissionStatus = data.Field<string>("SchoolEditPermissionStatus"),
                                   DeletePermissionCount = data.Field<int>("DeletePermissionCount"),
                                   MigYN = data.Field<string>("MigYN")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetMstSchoolList/getMstSchoolList(MMstSchool)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int chkDuplicateContactMstSchool(string emailId, string phoneNo, string mobileNo, string schoolId, string entType, ref string errDesc, string diseCode, string indexNo, string subDivisionId)
        {
            int err = 0;

            try
            {
                if (schoolId == DefaultSetting.DefaultValEnc)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                }
                else
                {
                    err = s.ChkDuplicateContactMstSchool(emailId, phoneNo, mobileNo, schoolId, entType, ref errDesc, diseCode, indexNo, Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(subDivisionId))));
                }

                errDesc = HttpContext.Current.Server.HtmlDecode(errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "Schools/chkDuplicateContactMstSchool(MMstSchool)", "AdminSchoolController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Schools/chkDuplicateContactMstSchool(MMstSchool)", "AdminSchoolController");
            }

            return err;
        }

        public string getMstSchoolView(string schoolId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = s.GetMstSchoolView(schoolId, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetSchoolView/getMstSchoolView(MMstSchool)", "AdminSchoolController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolView/getMstSchoolView(MMstSchool)", "AdminSchoolController");
            }

            return data;
        }

        public List<VMMstSchool> getMstSchoolDetails(string schoolId)
        {
            DataTable dt = new DataTable();
            List<VMMstSchool> result = new List<VMMstSchool>();

            try
            {
                dt = s.GetMstSchoolDetails(schoolId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchool
                               {
                                   SchoolId = data.Field<string>("SchoolId"),
                                   IndexNo = data.Field<string>("IndexNo"),
                                   DISECode = data.Field<string>("DISECode"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   SubDivisionId = data.Field<string>("SubDivisionId"),
                                   CircleId = data.Field<string>("CircleId"),
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
                                   SchoolHeadLastName = data.Field<string>("LastName"),
                                   OrderDetails = data.Field<string>("OrderDetails")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolDetails/getMstSchoolDetails(MMstSchool)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSchool> getMstSchoolDetailsByPreSchoolId(Int64 preSchoolId)
        {
            DataTable dt = new DataTable();
            List<VMMstSchool> result = new List<VMMstSchool>();

            try
            {
                dt = s.GetMstSchoolDetailsByPreSchoolId(preSchoolId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchool
                               {
                                   DISECode = data.Field<string>("DISECode"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   CircleId = data.Field<string>("CircleId"),
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
                MCommon.saveExceptionLog(ex.Message, "GetPreSchoolDetails/getMstSchoolDetailsByPreSchoolId(MMstSchool)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveMstSchool(VMMstSchool data, ref string errDesc)
        {
            int err = 0, createdBy = 0, userTypeid = 0;
            string mode = string.Empty, ipAddress = string.Empty, indexNo = string.Empty, addressLine1 = string.Empty, addressLine2 = string.Empty;
            string city = string.Empty, pinCode = string.Empty, mobileNo = string.Empty, emailId = string.Empty;
            Int64 schoolId = 0;

            try
            {
                //((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));
                mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : ((data.EntType == EntType.UNLOCK) ? Mode.UNLOCK : Mode.ERROR))));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                #region VALIDATIONS
                if (mode != Mode.ADD)
                {
                    //check whether school id is present in proper format or not. if not, runtime error will be created.
                    schoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolId)));
                }
                //check whether admin has edit right or not
                if (mode == Mode.EDIT)
                {
                    string editYN = chkSchoolEditPermission(schoolId, SchoolProfileEditFor.Admin);

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
                            Int64 preSchoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.PreSchoolId)));

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

                        indexNo = (Sanitizer.GetSafeHtmlFragment(data.IndexNo ?? "").Trim()).ToString();
                        if (indexNo.Length != 3)
                        {
                            err = 2;
                            errDesc = Message.School.InvalidIndexNo;
                            return err;
                        }

                        Match match = Regex.Match(indexNo, @"^([0-9]{3})$", RegexOptions.IgnoreCase);
                        if (!match.Success)
                        {
                            err = 2;
                            errDesc = Message.School.InvalidIndexNo;
                            return err;
                        }

                        string orderNo = (Sanitizer.GetSafeHtmlFragment(data.OrderNo ?? "").Trim()).ToString();
                        if (string.IsNullOrEmpty(orderNo))
                        {
                            err = 2;
                            errDesc = Message.School.OrderNoRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, orderNo))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidOrderNo;
                            return err;
                        }
                        string orderDate = (Sanitizer.GetSafeHtmlFragment(data.OrderDate ?? "").Trim()).ToString();
                        if (string.IsNullOrEmpty(orderDate))
                        {
                            err = 2;
                            errDesc = Message.School.OrderDateRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.Date, orderDate))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidOrderDate;
                            return err;
                        }
                    }

                    //school name
                    string schoolName = Sanitizer.GetSafeHtmlFragment((data.SchoolName ?? "").Trim());
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
                    string diseCode = Sanitizer.GetSafeHtmlFragment((data.DISECode ?? "").Trim());
                    if (diseCode == "")
                    {
                        err = 2;
                        errDesc = Message.School.DISECodeRequired;
                        return err;
                    }
                    if (diseCode != "")
                    {
                        Match match = Regex.Match(diseCode, @"^([0-9]{11})$", RegexOptions.IgnoreCase);
                        if (!match.Success)
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidDISECode;
                            return err;
                        }
                    }

                    //subdivision
                    try
                    {
                        int subDivisionId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SubDivisionId)));
                        if (subDivisionId <= 0)
                        {
                            err = 2;
                            errDesc = Message.School.SubDivisionRequired;
                            return err;
                        }
                    }
                    catch
                    {
                        err = 2;
                        errDesc = Message.School.SubDivisionRequired;
                        return err;
                    }

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
                        //mobile checking 
                        if (string.IsNullOrEmpty(mobileNo))
                        {
                            err = 2;
                            errDesc = Message.School.MobileNoRequired;
                            return err;
                        }
                        //email id                        
                        if (string.IsNullOrEmpty(emailId))
                        {
                            err = 2;
                            errDesc = Message.School.EmailIdRequired;
                            return err;
                        }
                        //school type required
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

                    //address1: Special character checking
                    if (addressLine1 != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, addressLine1))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidStreetName;
                            return err;
                        }
                    }
                    //address2: Special character checking
                    if (addressLine2 != "")
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
                    if (gramPanchayet != "")
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
                    if (postOffice != "")
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
                    if (policeStation != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, policeStation))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidPSName;
                            return err;
                        }
                    }
                    //city: Special character checking (if not blank or empty)
                    if (city != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, city))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidCityName;
                            return err;
                        }
                    }
                    //pinCode: Special character checking (if not blank or empty)
                    if (pinCode != "")
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
                    if (stdCode != "")
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
                    if (phoneNo != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.PhoneNo, phoneNo))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidContactNo;
                            return err;
                        }
                    }
                    //mobile no: Special character checking (if not blank or empty)
                    if (mobileNo != "")
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
                    if (faxNo != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Numeric, faxNo))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidFaxNo;
                            return err;
                        }
                    }

                    //if std code present, either phone no or fax no required
                    if (stdCode != "")
                    {
                        if (phoneNo == "" && faxNo == "")
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
                    if (emailId != "")
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
                    if (website != "")
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
                    if (firstName != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, firstName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidFirstName;
                            return err;
                        }
                    }
                    string middleName = Sanitizer.GetSafeHtmlFragment((data.SchoolHeadMiddleName ?? "").Trim());
                    if (middleName != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, middleName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidMiddleName;
                            return err;
                        }
                    }
                    string lastName = Sanitizer.GetSafeHtmlFragment((data.SchoolHeadLastName ?? "").Trim());
                    if (lastName != "")
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
                    err = chkDuplicateContactMstSchool((Sanitizer.GetSafeHtmlFragment(data.EmailId) ?? "").Trim(), (Sanitizer.GetSafeHtmlFragment(data.PhoneNo) ?? "").Trim()
                                                        , (Sanitizer.GetSafeHtmlFragment(data.MobileNo) ?? "").Trim()
                                                        , GblFunctions.Base64Decode((Sanitizer.GetSafeHtmlFragment(data.SchoolId) ?? DefaultSetting.DefaultValEnc).Trim())
                                                        , (data.EntType ?? "").Trim(), ref errDesc, (Sanitizer.GetSafeHtmlFragment(data.DISECode) ?? "").Trim()
                                                        , (Sanitizer.GetSafeHtmlFragment(data.IndexNo) ?? "").Trim(), (Sanitizer.GetSafeHtmlFragment(data.SubDivisionId) ?? "").Trim());

                    if (err > 0)
                    {
                        err = 2;
                        return err;
                    }
                }

                //check whether delete is permitted or not
                if (mode == Mode.DELETE)
                {
                    string tableName = "StudentRegistrationDetails";
                    string masterTableName = "MstSchool";
                    string fieldName = "SchoolId";

                    //check school data dependancy in registration table
                    err = new MCommon().chkDelete(tableName, fieldName, schoolId.ToString());

                    if (err > 0)
                    {
                        err = 2;
                        errDesc = Message.School.DeleteNotPermitted;
                        return err;
                    }

                    //check whether school is migrated or not
                    err = new MCommon().chkMigration(masterTableName, fieldName, schoolId.ToString());

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

                err = s.SaveMstSchool(data, userTypeid, mode, ipAddress, createdBy, ref errDesc, indexNo);
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }

            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "Schools/saveMstSchool(MMstSchool)", "AdminSchoolController");
            }

            return err;
        }

        public string chkSchoolEditPermission(Int64 schoolId, string editFor)
        {
            string data = string.Empty;

            try
            {
                data = new School().ChkSchoolEditPermission(schoolId, editFor);
            }
            catch (Exception ex)
            {
                data = "N";
                MCommon.saveExceptionLog(ex.Message, "ChkEdit/chkSchoolEditPermission(MMstSchool)", "AdminSchoolController");
            }

            return data;
        }

        public int unLoackPermission(VMSchoolEditPermission data, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            string schoolId = string.Empty, schoolName = string.Empty, ipAddress = string.Empty;

            try
            {
                //school Id               
                schoolId = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment((data.SchoolId ?? "").Trim()));
                if (string.IsNullOrEmpty(schoolId))
                {
                    err = 2;
                    errDesc = Message.School.SchoolIdRequired;
                    return err;
                }
                //Check User Supper Admin(WBBSE Authority) is allowed or not for UnLock permission
                if (Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]) != 1)
                {
                    string d = chkSchoolEditPermission(Convert.ToInt64(schoolId), SchoolProfileEditFor.SuperAdmin);

                    if (d == "N")
                    {
                        err = 2;
                        errDesc = Message.School.UnLockNotPermitted;
                        return err;
                    }
                }

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = s.saveSchoolEditPermission(data, ipAddress, createdBy, ref errDesc);

            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }

            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "SchoolEditPermission/unLoackPermission(MMstSchool)", "SchoolEditPermissionController");
            }

            return err;
        }

        public DataTable downloadMstSchoolList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, string searchType, string lockType, int reportFormat)
        {
            DataTable dt = new DataTable();
            
            try
            {
                dt = s.downloadMstSchoolList((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, page, limit, ref total, searchType, lockType, reportFormat);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "DownloadReport/downloadMstSchoolList(MMstSchool)", "AdminSchoolController");
            }
            
            return dt;
        }

        public List<VMMstSchool> getMstSchoolListAutoComplete(string searchString)
        {
            DataTable dt = new DataTable();
            List<VMMstSchool> result = new List<VMMstSchool>();

            try
            {
                dt = s.GetMstSchoolListAutoComplete(searchString);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchool
                               {
                                   SchoolId = data.Field<string>("SchoolId"),
                                   SchoolName = data.Field<string>("SchoolName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolList/getMstSchoolListAutoComplete(MMstSchool)", "AdminSchoolTransferController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getSubDivisionNameBySchoolId(Int64 schoolId)
        {
            string data = string.Empty;

            try
            {
                data = new SubDivision().GetSubDivisionNameBySchoolId(schoolId);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetCurrentSubDivisionName/getSubDivisionNameBySchoolId(MMstSchool)", "AdminSchoolTransferController");
            }
            return data;
        }

        public string getSchoolTransferView(Int64 schoolId, string deleteYN)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = s.GetSchoolTransferView(schoolId, deleteYN, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetSchoolTransferView/getMstSchoolView(MMstSchool)", "AdminSchoolTransferController");
                    data = Message.OperationError;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolTransferView/getMstSchoolView(MMstSchool)", "AdminSchoolTransferController");
            }

            return data;
        }

        public int saveSchoolTransfer(VMSchoolTransfer data, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            string mode = string.Empty, ipAddress = string.Empty;

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));
                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                #region VALIDATIONS
                if (mode != Mode.ADD)
                {
                    //check whether school id is present in proper format or not. if not, runtime error will be created.
                    Int64 schoolId = Convert.ToInt64(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SchoolId)));

                    if (schoolId == -1)
                    {
                        err = 1;
                        return err;
                    }
                }
                if (mode != Mode.DELETE)
                {
                    //subdivision
                    try
                    {
                        int subDivisionId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(data.SubDivisionId)));
                        if (subDivisionId <= 0)
                        {
                            err = 2;
                            errDesc = Message.School.SubDivisionRequired;
                            return err;
                        }
                    }
                    catch
                    {
                        err = 2;
                        errDesc = Message.School.SubDivisionRequired;
                        return err;
                    }
                }
                #endregion

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = s.SaveSchoolTransfer(data, mode, ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }

            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "Transfers/saveSchoolTransfer(MMstSchool)", "AdminSchoolTransferController");
            }

            return err;
        }
    }
}
