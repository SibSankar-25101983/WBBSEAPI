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

namespace WBBSE.Areas.Admin.Models
{
    public class MMstUser
    {
        User u = new User();

        public List<VMMstUser> getMstUserList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            List<VMMstUser> result = new List<VMMstUser>();

            try
            {
                dt = u.GetMstUserList((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstUser
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   UserId = data.Field<string>("UserId"),
                                   Name = data.Field<string>("Name"),
                                   UserType = data.Field<string>("UserType"),
                                   Designation = data.Field<string>("Designation"),
                                   IsActive = data.Field<string>("IsActive")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetUserList/getMstUserList(MMstUser)", "AdminUserController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getMstUserView(string userId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = u.GetMstUserView(userId, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetUserView/GetMstUserView(MMstUser)", "AdminUserController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetUserView/GetMstUserView(MMstUser)", "AdminUserController");
            }

            return data;
        }

        public List<VMMstUser> getMstUserDetails(string userId)
        {
            DataTable dt = new DataTable();
            List<VMMstUser> result = new List<VMMstUser>();

            try
            {
                dt = u.GetMstUserDetails(userId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstUser
                               {
                                   UserId = data.Field<string>("UserId"),
                                   SalutationId = data.Field<string>("SalutationId"),
                                   FirstName = data.Field<string>("FirstName"),

                                   MiddleName = data.Field<string>("MiddleName"),
                                   LastName = data.Field<string>("LastName"),
                                   EmailId = data.Field<string>("EmailId"),
                                   MobileNo = data.Field<string>("MobileNo"),
                                   AddressLine1 = data.Field<string>("AddressLine1"),
                                   AddressLine2 = data.Field<string>("AddressLine2"),
                                   City = data.Field<string>("City"),
                                   PinCode = data.Field<string>("PinCode"),
                                   UserType = data.Field<string>("UserType"),
                                   GroupName = data.Field<string>("GroupName"),
                                   UserTypeName = data.Field<string>("UserTypeName"),
                                   DesignationId = data.Field<string>("DesignationId"),
                                   DesignEditableYN = data.Field<string>("DesignEditableYN"),
                                   ActiveYN = data.Field<string>("ActiveYN")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetUserDetails/GetMstUserDetails(MMstUser)", "AdminUserController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int chkDuplicateContactMstUser(string emailId, string mobileNo, string userId, string entType, ref string errDesc)
        {
            int err = 0;

            try
            {
                if (userId == DefaultSetting.DefaultVal)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                }
                else
                {
                    err = u.ChkDuplicateContactMstUser(emailId, mobileNo, userId, entType, ref errDesc);
                }

                errDesc = HttpContext.Current.Server.HtmlDecode(errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "Users/chkDuplicateContactMstUser(MMstUser)", "AdminUserController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Users/chkDuplicateContactMstSchool(MMstUser)", "AdminUserController");
            }

            return err;
        }

        public int updateMstUserByAdmin(VMMstUser data, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            string mode=string.Empty, addressLine1 = string.Empty, addressLine2 = string.Empty, city = string.Empty, pinCode = string.Empty, mobileNo = string.Empty, emailId = string.Empty, ipAddress = string.Empty;

            try
            {
                if (data.EntType == EntType.EDIT)
                {
                    string firstName = Sanitizer.GetSafeHtmlFragment((data.FirstName ?? "").Trim());
                    if (!string.IsNullOrEmpty(firstName))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, firstName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidFirstName;
                            return err;
                        }
                    }
                    string middleName = Sanitizer.GetSafeHtmlFragment((data.MiddleName ?? "").Trim());
                    if (!string.IsNullOrEmpty(middleName))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, middleName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidMiddleName;
                            return err;
                        }
                    }
                    string lastName = Sanitizer.GetSafeHtmlFragment((data.LastName ?? "").Trim());
                    if (!string.IsNullOrEmpty(lastName))
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, lastName))
                        {
                            err = 2;
                            errDesc = Message.User.InvalidLastName;
                            return err;
                        }
                    }

                    addressLine1 = Sanitizer.GetSafeHtmlFragment((data.AddressLine1 ?? "").Trim());
                    addressLine2 = Sanitizer.GetSafeHtmlFragment((data.AddressLine2 ?? "").Trim());
                    city = Sanitizer.GetSafeHtmlFragment((data.City ?? "").Trim());
                    pinCode = Sanitizer.GetSafeHtmlFragment((data.PinCode ?? "").Trim());
                    mobileNo = Sanitizer.GetSafeHtmlFragment((data.MobileNo ?? "").Trim());
                    emailId = Sanitizer.GetSafeHtmlFragment((data.EmailId ?? "").Trim());

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
                }
                //check duplicate contact details
                err = u.ChkDuplicateContactMstUser((data.EmailId ?? "").Trim(), (data.MobileNo ?? "").Trim(), GblFunctions.Base64Decode((data.UserId ?? DefaultSetting.DefaultValEnc).Trim())
                    , (data.EntType ?? "").Trim(), ref errDesc);

                if (err > 0)
                {
                    err = 2;
                    return err;
                }

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = u.UpdateMstUserByAdmin(data, ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }

            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "Users/updateMstUserByAdmin(MMstUser)", "AdminUserController");
            }

            return err;
        }
    }
}
