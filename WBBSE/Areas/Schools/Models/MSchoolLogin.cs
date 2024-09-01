﻿using System;
using System.Web;
using System.Data;
using ViewModel;
using DAL;
using Common;

namespace WBBSE.Areas.Schools.Models
{
    public class MSchoolLogin
    {
        public static bool isValidUser(VMSchoolLogin ld, string salt)
        {
            bool status = false;
            Login l = new Login();
            DataTable dt = new DataTable();
            string groupId = string.Empty;
            string userId = string.Empty;
            string OrganizationId = string.Empty;
            string OrganizationCode = string.Empty;
            string name = string.Empty;
            string groupName = string.Empty;
            string newPasswordChangedYN = string.Empty;
            string emailVerifiedYN = string.Empty;
            string contactNoVerifiedYN = string.Empty;
            string userFor = string.Empty;
            string userTypeId = string.Empty;
            string profileImage = string.Empty;
            string organizationName = string.Empty;
            string subDivisionId = string.Empty;
            //string roundNo = string.Empty;

            try
            {
                dt = l.ValidateLogin(ld.UserName);

                if (dt != null && dt.Rows.Count > 0)
                {
                    userTypeId = dt.Rows[0]["UserTypeId"].ToString();

                    //if user is not of school/pre-school type, reject login
                    if (userTypeId != UserType.UserTypeID.SCHOOL && userTypeId != UserType.UserTypeID.PRESCHOOL)
                    {
                        status = false;
                    }
                    else
                    {
                        groupId = dt.Rows[0]["GroupId"].ToString();
                        userId = dt.Rows[0]["UserId"].ToString();
                        OrganizationId = dt.Rows[0]["OrganizationId"].ToString();
                        OrganizationCode = dt.Rows[0]["OrganizationCode"].ToString();
                        name = dt.Rows[0]["Name"].ToString();
                        groupName = dt.Rows[0]["GroupName"].ToString();
                        newPasswordChangedYN = dt.Rows[0]["NewPasswordChangedYN"].ToString();
                        emailVerifiedYN = dt.Rows[0]["EmailVerifiedYN"].ToString();
                        contactNoVerifiedYN = dt.Rows[0]["ContactNoVerifiedYN"].ToString();
                        profileImage = dt.Rows[0]["ProfileImage"].ToString();
                        organizationName = dt.Rows[0]["OrganizationName"].ToString();
                        subDivisionId = dt.Rows[0]["SubDivisionId"].ToString();

                        string password = dt.Rows[0]["LoginPwd"].ToString().Trim() + salt;
                        string hashedPasswordAndSalt = GblFunctions.CreatePasswordHash(password);

                        if (ld.LoginPwd.ToLower() == hashedPasswordAndSalt.ToLower())
                        {
                            HttpContext.Current.Session[SessionNames.GroupId] = groupId;
                            HttpContext.Current.Session[SessionNames.UserTypeId] = userTypeId;
                            HttpContext.Current.Session[SessionNames.OrganizationId] = (groupId == "1" || groupId == "2") ? "0" : OrganizationId;
                            HttpContext.Current.Session[SessionNames.OrganizationCode] = (groupId == "1" || groupId == "2") ? "NA" : OrganizationCode;
                            HttpContext.Current.Session[SessionNames.UserId] = userId;
                            HttpContext.Current.Session[SessionNames.Name] = name;
                            HttpContext.Current.Session[SessionNames.GroupName] = groupName;
                            HttpContext.Current.Session[SessionNames.NewPasswordChangedYN] = newPasswordChangedYN;
                            HttpContext.Current.Session[SessionNames.EmailVerifiedYN] = emailVerifiedYN;
                            HttpContext.Current.Session[SessionNames.ContactNoVerifiedYN] = contactNoVerifiedYN;
                            HttpContext.Current.Session[SessionNames.ProfileImage] = profileImage + "?" + DateTime.Now.ToFileTime();
                            HttpContext.Current.Session[SessionNames.OrganizationName] = organizationName;
                            HttpContext.Current.Session[SessionNames.SubDivisionId] = subDivisionId;

                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
                else
                {
                    status = false;
                }
            }
            catch
            {
                status = false;
            }
            finally
            {
                l = null;
                dt = null;
            }

            //save log
            string logId = userId;
            string category = ((userTypeId == "2") ? LogCategory.SchoolLogin : ((userTypeId == "3") ? LogCategory.PreSchoolLogin : LogCategory.AdminLogin));
            string logDetails = string.Empty;

            if (status == false)
            {
                logDetails = "Invalid login attempt(User : " + ld.UserName + ")";
            }
            else
            {
                logDetails = "User : " + ld.UserName + " logged in successfully.";
            }

            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                string errDesc = string.Empty;

                int error = new Login().SaveLogDetails(logId, category, "SELECT", logDetails, ipAddress, userId, ref errDesc);
            }
            catch
            {
                //nothing to be done
            }

            return status;
        }
    }
}