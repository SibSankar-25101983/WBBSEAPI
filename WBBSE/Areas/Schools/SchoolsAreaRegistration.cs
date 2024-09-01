using System.Web.Mvc;

namespace WBBSE.Areas.Schools
{
    public class SchoolsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Schools";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SchoolHome",
                "Schools/{controller}/{action}/{id}",
                new { Controller = "SchoolHome", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}
