using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Text;
using BaseCommon.Data;

namespace BaseControl.HtmlHelpers
{
    public static class AppAutoComplete
    {
        public static MvcHtmlString AppAutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string styleTage,string source)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string id = name + pageId;
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;

            TagBuilder tg = new TagBuilder("input");
            tg.MergeAttribute("name", name, true);
            tg.MergeAttribute("class", "text" + styleTage);
            tg.GenerateId(id);
            if (data != null)
            {
               tg.MergeAttribute("value", data.ToString());
            }
            tg.MergeAttribute("type", "text");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine(" $(document).ready(function () {");

            sb.AppendLine("var availableTags ="+source+";");
            sb.AppendLine("$('#"+id+"').autocomplete({");
            sb.AppendLine("source: availableTags");
            sb.AppendLine("});");

            sb.AppendLine(" });");
            sb.AppendLine("</script>");

            return MvcHtmlString.Create(tg.ToString(TagRenderMode.Normal) + sb.ToString());
        }


    }
}