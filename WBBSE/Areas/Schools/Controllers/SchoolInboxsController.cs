using System;
using System.Web.Mvc;
using WBBSE.Areas.Schools.Models;
using System.Web.Routing;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using System.Text;

namespace WBBSE.Areas.Schools.Controllers
{
    /*******************************************************************************
     * A BRIEF HISTORY OF SchoolInboxsController.
     * CONTAINS SCHOOL INBOX & BROADCASTING MESSAGES(CIRCULAR & NOTIFICATIONS) RELATED ACTIONS 
     * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
     * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
     * WBBSE.AREAS.SCHOOLS.MODELS: MODEL CLASS FOR SchoolHome RELATED BUSINESS LOGICS & FUNCTIONS. 
     ******************************************************************************/

    /* ROLE BASED ASP.NET MVC AUTHORIZATION [ONLY SCHOOL ROLE IS ALLOWED FOR ACCESSING THIS CONTROLLER]
     * NoCache: PREVENT CACHING IN MVC
     */

    [Authorize(Roles = UserType.SCHOOL), NoCache]
    public class SchoolInboxsController : Controller
    {
        /* makeData()-- IT IS USED FOR ROLE BASED PERMISSION LIKE ADD/EDIT/DELETE AND VIEW PERMISSION. IT IS ALSO USED DEFINE GRID VIEW COLUMNS. 
         * PARAM1: encRoleId= UNIQUE ROLE ID (IN ENCRYPTED FORM) OF MASTER ROLE TABLE.         
         */
        private int makeData(string encRoleId)
        {
            int status = 0;

            try
            {
                int groupId = (Session[SessionNames.GroupId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.GroupId]); // groupId::USER GROUP UNIQUE ID
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
                     * COLUMNLIST ARRAY INFORMATION :-
                     * ----------------------------
                     * 0 : DATABASE FIELD NAME
                     * 1 : COLUMN NAME TO DISPLAY IN GRID
                     * 2 : HIDDEN : TRUE/FALSE
                    ******************************************************/
                    string columnList = string.Empty; // FOR CIRCULAR GRID
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

                    string columnListAlternative = string.Empty; // FOR NOTIFICATION GRID
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

                    // ROLE BASED PERMISSION CHECKING FOR SPECIFIC ROLE ID
                    mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                    // MINIMUM VIEW PERMISSION CHECKING
                    if (ViewYN == "N")
                    {
                        status = 0; //CONFIGURE FOR UN-AUTHENTICATED USER ACCESS
                    }
                    else
                    {
                        columnList = new GblFunctions().makeGridColumns(columns, "N", "N", "Y", "Y");
                        ViewData[ViewDataNames.GridColumns] = columnList;

                        columnListAlternative = new GblFunctions().makeGridColumns(columnsAlternative, "N", "N", "Y", "Y");
                        ViewData[ViewDataNames.GridColumnsAlternative] = columnListAlternative;

                        status = 1; //SUCCESS STATUS
                    }
                }
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "makeData", "SchoolInboxs");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult SchoolInboxs(string x) //DEFAULT ACTION, x=ENCRYPTED ROLE ID
        {
            try
            {
                int status = makeData(x);

                if (status == 1) //IF SUCCESS STATUS=1
                {
                    if (TempData[TempDataNames.SaveStatus] == null) //NO MESSAGE PASSING SO ERROR IS NOT VISIBLE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveSuccess) //SHOWING SAVE SUCCESS MESSAGE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.SaveMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveDelete) //SHOWING DELETE SUCCESS MESSAGE
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.DeleteMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed) //SHOWING FAILED MESSAGE
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

                    //SETTING ACTIVE LINK AT LEFT HAND SIDE MENU HIGHLIGHT
                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString(); //A:: THIS IS USED TO REPRESENT THE ANCHOR TAG ELIMENT

                    return View();
                }
                else if (status == -1) //NO VIEW WHEN ERROR IS GENERATED
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
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "SchoolInboxs/View", "SchoolInboxsController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetCircularList(int? page, int? limit, string sortBy, string direction, string searchString = null, string contentType = null) //AJAX CALL FOR CIRCULAR LIST GENERATION
        {
            int total = 0;
            string archiveYN = "N"; //DEFAULT VALUE IS N FOR ARCHIVE FLAG

            try
            {
                //archiveYN = (Session[SessionNames.ContentType].ToString() == ContentType.ARCHIVE) ? "Y" : "N";
                archiveYN = (string.IsNullOrEmpty(contentType)) ? "N" : ((contentType == "A") ? "Y" : "N"); //IF contentType IS A FOR ARCHIVE DATA 
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "GetCircularList", "SchoolInboxsController");
            }

            //GETTING THE RECORD SET FROM MODEL CLASS
            var records = new MSchoolInboxs().getCircularList(page, limit, sortBy, direction, MenuCode.Circulars, ref total, searchString, archiveYN); 

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNotificationList(int? page, int? limit, string sortBy, string direction, string searchString = null, string contentType = null) //AJAX CALL FOR NOTIFICATION LIST GENERATION
        {
            int total = 0;
            string archiveYN = "N"; //DEFAULT VALUE IS N FOR ARCHIVE FLAG

            try
            {
                archiveYN = (string.IsNullOrEmpty(contentType)) ? "N" : ((contentType == "A") ? "Y" : "N"); //IF contentType IS A FOR ARCHIVE DATA 
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "GetNotificationList", "SchoolInboxsController");
            }

            //GETTING THE RECORD SET FROM MODEL CLASS
            var records = new MSchoolInboxs().getNotificationList(page, limit, sortBy, direction, MenuCode.Notification, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

    }
}
