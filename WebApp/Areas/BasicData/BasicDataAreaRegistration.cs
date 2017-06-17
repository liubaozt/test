using System.Web.Mvc;

namespace WebApp.Areas.BasicData
{
    public class BasicDataAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BasicData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BasicData_default",
                "BasicData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
