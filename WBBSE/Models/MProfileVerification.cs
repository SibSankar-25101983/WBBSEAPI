using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Security.Application;
using WBBSE.Areas.Admin.Models;
using ViewModel;
using DAL;
using Common;

namespace WBBSE.Models
{
    public class MProfileVerification
    {
        public int getProfileVerificationData(int userId, int userTypeId, ref string emailId, ref string contactNo, ref string resendEmailOTPYN, ref string resendContactOTPYN, ref string showHomeLinkYN)
        {
            int error = 0;
            DataTable dt = new DataTable();
            ProfileVerification pv = new ProfileVerification();

            try
            {
                dt = pv.GetProfileVerificationData(userId, userTypeId);

                if (dt == null || dt.Rows.Count == 0)
                {
                    error = 1;
                }
                else
                {
                    error = 0;
                    emailId = dt.Rows[0]["EMailId"].ToString();
                    contactNo = dt.Rows[0]["ContactNo"].ToString();
                    resendEmailOTPYN = dt.Rows[0]["ResendEmailOTPYN"].ToString();
                    resendContactOTPYN = dt.Rows[0]["ResendContactOTPYN"].ToString();
                    showHomeLinkYN = dt.Rows[0]["ShowHomeLinkYN"].ToString();
                }
                if (HttpContext.Current.Session[SessionNames.ProfileVerificationData] == null)
                {
                    HttpContext.Current.Session[SessionNames.ProfileVerificationData] = dt;
                }
            }
            catch
            {
                error = 1;
            }

            return error;
        }

        public int saveProfileVerificationData(VMProfileVerification vmpv, ref string errDesc)
        {
            int err = 0;
            ProfileVerification pv = new ProfileVerification();

            try
            {
                string firstName = string.Empty, middleName = string.Empty, lastName = string.Empty, contactNo = string.Empty, stdCode = string.Empty, phoneNo = string.Empty;
                string salutationID = string.Empty, emailId = string.Empty;

                int userId = (HttpContext.Current.Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                if (userId == 0)
                {
                    err = -1;
                    return err;
                }
                DataTable dt = (DataTable)HttpContext.Current.Session[SessionNames.ProfileVerificationData];
                if (dt == null || dt.Rows.Count == 0)
                {
                    err = -1;
                    return err;
                }

                #region FOR PASSWORD CHANGE
                if (vmpv.OperationType == "P")
                {
                    #region VALIDATIONS
                    if (vmpv.VerificationFor == UserType.SCHOOL || vmpv.VerificationFor == UserType.PRESCHOOL)
                    {
                        salutationID = GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(vmpv.SalutationId));
                        if (string.IsNullOrEmpty(salutationID))
                        {
                            err = -2;
                            errDesc = Message.ProfileVerification.SalutationIDRequired;
                            return err;
                        }
                        firstName = Sanitizer.GetSafeHtmlFragment(vmpv.FirstName ?? "").Trim();
                        if (string.IsNullOrEmpty(firstName))
                        {
                            err = -2;
                            errDesc = Message.ProfileVerification.FirstNameRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, firstName))
                        {
                            err = -2;
                            errDesc = Message.User.InvalidFirstName;
                            return err;
                        }
                        middleName = Sanitizer.GetSafeHtmlFragment(vmpv.MiddleName ?? "").Trim();
                        if (!string.IsNullOrEmpty(middleName))
                        {
                            if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, middleName))
                            {
                                err = -2;
                                errDesc = Message.User.InvalidMiddleName;
                                return err;
                            }
                        }
                        lastName = Sanitizer.GetSafeHtmlFragment(vmpv.LastName ?? "").Trim();
                        if (string.IsNullOrEmpty(lastName))
                        {
                            err = -2;
                            errDesc = Message.ProfileVerification.LastNameRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, lastName))
                        {
                            err = -2;
                            errDesc = Message.User.InvalidLastName;
                            return err;
                        }
                        emailId = Sanitizer.GetSafeHtmlFragment(vmpv.EMailId ?? "").Trim();
                        if (string.IsNullOrEmpty(emailId))
                        {
                            err = -2;
                            errDesc = Message.ProfileVerification.EmailIdRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.EmailId, emailId))
                        {
                            err = -2;
                            errDesc = Message.RegexMsg.InvalidEmailId;
                            return err;
                        }
                        contactNo = Sanitizer.GetSafeHtmlFragment(vmpv.ContactNo ?? "").Trim();
                        if (string.IsNullOrEmpty(contactNo))
                        {
                            err = -2;
                            errDesc = Message.ProfileVerification.ContactNoRequired;
                            return err;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.MobileNo, contactNo))
                        {
                            err = -2;
                            errDesc = Message.RegexMsg.InvalidMobileNo;
                            return err;
                        }
                        stdCode = Sanitizer.GetSafeHtmlFragment(vmpv.StdCode ?? "").Trim();
                        if (stdCode != "")
                        {
                            if (!GblFunctions.chkDataFormat(RegexType.StdCode, stdCode))
                            {
                                err = -2;
                                errDesc = Message.RegexMsg.InvalidSTDCode;
                                return err;
                            }
                        }
                        phoneNo = Sanitizer.GetSafeHtmlFragment(vmpv.PhoneNo ?? "").Trim();
                        if (phoneNo != "")
                        {
                            if (!GblFunctions.chkDataFormat(RegexType.Numeric, phoneNo))
                            {
                                err = -2;
                                errDesc = Message.RegexMsg.InvalidContactNo;
                                return err;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(vmpv.OldPassword))
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.OldPasswordRequired;
                        return err;
                    }
                    if (string.IsNullOrEmpty(vmpv.NewPassword))
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.NewPasswordRequired;
                        return err;
                    }
                    if (string.IsNullOrEmpty(vmpv.ConfirmPassword))
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.ConfirmPasswordRequired;
                        return err;
                    }
                    if (vmpv.NewPassword != vmpv.ConfirmPassword)
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.PasswordMismatch;
                        return err;
                    }
                    //Regex reg_Password = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\-_])[A-Za-z\d@$!%*?&\-_]{8,}$");
                    //if(!reg_Password.IsMatch(vmpv.NewPassword))
                    //{
                    //    err = -2;
                    //    errDesc = Message.ProfileVerification.PasswordPolicyMismatch;
                    //    return err;
                    //}
                    string oldPassword = dt.Rows[0]["LoginPwd"].ToString().Trim();
                    if (vmpv.OldPassword != oldPassword)
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.OldPasswordMismatch;
                        return err;
                    }
                    string oldPasswordList = dt.Rows[0]["OldPwdList"].ToString();
                    string[] oldPasswordListAry = oldPasswordList.Split(',');
                    if (Array.Exists(oldPasswordListAry, s => s.Equals(vmpv.NewPassword)))
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.LastThreePasswordNotAllowed;
                        return err;
                    }
                    #endregion

                    if (oldPasswordListAry.Length == 3)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            oldPasswordListAry[i] = oldPasswordListAry[i + 1];
                        }
                        oldPasswordListAry[2] = vmpv.NewPassword;
                    }
                    else
                    {
                        switch (oldPasswordListAry.Length)
                        {
                            case 1:
                                if (oldPasswordListAry[0] == "")
                                {
                                    Array.Resize(ref oldPasswordListAry, 1);
                                    oldPasswordListAry[0] = vmpv.NewPassword;
                                }
                                else
                                {
                                    Array.Resize(ref oldPasswordListAry, 2);
                                    oldPasswordListAry[1] = vmpv.NewPassword;
                                }
                                break;
                            case 2:
                                Array.Resize(ref oldPasswordListAry, 3);
                                oldPasswordListAry[2] = vmpv.NewPassword;
                                break;
                        }
                    }

                    if (vmpv.VerificationFor == UserType.SCHOOL || vmpv.VerificationFor == UserType.PRESCHOOL)
                    {
                        Int64 organizationId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId].ToString());
                        string userType = (HttpContext.Current.Session[SessionNames.UserTypeId].ToString());

                        userType = (userType == UserType.UserTypeID.SCHOOL) ? UserType.SCHOOL : ((userType == UserType.UserTypeID.PRESCHOOL) ? UserType.PRESCHOOL : "ERROR");
                        if (userType == "ERROR")
                        {
                            err = -1;
                            return err;
                        }
                        int groupId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId].ToString());

                        //check duplicate contact details
                        if (vmpv.VerificationFor == UserType.SCHOOL) //SCHOOL EMAIL-ID/PHONE NO/MOBILE NO DUPLICATE CHECKING WITH ANOTHER SCHOOL
                        {
                            err = new MMstSchool().chkDuplicateContactMstSchool(emailId, phoneNo, contactNo, organizationId.ToString(), EntType.EDIT
                                                                                , ref errDesc, string.Empty, string.Empty, DefaultSetting.DefaultValEnc);
                        }
                        else if (vmpv.VerificationFor == UserType.PRESCHOOL) //PRE-SCHOOL EMAIL-ID/PHONE NO/MOBILE NO DUPLICATE CHECKING WITH ANOTHER PRE-SCHOOL
                        {
                            err = new MMstPreSchool().chkDuplicateContactMstPreSchool(emailId, phoneNo, contactNo, organizationId.ToString(), EntType.EDIT
                                                                                , ref errDesc, string.Empty);
                        }

                        if (err > 0)
                        {
                            err = -2;
                            errDesc = errDesc.Replace("<br/>", "");
                            return err;
                        }

                        err = pv.UpdateProfileWithPassword(userId, organizationId, vmpv.NewPassword, string.Join(",", oldPasswordListAry), salutationID
                                                        , firstName, middleName, lastName, emailId, contactNo, stdCode, phoneNo, ipAddress, ref errDesc, userType, groupId);

                        if (err == 0)
                        {
                            HttpContext.Current.Session[SessionNames.Name] = errDesc;
                        }
                    }
                    else if (vmpv.VerificationFor == UserType.ADMIN)
                    {
                        err = pv.UpdatePassword(userId, vmpv.NewPassword, string.Join(",", oldPasswordListAry), ipAddress, ref errDesc);
                    }
                    else
                    {
                        err = -1;
                    }

                    if (err == 0)
                    {
                        HttpContext.Current.Session[SessionNames.NewPasswordChangedYN] = "Y";
                    }
                    if (err == 1)
                    {
                        MCommon.saveExceptionLog(errDesc, "saveProfileVerificationData(MProfileVerification)", "ProfileVerificationRelatedController");
                    }
                }
                #endregion
                #region FOR OTP SEND/ RESEND : EMAIL ID
                else if (vmpv.OperationType == "EG" || vmpv.OperationType == "ER")
                {
                    emailId = Sanitizer.GetSafeHtmlFragment(vmpv.EMailId ?? "").Trim();

                    if (string.IsNullOrEmpty(emailId))
                    {
                        err = -2;
                        errDesc = Message.EmailIdRequired;
                        return err;
                    }

                    //GENERATE OTP FOR EMAIL-ID/MOBILE NO VERIFICATION
                    string otp = GblFunctions.GenerateOTP(999999);

                    int userTypeId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserTypeId]);
                    Int64 organizationId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                    string otpType = string.Empty;

                    if (userTypeId.ToString() == UserType.UserTypeID.ADMIN)
                    {
                        otpType = OTPType.AdminEmail;
                    }
                    else if (userTypeId.ToString() == UserType.UserTypeID.SCHOOL)
                    {
                        otpType = OTPType.SchoolEmail;
                    }
                    else if (userTypeId.ToString() == UserType.UserTypeID.PRESCHOOL)
                    {
                        otpType = OTPType.PreSchoolEmail;
                    }
                    else
                    {
                        err = -1;
                        return err;
                    }

                    //check for duplicate email id in user/school/pre-school based on user type
                    if (userTypeId.ToString() == UserType.UserTypeID.ADMIN)
                    {
                        err = new User().ChkDuplicateContactMstUser(emailId, string.Empty, userId.ToString(), EntType.EDIT, ref errDesc);

                        if (err > 0)
                        {
                            err = -2;
                            errDesc = errDesc.Replace("<br/>", "");
                            return err;
                        }
                    }
                    else
                    {
                        if (userTypeId.ToString() == UserType.UserTypeID.SCHOOL) //SCHOOL EMAIL-ID/PHONE NO/MOBILE NO DUPLICATE CHECKING WITH ANOTHER SCHOOL
                        {
                            err = new MMstSchool().chkDuplicateContactMstSchool(emailId, string.Empty, string.Empty, organizationId.ToString(), EntType.EDIT
                                                                                , ref errDesc, string.Empty, string.Empty, DefaultSetting.DefaultValEnc);
                        }
                        else if (userTypeId.ToString() == UserType.UserTypeID.PRESCHOOL) //PRE-SCHOOL EMAIL-ID/PHONE NO/MOBILE NO DUPLICATE CHECKING WITH ANOTHER PRE-SCHOOL
                        {
                            err = new MMstPreSchool().chkDuplicateContactMstPreSchool(emailId, phoneNo, contactNo, organizationId.ToString(), EntType.EDIT
                                                                                , ref errDesc, string.Empty);
                        }

                            if (err > 0)
                            {
                                err = -2;
                                errDesc = errDesc.Replace("<br/>", "");
                                return err;
                            }
                    }


                    err = pv.SaveOTP(userId, otp, otpType, ipAddress, ref errDesc, userTypeId, organizationId);

                    if (err == 0)
                    {
                        //string emailId = vmpv.EMailId.Trim();
                        string subject = "Email Id Verification For WBBSE ";

                        if (userTypeId.ToString() == UserType.UserTypeID.ADMIN)
                        {
                            subject += "Admin Portal";
                        }
                        else if (userTypeId.ToString() == UserType.UserTypeID.SCHOOL)
                        {
                            subject += "School Portal";
                        }
                        else if (userTypeId.ToString() == UserType.UserTypeID.PRESCHOOL)
                        {
                            subject += "Junior School Portal";
                        }
                        else
                        {
                            err = -1;
                            return err;
                        }

                        string message = ProfileVerificationOTPMsg.Email.Replace("#", otp);
                        //"Hi,<br/>Below is your OTP for Email Id Verification-<br/><br/><b>" + otp + "</b><br/><br/>* <b>NOTE</b> : OTP is only valid for 30 minutes.<br/><br/>Regards,<br/>WBBSE";

                        err = GblFunctions.sendMailGmail(emailId, subject, message);

                        dt.Rows[0]["EMailId"] = emailId;
                        dt.AcceptChanges();
                        HttpContext.Current.Session[SessionNames.ProfileVerificationData] = dt;
                    }
                    if (err == 1)
                    {
                        MCommon.saveExceptionLog(errDesc, "saveProfileVerificationData(MProfileVerification)", "ProfileVerificationRelatedController");
                    }
                }
                #endregion
                #region FOR OTP VERIFICATION : EMAIL ID
                else if (vmpv.OperationType == "EC")
                {
                    if (string.IsNullOrEmpty(vmpv.OTP))
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.OTPRequired;
                        return err;
                    }

                    emailId = dt.Rows[0]["EMailId"].ToString();
                    if (emailId != Sanitizer.GetSafeHtmlFragment(vmpv.EMailId ?? "").Trim())
                    {
                        err = -2;

                        errDesc = Message.RegexMsg.InvalidEmailId;
                        return err;
                    }

                    if (string.IsNullOrEmpty(emailId))
                    {
                        err = -2;
                        errDesc = Message.EmailIdRequired;
                        return err;
                    }

                    int userTypeId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserTypeId]);
                    Int64 organizationId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                    int groupId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]);
                    string otpType = string.Empty;

                    if (userTypeId.ToString() == UserType.UserTypeID.ADMIN)
                    {
                        otpType = OTPType.AdminEmail;
                    }
                    else if (userTypeId.ToString() == UserType.UserTypeID.SCHOOL)
                    {
                        otpType = OTPType.SchoolEmail;
                    }
                    else if (userTypeId.ToString() == UserType.UserTypeID.PRESCHOOL)
                    {
                        otpType = OTPType.PreSchoolEmail;
                    }
                    else
                    {
                        err = -1;
                        return err;
                    }

                    string otpDB = pv.GetOTP(userId, otpType, userTypeId, organizationId);

                    if (otpDB == DefaultSetting.NotFound)
                    {
                        err = -1;
                        return err;
                    }

                    if (Sanitizer.GetSafeHtmlFragment(vmpv.OTP).Trim() == otpDB)
                    {
                        err = pv.UpdateOTPVerificationStatus(userId, otpType, ipAddress, ref errDesc, userTypeId, organizationId, groupId, emailId, dt.Rows[0]["ContactNo"].ToString().Trim());

                        if (err == 0)
                        {
                            HttpContext.Current.Session[SessionNames.EmailVerifiedYN] = "Y";
                        }
                        if (err == 1)
                        {
                            MCommon.saveExceptionLog(errDesc, "saveProfileVerificationData(MProfileVerification)", "ProfileVerificationRelatedController");
                        }
                    }
                    else
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.OTPMismatch;
                    }
                }
                #endregion
            }
            catch(Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "saveProfileVerificationData(MProfileVerification)", "ProfileVerificationRelatedController");
                err = -1;
            }
            return err;
        }

        public VMProfileData getProfileData(int userId)
        {
            VMProfileData vmProfileData = new VMProfileData();
            ProfileVerification pv = new ProfileVerification();
            DataTable dt = new DataTable();

            try
            {
                dt = pv.GetProfileData(userId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    vmProfileData.ProfileData = dt.Rows[0]["ProfileData"].ToString();
                    vmProfileData.OldPwdList = dt.Rows[0]["OldPwdList"].ToString();
                    vmProfileData.LoginPwd = dt.Rows[0]["LoginPwd"].ToString();
                    vmProfileData.SalutationId = dt.Rows[0]["SalutationId"].ToString();
                    vmProfileData.FirstName = dt.Rows[0]["FirstName"].ToString();
                    vmProfileData.MiddleName = dt.Rows[0]["MiddleName"].ToString();
                    vmProfileData.LastName = dt.Rows[0]["LastName"].ToString();
                    vmProfileData.MobileNo = dt.Rows[0]["MobileNo"].ToString();
                    vmProfileData.EmailId = dt.Rows[0]["EmailId"].ToString();
                    HttpContext.Current.Session[SessionNames.ProfileVerificationData] = vmProfileData;
                }
                else
                {
                    vmProfileData = null;
                }
            }
            catch
            {
                vmProfileData = null;
            }
            finally
            {
                dt = null;
                pv = null;
            }

            return vmProfileData;
        }

        public int saveProfileData(VMProfileData vmpd, ref string errDesc)
        {
            int err = 0;
            ProfileVerification pv = new ProfileVerification();

            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                int userId = (HttpContext.Current.Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);
                if (userId == 0)
                {
                    err = -1;
                    errDesc = Message.OperationError;
                    return err;
                }
                VMProfileData oldData = (VMProfileData)HttpContext.Current.Session[SessionNames.ProfileVerificationData];
                if (oldData == null)
                {
                    err = -1;
                    errDesc = Message.OperationError;
                    return err;
                }

                #region VALIDATIONS

                if (string.IsNullOrEmpty(vmpd.OldPassword))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.OldPasswordRequired;
                    return err;
                }
                if (string.IsNullOrEmpty(vmpd.NewPassword))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.NewPasswordRequired;
                    return err;
                }
                if (string.IsNullOrEmpty(vmpd.ConfirmPassword))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.ConfirmPasswordRequired;
                    return err;
                }
                if (vmpd.NewPassword != vmpd.ConfirmPassword)
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.PasswordMismatch;
                    return err;
                }

                string oldPassword = oldData.LoginPwd;
                if (vmpd.OldPassword != oldPassword)
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.OldPasswordMismatch;
                    return err;
                }

                #endregion

                string oldPasswordList = oldData.OldPwdList;
                string[] oldPasswordListAry = oldPasswordList.Split(',');
                if (Array.Exists(oldPasswordListAry, s => s.Equals(vmpd.NewPassword)))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.LastThreePasswordNotAllowed;
                    return err;
                }

                if (oldPasswordListAry.Length == 3)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        oldPasswordListAry[i] = oldPasswordListAry[i + 1];
                    }
                    oldPasswordListAry[2] = vmpd.NewPassword;
                }
                else
                {
                    switch (oldPasswordListAry.Length)
                    {
                        case 1:
                            if (oldPasswordListAry[0] == "")
                            {
                                Array.Resize(ref oldPasswordListAry, 1);
                                oldPasswordListAry[0] = vmpd.NewPassword;
                            }
                            else
                            {
                                Array.Resize(ref oldPasswordListAry, 2);
                                oldPasswordListAry[1] = vmpd.NewPassword;
                            }
                            break;
                        case 2:
                            Array.Resize(ref oldPasswordListAry, 3);
                            oldPasswordListAry[2] = vmpd.NewPassword;
                            break;
                    }
                }

                err = pv.UpdatePassword(userId, vmpd.NewPassword, string.Join(",", oldPasswordListAry), ipAddress, ref errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "saveProfileData(MProfileVerification)", "ProfileRelatedController");
                }
            }
            catch(Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "saveProfileData(MProfileVerification)", "ProfileRelatedController");
                err = -1;
                errDesc = Message.OperationError;
            }

            return err;
        }

        public int saveEditProfileData(VMProfileData vmpd, ref string errDesc)
        {
            int err = 0;
            ProfileVerification pv = new ProfileVerification();

            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                int userId = (HttpContext.Current.Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);
                
                if (userId == 0)
                {
                    err = -1;
                    errDesc = Message.OperationError;
                    return err;
                }

                #region VALIDATIONS

                try
                {
                    int SalutationId = Convert.ToInt32(GblFunctions.Base64Decode(Sanitizer.GetSafeHtmlFragment(vmpd.SalutationId)));

                    if (SalutationId <= 0)
                    {
                        err = -2;
                        errDesc = Message.ProfileVerification.SalutationIDRequired;
                        return err;
                    }
                }
                catch
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.SalutationIDRequired;
                    return err;
                }  

                //First Name
                string FirstName = Sanitizer.GetSafeHtmlFragment((vmpd.FirstName ?? "").Trim());
                if (string.IsNullOrEmpty(FirstName))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.FirstNameRequired;
                    return err;
                }
                if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, FirstName))
                {
                    err = -2;
                    errDesc = Message.User.InvalidFirstName;
                    return err;
                }

                //Middle Name
                string MiddleName = Sanitizer.GetSafeHtmlFragment((vmpd.MiddleName ?? "").Trim());
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, MiddleName))
                    {
                        err = -2;
                        errDesc = Message.User.LastNameRequired;
                        return err;
                    }
                }

                //Last Name
                string LastName = Sanitizer.GetSafeHtmlFragment((vmpd.LastName ?? "").Trim());
                if (string.IsNullOrEmpty(LastName))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.LastNameRequired;
                    return err;
                }
                if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, LastName))
                {
                    err = -2;
                    errDesc = Message.User.LastNameRequired;
                    return err;
                }

                //mobile no
                string mobileNo = Sanitizer.GetSafeHtmlFragment((vmpd.MobileNo ?? "").Trim());
                if (string.IsNullOrEmpty(mobileNo))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.ContactNoRequired;
                    return err;
                }
                if (!string.IsNullOrEmpty(mobileNo))
                {
                    if (!GblFunctions.chkDataFormat(RegexType.MobileNo, mobileNo))
                    {
                        err = -2;
                        errDesc = Message.RegexMsg.InvalidMobileNo;
                        return err;
                    }
                }                

                //email id
                string emailId = Sanitizer.GetSafeHtmlFragment((vmpd.EmailId ?? "").Trim());
                if (string.IsNullOrEmpty(emailId))
                {
                    err = -2;
                    errDesc = Message.ProfileVerification.EmailIdRequired;
                    return err;
                }
                if (!string.IsNullOrEmpty(emailId))
                {
                    if (!GblFunctions.chkDataFormat(RegexType.EmailId, emailId))
                    {
                        err = -2;
                        errDesc = Message.RegexMsg.InvalidEmailId;
                        return err;
                    }
                } 
               
                #endregion

                err = pv.UpdateUserProfile(userId, GblFunctions.Base64Decode(vmpd.SalutationId), vmpd.FirstName, vmpd.MiddleName, vmpd.LastName, vmpd.EmailId, vmpd.MobileNo, ipAddress, ref errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "saveEditProfileData(MProfileVerification)", "UserProfileRelatedController");
                }

                if (err == 0)
                {
                    //update user name in session
                    HttpContext.Current.Session[SessionNames.Name] = errDesc;
                }
            }
            catch(Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "saveEditProfileData(MProfileVerification)", "UserProfileRelatedController");
                err = -1;
                errDesc = Message.OperationError;
            }

            return err;
        }

        public int saveProfileImage(HttpPostedFileBase[] postedFiles, ref string errDesc)
        {
            int err = 0;

            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                int userId = (HttpContext.Current.Session[SessionNames.UserId] == null) ? 0 : Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);
                if (userId == 0)
                {
                    err = -1;
                    errDesc = Message.OperationError;
                    return err;
                }
                string imagePath = HttpContext.Current.Server.MapPath("~/ReadWriteData/ProfileImages/");
                string dbImagePath = @"../../../ReadWriteData/ProfileImages/";
                string imageFileExtension = Path.GetExtension(postedFiles[0].FileName);
                //local image path
                imagePath += "U_" + userId.ToString() + imageFileExtension;
                //db image path
                dbImagePath += "U_" + userId.ToString() + imageFileExtension;

                err = new ProfileVerification().SaveProfileImage(userId, dbImagePath, ipAddress, ref errDesc);

                if (err == 0)
                {
                    postedFiles[0].SaveAs(imagePath);
                    errDesc = dbImagePath;
                }
                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "saveProfileImage(MProfileVerification)", "ProfileRelatedController");
                }
            }
            catch(Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "saveProfileImage(MProfileVerification)", "ProfileRelatedController");
                err = -1;
                errDesc = Message.OperationError;
            }

            return err;
        }

        public VMProfileVerification getProfileVerificationData(ref int err, int userId, ref string resendEmailOTPYN, ref string resendContactOTPYN, ref string showHomeLinkYN, int userTypeId, string mode)
        {
            VMProfileVerification data = new VMProfileVerification();
            DataTable dt = new DataTable();

            try
            {
                if (mode == ProfileVerificationDataFetchMode.New)
                {
                    dt = new ProfileVerification().GetProfileVerificationData(userId, userTypeId);
                }
                else
                {
                    dt = (DataTable)HttpContext.Current.Session[SessionNames.ProfileVerificationData];
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    err = 1;
                }
                else
                {
                    err = 0;
                    data.EMailId = dt.Rows[0]["EMailId"].ToString();
                    data.ContactNo = dt.Rows[0]["ContactNo"].ToString();
                    data.SalutationId = dt.Rows[0]["SalutationId"].ToString();
                    data.FirstName = dt.Rows[0]["FirstName"].ToString();
                    data.MiddleName = dt.Rows[0]["MiddleName"].ToString();
                    data.LastName = dt.Rows[0]["LastName"].ToString();
                    resendEmailOTPYN = dt.Rows[0]["ResendEmailOTPYN"].ToString();
                    resendContactOTPYN = dt.Rows[0]["ResendContactOTPYN"].ToString();
                    showHomeLinkYN = dt.Rows[0]["ShowHomeLinkYN"].ToString();
                }
                if (HttpContext.Current.Session[SessionNames.ProfileVerificationData] == null)
                {
                    HttpContext.Current.Session[SessionNames.ProfileVerificationData] = dt;
                }
            }
            catch
            {
                err = 1;
            }

            return data;
        }
    }
}
