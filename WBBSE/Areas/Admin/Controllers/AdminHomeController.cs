using System;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Linq;
using Common;
using WBBSE.Models;
using WBBSE.Areas.Admin.Models;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminHomeController : Controller
    {
        public ActionResult AdminHome()
        {
            try
            {
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "N" || (Session[SessionNames.EmailVerifiedYN].ToString() == "N" && Session[SessionNames.ContactNoVerifiedYN].ToString() == "N"))
                {
                    return RedirectToAction("AdminProfileVerification", "AdminProfileVerification");
                }

                if (Session[SessionNames.UserTypeId] == null || Session[SessionNames.UserTypeId].ToString() != UserType.UserTypeID.ADMIN)
                {
                    //return Redirect("~/Error/Unexpected.html");
                    return RedirectToActionPermanent("Login", "AdminLogin");
                }

                string dashBoard = string.Empty;
                int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]);
                MAdminHome ah = new MAdminHome();

                dashBoard = ah.getMenuDetails(ForSite.Admin, groupId);

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
                /********Handleing catch exception Log *****************/
                MCommon.saveExceptionLog(ex.Message, "AdminHome", "AdminHomeController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetStat1View()
        {
            int err = 0;
            DataTable dt = new DataTable();

            try
            {
                dt = new MAdminHome().getAdminWebsiteDashboardRegistrationStatistics(ref err);

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
            catch(Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "GetStat1View", "AdminHomeController");
                return Json(new { err }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                dt = null;
            }
        }

        [HttpGet]
        public JsonResult GetStat2View()
        {
            string View = string.Empty;

            try
            {
                View = new MAdminHome().getAdminWebsiteDashboardStatistics();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetStat2View", "AdminHomeController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }
    }
}
