using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;

namespace BaseControl.HtmlHelpers
{
    public static class AppDatePicker
    {
        private const string DatePick_FORMAT = "yyyy-MM-dd";
    
        /// <summary>
        /// 日期控件
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="backColor">背景色</param>
        /// <param name="ctrWidth">宽度</param>
        /// <returns></returns>
        public static MvcHtmlString AppDatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string styleTage)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string id = name + pageId;
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;
            //ModelMetadata metaproperty = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(TModel), name);
            StringBuilder sbGrid = new StringBuilder();
            sbGrid.AppendLine("<script type=\"text/javascript\">");
            sbGrid.AppendLine("$(function(){");
            sbGrid.AppendLine("$('#%%datepicker%%').datepicker({".Replace("%%datepicker%%", id));
            sbGrid.AppendFormat(" inline: true");
            sbGrid.AppendLine("});");
            sbGrid.AppendLine("});");
            sbGrid.AppendLine("</script>");
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.MergeAttribute("class", "text" + styleTage);
            tagBuilder.GenerateId(id);
            //tagBuilder.MergeAttributes(dicHtml);
            tagBuilder.MergeAttribute("type", "text");
            //使用meioMask扩展只允许输入日期类型的数据
            tagBuilder.MergeAttribute("alt", "amcdate");
            tagBuilder.AddCssClass("datepicker");
            DateTime value;
            string format = DatePick_FORMAT;
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                if (value != DateTime.MinValue)
                {
                    tagBuilder.MergeAttribute("value", value.ToString(format));
                }
            }
            return MvcHtmlString.Create(sbGrid.ToString() + tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}