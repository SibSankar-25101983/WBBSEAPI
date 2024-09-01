using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WBBSE.Models;
using Common;

namespace WBBSE.Controllers
{
    public class AuthenticateUserController : Controller
    {
        public ActionResult Authenticate()
        {
            try
            {
                string[] aryPath = Request.Url.PathAndQuery.Split('/');
                string actualPage = aryPath[2].Split('%')[1];
                actualPage = actualPage.Replace("2f", "");
                if (actualPage.Length >= 5)
                {
                    if (actualPage.Substring(0, 5).ToUpper() == UserType.ADMIN)
                    {
                        Session[SessionNames.Salt] = GblFunctions.CreateSalt(5);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "hidden";
                        return RedirectToActionPermanent("Login", "Admin/AdminLogin");
                    }
                    else if (actualPage.Substring(0, 6).ToUpper() == UserType.SCHOOL)
                    {
                        Session[SessionNames.Salt] = GblFunctions.CreateSalt(5);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "hidden";
                        return RedirectToActionPermanent("LogOut", "Web");
                    }
                    else if (actualPage.Substring(0, 9).ToUpper() == UserType.PRESCHOOL)
                    {
                        Session[SessionNames.Salt] = GblFunctions.CreateSalt(5);
                        ViewData[ViewDataNames.divLoginAlertVisibility] = "hidden";
                        return RedirectToActionPermanent("LogOut", "Web");
                    }
                    else
                    {
                        return RedirectPermanent("~/Error/Unexpected.html");
                    }
                }
                else
                {
                    return RedirectPermanent("~/Error/Unexpected.html");
                }
            }
            catch (Exception ex)
            {
                /********Handleing catch exception Log *****************/
                MCommon.saveExceptionLog(ex.Message, "Authenticate", "AuthenticateUserController");
                return RedirectPermanent("~/Error/Unexpected.html");
            }
        }
    }
}
