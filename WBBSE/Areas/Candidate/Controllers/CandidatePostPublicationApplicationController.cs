using System;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Linq;
using System.Web.Routing;
using System.Collections.Generic;
using WBBSE.Models;
using WBBSE.Areas.Candidate.Models;
using Common;
using ViewModel;

namespace WBBSE.Areas.Candidate.Controllers
{
    [Authorize(Roles = UserType.CANDIDATE), NoCache]
    public class CandidatePostPublicationApplicationController : Controller
    {
        private int makeData(string encRoleId)
        {
            int status = 0;

            try
            {
                int groupId = (Session[SessionNames.GroupId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.GroupId]);
                if (string.IsNullOrEmpty(encRoleId))
                {
                    status = 0;
                }
                else
                {
                    int roleId = Convert.ToInt32(GblFunctions.decryptPassword(encRoleId));
                    Session[SessionNames.RoleId] = roleId;

                    string ViewYN = string.Empty, AddYN = string.Empty, EditYN = string.Empty, DeleteYN = string.Empty, ReportYN = string.Empty, SystemYN = string.Empty;

                    /******************************************************
                     * columnList array information :-
                     * ----------------------------
                     * 0 : Database Field Name
                     * 1 : Column Name To Display In Grid
                     * 2 : Hidden : true/false
                    ******************************************************/

                    //Except Admin group, check permission setup.
                    if (groupId != 1 && groupId != 2)
                    {
                        MUserPermission mu = new MUserPermission();

                        //Check add/edit/delete/ view permission against a Role Id
                        mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                        if (ViewYN == "N")
                        {
                            status = 0; //configure for un-authenticated user access
                        }
                        else
                        {
                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.ReportYN] = (ReportYN == "Y") ? "visible" : "hidden";

                            status = 1;
                        }
                    }
                    else
                    {
                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.ReportYN] = "visible";
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "CandidatePostPublicationApplicationController");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult ApplicationForm(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    if (TempData[TempDataNames.SaveStatus] == null)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveSuccess)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.Candidate.PPApplicationSaveSuccess;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.Candidate.PPApplicationSaveError;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.ValidationFailed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = TempData[TempDataNames.ErrDesc];
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }

                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    int err = 0;
                    string errDesc = string.Empty;

                    //check module availaility first
                    err = new MPostPublication().chkCandidatePPRPPSModuleAvailability(PostPublicationMode.ApplicationForm, ref errDesc);

                    if (err > 0)
                    {
                        ViewData[ViewDataNames.ErrorAlertVisibility] = "";
                        ViewData[ViewDataNames.ErrDesc] = errDesc;
                        return View();
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorAlertVisibility] = "none";

                        //fetch necessary data
                        var applicationData = new MPostPublication().getCandidatePPRPPSApplicationDetails();

                        return View(applicationData);
                    }
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationForm/View", "CandidatePostPublicationApplicationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ApplicationForm(List<VMPostPublication> data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                string errDesc = string.Empty;

                if (ModelState.IsValid)
                {
                    err = new MPostPublication().saveCandidatePPRPPSDetails(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
                    }
                    else if (err == 2)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = errDesc;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }
                    return RedirectToAction("ApplicationForm", "CandidatePostPublicationApplication", rvd);
                }
                else
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";

                    status = makeData(Session[SessionNames.URL].ToString());

                    if (status == 1)
                    {
                        return View();
                    }
                    else
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationForm/Save", "CandidatePostPublicationApplicationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult ApplicationFormPrint(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    ViewData[ViewDataNames.RawData] = new MPostPublication().getCandidatePPRPPSApplicationFormPrintData();

                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    return View();
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationFormPrint", "CandidatePostPublicationApplicationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult ApplicationPaymentPrint(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    ViewData[ViewDataNames.RawData] = new MPostPublication().getCandidatePPRPPSApplicationPaymentPrintData();

                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    return View();
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationPaymentPrint", "CandidatePostPublicationApplicationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
