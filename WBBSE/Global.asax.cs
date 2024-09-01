using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WBBSE.Models;
using Common;

namespace WBBSE
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        static string hash = string.Empty;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            //var cuurentUser = HttpContext.Current.User;
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);

                        //restrict direct url access of sensitive js folder
                        try
                        {
                            string x = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                            Uri y = HttpContext.Current.Request.UrlReferrer;

                            if ((x.Contains("/admin/js") || x.Contains("/schools/js") || x.Contains("/preschools/js")) && y == null)
                            {
                                Response.Redirect("~/Error/Unexpected.html");
                            }
                        }
                        catch
                        {
                            //nothing to do
                        }
                    }
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Set("Server", "NOTFOUND");
            Response.Headers.Set("X-AspNet-Version", "NOTFOUND");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            //Response.Headers.Add("Content-Security-Policy", "default-src 'self';script-src 'self' 'nonce-" + hash + "';style-src 'self' 'unsafe-inline'; img-src 'self' data: https://source.unsplash.com; font-src 'self' 'unsafe-inline' https://fonts.gstatic.com https://fonts.googleapis.com; frame-src 'self' https://www.google.com https://maps.google.com/");
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            
        }

        void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception exe = Server.GetLastError();
                Response.TrySkipIisCustomErrors = true; 
                if (exe.GetType().FullName == "System.Web.HttpException")
                {
                    if (exe.Message.Contains("The controller for path '/Schools/Web/Default' was not found"))
                    {
                        // You've handled the error, so clear it. Leaving the server in an error state can cause unintended side effects as the server continues its attempts to handle the error.
                        Server.ClearError();
                        // Possible that a partially rendered page has already been written to response buffer before encountering error, so clear it.
                        Response.Clear();
                        Response.RedirectPermanent("/Web/LogOut");
                    }
                    else if (exe.Message.Contains(".pdf"))
                    {
                        // You've handled the error, so clear it. Leaving the server in an error state can cause unintended side effects as the server continues its attempts to handle the error.
                        Server.ClearError();
                        // Possible that a partially rendered page has already been written to response buffer before encountering error, so clear it.
                        Response.Clear();
                        Response.RedirectPermanent("~/Error/DocumentNotFound.html");
                    }
                    else
                    {
                        // You've handled the error, so clear it. Leaving the server in an error state can cause unintended side effects as the server continues its attempts to handle the error.
                        Server.ClearError();
                        // Possible that a partially rendered page has already been written to response buffer before encountering error, so clear it.
                        Response.Clear();
                        Response.RedirectPermanent("~/Error/404.html");
                    }
                }
                else
                {
                    // You've handled the error, so clear it. Leaving the server in an error state can cause unintended side effects as the server continues its attempts to handle the error.
                    Server.ClearError();
                    // Possible that a partially rendered page has already been written to response buffer before encountering error, so clear it.
                    Response.Clear();
                    Response.RedirectPermanent("~/Error/Unexpected.html");
                }

            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Application_Error", "Global.asax");
                // You've handled the error, so clear it. Leaving the server in an error state can cause unintended side effects as the server continues its attempts to handle the error.
                Server.ClearError();
                // Possible that a partially rendered page has already been written to response buffer before encountering error, so clear it.
                Response.Clear();
            }
            //string str1 = string.Empty, fileName = string.Empty;
            //str1 = "~/Error/Error.html";
            //fileName = HttpContext.Current.Request.Url.ToString();
            //Context.RewritePath(str1.ToString());
            //if (exe.GetType() == typeof(HttpException))
            //{
            //    if (exe.Message.Contains("NoCatch") || exe.Message.Contains("maxUrlLength") || exe.Message.Contains("File does not exist"))
            //    {
            //        return;
            //    }
            //}
        }
    }
}