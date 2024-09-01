using System;
using System.Web;
using System.Net;
using System.Data;
using System.Web.UI;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Routing;
using WBBSE.Areas.Schools.Models;
using Common;
using WBBSE.Models;
using ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace WBBSE.Areas.Schools.Controllers
{
    /*******************************************************************************
  * A BRIEF HISTORY OF SchoolValidationController.
  * CONTAINS SCHOOL DATA VALIDATION RELATED ACTIONS 
  * COMMON: COMMON PROJECT FOR UTILITY FUNCTIONS
  * ViewModel: THIS IS A PROJECT WHERE ALL VIEW MODEL CLASSES ARE WRITTEN AS CLASS FILE.
  * WBBSE.MODELS: COMMON MODEL CLASS FOR COMMON ACTION/FUNCTIONS LIKE EXCEPTION LOG.  
  * WBBSE.AREAS.SCHOOLS.MODELS: MODEL CLASS FOR SchoolHome RELATED BUSINESS LOGICS & FUNCTIONS. 
  ******************************************************************************/

    /* ROLE BASED ASP.NET MVC AUTHORIZATION [ONLY SCHOOL ROLE IS ALLOWED FOR ACCESSING THIS CONTROLLER]
     * NoCache: PREVENT CACHING IN MVC
     */
    public class SchoolValidationController : Controller
    {
        public ActionResult SchoolValidation(string x) //DEFAULT ACTION, x=ENCRYPTED ROLE ID
        {
            try
            {
               ViewData[ViewDataNames.RawData] = x;
               return View();
             
            }
            catch (Exception ex)
            {
                /********HANDLEING CATCH EXCEPTION LOG USING COMMON EXCEPTION HANDLING & ERROR VIEW PAGE REDIRECTION *****************/
                MCommon.saveExceptionLog(ex.Message, "SchoolValidation/View", "SchoolValidationController");
                return Redirect("~/Error/Unexpected.html");
            }
        }

    }
}
