using System;
using System.Web;
using System.Net;
using System.Data;
using System.Web.UI;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Routing;
using WBBSE.Areas.Schools.Models;
using Common;
using WBBSE.Models;
using ViewModel;

namespace WBBSE.Areas.Schools.Controllers
{
    public class SchoolCandidateRegistrationApprovalController : Controller
    {
        private int makeData(string encRoleId)
        {
            int status = 0;

            try
            {
                /******************************************************
                 * columnList array information :-
                 * ----------------------------
                 * 0 : Database Field Name
                 * 1 : Column Name To Display In Grid
                 * 2 : Hidden : true/false
                ******************************************************/

                int roleId = Convert.ToInt32(GblFunctions.decryptPassword(encRoleId));
                Session[SessionNames.RoleId] = roleId;

                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "Sl No", "false"},
                        {"StudentId", "Student Id", "true"},
                        {"RegistrationYear", "Registration Year", "false"},
                        {"MadhyamikParikshaYear", "Madhyamik Pariksha Year", "true"},
                        {"SchoolIndex", "School Index", "true"},
                        {"FormNo", "Form No", "false"}, 
                        {"SchoolName", "School Name", "true"},
                        {"StudentName", "Student Name", "false"},
                        {"FathersName", "Fathers Name", "false"},
                        {"MothersName", "Mothers Name", "true"},
                        {"GuardiansName", "Guardians Name", "true"}, 
                        {"Address", "Address", "true"},
                        {"Pin", "Pin", "true"},
                        {"ContactNo", "Contact No", "true"},
                        {"NationalityCode", "Nationality Code", "true"},
                        {"ReligionCode", "Religion Code", "true"}, 
                        {"CasteCode", "Caste Code", "true"},
                        {"PhysicallychallengedCode", "Physicallychallenged Code", "true"},
                        {"SexCode", "Sex Code", "false"},
                        {"DateOfBirth", "Date Of Birth", "false"},
                        {"FirstLanguageId", "First Language ID", "true"},
                        {"FirstLanguageSymbol", "First Language Symbol", "true"}, 
                        {"FirstLanguageCode", "First Language Code", "true"},
                        {"SecondLanguageId", "Second Language ID", "true"}, 
                        {"SecondLanguageSymbol", "Second Language Symbol", "true"},
                        {"SecondLanguageCode", "Second Language Code", "true"}, 
                        {"OptionalElectiveId", "Optional Elective ID", "true"},
                        {"OptionalElectiveSymbol", "Optional Elective Symbol", "true"},
                        {"OptionalElectiveCode", "Optional Elective Code", "true"},
                        {"PupilAdmitted", "Pupil Admitted", "true"},
                        {"DateOfAdmission", "Date Of Admission", "true"},
                        {"StudentImageName", "Student Image Name", "true"}, 
                        {"StudentImagePath", "Student Image Path", "true"},
                        {"StudentSignatureName", "Student Signature Name", "true"},
                        {"StudentSignaturePath", "Student Signature Path", "true"}, 
                    };

                columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "N", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;
                status = 1;
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "CandidateRegistration", "CandidateRegistrationController/makeData");
                status = -1;
            }

            return status;
        }

        public ActionResult CandidateRegistrationApproval(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    //check edit permission
                    Int64 schoolId = Convert.ToInt64(Session[SessionNames.OrganizationId]);
                    string editFor = SchoolProfileEditFor.Others;

                    string EditYN = new MSchoolProfile().chkSchoolEditPermission(schoolId, editFor);

                    if (EditYN == "Y")
                    {
                        RouteValueDictionary rvd = new RouteValueDictionary();
                        rvd.Add("x", Message.School.RegPermission);
                        return RedirectToAction("SchoolValidation", "SchoolValidation", rvd);
                    }

                    if (TempData["SaveStatus"] == null)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }
                    else if (TempData["SaveStatus"].ToString() == "SS")
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.SaveMsg;
                    }
                    else if (TempData["SaveStatus"].ToString() == "SD")
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.DeleteMsg;
                    }
                    else if (TempData["SaveStatus"].ToString() == "F")
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
                    }

                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                    return View();
                }
                else if (status == -1)
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                else //CONFIGURE FOR NO VIEW PERMISSION
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "CandidateRegistration/View", "CandidateRegistrationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult StudentRegistrationList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            var records = new MCandidateRegistrationApproval().GetStudentRegistrationApprovalDetailsList(page, limit, sortBy, direction, searchString, ref total);
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStudentRegistrationApprovalView(string s)
        {
            string View = string.Empty;
            string ViewApprovalId = "Approval";
            try
            {
                View = new MCandidateRegistrationApproval().getStudentRegistrationApprovalView(GblFunctions.Base64Decode(s), ViewApprovalId);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetStudentRegistrationApprovalView", "SchoolCandidateRegistrationApprovalController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CandidateRegistrationApproval(VMCandidateRegistration data)
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

                    err = new MCandidateRegistrationApproval().saveCandidateRegistration(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
                    }
                    else if (err == 2)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = errDesc;

                        status = makeData(Session[SessionNames.URL].ToString());

                        if (status == 1)
                        {
                            return View();
                        }
                        else if (status == -1)
                        {
                            return Redirect("~/Error/Unexpected.html");
                        }
                        else //CONFIGURE FOR NO VIEW PERMISSION
                        {
                            return Redirect("~/Error/Unexpected.html");
                        }
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }
                    return RedirectToAction("CandidateRegistrationApproval", "SchoolCandidateRegistrationApproval", rvd);
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
                    else if (status == -1)
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                    else //CONFIGURE FOR NO VIEW PERMISSION
                    {
                        return Redirect("~/Error/Unexpected.html");
                    }
                }

            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "CandidateRegistrationApproval/Save", "SchoolCandidateRegistrationApprovalController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
