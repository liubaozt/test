using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppDropDownList
    {
        public static MvcHtmlString AppDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string dataSourceUrl, string styleTage, bool firstItemIsNull = true)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;
            string id = name + pageId;
            string selectID = name + pageId;
            if (selectID.Substring(0, 1) != "#")
                selectID = "#" + selectID;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(function(){");
            sb.AppendFormat(@" $(""{0}"").empty();", selectID);
            if (firstItemIsNull)
                sb.AppendFormat(@"
                $.getJSON(""{0}"", function(data) {{
                    AppAppendSelect2(data,""{1}"");
                    $(""{1}"").attr(""value"",'{2}');
                }}); ", dataSourceUrl, selectID, data == null ? string.Empty : data.ToString());
            else
                sb.AppendFormat(@"
                $.getJSON(""{0}"", function(data) {{
                    AppAppendSelect(data,""{1}"");
                    $(""{1}"").attr(""value"",'{2}');
                }}); ", dataSourceUrl, selectID, data == null ? string.Empty : data.ToString());
            sb.AppendLine("});");

            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine(" $('" + selectID + "Label').click(function () {");
            if (firstItemIsNull)
                sb.AppendFormat(@"
                $.getJSON(""{0}"", function(data) {{
                    AppAppendSelect2(data,""{1}"");
                    $(""{1}"").attr(""value"",'{2}');
                }}); ", dataSourceUrl, selectID, data == null ? string.Empty : data.ToString());
            else
                sb.AppendFormat(@"
                $.getJSON(""{0}"", function(data) {{
                    AppAppendSelect(data,""{1}"");
                    $(""{1}"").attr(""value"",'{2}');
                }}); ", dataSourceUrl, selectID, data == null ? string.Empty : data.ToString());
            sb.AppendLine("});");
            sb.AppendLine("});");

            sb.AppendLine("</script>");

            TagBuilder tg = new TagBuilder("select");
            tg.MergeAttribute("name", name, true);
            tg.MergeAttribute("class", "select" + styleTage);
            tg.GenerateId(id);
            return MvcHtmlString.Create(sb.ToString() + Environment.NewLine + tg.ToString(TagRenderMode.Normal));
        }
    }
}