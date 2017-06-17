using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace BaseControl.HtmlHelpers
{
    public static class AppForm
    {
        public static MvcHtmlString AppBeginForm(this HtmlHelper htmlHelper, string pageId, string formId, string submitUrl = "")
        {
            formId = formId + pageId;
            string sb = "";
            sb="<form id='"+formId+"'";
            if (submitUrl != "")
                sb += " action='" + submitUrl + "' ";
            sb += ">";
            return MvcHtmlString.Create(sb);
        }

        public static MvcHtmlString AppEndForm(this HtmlHelper htmlHelper)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("</form>");
            return MvcHtmlString.Create( sb.ToString());
        }
    }
}