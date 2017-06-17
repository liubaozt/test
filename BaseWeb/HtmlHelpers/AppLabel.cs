using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Text;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppLabel
    {
        public static MvcHtmlString AppLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string styleTage )
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string id = name + pageId + "Label";
            TagBuilder tg = new TagBuilder("label");
            tg.MergeAttribute("for", name);
            tg.MergeAttribute("class", "label" + styleTage);
            tg.GenerateId(id);
            ModelMetadata metaproperty = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(TModel), name);
            tg.InnerHtml = metaproperty.GetDisplayName();
            return MvcHtmlString.Create(tg.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString AppRequiredFlag(this HtmlHelper htmlHelper)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class='RequiredFlag'>*</div>");
            return MvcHtmlString.Create(sb.ToString());
        }

    }
}