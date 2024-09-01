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
    public class AdminBooksController : Controller
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
                        {"BookId", "BookId", "true", "0"},
                        {"BookName", "Book", "false", "600"},
                        {"BookCode", "BookCode", "true", "0"},
                        {"Class", "Class", "false", "80"},
                        {"SchoolMediumId", "SchoolMediumId", "true", "0"},
                        {"SchoolMediumName", "SchoolMediumName", "true", "0"},
                        {"BookPrice", "BookPrice", "true", "0"}
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
                            columnList = new GblFunctions().makeGridColumns(columns, EditYN, DeleteYN, "Y", "Y");

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.GridColumns] = columnList;

                            status = 1;
                        }
                    }
                    else
                    {
                        columnList = new GblFunctions().makeGridColumns(columns, "Y", "Y", "Y", "Y");

                        ViewData[ViewDataNames.AddYN] = "visible";
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminBooksController");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult BookList(string x)
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
                MCommon.saveExceptionLog(ex.Message, "BookList/View", "AdminBooksController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetBookList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;

            var records = new MBooks().getMstBookList(page, limit, sortBy, direction, ref total, searchString);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolMediumList()
        {
            var records = new MSchoolParameters().getMstSchoolMediumListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult BookList(VMMstBooks data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                string errDesc = string.Empty;
                string mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    return RedirectToAction("BookList", "AdminBooks", rvd);
                }

                if (mode == Mode.DELETE)
                {
                    err = new MBooks().saveMstBook(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
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
                    return RedirectToAction("BookList", "AdminBooks", rvd);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        err = new MBooks().saveMstBook(data, ref errDesc);

                        if (err == 0)
                        {
                            TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
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
                        return RedirectToAction("BookList", "AdminBooks", rvd);
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
                        else
                        {
                            return Redirect("~/Error/Unexpected.html");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "BookList/Save", "AdminBooksController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
