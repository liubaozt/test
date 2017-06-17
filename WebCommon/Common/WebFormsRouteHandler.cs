using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;

namespace WebCommon.Common
{
    public class WebFormsRouteHandler : IRouteHandler
    {
        private string pageName = string.Empty;
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // 从URL中获取page参数
            pageName = requestContext.RouteData.GetRequiredString("page");
            // 创建实例
            // 根据 page 参数拼接成类似/WebForms/page.aspx地址来访问WebForms页面
            IHttpHandler hander = BuildManager.CreateInstanceFromVirtualPath("/WebForms/" + this.pageName + ".aspx", typeof(System.Web.UI.Page)) as IHttpHandler;
            return hander;
        }
    }
}