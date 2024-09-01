using System.Web.Mvc;

namespace WBBSE.Areas.PreSchools
{
    public class PreSchoolsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PreSchools";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PreSchools_default",
                "PreSchools/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
