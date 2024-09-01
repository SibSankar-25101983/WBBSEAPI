using System;
using System.Web.Mvc;
using WBBSE.Areas.Admin.Models;
using System.Web.Routing;
using System.Collections.Generic;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using System.Web;

namespace WBBSE.Areas.Admin.Controllers
{
    [Authorize(Roles = UserType.ADMIN), NoCache]
    public class AdminMacAuthorizationController : Controller
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
                            ViewData[ViewDataNames.AddYN] = (AddYN == "Y") ? "visible" : "hidden";

                            status = 1;
                        }
                    }
                    else
                    {
                        ViewData[ViewDataNames.AddYN] = "visible";
                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminMacAuthorizationController");
                status = -1;
            }

            return status;
        }

        public ActionResult MacRequestList(string x)
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
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveApproveData)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.MAC.AuthorizationAssignSuccessful;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.FailedApproveData)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.MAC.AuthorizationAssignFailure;
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                    }

                    //fetch mac authorization data
                    var data = new MMac().loadMACList(null, null, SearchType.MAC.ComputerName, string.Empty);

                    if (data.Count == 0)
                    {
                        ViewData[ViewDataNames.ErrorAlertVisibility] = "";
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorAlertVisibility] = "none";
                    }

                    Session[SessionNames.URL] = x;
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

                    return View(data);
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
                MCommon.saveExceptionLog(ex.Message, "MacRequestList/View", "AdminMacAuthorizationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MacRequestListSearch(string x)
        {
            try
            {
                int status = makeData(x);

                if (status == 1)
                {
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                    DateTime? fromDate = DateTime.Now;
                    DateTime? toDate = DateTime.Now;
                    string searchString = string.Empty, searchType = string.Empty;

                    try
                    {
                        fromDate = (string.IsNullOrEmpty(Request.Form[MACSearchParameters.FromDate])) ? null : GblFunctions.setDate(Sanitizer.GetSafeHtmlFragment(Request.Form[MACSearchParameters.FromDate]));
                        toDate = (string.IsNullOrEmpty(Request.Form[MACSearchParameters.ToDate])) ? null : GblFunctions.setDate(Sanitizer.GetSafeHtmlFragment(Request.Form[MACSearchParameters.ToDate]));
                        searchString = Sanitizer.GetSafeHtmlFragment(Request.Form[MACSearchParameters.SearchString]);
                        searchType = Sanitizer.GetSafeHtmlFragment(Request.Form[MACSearchParameters.SearchType]);
                    }
                    catch
                    {
                        //nothing to do. data may not load.
                    }

                    //fetch mac authorization data
                    var data = new MMac().loadMACList(fromDate, toDate, searchType, searchString);

                    if (data.Count == 0)
                    {
                        ViewData[ViewDataNames.ErrorAlertVisibility] = "";
                    }
                    else
                    {
                        ViewData[ViewDataNames.ErrorAlertVisibility] = "none";
                    }

                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "none";

                    return View("~/Areas/Admin/Views/AdminMacAuthorization/MacRequestList.cshtml", data);
                }
                else
                {
                    return Redirect("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "MacRequestListSearch/View", "AdminMacAuthorizationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MacRequestList(List<VMMac> data)
        {
            try
            {
                int status = 0, err = 0;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();
                string errDesc = string.Empty;

                if (ModelState.IsValid)
                {
                    err = new MMac().saveMacAuthorizationData(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.SaveApproveData;
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.FailedApproveData;
                    }
                    return RedirectToAction("MacRequestList", "AdminMacAuthorization", rvd);
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
                MCommon.saveExceptionLog(ex.Message, "MacRequestList/Save", "AdminMacAuthorizationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
