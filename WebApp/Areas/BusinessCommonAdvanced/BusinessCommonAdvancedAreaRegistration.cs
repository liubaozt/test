using System.Web.Mvc;

namespace WebApp.Areas.BusinessCommonAdvanced
{
    public class BusinessCommonAdvancedAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BusinessCommonAdvanced";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BusinessCommonAdvanced_default",
                "BusinessCommonAdvanced/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
