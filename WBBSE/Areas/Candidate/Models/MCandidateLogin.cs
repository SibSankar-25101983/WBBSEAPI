using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WBBSE.Models;
using System.Web.Security;
using Common;
using DAL;
using ViewModel;
using Microsoft.Security.Application;
using System.IO;
using System.Data;

namespace WBBSE.Areas.Candidate.Models
{
    public class MCandidateLogin
    {
        public static bool isValidUser(VMCandidateLogin ld, ref string errDesc)
        {
            bool status = false;
            DataTable dt = new DataTable();
            string groupId = string.Empty;
            string userId = string.Empty;
            string OrganizationId = string.Empty;
            string OrganizationCode = string.Empty;
            string name = string.Empty;
            string indexNo = string.Empty;
            string dob = string.Empty;
            string groupName = string.Empty;
            //string newPasswordChangedYN = string.Empty;
            //string emailVerifiedYN = string.Empty;
            //string contactNoVerifiedYN = string.Empty;
            string userFor = string.Empty;
            string userTypeId = string.Empty;
            string profileImage = string.Empty;
            string organizationName = string.Empty;
            //string subDivisionId = string.Empty;

            try
            {
                string rollNo = Sanitizer.GetSafeHtmlFragment(ld.RollNo ?? string.Empty).Trim();
                string schoolIndexNoTyped = Sanitizer.GetSafeHtmlFragment(ld.SchoolIndexNo ?? string.Empty).Trim();
                string studentNameTyped = Sanitizer.GetSafeHtmlFragment(ld.StudentName ?? string.Empty).Trim();
                string dobTyped = Sanitizer.GetSafeHtmlFragment(ld.DOB ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(rollNo))
                {
                    errDesc = Message.Candidate.RollNoRequired;
                    return false;
                }
                if (!GblFunctions.chkDataFormat(RegexType.RollNo, rollNo))
                {
                    errDesc = Message.Candidate.InvalidRollNo;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(schoolIndexNoTyped))
                {
                    errDesc = Message.Candidate.SchoolIndexNoRequired;
                    return false;
                }
                if (!GblFunctions.chkDataFormat(RegexType.SchoolIndexNo, schoolIndexNoTyped))
                {
                    errDesc = Message.Candidate.InvalidSchoolIndexNo;
                    return false;
                }
                if (string.IsNullOrWhiteSpace(studentNameTyped))
                {
                    errDesc = Message.Candidate.NameRequired;
                    return false;
                }
                if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, studentNameTyped))
                {
                    errDesc = Message.Candidate.InvalidName;
                    return false;
                }

                dt = new Login().CandidateValidateLogin(Sanitizer.GetSafeHtmlFragment(ld.RollNo ?? string.Empty).Trim());

                if (dt != null && dt.Rows.Count > 0)
                {
                    userTypeId = dt.Rows[0]["UserTypeId"].ToString();

                    //if user is not of candidate, reject login
                    if (userTypeId != UserType.UserTypeID.CANDIDATE)
                    {
                        errDesc = Message.InvalidRequest;
                        status = false;
                    }
                    else
                    {
                        groupId = dt.Rows[0]["GroupId"].ToString();
                        userId = dt.Rows[0]["UserId"].ToString();
                        OrganizationId = dt.Rows[0]["OrganizationId"].ToString();
                        OrganizationCode = dt.Rows[0]["OrganizationCode"].ToString();
                        indexNo = OrganizationCode;
                        organizationName = dt.Rows[0]["OrganizationName"].ToString();
                        name = dt.Rows[0]["Name"].ToString();
                        dob = dt.Rows[0]["DOB"].ToString();
                        groupName = dt.Rows[0]["GroupName"].ToString();
                        //newPasswordChangedYN = dt.Rows[0]["NewPasswordChangedYN"].ToString();
                        //emailVerifiedYN = dt.Rows[0]["EmailVerifiedYN"].ToString();
                        //contactNoVerifiedYN = dt.Rows[0]["ContactNoVerifiedYN"].ToString();
                        profileImage = dt.Rows[0]["ProfileImage"].ToString();
                        //subDivisionId = dt.Rows[0]["SubDivisionId"].ToString();

                        if (indexNo.ToLower() == schoolIndexNoTyped.ToLower() && name.ToLower() == studentNameTyped.ToLower() && dob.ToLower() == dobTyped.ToLower())
                        {
                            HttpContext.Current.Session[SessionNames.Role] = UserType.CANDIDATE;
                            HttpContext.Current.Session[SessionNames.GroupId] = groupId;
                            HttpContext.Current.Session[SessionNames.UserTypeId] = userTypeId;
                            HttpContext.Current.Session[SessionNames.OrganizationId] = OrganizationId;
                            HttpContext.Current.Session[SessionNames.OrganizationCode] = OrganizationCode;
                            HttpContext.Current.Session[SessionNames.UserId] = userId;
                            HttpContext.Current.Session[SessionNames.Name] = name;
                            HttpContext.Current.Session[SessionNames.GroupName] = groupName;
                            HttpContext.Current.Session[SessionNames.ProfileImage] = profileImage + "?" + DateTime.Now.ToFileTime();
                            HttpContext.Current.Session[SessionNames.NewPasswordChangedYN] = "Y";

                            status = true;
                        }
                        else
                        {
                            status = false;
                            errDesc = Message.Candidate.InvalidLoginAttempt;
                        }
                    }
                }
                else
                {
                    status = false;
                    errDesc = Message.Candidate.InvalidLoginAttempt;
                }
            }
            catch
            {
                status = false;
                errDesc = Message.OperationError;

            }
            finally
            {
                dt = null;
            }

            //save log
            string logId = userId;
            string category = LogCategory.CandidateLogin;
            string logDetails = string.Empty;

            if (status == false)
            {
                logDetails = "Invalid login attempt(Roll No : " + ld.RollNo + ")";
            }
            else
            {
                logDetails = "Candidate with Roll No : " + ld.RollNo + " logged in successfully.";
            }

            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                string errDesc1 = string.Empty;

                int error = new Login().SaveLogDetails(logId, category, "SELECT", logDetails, ipAddress, userId, ref errDesc1);
            }
            catch
            {
                //nothing to be done
            }

            return status;
        }
    }
}
