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
using ViewModel;
using DAL;
using Common;

namespace WBBSE.Areas.Schools.Models
{
    public class MSchoolProfileVerification
    {
        public VMProfileVerification getOrganizationProfileVerificationData(ref int err, int userId, ref string resendEmailOTPYN, ref string resendContactOTPYN, ref string showHomeLinkYN, int userTypeId, Int64 organizationId, int groupId, string mode)
        {
            VMProfileVerification data = new VMProfileVerification();
            DataTable dt = new DataTable();

            try
            {
                if (mode == ProfileVerificationDataFetchMode.New)
                {
                    dt = new ProfileVerification().GetOrganizationProfileVerificationData(userTypeId, organizationId, groupId);
                }
                else
                {
                    dt = (DataTable)HttpContext.Current.Session[SessionNames.ProfileVerificationData];
                }

                //if (HttpContext.Current.Session[SessionNames.ProfileVerificationData] == null)
                //{
                //    dt = new ProfileVerification().GetOrganizationProfileVerificationData(userTypeId, organizationId, groupId);
                //}
                //else
                //{
                //    dt = (DataTable)HttpContext.Current.Session[SessionNames.ProfileVerificationData];
                //}

                if (dt == null || dt.Rows.Count == 0)
                {
                    err = 1;
                }
                else
                {
                    err = 0;
                    data.EMailId = dt.Rows[0]["EMailId"].ToString();
                    data.ContactNo = dt.Rows[0]["ContactNo"].ToString(); //Mobile No
                    data.StdCode = dt.Rows[0]["StdCode"].ToString();
                    data.PhoneNo = dt.Rows[0]["PhoneNo"].ToString();
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