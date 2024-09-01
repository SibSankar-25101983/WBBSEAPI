using System;
using System.Web.Mvc;
using WBBSE.Areas.Admin.Models;
using System.Web.Routing;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using System.Web;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminBoardDeskController : Controller
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
                        {"ContentId", "ContentId", "true"},
                        {"BodyTextPartial", "Content", "false"},
                        {"BodyText", "BodyText", "true"},
                        {"ImagePath", "ImagePath", "true"}
                    };

                    if (groupId != 1 && groupId != 2)
                    {
                        MUserPermission mu = new MUserPermission();

                        mu.chkPagePermission(roleId, ref ViewYN, ref AddYN, ref EditYN, ref DeleteYN, ref ReportYN, ref SystemYN);

                        if (ViewYN == "N")
                        {
                            status = 0; //configure for un-authenticated user access
                        }
                        else
                        {
                            columnList = new GblFunctions().makeGridColumns(columns, EditYN, DeleteYN, "N", "Y");

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.GridColumns] = columnList;

                            status = 1;
                        }
                    }
                    else
                    {
                        columnList = new GblFunctions().makeGridColumns(columns, "Y", "Y", "N", "Y");

                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminUserController");
                status = -1;
            }

            return status;
        }

        public ActionResult PresidentDesk(string x)
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
                MCommon.saveExceptionLog(ex.Message, "PresidentDesk/View", "AdminBoardDeskController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetPresidentDeskList(string sortBy, string direction)
        {
            int total = 0;
            int? page = 1;
            int? limit = 1;

            var records = new MBoardDesk().getBoardDeskList(page, limit, sortBy, direction, MenuCode.PresidentDesk, ref total);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult PresidentDesk(HttpPostedFileBase[] postedFiles, VMBoardDesk data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                string errDesc = string.Empty;
                string mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    TempData[TempDataNames.SaveStatus] = "F";
                    return RedirectToAction("PresidentDesk", "AdminBoardDesk", rvd);
                }

                if (mode == Mode.DELETE) //if mode = delete, bypass model state checking
                {
                    err = new MBoardDesk().saveBoardDesk(postedFiles, data, MenuCode.PresidentDesk, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }
                    return RedirectToAction("PresidentDesk", "AdminBoardDesk", rvd);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        data.HeaderText = BoardDeskType.President;

                        err = new MBoardDesk().saveBoardDesk(postedFiles, data, MenuCode.PresidentDesk, ref errDesc);

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
                        return RedirectToAction("PresidentDesk", "AdminBoardDesk", rvd);
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
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PresidentDesk/Save", "AdminBoardDeskController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        public ActionResult SecretaryDesk(string x)
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
                MCommon.saveExceptionLog(ex.Message, "SecretaryDesk/View", "AdminBoardDeskController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSecretaryDeskList(string sortBy, string direction)
        {
            int total = 0;
            int? page = 1;
            int? limit = 1;

            var records = new MBoardDesk().getBoardDeskList(page, limit, sortBy, direction, MenuCode.SecretaryDesk, ref total);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult SecretaryDesk(HttpPostedFileBase[] postedFiles, VMBoardDesk data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                string errDesc = string.Empty;
                string mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    TempData[TempDataNames.SaveStatus] = "F";
                    return RedirectToAction("SecretaryDesk", "AdminBoardDesk", rvd);
                }

                if (mode == Mode.DELETE) //if mode = delete, bypass model state checking
                {
                    err = new MBoardDesk().saveBoardDesk(postedFiles, data, MenuCode.SecretaryDesk, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }
                    return RedirectToAction("SecretaryDesk", "AdminBoardDesk", rvd);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        data.HeaderText = BoardDeskType.Secretary;

                        err = new MBoardDesk().saveBoardDesk(postedFiles, data, MenuCode.SecretaryDesk, ref errDesc);

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
                        return RedirectToAction("SecretaryDesk", "AdminBoardDesk", rvd);
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
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SecretaryDesk/Save", "AdminBoardDeskController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
