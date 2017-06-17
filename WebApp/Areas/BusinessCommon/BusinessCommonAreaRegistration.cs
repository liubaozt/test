using System.Web.Mvc;

namespace WebApp.Areas.BusinessCommon
{
    public class BusinessCommonAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BusinessCommon";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BusinessCommon_default",
                "BusinessCommon/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
