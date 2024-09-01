using System;
using System.Web;
using System.Web.Mvc;
using WBBSE.Models;

namespace WBBSE.Controllers
{
    public abstract class ApplicationController : Controller
    {
        public ApplicationController()
        {
            try
            {
                int err = new MWeb().getWebsiteHeader();

                if (err == 1)
                {
                    Response.Redirect("~/Error/Unexpected.html");
                }

                //if (Session == null)
                //{
                //    Response.Redirect("~/Error/Unexpected.html");
                //}
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ApplicationController", "ApplicationController");
            }
        }
    }
}
