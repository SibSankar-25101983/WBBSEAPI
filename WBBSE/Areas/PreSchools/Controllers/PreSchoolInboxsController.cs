using System;
using System.Web.Mvc;
using WBBSE.Areas.PreSchools.Models;
using System.Web.Routing;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using System.Text;

namespace WBBSE.Areas.PreSchools.Controllers
{
    [Authorize(Roles = UserType.PRESCHOOL), NoCache]
    public class PreSchoolInboxsController : Controller
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
                    string columnList = string.Empty;
                    string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Circular", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "80"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                    string columnListAlternative = string.Empty;
                    string[,] columnsAlternative = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Notification", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "80"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                    MUserPermission mu = new MUserPermission();

                    mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                    if (ViewYN == "N")
                    {
                        status = 0; //configure for un-authenticated user access
                    }
                    else
                    {
                        columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "Y", "Y");
                        ViewData[ViewDataNames.GridColumns] = columnList;

                        columnListAlternative = new GblFunctions().makeGridColumns(columnsAlternative, "N", "N", "Y", "Y");
                        ViewData[ViewDataNames.GridColumnsAlternative] = columnListAlternative;

                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "PreSchoolInboxs");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult PreSchoolInboxs(string x)
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
                        ViewData[ViewDataNames.SaveInfo] = Message.SaveMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveDelete)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.DeleteMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.ErrorMsg;
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
                MCommon.saveExceptionLog(ex.Message, "PreSchoolInboxs/View", "PreSchoolInboxsController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetCircularList(int? page, int? limit, string sortBy, string direction, string searchString = null, string contentType = null)
        {
            int total = 0;
            string archiveYN = "N";

            try
            {                
                archiveYN = (string.IsNullOrEmpty(contentType)) ? "N" : ((contentType == "A") ? "Y" : "N");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetCircularList", "PreSchoolInboxsController");
            }

            var records = new MPreSchoolInboxs().getCircularList(page, limit, sortBy, direction, MenuCode.Circulars, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNotificationList(int? page, int? limit, string sortBy, string direction, string searchString = null, string contentType = null)
        {
            int total = 0;
            string archiveYN = "N";

            try
            {
                archiveYN = (string.IsNullOrEmpty(contentType)) ? "N" : ((contentType == "A") ? "Y" : "N");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetNotificationList", "PreSchoolInboxsController");
            }

            var records = new MPreSchoolInboxs().getNotificationList(page, limit, sortBy, direction, MenuCode.Notification, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

    }
}
