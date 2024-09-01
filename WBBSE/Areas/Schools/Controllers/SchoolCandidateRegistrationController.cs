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
using System.Collections.Generic;
using System.Linq;

namespace WBBSE.Areas.Schools.Controllers
{
    [Authorize]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class SchoolCandidateRegistrationController : Controller
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

                //columnList = new GblFunctions().makeGridColumns(columns, "Y", "Y");
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

        public ActionResult CandidateRegistration(string x)
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

                    ViewData[ViewDataNames.SchoolIndexNo] = Session[SessionNames.OrganizationCode].ToString();

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
                MCommon.saveExceptionLog(ex.Message, "CandidateRegistration/View", "SchoolCandidateRegistrationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetNationalityList()
        {
            var records = new MCandidateRegistration().GetNationalityList();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReligionList()
        {
            var records = new MCandidateRegistration().GetReligionList();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCasteList()
        {
            var records = new MCandidateRegistration().GetCasteList();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPhysicallyChallengedList()
        {
            var records = new MCandidateRegistration().GetPhysicallyChallengedList();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSexList()
        {
            var records = new MCandidateRegistration().GetSexList();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubjectListFilterFirstLanguage()
        {
            var records = new MCandidateRegistration().GetSubjectListFilter(1);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubjectListFilterSecondLanguage()
        {
            var records = new MCandidateRegistration().GetSubjectListFilter(2);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubjectListFilterOptionalElective()
        {
            var records = new MCandidateRegistration().GetSubjectListFilter(9);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubjectSymbolCode(string subID, HttpRequestMessage hms)
        {
            string LanguageSymbol = string.Empty, LanguageCode = string.Empty;
            new MCandidateRegistration().GetSubjectSymbolCode(Convert.ToInt32(subID), ref LanguageSymbol, ref LanguageCode);
            return Json(new { LanguageSymbol, LanguageCode }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CandidateRegistration(HttpPostedFileBase[] postedFiles, VMCandidateRegistration cr)
        {
            try
            {
                int status = 0;

                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);

                string mode = ((cr.EntType == "I") ? "ADD" : ((cr.EntType == "E") ? "EDIT" : ((cr.EntType == "D") ? "DELETE" : "ERROR")));

                if (mode == "ERROR") //if mode not retrieved, return error
                {
                    TempData["SaveStatus"] = "F";
                    return RedirectToAction("CandidateRegistration", "SchoolCandidateRegistration", rvd);
                }

                MCandidateRegistration oCr = new MCandidateRegistration();
                string errDesc = string.Empty;
                if (mode == "DELETE")
                {
                    int error = oCr.saveCandidateRegistration(postedFiles, cr, ref errDesc);

                    if (error == 0)
                    {
                        TempData["SaveStatus"] = "SD";
                        return RedirectToAction("CandidateRegistration", "SchoolCandidateRegistration", rvd);
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = errDesc;

                        status = makeData(Session[SessionNames.URL].ToString());

                        if (status == 1)
                        {
                            return View("~/Views/SchoolCandidateRegistration/CandidateRegistration.cshtml");
                        }
                        else if (status == -1)
                        {
                            return Redirect("~/Error/Unexpected.html");
                        }
                        else
                        {
                            return Redirect("~/Error/Unexpected.html");
                        }
                    }

                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        int error = oCr.saveCandidateRegistration(postedFiles, cr, ref errDesc);
                        if (error == 0)
                        {
                            TempData["SaveStatus"] = "SS";
                        }
                        else
                        {
                            TempData["SaveStatus"] = "F";
                        }
                        return RedirectToAction("CandidateRegistration", "SchoolCandidateRegistration", rvd);
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";

                        status = makeData(Session[SessionNames.URL].ToString());

                        if (status == 1)
                        {
                            return View("~/Views/SchoolCandidateRegistration/CandidateRegistration.cshtml");
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

            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "CandidateRegistration/Save", "SchoolCandidateRegistrationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult StudentRegistrationList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            var records = new MCandidateRegistration().GetStudentRegistrationList(page, limit, sortBy, direction, searchString, ref total);
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStudentRegistrationApprovalView(string s)
        {
            string View = string.Empty;
            string ViewApprovalId = "View";
            try
            {
                View = new MCandidateRegistration().getStudentRegistrationApprovalView(GblFunctions.Base64Decode(s), ViewApprovalId);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetStudentRegistrationApprovalView", "SchoolCandidateRegistrationApprovalController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

    }
}
