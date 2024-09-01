using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using WBBSE.Models;
using WBBSE.Areas.Admin.Models;
using ViewModel;
using Common;
using Microsoft.Security.Application;
using System.Web.SessionState;

namespace WBBSE.Areas.Admin.Controllers
{
    [AllowAnonymous, NoCache]
    public class AdminLoginController : Controller
    {
        public ActionResult Login(string id, string m = null, string d = null, string n = null, string t = null)
        {
            Session[SessionNames.Salt] = GblFunctions.CreateSalt(5);

            //m :: MACHINE ID, d :: AUTHORIZATION TOKEN, n :: MAC DUPLICATION CHECKING, t :: assigned datetime against a token :::: FOR MACHINE RESTRICTION
            if (!string.IsNullOrEmpty(m) && !string.IsNullOrEmpty(d) && !string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(t))
            {
                Session[SessionNames.MachineId] = m;
                Session[SessionNames.MACAuthorizationToken] = d;
                Session[SessionNames.MACDuplicateYN] = n;
                Session[SessionNames.MACTimeStamp] = t;
            }

            if (id == "logout")
            {
                ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                ModelState.AddModelError(ViewDataNames.LoginInfo, Message.LoggedOut);
            }
            else
            {
                ViewData[ViewDataNames.divLoginAlertVisibility] = "none";
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(VMAdminLogin ld)
        {
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
                        ModelState.AddModelError(ViewDataNames.LoginInfo, Message.OperationError);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return View();
                    }
                    string captchaTyped = (string.IsNullOrEmpty(ld.Captcha) == true) ? "error" : ld.Captcha;
                    if (captchaAns != captchaTyped)
                    {
                        ModelState.AddModelError(ViewDataNames.InvalidCaptcha, Message.InvalidCaptcha);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return View();
                    }

                    string salt = (Session[SessionNames.Salt] == null) ? "error" : Session[SessionNames.Salt].ToString();
                    //salt not retrieved
                    if (salt == "error")
                    {
                        //configure error page
                        ModelState.AddModelError(ViewDataNames.LoginInfo, Message.OperationError);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return View();
                    }
                    //standard password length(64) not matched
                    if (ld.LoginPwd.Length != 64)
                    {
                        ModelState.AddModelError(ViewDataNames.LoginInfo, Message.InvalidPasswordLength);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return View();
                    }
                    //special characters/strings not allowed in username field
                    if (GblFunctions.CheckScriptAndSpecialChar(ld.UserName) == false)
                    {
                        ModelState.AddModelError(ViewDataNames.InvalidChar, Message.InvalidChar);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return View();
                    }

                    //login check
                    if (MAdminLogin.isValidUser(ld, salt))
                    {
                        /***************************************
                        MAC AUTHORIZATION LOGIC
                        =======================
                         * SESSION :: MACActivatedYN WILL TELL WHETHER MAC IS ACTIVATED OR NOT
                         * IF NOT ACTIVATED, PROCEED NORMALLY
                         * IF ACTIVATED CHECK WHETHER ACCESSED MACHINE IS AUTHORIZED TO ACCESS ADMIN PORTAL OR NOT BY FOLLOWING LOGIC - 
                            * NIC ADMIN IS NOT CHECKED FOR THE TIME BEING
                            * FOR OTHERS, CHECK WHETHER AUTHORIZATION TOKEN IS PRESENT IN SESSION : AuthorizationToken. IF NOT PRESENT, UN-AUTHORIZED.
                            * IF AUTHORIZATION TOKEN IS PRESENT IN SESSION : AuthorizationToken, CHECK WHETHER TOKEN IS VALID OR NOT. ASSIGNED TOKEN IS VALID FOR : 1 MINUTE FROM THE TIME OF ASSIGNMENT.
                              IF NOT VALID/NOT FOUND, UN-AUTHORIZED.
                        ***************************************/

                        //for testing purpose. this line should be commented before release
                        //Session[SessionNames.MACActivatedYN] = "N";

                        if (Session[SessionNames.MACActivatedYN].ToString() == "N")
                        {
                            //form authentication cookie
                            FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, ld.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, UserType.ADMIN, FormsAuthentication.FormsCookiePath);
                            string st = FormsAuthentication.Encrypt(tkt);
                            var ck = new HttpCookie(FormsAuthentication.FormsCookieName, st);
                            Response.Cookies.Add(ck);
                            //identity cookie
                            Session[SessionNames.Identity] = DateTime.Now.ToFileTime().ToString();

                            if (Session[SessionNames.NewPasswordChangedYN].ToString() == "N" || Session[SessionNames.EmailVerifiedYN].ToString() == "N" || Session[SessionNames.ContactNoVerifiedYN].ToString() == "N")
                            {
                                return RedirectToAction("AdminProfileVerification", "AdminProfileVerification");
                            }
                            else
                            {
                                return RedirectToAction("AdminHome", "AdminHome");
                            }
                        }
                        else if (Session[SessionNames.MACActivatedYN].ToString() == "Y")
                        {
                            if (Session[SessionNames.GroupId].ToString() == GroupType.NICADMIN) //bypass nic admin
                            {
                                //form authentication cookie
                                FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, ld.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, UserType.ADMIN, FormsAuthentication.FormsCookiePath);
                                string st = FormsAuthentication.Encrypt(tkt);
                                var ck = new HttpCookie(FormsAuthentication.FormsCookieName, st) { Secure = true };
                                Response.Cookies.Add(ck);
                                //identity cookie
                                Session[SessionNames.Identity] = DateTime.Now.ToFileTime().ToString();

                                if (Session[SessionNames.NewPasswordChangedYN].ToString() == "N" || Session[SessionNames.EmailVerifiedYN].ToString() == "N" || Session[SessionNames.ContactNoVerifiedYN].ToString() == "N")
                                {
                                    return RedirectToAction("AdminProfileVerification", "AdminProfileVerification");
                                }
                                else
                                {
                                    return RedirectToAction("AdminHome", "AdminHome");
                                }
                            }
                            else
                            {
                                //get required details from session
                                string machineId = GblFunctions.decryptToken((Session[SessionNames.MachineId] ?? "").ToString());
                                string tokenId = GblFunctions.decryptToken((Session[SessionNames.MACAuthorizationToken] ?? "").ToString());
                                string successYN = GblFunctions.decryptToken((Session[SessionNames.MACDuplicateYN] ?? "").ToString());
                                DateTime timeStamp = DateTime.FromFileTime(Convert.ToInt64(GblFunctions.decryptToken((Session[SessionNames.MACTimeStamp] ?? GblFunctions.encryptToken(DateTime.Now.ToFileTime().ToString())).ToString())));
                                DateTime current = DateTime.Now;
                                int err = 0;

                                if (current.Subtract(timeStamp).TotalMinutes > 1) //requested token is valid for one minute only
                                {
                                    err = new MMac().deleteMACToken();
                                    string userId = Session[SessionNames.UserId].ToString();
                                    Session.Clear();
                                    Session.Abandon();
                                    FormsAuthentication.SignOut();
                                    return RedirectToAction("UnAuthorized", new RouteValueDictionary(new { Controller = "../Web", Action = "UnAuthorized", u = GblFunctions.encryptPassword(userId) }));
                                }

                                err = new MMac().validateMAC(machineId, tokenId, successYN);

                                if (err == 0)
                                {
                                    //form authentication cookie
                                    FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, ld.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, UserType.ADMIN, FormsAuthentication.FormsCookiePath);
                                    string st = FormsAuthentication.Encrypt(tkt);
                                    var ck = new HttpCookie(FormsAuthentication.FormsCookieName, st);
                                    Response.Cookies.Add(ck);
                                    //identity cookie
                                    Session[SessionNames.Identity] = DateTime.Now.ToFileTime().ToString();

                                    if (Session[SessionNames.NewPasswordChangedYN].ToString() == "N" || Session[SessionNames.EmailVerifiedYN].ToString() == "N" || Session[SessionNames.ContactNoVerifiedYN].ToString() == "N")
                                    {
                                        return RedirectToAction("AdminProfileVerification", "AdminProfileVerification");
                                    }
                                    else
                                    {
                                        return RedirectToAction("AdminHome", "AdminHome");
                                    }
                                }
                                else
                                {
                                    string userId = Session[SessionNames.UserId].ToString();
                                    Session.Clear();
                                    Session.Abandon();
                                    FormsAuthentication.SignOut();
                                    return RedirectToAction("UnAuthorized", new RouteValueDictionary(new { Controller = "../Web", Action = "UnAuthorized", u = GblFunctions.encryptPassword(userId) }));
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(ViewDataNames.LoginInfo, Message.InvalidUser);
                            ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(ViewDataNames.LoginInfo, Message.InvalidUser);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                        return View();
                    }
                }
                else
                {
                    ViewData[ViewDataNames.divLoginAlertVisibility] = "";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ViewDataNames.LoginInfo, Message.OperationError);
                ViewData[ViewDataNames.divLoginAlertVisibility] = "";

                /********Handleing catch exception Log *****************/
                MCommon.saveExceptionLog(ex.Message, "validateLogin", "AdminLoginController");
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", new RouteValueDictionary(new { Controller = "AdminLogin", Action = "Login", id = "logout" }));
        }
    }
}
