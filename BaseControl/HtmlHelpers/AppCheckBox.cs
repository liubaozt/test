using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Text;

namespace BaseControl.HtmlHelpers
{
    public static class AppCheckBox
    {
        public static MvcHtmlString AppCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, string pageId, string styleTage)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string id = name + pageId ;
            object data = ModelMetadata.FromLambdaExpression<TModel, bool>(expression, htmlHelper.ViewData).Model;
            TagBuilder tg = new TagBuilder("input");
            tg.MergeAttribute("name", name, true);
            tg.MergeAttribute("class", "checkbox" + styleTage);
            tg.GenerateId(id);
            if (data != null)
            {
                tg.MergeAttribute("value", data.ToString().ToLower());
                if (data.ToString().ToLower() == "true")
                {
                    tg.MergeAttribute("checked", "true");
                }
            }
            tg.MergeAttribute("type", "checkbox");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine(" $(document).ready(function () {");

            sb.AppendLine("$('#" + id + "').click(function () {");
            sb.AppendLine("if ($('#" + id + "').attr('checked')) {");
            sb.AppendLine("$('#" + id + "').val('true');");
            sb.AppendLine(" }");
            sb.AppendLine(" else {");
            sb.AppendLine("$('#" + id + "').val('false');");
            sb.AppendLine(" }");
            sb.AppendLine(" });");

            sb.AppendLine(" });");
            sb.AppendLine("</script>");
            return MvcHtmlString.Create(tg.ToString(TagRenderMode.Normal) + sb.ToString());
        }

    }
}