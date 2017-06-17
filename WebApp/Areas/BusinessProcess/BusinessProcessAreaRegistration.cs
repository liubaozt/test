using System.Web.Mvc;

namespace WebApp.Areas.BusinessProcess
{
    public class BusinessProcessAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BusinessProcess";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BusinessProcess_default",
                "BusinessProcess/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
