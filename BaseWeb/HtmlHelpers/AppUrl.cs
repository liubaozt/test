using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppUrl
    {
        public static string AppActionUrl(this UrlHelper helper, string urlString)
        {
            string[] urlStrings = urlString.Split('/');
            return helper.Action(urlStrings[3], urlStrings[2], new { Area = urlStrings[1] });
        }
    }
}