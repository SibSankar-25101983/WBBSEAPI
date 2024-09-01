using System;
using System.Web.Mvc;
using WBBSE.Areas.Admin.Models;
using System.Web.Routing;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminUserController : Controller
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
                        {"SlNo", "Sl No", "false", "0"},
                        {"UserId", "UserId", "true", "0"},
                        {"Name", "Name", "false", "400"},
                        {"UserType", "User Type", "false", "0"},
                        {"Designation", "Designation", "false", "0"},
                        {"IsActive", "Is Active", "false", "0"}
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
                        //if (groupId == 1)
                        //{
                        //    columnList = new GblFunctions().makeGridColumns(columns, "Y", "Y", "Y", "Y");
                        //}
                        //else
                        //{
                        //    columnList = new GblFunctions().makeGridColumns(columns, "Y", "N", "Y", "Y");
                        //}

                        columnList = new GblFunctions().makeGridColumns(columns, "Y", "N", "Y", "Y");

                        ViewData[ViewDataNames.AddYN] = "hidden";
                        //ViewData[ViewDataNames.AddYN] = "visible";
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

        public ActionResult Users(string x)
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
                MCommon.saveExceptionLog(ex.Message, "Schools/View", "AdminSchoolController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]

        public JsonResult GetUserList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;

            var records = new MMstUser().getMstUserList(page, limit, sortBy, direction, searchString, ref total);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserView(string u)
        {
            string View = string.Empty;

            try
            {
                View = new MMstUser().getMstUserView(GblFunctions.Base64Decode(u));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetUserView", "AdminUserController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSalutationList()
        {
            var records = new MCommon().getMstSalutationListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDesignationList()
        {
            var records = new MCommon().getMstDesignationListDropDown(UserType.SCHOOL);

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserDetails(string u)
        {
            var Records = new MMstUser().getMstUserDetails(GblFunctions.Base64Decode(u));

            return Json(new { Records }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChkDuplicateContact(string e, string m, string u, string et)
        {
            int err = 0;
            string errDesc = string.Empty;

            err = new MMstUser().chkDuplicateContactMstUser((e ?? string.Empty), (m ?? string.Empty), GblFunctions.Base64Decode(u ?? DefaultSetting.DefaultValEnc), et, ref errDesc);

            return Json(new { err, errDesc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Users(VMMstUser data)
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
                    TempData[TempDataNames.SaveStatus] = "F";
                    return RedirectToAction("Users", "AdminUser", rvd);
                }

                if (mode == Mode.DELETE) //if mode = delete, bypass model state checking
                {
                    //future scope
                    return RedirectToAction("Schools", "AdminSchool", rvd);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (mode == Mode.EDIT)
                        {
                            err = new MMstUser().updateMstUserByAdmin(data, ref errDesc);
                        }
                        else
                        {
                            //add :: future scope
                            return RedirectToAction("Schools", "AdminSchool", rvd);
                        }

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
                        return RedirectToAction("Users", "AdminUser", rvd);
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
                MCommon.saveExceptionLog(ex.Message, "Users/Save", "AdminUserController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
