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

namespace WBBSE.Areas.PreSchools.Models
{
    public class MPreSchoolProfile
    {
        public int chkDuplicateContactMstSchool(string emailId, string phoneNo, string mobileNo, string preSchoolId, string entType, ref string errDesc, string diseCode)
        {
            int err = 0;

            try
            {
                if (preSchoolId == DefaultSetting.DefaultValEnc)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                }
                else
                {
                    err = new PreSchool().ChkDuplicateContactMstPreSchool(emailId, phoneNo, mobileNo, preSchoolId, entType, ref errDesc, diseCode);
                }

                errDesc = HttpContext.Current.Server.HtmlDecode(errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "PreSchoolProfile/chkDuplicateContactMstPreSchool(MPreSchoolProfile)", "PreSchoolProfileController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PreSchoolProfile/chkDuplicateContactMstPreSchool(MPreSchoolProfile)", "PreSchoolProfileController");
            }

            return err;
        }

        public int updatePreSchoolProfile(VMPreSchoolProfile data, ref string errDesc)
        {
            int err = 0, createdBy = 0, userTypeid = 0;
            string mode = string.Empty, ipAddress = string.Empty;

            try
            {               
                mode = Sanitizer.GetSafeHtmlFragment(data.EntType ?? Mode.ERROR).Trim();
                if (mode != EntType.EDIT && mode != EntType.LOCK)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
                if (mode == EntType.EDIT)
                {
                    mode = Mode.EDIT;
                }
                else if (mode == EntType.LOCK)
                {
                    mode = Mode.LOCK;
                }
                else
                {
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }

                #region VALIDATIONS
                if (mode != Mode.DELETE)
                {
                    //school name
                    //string schoolName = Sanitizer.GetSafeHtmlFragment((data.SchoolName ?? "").Trim());
                    //if (string.IsNullOrEmpty(schoolName))
                    //{
                    //    err = 2;
                    //    errDesc = Message.School.SchoolNameRequired;
                    //    return err;
                    //}
                    //if (!GblFunctions.chkDataFormat(RegexType.Alpha, schoolName))
                    //{
                    //    err = 2;
                    //    errDesc = Message.School.InvalidSchoolName;
                    //    return err;
                    //}

                    //dise code
                    string diseCode = Sanitizer.GetSafeHtmlFragment((data.DISECode ?? "").Trim());
                    if (string.IsNullOrEmpty(diseCode))
                    {
                        err = 2;
                        errDesc = Message.School.DISECodeRequired;
                        return err;
                    }
                    if (!string.IsNullOrEmpty(diseCode))
                    {
                        Match match = Regex.Match(diseCode, @"^([0-9]{11})$", RegexOptions.IgnoreCase); //Lenth must be 11 digit and numeric 
                        if (!match.Success)
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidDISECode;
                            return err;
                        }
                    }

                    //address checking
                    string addressLine1 = Sanitizer.GetSafeHtmlFragment((data.AddressLine1 ?? "").Trim());
                    if (string.IsNullOrEmpty(addressLine1))
                    {
                        err = 2;
                        errDesc = Message.School.StreetNameRequired;
                        return err;
                    }
                    if (!string.IsNullOrEmpty(addressLine1))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, addressLine1))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidStreetName;
                            return err;
                        }
                    }
                    string addressLine2 = Sanitizer.GetSafeHtmlFragment((data.AddressLine2 ?? "").Trim());
                    if (addressLine2 == "")
                    {
                        err = 2;
                        errDesc = Message.School.AreaNameRequired;
                        return err;
                    }
                    if (addressLine2 != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, addressLine2))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidAreaName;
                            return err;
                        }
                    }
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
                    string city = Sanitizer.GetSafeHtmlFragment((data.City ?? "").Trim());
                    if (city == "")
                    {
                        err = 2;
                        errDesc = Message.School.CityRequired;
                        return err;
                    }
                    if (city != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, city))
                        {
                            err = 2;
                            errDesc = Message.School.InvalidCityName;
                            return err;
                        }
                    }
                    string pinCode = Sanitizer.GetSafeHtmlFragment((data.PinCode ?? "").Trim());
                    if (pinCode == "")
                    {
                        err = 2;
                        errDesc = Message.School.PinCodeRequired;
                        return err;
                    }
                    if (pinCode != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.PinCode, pinCode))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidPinCode;
                            return err;
                        }
                    }

                    //std code
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

                    //phone no
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

                    //mobile no
                    string mobileNo = Sanitizer.GetSafeHtmlFragment((data.MobileNo ?? "").Trim());
                    if (mobileNo == "")
                    {
                        err = 2;
                        errDesc = Message.School.MobileNoRequired;
                        return err;
                    }
                    if (mobileNo != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.MobileNo, mobileNo))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidMobileNo;
                            return err;
                        }
                    }

                    //fax no
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

                    //email id
                    string emailId = Sanitizer.GetSafeHtmlFragment((data.EmailId ?? "").Trim());
                    if (emailId == "")
                    {
                        err = 2;
                        errDesc = Message.School.EmailIdRequired;
                        return err;
                    }
                    if (emailId != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.EmailId, emailId))
                        {
                            err = 2;
                            errDesc = Message.RegexMsg.InvalidEmailId;
                            return err;
                        }
                    }

                    //website
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
                                                        , Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]).ToString()
                                                        , EntType.EDIT, ref errDesc, string.Empty);

                    if (err > 0)
                    {
                        err = 2;
                        return err;
                    }
                }
                #endregion

                userTypeid = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserTypeId]);
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new PreSchool().UpdatePreSchoolProfile(data, userTypeid, mode, ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }

            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "PreSchoolProfile/updatePreSchoolProfile(MPreSchoolProfile)", "PreSchoolProfileController");
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
                MCommon.saveExceptionLog(ex.Message, "makeData/chkPreSchoolEditPermission(MPreSchoolProfile)", "PreSchoolProfileController");
            }

            return data;
        }
    }
}