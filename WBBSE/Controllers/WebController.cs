using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WBBSE.Models;
using WBBSE.Areas.Admin.Models;
using System.Web.Security;
using Common;
using ViewModel;
using WBBSE.Areas.Schools.Models;
using WBBSE.Areas.Candidate.Models;
using Microsoft.Security.Application;
using System.IO;

namespace WBBSE.Controllers
{
    public class WebController : ApplicationController
    {
        [HttpGet]
        public ActionResult Default()
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd.Add("l", "Z//2JeEw6P/kXiUjdLauQg==");
            return RedirectToAction("Home", "Web", rvd);
        }

        [HttpGet]
        public ActionResult Home(string l)
        {
            try
            {
                string section1 = string.Empty, section2 = string.Empty, section3 = string.Empty, section4 = string.Empty, section5 = string.Empty, section6 = string.Empty;

                int err = new MWeb().getWebsiteHomePageContent(ref section1, ref section2, ref section3, ref section4, ref section5, ref section6);

                //if (err == 1)
                //{
                //    return Redirect("~/Error/Unexpected.html");
                //}

                ViewData[ViewDataNames.Section1] = section1;
                ViewData[ViewDataNames.Section2] = section2;
                ViewData[ViewDataNames.Section3] = section3;
                ViewData[ViewDataNames.Section4] = section4;
                ViewData[ViewDataNames.Section5] = section5;
                ViewData[ViewDataNames.Section6] = section6;

                Session[SessionNames.HomeLink] = l;
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                //salt creation for school login
                Session[SessionNames.Salt] = GblFunctions.CreateSalt(5);
                ViewData[ViewDataNames.divLoginAlertVisibility] = "none";

                if (TempData[TempDataNames.SaveStatus] != null)
                {
                    if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.ValidationFailed)
                    {
                        ModelState.AddModelError(ViewDataNames.LoginInfo, TempData[TempDataNames.SaveInfo].ToString());
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Home", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult SchoolLogin(string l)
        {
            try
            {

                //Session[SessionNames.HomeLink] = l;
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                //salt creation for school login
                Session[SessionNames.Salt] = GblFunctions.CreateSalt(5);
                ViewData[ViewDataNames.divLoginAlertVisibility] = "none";

                if (TempData[TempDataNames.SaveStatus] != null)
                {
                    if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.ValidationFailed)
                    {
                        ModelState.AddModelError(ViewDataNames.LoginInfo, TempData[TempDataNames.SaveInfo].ToString());
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                    }
                }

                Session[SessionNames.SchoolLoginURL] = l;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SchoolLogin", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SchoolLogin(VMSchoolLogin ld)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd.Add("l", Session[SessionNames.HomeLink]);

            try
            {
                if (ModelState.IsValid)
                {
                    //check captcha
                    string captchaAns = (Session[SessionNames.Captcha] == null) ? "error" : Session[SessionNames.Captcha].ToString();
                    //captcha not retrieved
                    if (captchaAns == "error")
                    {
                        //configure error page
                        //ModelState.AddModelError(ViewDataNames.LoginInfo, Message.OperationError);
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.SaveInfo] = Message.OperationError;
                        //ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return RedirectToAction("Home", "Web", rvd);
                    }
                    string captchaTyped = (string.IsNullOrEmpty(ld.Captcha) == true) ? "error" : ld.Captcha;
                    if (captchaAns != captchaTyped)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.SaveInfo] = Message.InvalidCaptcha;
                        //ModelState.AddModelError(ViewDataNames.InvalidCaptcha, Message.InvalidCaptcha);
                        //ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return RedirectToAction("SchoolLogin", "Web", rvd);
                    }

                    string salt = (Session[SessionNames.Salt] == null) ? "error" : Session[SessionNames.Salt].ToString();
                    //salt not retrieved
                    if (salt == "error")
                    {
                        //configure error page
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.SaveInfo] = Message.OperationError;
                        //ModelState.AddModelError(ViewDataNames.LoginInfo, Message.OperationError);
                        //ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return RedirectToAction("SchoolLogin", "Web", rvd);
                    }
                    //standard password length(64) not matched
                    if (ld.LoginPwd.Length != 64)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.SaveInfo] = Message.InvalidPasswordLength;
                        //ModelState.AddModelError(ViewDataNames.LoginInfo, Message.InvalidPasswordLength);
                        //ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return RedirectToAction("SchoolLogin", "Web", rvd);
                    }
                    //special characters/strings not allowed in username field
                    if (GblFunctions.CheckScriptAndSpecialChar(ld.UserName) == false)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.SaveInfo] = Message.InvalidChar;
                        //ModelState.AddModelError(ViewDataNames.InvalidChar, Message.InvalidChar);
                        //ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return RedirectToAction("SchoolLogin", "Web", rvd);
                    }

                    //login check
                    if (MSchoolLogin.isValidUser(ld, salt))
                    {
                        string userType = string.Empty;
                        userType = Session[SessionNames.UserTypeId].ToString();
                        if (userType == UserType.UserTypeID.SCHOOL)
                        {
                            userType = UserType.SCHOOL;
                        }
                        else if (userType == UserType.UserTypeID.PRESCHOOL)
                        {
                            userType = UserType.PRESCHOOL;
                        }
                        else
                        {
                            TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                            TempData[TempDataNames.SaveInfo] = Message.InvalidUser;

                            return RedirectToAction("SchoolLogin", "Web", rvd);
                        }

                        //form authentication cookie
                        FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, ld.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userType, FormsAuthentication.FormsCookiePath);
                        string st = FormsAuthentication.Encrypt(tkt);
                        var ck = new HttpCookie(FormsAuthentication.FormsCookieName, st);
                        Response.Cookies.Add(ck);
                        //identity cookie
                        Session[SessionNames.Identity] = DateTime.Now.ToFileTime().ToString();

                        if (Session[SessionNames.NewPasswordChangedYN].ToString() == "N") // || Session[SessionNames.EmailVerifiedYN].ToString() == "N" || Session[SessionNames.ContactNoVerifiedYN].ToString() == "N"
                        {
                            if (Session[SessionNames.UserTypeId].ToString() == UserType.UserTypeID.SCHOOL) //For School login
                            {
                                return RedirectToAction("SchoolProfileVerification", "SchoolProfileVerification", new { area = "Schools" });
                            }
                            else if (Session[SessionNames.UserTypeId].ToString() == UserType.UserTypeID.PRESCHOOL)
                            {
                                return RedirectToAction("PreSchoolProfileVerification", "PreSchoolProfileVerification", new { area = "PreSchools" }); //configure pre-school VERIFICATION
                            }
                            else
                            {
                                TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                                TempData[TempDataNames.SaveInfo] = Message.InvalidUser;

                                return RedirectToAction("SchoolLogin", "Web", rvd);
                            }
                        }
                        else
                        {
                            if (Session[SessionNames.UserTypeId].ToString() == UserType.UserTypeID.SCHOOL)
                            {
                                return RedirectToAction("SchoolHome", "SchoolHome", new { area = "Schools" });
                            }
                            else if (Session[SessionNames.UserTypeId].ToString() == UserType.UserTypeID.PRESCHOOL)
                            {
                                return RedirectToAction("PreSchoolHome", "PreSchoolHome", new { area = "PreSchools" }); //configure pre-school login
                            }
                            else
                            {
                                TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                                TempData[TempDataNames.SaveInfo] = Message.InvalidUser;

                                return RedirectToAction("SchoolLogin", "Web", rvd);
                            }
                        }
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.SaveInfo] = Message.InvalidUser;

                        return RedirectToAction("SchoolLogin", "Web", rvd);
                    }
                }
                else
                {
                    TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                    TempData[TempDataNames.SaveInfo] = Message.InvalidUser;

                    return RedirectToAction("SchoolLogin", "Web", rvd);
                }
            }
            catch (Exception ex)
            {
                TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                TempData[TempDataNames.SaveInfo] = Message.OperationError;

                /********Handleing catch exception Log *****************/
                MCommon.saveExceptionLog(ex.Message, "SchoolLogin", "WebController");
                return RedirectToAction("SchoolLogin", "Web", rvd);
            }
        }

        public ActionResult LogOut()
        {
            string homeLink = string.Empty;

            try
            {
                homeLink = Session[SessionNames.SchoolLoginURL].ToString();
            }
            catch{ }

            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            FormsAuthentication.SignOut();

            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd.Add("l", homeLink);
            return RedirectToAction("SchoolLogin", "Web", rvd);
        }

        [HttpGet]
        public ActionResult AboutWBBSE(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View("~/Views/Web/AboutWBBSE.cshtml");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AboutWBBSE", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult VisionAndMission(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "VisionAndMission", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult WBBSESections(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View("~/Views/Web/WBBSESections.cshtml");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSESections", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult WBBSEAuthorities(string l)
        {
            try
            {
                //string activeLink = GblFunctions.decryptPassword(l);

                //ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View("~/Views/Web/WBBSEAuthorities.cshtml");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEAuthorities", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult PhotoGallery(string l)
        {
            try
            {
                int err = 0;
                string pageLink = string.Empty;

                string data = new MImage().getPhotogalleryList(MenuCode.PhotoGallery, ref err, ref pageLink);

                if (err == 1)
                {
                    return Redirect("~/Error/Unexpected.html");
                }

                ViewData[ViewDataNames.PageLink] = pageLink;
                ViewData[ViewDataNames.RawData] = data;

                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View("~/Views/Web/PhotoGallery.cshtml");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PhotoGallery", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //for static pdf loading (m = menu code)
        [HttpGet]
        public ActionResult PdfViewerStatic(string m)
        {
            try
            {
                string menuCode = GblFunctions.decryptPassword(m);

                if(menuCode == MenuCode.OrganizationStructure)
                {
                    ViewData[ViewDataNames.RawData] = FilePath.OrganizationStructure;
                }
                else
                {
                    ViewData[ViewDataNames.RawData] = "";
                }
                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PdfViewerStatic", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //for dynamic pdf loading
        [HttpGet]
        public ActionResult PdfViewer(string l)
        {
            try
            {
                Int64 contentId = Convert.ToInt64(GblFunctions.Base64Decode(l));
                string headerData = string.Empty;

                string data = new MCommon().getContentLink(contentId, Convert.ToInt32(LinkTypes.PDF), ref headerData);

                if(data == string.Empty)
                {
                    return Redirect("~/Error/DocumentNotFound.html");
                }

                ViewData[ViewDataNames.RawData] = data;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PdfViewer", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        [Authorize(Roles = UserType.ADMIN + "," + UserType.SCHOOL + "," + UserType.PRESCHOOL)]
        public ActionResult NoticeViewer(string l)
        {
            try
            {
                Int64 inboxId = Convert.ToInt64(GblFunctions.Base64Decode(l));
                string headerData = string.Empty;

                string data = new MCommon().getInboxLink(inboxId, Convert.ToInt32(LinkTypes.PDF), ref headerData);

                ViewData[ViewDataNames.RawData] = data;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "NoticeViewer", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult ContentViewer(string l, string d)
        {
            try
            {
                string activeLink = GblFunctions.Base64Decode(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;
                Int64 contentId = Convert.ToInt64(GblFunctions.Base64Decode(d));
                string headerData = string.Empty;

                string data = new MCommon().getContentLink(contentId, Convert.ToInt32(LinkTypes.CONTENT), ref headerData);

                ViewData[ViewDataNames.RawData] = data;
                ViewData[ViewDataNames.RawDataHeader] = headerData;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PdfViewer", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult Circular(string l)
        {
            try
            {
                string contentType = GblFunctions.Base64Decode(l);
                if (contentType == ContentType.MAIN)
                {
                    Session[SessionNames.ContentType] = ContentType.MAIN;
                }
                else
                {
                    Session[SessionNames.ContentType] = ContentType.ARCHIVE;
                }
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

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;
                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Circular", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetCircularList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            string archiveYN = "N";

            try
            {
                archiveYN = (Session[SessionNames.ContentType].ToString() == ContentType.ARCHIVE) ? "Y" : "N";
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetCircularList", "WebController");
            }

            var records = new MCircular().getCircularList(page, limit, sortBy, direction, MenuCode.Circulars, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Notification(string l, string t, string x)
        {
            /*
             NOTE
             ====
             * l = MAIN/ARCHIVE (WHETHER TO LOAD ARCHIVE DATA OR MAIN DATA)
             * t = TYPE OF NOTIFICATION (TD: TENDER, N2 : NOTIFICATION, AN : APPOINTMENT)
             * x = MAIN MENU LINK FOR HIGHLIGHTING
            */

            try
            {
                string contentType = GblFunctions.Base64Decode(l);
                if (contentType == ContentType.MAIN)
                {
                    Session[SessionNames.ContentType] = ContentType.MAIN;
                }
                else
                {
                    Session[SessionNames.ContentType] = ContentType.ARCHIVE;
                }
                Session[SessionNames.NotificationType] = null;
                if (t != null)
                {
                    Session[SessionNames.NotificationType] = t;
                }

                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Notification", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        //{"UpdationDateTime", "Last Updated", "false", "80"},
                        {"NotificationType", "Type", "false", "100"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(x);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Notification", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetNotificationList(int? page, int? limit, string sortBy, string direction, string searchString = null, string sd = null)
        {
            int total = 0;
            string archiveYN = "N", menuCode = "-1";

            try
            {
                if (!string.IsNullOrEmpty(sd))
                {
                    //notificationTypeId = Convert.ToInt32(GblFunctions.Base64Decode(sd));
                    menuCode = GblFunctions.Base64Decode(sd);
                }
                else
                {
                    menuCode = GblFunctions.Base64Decode((Session[SessionNames.NotificationType] == null) ? DefaultSetting.DefaultValEnc : Session[SessionNames.NotificationType].ToString());
                }
                archiveYN = (Session[SessionNames.ContentType].ToString() == ContentType.ARCHIVE) ? "Y" : "N";
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetNotificationList", "WebController");
            }

            //var records = new MNotification().getNotificationList(page, limit, sortBy, direction, MenuCode.Notification, ref total, searchString, archiveYN);
            var records = new MNotification().getWebNotificationList(page, limit, sortBy, direction, menuCode, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult WBBSEContact(string l)
        {
            try
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
                    ViewData[ViewDataNames.SaveInfo] = Message.SendMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.FailedMsg;
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
                Session[SessionNames.URL] = l;
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View("~/Views/Web/ContactUs.cshtml");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ContactUs", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult WBBSESendContact(VMContactUs data)
        {
            MWeb oWeb = new MWeb();
            string errDesc = string.Empty;
            try
            {
                TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("l", Session[SessionNames.URL]);

                //int err = oWeb.sendMailForContactUs(data, ref errDesc);
                //if (err == 0)
                //{
                //    TempData[TempDataNames.SaveStatus] = SaveStatus.SaveSuccess;
                //}
                //else if (err == 2)
                //{
                //    TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;                    
                //    TempData[TempDataNames.ErrDesc] = errDesc;
                //}
                //else
                //{
                //    TempData[TempDataNames.SaveStatus] = SaveStatus.Failed;
                //}

                return RedirectToAction("WBBSEContact", "Web", rvd);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ContactUs", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult WBBSEHowToApply()
        {
            try
            {

                return View("~/Views/Web/HowToApply.cshtml");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "HowToApply", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult PastPresident()
        {
            try
            {
                return View("~/Views/Web/PastPresident.cshtml");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PastPresident", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult SyllabusCurriculum(string l)
        {
            try
            {
                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Content", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "80"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SyllabusCurriculum", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSyllabusCurriculumList(int? page, int? limit, string sortBy, string direction, string searchString = null, string archiveYN = "N")
        {
            int total = 0;

            var records = new MWeb().getSyllabusCurriculumList(page, limit, sortBy, direction, MenuCode.SyllabusCurriculum, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModelQuestions(string l)
        {
            try
            {
                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Content", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "80"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ModelQuestions", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetModelQuestionsList(int? page, int? limit, string sortBy, string direction, string searchString = null, string archiveYN = "N")
        {
            int total = 0;

            var records = new MWeb().getModelQuestionsList(page, limit, sortBy, direction, MenuCode.ModelQuestions, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadForms(string l)
        {
            try
            {
                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Content", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "80"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "DownloadForms", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetDownloadFormsList(int? page, int? limit, string sortBy, string direction, string searchString = null, string archiveYN = "N")
        {
            int total = 0;

            var records = new MWeb().getDownloadFormsList(page, limit, sortBy, direction, MenuCode.DownloadForms, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RTI(string l)
        {
            try
            {
                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Content", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "80"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "RTI", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetRTIList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            var records = new MWeb().getRTIList(page, limit, sortBy, direction, MenuCode.RTI, ref total, searchString, "N");

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult PresidentDesk()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        MCommon.saveExceptionLog(ex.Message, "PresidentDesk", "WebController");
        //        return Redirect("~/Error/Unexpected.html");
        //    }
        //}

        //[HttpGet]
        //public ActionResult SecretaryDesk()
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        MCommon.saveExceptionLog(ex.Message, "SecretaryDesk", "WebController");
        //        return Redirect("~/Error/Unexpected.html");
        //    }
        //}

        [HttpGet]
        public ActionResult WBBSEObjectives(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEObjectives", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult WBBSEPrimeFocus(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEPrimeFocus", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult Disclaimer()
        {
            try
            {
                string data = new MWeb().getDisclaimer();

                ViewData[ViewDataNames.RawData] = data;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Disclaimer", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult Tender(string l)
        {
            try
            {
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

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Tender", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetTenderList(int? page, int? limit, string sortBy, string direction, string searchString = null, string contentType = null)
        {
            int total = 0;
            string archiveYN = "N";

            try
            {
                archiveYN = (string.IsNullOrEmpty(contentType)) ? "N" : ((contentType == "A") ? "Y" : "N");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetTenderList", "WebController");
            }

            var records = new MWeb().getTenderList(page, limit, sortBy, direction, MenuCode.Tender, ref total, searchString, archiveYN);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        //school directory
        [HttpGet]
        public ActionResult SchoolDirectory(string l)
        {
            try
            {
                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"IndexNo", "Index No", "false", "90"},
                        {"DISECode", "DISE Code", "false", "90"},
                        {"SchoolName", "School Name", "false", "300"},
                        {"SubDivisionName", "SubDivision Name", "true", "0"},
                        {"DistrictName", "District Name", "true", "0"},
                        {"ZoneName", "Zone Name", "true", "0"},
                        {"Designation", "Designation", "true", "0"},
                        {"ContactNo", "Contact No", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SchoolDirectory", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSchoolDirectoryList(int? page, int? limit, string sortBy, string direction, string searchString = null, string searchType = null, string sd = null)
        {
            int total = 0, subdivisionId = 0;

            try
            {
                if (!string.IsNullOrEmpty(sd))
                {
                    subdivisionId = Convert.ToInt32(GblFunctions.Base64Decode(sd));
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolDirectoryList", "WebController");
            }

            var records = new MWeb().getSchoolDirectory(page, limit, sortBy, direction, Sanitizer.GetSafeHtmlFragment(searchString), ref total, Sanitizer.GetSafeHtmlFragment(searchType), subdivisionId);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSchoolView(string s)
        {
            string View = string.Empty;

            try
            {
                View = new MWeb().getMstSchoolView(GblFunctions.Base64Decode(s));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolView", "WebController");
            }

            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EBooks(string l)
        {
            try
            {
                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "SlNo", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Book Name", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"LinkTypeIcon", "LinkType", "false", "80"},
                        {"UpdationDateTime", "Last Updated", "false", "80"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "EBooks", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetEBooksList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            var records = new MWeb().getEBooksList(page, limit, sortBy, direction, MenuCode.EBooks, ref total, searchString, "N");

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChkRequestPermision()
        {
            string permission = DefaultSetting.DefaultValN;

            try
            {
                string machineId = Session[SessionNames.MachineId].ToString();

                permission = new MMac().chkMACAuthorizationRequestPermision(machineId);
            }
            catch
            {
                permission = DefaultSetting.DefaultValN;
            }

            return Json(new { permission }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UnAuthorized(string u)
        {
            try
            {
                string permittedYN = new MMac().chkMACSoftwareDownloadPermission(Convert.ToInt32(GblFunctions.decryptPassword(u)));
                ViewData[ViewDataNames.RawData] = u;
                ViewData[ViewDataNames.ErrorVisibility] = "none";

                if (permittedYN == "N")
                {
                    return Redirect("~/Error/Unexpected.html"); //configure for tresspassing
                }

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "UnAuthorized", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public FileResult PublicKey()
        {
            byte[] fileBytes = null;
            string fileName = string.Empty;

            try
            {
                fileName = "NIC Kolkata - WBBSE Public Key.asc";
                fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FilePath.PublicKey));
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PublicKey", "WebController");
            }
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UnAuthorized()
        {
            byte[] fileBytes = null;
            string fileName = string.Empty;

            try
            {
                string downloadType = Request.Form["DownloadType"];
                string errDesc = string.Empty;
                int err = 0;
                int userId = Convert.ToInt32(GblFunctions.decryptPassword(Request.Form["u"]));

                if (downloadType == FileDownloadOption.MACToken)
                {
                    err = new MMac().SaveMACSoftwareDownloadDetails(userId, FileDownloadOption.MACToken, ref errDesc);

                    if (err > 0)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                        return View();
                    }

                    fileName = "WBBSEToken.zip";
                    fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FilePath.MACToken));

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
                }
                if (downloadType == FileDownloadOption.MACTokenManual)
                {
                    fileName = "WBBSETokenSoftwareManual.docx";
                    fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FilePath.MACTokenManual));

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else if (downloadType == FileDownloadOption.MACAuthorization)
                {
                    err = new MMac().SaveMACSoftwareDownloadDetails(userId, FileDownloadOption.MACAuthorization, ref errDesc);

                    if (err > 0)
                    {
                        ViewData[ViewDataNames.ErrorVisibility] = "";
                        ViewData[ViewDataNames.SaveInfo] = Message.OperationError;
                        return View();
                    }

                    fileName = "WBBSEAuthorization.zip";
                    fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FilePath.MACAuthorization));

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
                }
                else if (downloadType == FileDownloadOption.MACAuthorizationManual)
                {
                    fileName = "WBBSEAuthorizationSoftwareManual.docx";
                    fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FilePath.MACAuthorizationManual));

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "UnAuthorized/Download", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public ActionResult Token(string m, string c)
        {
            try
            {
                VMMac data = new VMMac();
                string machineId = GblFunctions.decryptToken(m);
                string computerName = GblFunctions.decryptToken(c);

                data.MachineId = machineId;
                data.ComputerName = computerName;

                Session[SessionNames.MachineName] = computerName;
                Session[SessionNames.MachineId] = machineId;

                ViewData[ViewDataNames.ErrorVisibility] = "none";
                ViewData[ViewDataNames.SucessVisibility] = "none";

                return View(data);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Token/View", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Token()
        {
            try
            {
                VMMac data = new VMMac();
                string errDesc = string.Empty;

                if (Session[SessionNames.MachineName] == null || Session[SessionNames.MachineId] == null)
                {
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.ErrorVisibility] = "";

                    ViewData[ViewDataNames.SaveInfo] = Message.MAC.SessionExpiredCurrentRequest;
                    data.ComputerName = "NA";
                    data.MachineId = "NA";
                    return View(data);
                }

                data.ComputerName = Session[SessionNames.MachineName].ToString();
                data.MachineId = Session[SessionNames.MachineId].ToString();
                data.EntType = EntType.ADD;
                data.AuthorizedYN = "N";
                data.TblId = "0";

                int err = new MMac().saveMachineDetails(data, 1, ref errDesc);

                if (err == 0)
                {
                    Session[SessionNames.MachineName] = null;
                    Session[SessionNames.MachineId] = null;
                    ViewData[ViewDataNames.ErrorVisibility] = "none";
                    ViewData[ViewDataNames.SucessVisibility] = "";
                    ViewData[ViewDataNames.SaveInfo] = Message.MAC.AuthorizationRequestSuccessful;
                }
                else if (err == 2)
                {
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.ErrorVisibility] = "";

                    ViewData[ViewDataNames.SaveInfo] = errDesc;
                }
                else
                {
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.ErrorVisibility] = "";

                    ViewData[ViewDataNames.SaveInfo] = Message.MAC.AuthorizationRequestFailure;
                }
                return View(data);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Token/Save", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //For President's/Secretary's Desk 
        [HttpGet]
        public ActionResult GetBoardDeskDetails(string l)
        {
            string menuCode = string.Empty, bodyText = string.Empty, headerText = string.Empty, img = string.Empty;
            try
            {
                if (GblFunctions.Base64Decode(l) == "1")
                {
                    menuCode = MenuCode.PresidentDesk;
                }
                else if (GblFunctions.Base64Decode(l) == "2")
                {
                    menuCode = MenuCode.SecretaryDesk;
                }
                bodyText = new MWeb().getBoardDeskDetails(menuCode, ref headerText, ref img);

                ViewData[ViewDataNames.RawDataHeader] = headerText;
                ViewData[ViewDataNames.Section1] = img;
                ViewData[ViewDataNames.RawData] = bodyText;


                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Disclaimer", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //Section/Department : Examination
        [HttpGet]
        public ActionResult WBBSEExamSection(string l, string a = null)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;
                if (a == null)
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = "";
                }
                else
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = GblFunctions.Base64Decode(a);
                }

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEExamSection", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //Section/Department : General
        [HttpGet]
        public ActionResult WBBSEGeneralSection(string l, string a = null)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                if (a == null)
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = "";
                }
                else
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = GblFunctions.Base64Decode(a);
                }

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEGeneralSection", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //Section/Department : Academic
        [HttpGet]
        public ActionResult WBBSEAcademicSection(string l)
        {
            try
            {              
                
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                ////For pdf file view in Grid

                string columnList = string.Empty;
                string[,] columns = new string[,] {
                        {"SlNo", "Sl No", "false", "80"},
                        {"BodyText", "Description", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnList = new GblFunctions().makeGridColumnsWebsite(columns, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnList;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEAcademicSection", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetSyllabusList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;

            var records = new MWeb().getSyllabusList(page, limit, sortBy, direction, MenuCode.Syllabus, ref total, searchString);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        //Section/Department : Duplicate Record
        [HttpGet]
        public ActionResult WBBSEDuplicateRecordSection(string l, string a = null)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                if (a == null)
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = "";
                }
                else
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = GblFunctions.Base64Decode(a);
                }

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEDuplicateRecordSection", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //section/department
        [HttpGet]
        public ActionResult Section(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Section", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //About WBBSE : About Us
        [HttpGet]
        public ActionResult AboutUs(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AboutUs", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //services
        [HttpGet]
        public ActionResult Services(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Services", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //About WBBSE : Regional Office
        [HttpGet]
        public ActionResult WBBSERegionalOffice(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View("");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSERegionalOffice", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //About WBBSE : Authorites
        [HttpGet]
        public ActionResult WBBSEBoardDesk(string l, string a = null)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                if (a == null)
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = "";
                }
                else
                {
                    ViewData[ViewDataNames.ActiveLinkTab] = GblFunctions.Base64Decode(a);
                }

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEBoardDesk", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //About Us : Past Authorities
        [HttpGet]
        public ActionResult WBBSEBoardAuthoritiesDesk(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View("");
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEBoardAuthoritiesDesk", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //Section/Department : Book Sale Counter
        [HttpGet]
        public ActionResult WBBSEBookSaleCounter(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                string columnListRequisition = string.Empty;
                string[,] columnsRequisition = new string[,] {
                        {"SlNo", "Sl No", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Description", "false", "600"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columnListRequisition = new GblFunctions().makeGridColumnsWebsite(columnsRequisition, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columnListRequisition;

                string columnListBook = string.Empty;
                string[,] columnsBook = new string[,] {
                        {"SlNo", "Sl No", "false", "80"},
                        //{"BookId", "BookId", "true", "0"},
                        {"BookName", "Subject", "false", "600"},
                        {"BookCode", "Book Code", "false", "120"},
                        {"Class", "Class", "false", "80"},
                        //{"SchoolMediumId", "SchoolMediumId", "true", "0"},
                        {"SchoolMediumName", "Medium", "false", "150"},
                        {"BookPrice", "Price", "false", "80"}
                    };

                columnListBook = new GblFunctions().makeGridColumnsWebsite(columnsBook, "N", "N", "Y", "N");
                ViewData[ViewDataNames.GridColumnsAlternative] = columnListBook;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSEBookSaleCounter", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        /*CANDIDATE PPR/PPS RELATED WORKS*/
        [HttpGet]
        public ActionResult CandidatePPLogin()
        {
            try
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
                    ViewData[ViewDataNames.SaveInfo] = Message.SendMsg;
                }
                else if (TempData[TempDataNames.SaveStatus].ToString() == SaveStatus.Failed)
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    ViewData[ViewDataNames.SucessVisibility] = "none";
                    ViewData[ViewDataNames.SaveInfo] = Message.FailedMsg;
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
                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "CandidatePPLogin", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CandidatePPLogin(VMCandidateLogin ld)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errDesc = string.Empty;
                    /****check captcha****/
                    string captchaAns = (Session[SessionNames.Captcha] == null) ? "error" : Session[SessionNames.Captcha].ToString();
                    //captcha not retrieved
                    if (captchaAns == "error")
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = Message.OperationError;
                        return RedirectToAction("CandidatePPLogin", "Web");
                    }
                    string captchaTyped = (string.IsNullOrEmpty(ld.Captcha) == true) ? "error" : ld.Captcha;
                    if (captchaAns != captchaTyped)
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = Message.InvalidCaptcha;
                        return RedirectToAction("CandidatePPLogin", "Web");
                    }
                    /*********************/
                    if (MCandidateLogin.isValidUser(ld, ref errDesc))
                    {
                        //form authentication cookie
                        FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, ld.RollNo, DateTime.Now, DateTime.Now.AddMinutes(15), false, UserType.CANDIDATE, FormsAuthentication.FormsCookiePath);
                        string st = FormsAuthentication.Encrypt(tkt);
                        var ck = new HttpCookie(FormsAuthentication.FormsCookieName, st);
                        Response.Cookies.Add(ck);
                        //identity cookie
                        Session[SessionNames.Identity] = DateTime.Now.ToFileTime().ToString();

                        return RedirectToAction("Home", "CandidateHome", new { area = "Candidate" });
                    }
                    else
                    {
                        TempData[TempDataNames.SaveStatus] = SaveStatus.ValidationFailed;
                        TempData[TempDataNames.ErrDesc] = errDesc;

                        return RedirectToAction("CandidatePPLogin", "Web");
                    }
                }
                else
                {
                    ViewData[ViewDataNames.ErrorVisibility] = "";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ViewDataNames.SaveInfo, Message.OperationError);
                ViewData[ViewDataNames.ErrorVisibility] = "";
                MCommon.saveExceptionLog(ex.Message, "CandidatePPLogin", "WebController");
                return View();
            }
        }

        public ActionResult CandidatePPLogOut()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("CandidatePPLogin", "Web");
        }
        /*********************************/

        [HttpGet]
        public ActionResult SearchPage(string l, string a = null)
        {
            string menuCode = GblFunctions.decryptPassword(l);
            try
            {
                var redirectUrl = new MWeb().getWebsiteMenuLink(menuCode);

                if(a != null)
                {
                    redirectUrl += "&a=" + a;
                }

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "SearchPage", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //for dynamic pdf loading
        [HttpGet]
        public JsonResult PdfViewerDynamic(string m)
        {
            string View = string.Empty;
            try
            {
                string menuCode = GblFunctions.decryptPassword(m);

                string data = new MWeb().getPdfView(menuCode);

                if (string.IsNullOrEmpty(data))
                {
                    View = "";
                }
                else
                {
                    View = data;
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "PdfViewerDynamic", "WebController");
            }

            Response.ContentType = "application/pdf";
            return Json(new { View }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpGet]
        public JsonResult GetRequisitionSlipList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;

            var records = new MWeb().getRequisitionSlipList(page, limit, sortBy, direction, MenuCode.RequisitionSlip, ref total, searchString);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBookList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;

            var records = new MWeb().getMstBookList(page, limit, sortBy, direction, ref total, searchString);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        //record correction
        [HttpGet]
        public ActionResult WBBSERecordCorrection(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSERecordCorrection", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //record verification
        [HttpGet]
        public ActionResult WBBSERecordVerification(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSERecordVerification", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //recognition
        [HttpGet]
        public ActionResult WBBSERecognition(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "WBBSERecognition", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //appointment cell
        public ActionResult AppointmentCell(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AppointmentCell", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //section : establishment
        public ActionResult Establishment(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Establishment", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //section : administration
        public ActionResult Administration(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Administration", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        //section : law cell
        public ActionResult LawCell(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);

                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "LawCell", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        /****Section/Department : results (abstract)****/
        [HttpGet]
        public ActionResult ResultsAbstract(string l)
        {
            try
            {
                string activeLink = GblFunctions.decryptPassword(l);
                ViewData[ViewDataNames.ActiveLinkLI] = activeLink;

                string columns = string.Empty;
                string[,] columnsResultAbstract = new string[,] {
                        {"SlNo", "Sl No", "false", "80"},
                        {"ContentId", "ContentId", "true", "0"},
                        {"BodyText", "Year", "false", "400"},
                        {"LinkType", "Link Type", "true", "0"},
                        {"URL", "URL", "true", "0"},
                        {"PdfFilePath", "PdfFilePath", "true", "0"}
                    };

                columns = new GblFunctions().makeGridColumnsWebsite(columnsResultAbstract, "N", "N", "Y", "Y");
                ViewData[ViewDataNames.GridColumns] = columns;

                return View();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ResultsAbstract", "WebController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

        [HttpGet]
        public JsonResult GetResultsAbstract(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;

            var records = new MWeb().getResultsAbstract(page, limit, sortBy, direction, MenuCode.ResultsAbstract, ref total, searchString);

            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
        /***********************************************/
    }
}
