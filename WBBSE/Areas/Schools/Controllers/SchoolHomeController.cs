using System;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Data;
using System.Linq;
using WBBSE.Models;
using WBBSE.Areas.Schools.Models;

namespace WBBSE.Areas.Schools.Controllers
{
    [Authorize(Roles = UserType.SCHOOL), NoCache]
    public class SchoolHomeController : Controller
    {
        public ActionResult SchoolHome(string m)
        {
            try
            {
                /* CHECKING IF DEFAULT PASSWORD NOT CHANGED THEN REDIRECT TO PASSWORD CHANGE PAGE FOR BESIC SCHOOL PROFILE DATA CAPTURE
                 * NewPasswordChangedYN='N' MEANS PASSWORD NOT CHANGED YET
                 */
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "N") // || (Session[SessionNames.EmailVerifiedYN].ToString() == "N" && Session[SessionNames.ContactNoVerifiedYN].ToString() == "N")
                {
                    return RedirectToAction("SchoolProfileVerification", "SchoolProfileVerification");
                }

                /*SHOWING THE MESSAGE TO VIEW PAGE THROUGH VIEWDATA*/
                if (!string.IsNullOrEmpty(m))
                {
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = GblFunctions.decryptPassword(m);
                }
                else
                {
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                }

                /*VERIFICATION FOR RESPECTIVE USER TYPE, ONLY SCHOOL USER TYPE ALLOWED*/
                if (Session[SessionNames.UserTypeId] == null || Session[SessionNames.UserTypeId].ToString() != UserType.UserTypeID.SCHOOL)
                {
                    return RedirectToAction("Default", "Web");
                }

                string dashBoard = string.Empty;
                int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]);  //RETRIVING THE UNIQUE USER GROUP ID FROM SESSION IF NULL THEN '-1' MEANS NOTHING

                /*CALLING MODEL MSchoolHome() FOR DASHBOARD MENU DATA*/
                dashBoard = new MSchoolHome().getMenuDetails(ForSite.School, groupId);

                /*IF RETURN ERROR THEN REDIRECT TO COMMON ERROR VIEW PAGE*/
                if (dashBoard == "error")
                {
                    return Redirect("~/Error/Unexpected.html");
                }
                else
                {
                    /*POPULATION OF DASHBOARD, PASSING DATA TO VIEW THROUGH VIEWGAG*/
                    ViewBag.HtmlStr = dashBoard;
                    return View();
                }
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "SchoolHome", "SchoolHomeController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetStat1View()
        {
            var ProfileData = new MSchoolHome().getSchoolProfileDashBoardData();
            return Json(new { ProfileData }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStat2View()
        {
            int err = 0;
            DataTable dt = new DataTable();

            try
            {
                dt = new MSchoolHome().getSchoolDashboardRegistrationStatistics(ref err);

                if (err > 0 || dt == null)
                {
                    err = 1;
                    return Json(new { err }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int[] RegistrationStatValue = dt.Rows[0]["RegistrationStatValue"].ToString().Split('#').Select(n => Convert.ToInt32(n)).ToArray();
                    string[] RegistrationStatValueLabel = dt.Rows[0]["RegistrationStatValueLabel"].ToString().Split('#');
                    string RegistrationLabel = dt.Rows[0]["RegistrationLabel"].ToString();
                    string[] RegistrationColors = dt.Rows[0]["RegistrationColors"].ToString().Split(',');
                    int[] RegistrationApprovalStatValue = dt.Rows[0]["RegistrationApprovalStatValue"].ToString().Split('#').Select(n => Convert.ToInt32(n)).ToArray();
                    string[] RegistrationApprovalStatValueLabel = dt.Rows[0]["RegistrationApprovalStatValueLabel"].ToString().Split('#');
                    string RegistrationApprovalLabel = dt.Rows[0]["RegistrationApprovalLabel"].ToString();
                    string[] RegistrationApprovalColors = dt.Rows[0]["RegistrationApprovalColors"].ToString().Split(',');

                    return Json(new { err, RegistrationStatValue, RegistrationStatValueLabel, RegistrationLabel, RegistrationColors, RegistrationApprovalStatValue, RegistrationApprovalStatValueLabel, RegistrationApprovalLabel, RegistrationApprovalColors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "GetStat2View", "SchoolHomeController");
                return Json(new { err }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                dt = null;
            }
        }

        [HttpGet]
        public JsonResult GetUnreadNoticeList()
        {
            var Notice = (Session[SessionNames.UnreadNotice] == null) ? new MSchoolInboxs().getSchoolUnreadNoticeList() : string.Empty;
            return Json(new { Notice }, JsonRequestBehavior.AllowGet);
        }
    }
}
