using System;
using System.Web;
using System.Web.Mvc;
using Common;
using WBBSE.Models;
using WBBSE.Areas.PreSchools.Models;

namespace WBBSE.Areas.PreSchools.Controllers
{
    [Authorize(Roles = UserType.PRESCHOOL), NoCache]
    public class PreSchoolHomeController : Controller
    {
        public ActionResult PreSchoolHome(string m)
        {
            try
            {
                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "N") // || (Session[SessionNames.EmailVerifiedYN].ToString() == "N" && Session[SessionNames.ContactNoVerifiedYN].ToString() == "N")
                {
                    return RedirectToAction("PreSchoolProfileVerification", "PreSchoolProfileVerification");
                }

                if (!string.IsNullOrEmpty(m))
                {
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = GblFunctions.decryptPassword(m);
                }
                else
                {
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                }

                if (Session[SessionNames.UserTypeId] == null || Session[SessionNames.UserTypeId].ToString() != UserType.UserTypeID.PRESCHOOL)
                {
                    return RedirectToAction("Default", "Web");
                }

                string dashBoard = string.Empty;
                int groupId = (Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(Session[SessionNames.GroupId]);

                dashBoard = new MPreSchoolHome().getMenuDetails(ForSite.PreSchool, groupId);

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
                MCommon.saveExceptionLog(ex.Message, "PreSchoolHome", "PreSchoolHomeController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetPreSchoolProfileDashBoardData()
        {
            var ProfileData = new MPreSchoolHome().getPreSchoolProfileDashBoardData();

            return Json(new { ProfileData }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnreadNoticeList()
        {
            var Notice = (Session[SessionNames.UnreadNotice] == null) ? new MPreSchoolInboxs().getPreSchoolUnreadNoticeList() : string.Empty;
            return Json(new { Notice }, JsonRequestBehavior.AllowGet);
        }
    }
}
