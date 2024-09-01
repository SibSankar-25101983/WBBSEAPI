using System;
using System.Web;
using System.Net;
using System.Data;
using System.Web.UI;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Routing;
using WBBSE.Models;
using WBBSE.Areas.Admin.Models;
using ViewModel;
using Common;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminUserGroupController : Controller
    {
        private int makeData(string encRoleId)
        {
            int status = 0;

            try
            {
                DataTable dt = new DataTable();
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
                        {"GroupId", "GroupId", "true"},
                        {"UserTypeId", "UserTypeId", "true"},
                        {"ActiveYN", "ActiveYN", "true"},
                        {"GroupName", "Group Name", "false"},
                        {"UserType", "Group Type", "false"}
                    };

                    string roleDetailsColumnList = @"{ field: 'RoleId', title: 'Id', width: 30, sortable: false, hidden:true },
                                                 { field: 'RoleName',title: 'Role Name', width: 180, sortable: false },
                                                 { title: 'View', align:'center', type: 'checkbox', editor:true, mode:'editOnly', sortable: false },
                                                 { title: 'Add', align:'center', type: 'checkbox', editor:true, mode:'editOnly', sortable: false },
                                                 { title: 'Edit', align:'center', type: 'checkbox', editor:true, mode:'editOnly', sortable: false },
                                                 { title: 'Delete', align:'center', type: 'checkbox', editor:true, mode:'editOnly', sortable: false },
                                                 { title: 'Report', align:'center', type: 'checkbox', editor:true, mode:'editOnly', sortable: false },";

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
                            columnList = new GblFunctions().makeGridColumns(columns, EditYN, DeleteYN);

                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";
                            ViewData[ViewDataNames.GridColumns] = columnList;
                            ViewData[ViewDataNames.RoleDetailsGridColumns] = roleDetailsColumnList;

                            status = 1;
                        }
                    }
                    else
                    {
                        if (groupId == 1)
                        {
                            roleDetailsColumnList += "{ title: 'System', align:'center', type: 'checkbox', editor:true, mode:'editOnly', sortable: false }";
                        }
                        ViewData[ViewDataNames.AddYN] = "visible";
                        columnList = new GblFunctions().makeGridColumns(columns, "Y", "Y");
                        ViewData[ViewDataNames.GridColumns] = columnList;
                        ViewData[ViewDataNames.RoleDetailsGridColumns] = roleDetailsColumnList;
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminUserGroup", "AdminUserGroupController/makeData");
                status = -1;
            }

            return status;
        }

        public ActionResult AdminUserGroup(string x)
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
                MCommon.saveExceptionLog(ex.Message, "AdminUserGroup/View", "AdminUserGroupController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetUserGroupList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            int groupId = (Session[SessionNames.GroupId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.GroupId]);

            var records = new MAdminUserGroup().getUserGroupList(page, limit, sortBy, direction, searchString, ref total, groupId);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserTypeList()
        {
            var records = new MCommon().getUserTypeList();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetParentRoleList()
        {
            var records = new MAdminUserGroup().getParentRoleList();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRoleList(string p, string m, string g) //p : parent id, g : user group id, m : mode
        {
            int groupId = Convert.ToInt32(Session[SessionNames.GroupId]);
            p = (string.IsNullOrEmpty(p) == true) ? "MA==" : p; //MA== : 0
            g = (string.IsNullOrEmpty(g) == true) ? "MA==" : g; //MA== : 0
            string systemYN = (groupId == 1) ? "Y" : "N";

            var records = new MAdminUserGroup().getRoleList(p, systemYN, m, g);

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AdminUserGroup(VMAdminUserGroup aug)
        {
            try
            {
                int status = 0;
                string errDesc = string.Empty;

                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                int error = 0;

                if (ModelState.IsValid)
                {
                    MAdminUserGroup ug = new MAdminUserGroup();

                    string mode = ((aug.VMMstUserGroup.EntType == EntType.ADD) ? Mode.ADD : ((aug.VMMstUserGroup.EntType == EntType.EDIT) ? Mode.EDIT : ((aug.VMMstUserGroup.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));

                    if (mode == Mode.DELETE)
                    {
                        string tableName = "LoginDetails";
                        string fieldName = "GroupId";
                        string fieldValue = GblFunctions.Base64Decode(aug.VMMstUserGroup.GroupId);

                        error = new MCommon().chkDelete(tableName, fieldName, fieldValue);

                        if (error == 1)
                        {
                            TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                            TempData[TempDataNames.ErrDesc] = Message.UserGroup.UserGroupDeleteNotPermitted;
                            return RedirectToAction("AdminUserGroup", "AdminUserGroup", rvd);
                        }
                    }

                    error = ug.saveMstUserGroup(aug, ref errDesc);

                    if (error == 0)
                    {
                        if (mode == Mode.ADD || mode == Mode.EDIT)
                        {
                            TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
                        }
                        else
                        {
                            TempData[TempDataNames.SaveStatus] = SaveStatus.SaveDelete;
                        }
                    }
                    else if (error == 2)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = errDesc;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    }

                    return RedirectToAction("AdminUserGroup", "AdminUserGroup", rvd);
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
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminUserGroup/Save", "AdminUserGroupController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult ChkUserGroupDelete(string g)
        {
            int err = 0;

            try
            {
                string tableName = "LoginDetails";
                string fieldName = "GroupId";
                string fieldValue = GblFunctions.Base64Decode(g);

                err = new MCommon().chkDelete(tableName, fieldName, fieldValue);
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "ChkUserGroupDelete", "AdminUserGroupController");
            }
            return Json(err, JsonRequestBehavior.AllowGet);
        }
    }
}
