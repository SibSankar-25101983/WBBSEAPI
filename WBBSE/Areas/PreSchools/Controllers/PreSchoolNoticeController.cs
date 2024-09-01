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
    public class PreSchoolNoticeController : Controller
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
                        {"SlNo", "Sl No", "false", "60"},
                        {"InboxId", "InboxId", "true", "0"},
                        {"BodyTextUnread", "Notice", "false", "600"},
                        {"LinkType", "Link Type", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "100"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"},
                        {"NewYN", "NewYN", "true", "0"},
                        {"ArchiveYN", "ArchiveYN", "true", "0"},
                        {"LinkTypeId", "LinkTypeId", "true", "0"},
                        {"SchoolIdList", "SchoolIdList", "true", "0"}
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

                        ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                        ViewData[ViewDataNames.GridColumns] = columnList;

                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "PreSchoolNoticeController");
                status = -1;
            }

            return status;
        }

        public ActionResult Notice(string x)
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

                    //set update count = 1
                    Session[SessionNames.UnreadNoticeUpdateCount] = 1;
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
                MCommon.saveExceptionLog(ex.Message, "Notice/View", "PreSchoolNoticeController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSchoolInboxList(int? page, int? limit, string sortBy, string direction, string searchString = null, string contentType = null)
        {
            int total = 0;
            string archiveYN = "N";

            try
            {
                archiveYN = (string.IsNullOrEmpty(contentType)) ? "N" : ((contentType == "A") ? "Y" : "N");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxList", "PreSchoolNoticeController");
            }

            var records = new MPreSchoolInboxs().getAdminPreSchoolInboxList(page, limit, sortBy, direction, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UnreadNoticeListStatus()
        {
            int Err = 0;

            Err = new MPreSchoolInboxs().updateAdminPreSchoolReadStatus();

            return Json(new { Err }, JsonRequestBehavior.AllowGet);
        }
    }
}
