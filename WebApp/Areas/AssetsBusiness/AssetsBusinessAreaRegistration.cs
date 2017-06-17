using System.Web.Mvc;

namespace WebApp.Areas.AssetsBusiness
{
    public class AssetsBusinessAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AssetsBusiness";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AssetsBusiness_default",
                "AssetsBusiness/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
