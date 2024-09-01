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
    public class AdminSchoolTransferController : Controller
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
                        DeleteYN = DefaultSetting.DefaultValY;
                        status = 1;
                    }
                    Session[SessionNames.DeleteYN] = DeleteYN;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "makeData", "AdminSchoolTransferController");
                status = -1;
            }

            return status;
        }

        [HttpGet]
        public ActionResult Transfers(string x)
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
                        ViewData[ViewDataNames.SaveInfo] = Message.SchoolTransfer.SaveMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.SaveDelete)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "none";
                        ViewData[ViewDataNames.SucessVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.SchoolTransfer.DeleteMsg;
                    }
                    else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SucessVisibility] = "none";
                        ViewData[ViewDataNames.SaveInfo] = Message.SchoolTransfer.ErrorMsg;
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
                MCommon.saveExceptionLog(ex.Message, "Transfers/View", "AdminSchoolTransferController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSubDivisionList(string s)
        {
            Int64 schoolId = 0;

            try
            {
                schoolId = Convert.ToInt64(GblFunctions.Base64Decode(s));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSubDivisionList", "AdminSchoolTransferController");
            }

            var records = new MMstSubDivision().getMstSubDivisionListSchoolTransferDropDown(schoolId);

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolList(string s)
        {
            var records = new MMstSchool().getMstSchoolListAutoComplete(Sanitizer.GetSafeHtmlFragment(s));

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCurrentSubDivisionName(string s)
        {
            Int64 schoolId = 0;

            try
            {
                schoolId = Convert.ToInt64(GblFunctions.Base64Decode(s));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetCurrentSubDivisionName", "AdminSchoolTransferController");
            }

            var SubDivisionName = new MMstSchool().getSubDivisionNameBySchoolId(schoolId);

            return Json(new { SubDivisionName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolTransferView(string s)
        {
            Int64 schoolId = 0;
            string deleteYN = DefaultSetting.DefaultValN;

            try
            {
                schoolId = Convert.ToInt64(GblFunctions.Base64Decode(s));
                deleteYN = Session[SessionNames.DeleteYN].ToString();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolTransferView", "AdminSchoolTransferController");
            }

            var View = new MMstSchool().getSchoolTransferView(schoolId, deleteYN);

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Transfers(VMSchoolTransfer data)
        {
            try
            {
                int status = 0, err = 0;
                string errDesc = string.Empty;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("x", Session[SessionNames.URL]);
                string mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));
                if (mode == Mode.ERROR)
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                    return RedirectToAction("Transfers", "AdminSchoolTransfer", rvd);
                }
                if (ModelState.IsValid)
                {
                    err = new MMstSchool().saveSchoolTransfer(data, ref errDesc);

                    if (err == 0)
                    {
                        TempData[TempDataNames.SaveStatus] = (mode == Mode.ADD) ? SaveStatus.SaveSuccess : SaveStatus.SaveDelete;
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
                    return RedirectToAction("Transfers", "AdminSchoolTransfer", rvd);
                }
                else
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.ActiveLinkA] = "A" + Session[SessionNames.RoleId].ToString();

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
                MCommon.saveExceptionLog(ex.Message, "Transfers/Save", "AdminSchoolTransferController");
                return Redirect("~/Error/Unexpected.html");
            }
        }
    }
}
