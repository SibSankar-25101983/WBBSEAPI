using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Security.Application;
using WBBSE.Models;
using DAL;
using Common;
using ViewModel;

namespace WBBSE.Areas.Candidate.Models
{
    public class MPostPublication
    {
        public int chkCandidatePPRPPSModuleAvailability(int mode, ref string errDesc)
        {
            int err = 0;

            try
            {
                string rollNo = (HttpContext.Current.Session[SessionNames.UserId]).ToString();

                err = new PostPublication().ChkCandidatePPRPPSModuleAvailability(rollNo, mode, ref errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "ApplicationForm/chkCandidatePPRPPSModuleAvailability(MPostPublication)", "CandidatePostPublicationApplicationController");
                    errDesc = Message.OperationError;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationForm/chkCandidatePPRPPSModuleAvailability(MPostPublication)", "CandidatePostPublicationApplicationController");
                err = 1;
                errDesc = Message.OperationError;
            }

            return err;
        }

        public List<VMPostPublication> getCandidatePPRPPSApplicationDetails()
        {
            DataTable dt = new DataTable();
            List<VMPostPublication> result = new List<VMPostPublication>();

            try
            {
                string rollNo = (HttpContext.Current.Session[SessionNames.UserId]).ToString();

                dt = new PostPublication().GetCandidatePPRPPSApplicationDetails(rollNo);

                var records = (from data in dt.AsEnumerable()
                               select new VMPostPublication
                               {
                                   ScrutinySubjectId = data.Field<int>("ScrutinySubjectId"),
                                   SubjectName = data.Field<string>("SubjectName"),
                                   SubjectAbbreviation = data.Field<string>("SubjectAbbreviation"),
                                   SubjectCode = data.Field<string>("SubjectCode"),
                                   Marks = (float)data.Field<double>("Marks"),
                                   Grade = data.Field<string>("Grade"),
                                   AppliedYN = data.Field<string>("AppliedYN")
                               });

                result = records.ToList();
            }
            catch (Exception ex)
            {
                result = null;
                MCommon.saveExceptionLog(ex.Message, "ApplicationForm/getCandidatePPRPPSApplicationDetails(MPostPublication)", "CandidatePostPublicationApplicationController");
            }

            return result;
        }

        public int saveCandidatePPRPPSDetails(List<VMPostPublication> data, ref string errDesc)
        {
            int err = 0, totalSubject = 0;
            float totalPrice = 0;
            string ipAddress = string.Empty, postPublicationType = string.Empty, rollNo = string.Empty;
            StringBuilder applicationDetails = new StringBuilder();

            try
            {
                rollNo = HttpContext.Current.Session[SessionNames.UserId].ToString();
                postPublicationType = HttpContext.Current.Session[SessionNames.PostPublicationType].ToString();

                applicationDetails.Append("<PPDetails>");
                for (int i = 0; i < data.Count; i++)
                {
                    string appliedYN = Sanitizer.GetSafeHtmlFragment((data[i].AppliedYN ?? "N").Trim());
                    if (appliedYN != DefaultSetting.DefaultValN && appliedYN != DefaultSetting.DefaultValY)
                    {
                        err = 2;
                        errDesc = Message.InvalidRequest;
                        return err;
                    }
                    if (appliedYN == DefaultSetting.DefaultValY)
                    {
                        applicationDetails.Append("<Subject>");
                        applicationDetails.Append("<ScrutinySubjectId>" + Sanitizer.GetSafeHtmlFragment(data[i].ScrutinySubjectId.ToString()) + "</ScrutinySubjectId>");
                        applicationDetails.Append("</Subject>");
                        totalSubject++;
                    }
                }
                applicationDetails.Append("</PPDetails>");

                if (totalSubject == 0)
                {
                    err = 2;
                    errDesc = Message.Candidate.SubjectSelectionRequired;
                    return err;
                }

                totalPrice = Convert.ToSingle(HttpContext.Current.Session[SessionNames.PostPublicationPrice]);
                totalPrice *= totalSubject;

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                err = new PostPublication().SaveCandidatePPRPPSDetails(rollNo, postPublicationType, totalSubject, totalPrice, applicationDetails.ToString(), ipAddress, ref errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "ApplicationForm/saveCandidatePPRPPSDetails(MPostPublication)", "CandidatePostPublicationApplicationController");
                    errDesc = Message.OperationError;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = Message.OperationError;
                MCommon.saveExceptionLog(ex.Message, "ApplicationForm/saveCandidatePPRPPSDetails(MPostPublication)", "CandidatePostPublicationApplicationController");
            }
            finally
            {
                applicationDetails = null;
            }

            return err;
        }

        public string getCandidatePPRPPSApplicationFormPrintData()
        {
            string data = string.Empty;
            int err = 0;

            try
            {
                string rollNo = (HttpContext.Current.Session[SessionNames.UserId]).ToString();

                err = new PostPublication().GetCandidatePPRPPSApplicationFormPrintData(rollNo, ref data);

                if (err == 1)
                {
                    data = "<div class='row d-print-none'><div class='col-md-8 mx-auto'><div class='alert alert-danger m-0'><strong>" + Message.OperationError + "</strong></div></div></div>";
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationFormPrint/getCandidatePPRPPSApplicationFormPrintData(MPostPublication)", "CandidatePostPublicationApplicationController");
                data = "<div class='row d-print-none'><div class='col-md-8 mx-auto'><div class='alert alert-danger m-0'><strong>" + Message.OperationError + "</strong></div></div></div>";
            }

            return data;
        }

        public string getCandidatePPRPPSApplicationPaymentPrintData()
        {
            string data = string.Empty;
            int err = 0;

            try
            {
                string rollNo = (HttpContext.Current.Session[SessionNames.UserId]).ToString();

                err = new PostPublication().GetCandidatePPRPPSApplicationPaymentPrintData(rollNo, ref data);

                if (err == 1)
                {
                    data = "<div class='row d-print-none'><div class='col-md-8 mx-auto'><div class='alert alert-danger m-0'><strong>" + Message.OperationError + "</strong></div></div></div>";
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationPaymentPrint/getCandidatePPRPPSApplicationPaymentPrintData(MPostPublication)", "CandidatePostPublicationApplicationController");
                data = "<div class='row d-print-none'><div class='col-md-8 mx-auto'><div class='alert alert-danger m-0'><strong>" + Message.OperationError + "</strong></div></div></div>";
            }

            return data;
        }
    }
}
