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
    public static class AppTextBox
    {
        public static MvcHtmlString AppTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string styleTage, TextType textType = TextType.Text)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string id = name + pageId ;
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;

            TagBuilder tg = new TagBuilder("input");
            tg.MergeAttribute("name", name, true);
            tg.MergeAttribute("class", "text" + styleTage);
            tg.GenerateId(id);

            if (data != null)
            {
                tg.MergeAttribute("value", data.ToString());
            }

            if (textType == TextType.Password)
            {
                tg.MergeAttribute("type", "password");
            }
            else if (textType == TextType.Hidden)
            {
                tg.MergeAttribute("type", "hidden");
            }
            else
            {
                tg.MergeAttribute("type", "text");
            }
            return MvcHtmlString.Create(tg.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString AppHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string id = name + pageId;
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;

            TagBuilder tg = new TagBuilder("input");
            tg.MergeAttribute("name", name, true);
            tg.GenerateId(id);

            if (data != null)
            {
                tg.MergeAttribute("value", data.ToString());
            }

            tg.MergeAttribute("type", "hidden");

            return MvcHtmlString.Create(tg.ToString(TagRenderMode.Normal));
        }
    }
}