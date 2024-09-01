using System;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Data;
using System.Linq;
using WBBSE.Models;
using WBBSE.Areas.Candidate.Models;

namespace WBBSE.Areas.Candidate.Controllers
{
    [Authorize(Roles = UserType.CANDIDATE), NoCache]
    public class CandidateHomeController : Controller
    {
        [HttpGet]
        public ActionResult Home()
        {
            try
            {
                /*VERIFICATION FOR RESPECTIVE USER TYPE, ONLY SCHOOL USER TYPE ALLOWED*/
                if (Session[SessionNames.UserTypeId] == null || Session[SessionNames.UserTypeId].ToString() != UserType.UserTypeID.CANDIDATE)
                {
                    return RedirectToAction("LogOut", "Web");
                }

                string dashBoard = string.Empty;
                int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]);

                dashBoard = new MCandidateHome().getMenuDetails(ForSite.Candidate, groupId);

                if (dashBoard == "error")
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                else
                {
                    ViewBag.HtmlStr = dashBoard;
                    return View();
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Home", "CandidateHomeController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetCandidateDetails()
        {
            int Err = 0;
            string CandidateData = string.Empty, ResultData = string.Empty, Eligibility = string.Empty;

            Err = new MCandidateHome().getCandidatePPRPPSDetails(ref CandidateData, ref ResultData, ref Eligibility);

            return Json(new { Err, CandidateData, ResultData, Eligibility }, JsonRequestBehavior.AllowGet);
        }
    }
}
