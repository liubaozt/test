using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Data;

namespace WebCommon.HttpModule
{
    public class AuthUserModule : IHttpModule
    {
        #region IHttpModule 成员

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateExecute);
        }

        void context_AuthenticateExecute(object sender, EventArgs e)
        {
            HttpApplication ha = (HttpApplication)sender;
            if (ha.User != null)
            {
                if (DataConvert.ToString(ha.User.Identity.Name) == "")
                {
                    ha.Response.Redirect("~/Home/Error");
                }
                if (!ha.User.Identity.IsAuthenticated)
                {
                    ha.Response.Redirect("~/Home/NoAccess");
                }
            }
        }

        #endregion
    }
}