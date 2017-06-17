using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Text;
using BaseCommon.Data;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppDialog
    {
        public static MvcHtmlString AppDialogFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string urlString, string styleString = "")
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
            sb.AppendFormat(@"$(""{0}"").empty();", selectID);
            sb.AppendLine("$('<option></option>').val('').text('').appendTo($('" + selectID + "'));");

            sb.AppendLine("$.getJSON('" + urlString + "', function (data) {");
            sb.AppendLine(" AppAppendSelect(data, '" + selectID + "');");
            sb.AppendLine("$('" + selectID + "').val('" + DataConvert.ToString(data) + "');");
            sb.AppendLine("});");

            sb.AppendLine("$('#" + id + "Button').button({");
            sb.AppendLine("text: false,");
            sb.AppendLine("icons: {");
            sb.AppendLine("primary: 'ui-icon-search'");
            sb.AppendLine("}");
            sb.AppendLine("});");

            sb.AppendLine("});");
            sb.AppendLine("</script>");

            TagBuilder tg = new TagBuilder("select");
            tg.MergeAttribute("name", name, true);
            tg.MergeAttribute("class", "inputdialog");
            tg.GenerateId(id);
            if (styleString != "")
                tg.MergeAttribute("style", styleString);
            sb.AppendLine("<button id='" + id + "Button' value='OpenDialog' style='height:21px;'>");
            sb.AppendLine("</button>");
            sb.AppendLine("<div id='" + id + "Dialog'>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(Environment.NewLine + tg.ToString(TagRenderMode.Normal) + sb.ToString());
        }

        public static MvcHtmlString AppTreeDialogFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string dropUrl, string dialogUrl, string dialogTitle, string dialogTreeId, string styleTage)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;
            string id = name + pageId;
            dialogTreeId = dialogTreeId + pageId;
            string selectID = name + pageId;
            if (selectID.Substring(0, 1) != "#")
                selectID = "#" + selectID;
            //string treeId = "tree" + pageId;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(function(){");
            sb.AppendFormat(@"$(""{0}"").empty();", selectID);
            sb.AppendLine("$('<option></option>').val('').text('').appendTo($('" + selectID + "'));");

            //if (DataConvert.ToString(dropUrl).Contains("filterExpression"))
            //{
                sb.AppendLine("$.getJSON('" + dropUrl + "', function (data) {");
                sb.AppendLine(" AppAppendSelect2(data, '" + selectID + "');");
                sb.AppendLine("$('" + selectID + "').val('" + DataConvert.ToString(data) + "');");
                sb.AppendLine("});");
            //}

            //按钮设置样式
            //sb.AppendLine("$('" + selectID + "Button').button({");
            //sb.AppendLine("text: false,");
            //sb.AppendLine("icons: {");
            //sb.AppendLine("primary: 'ui-icon-search'");
            //sb.AppendLine("}");
            //sb.AppendLine("});");
            sb.AppendLine("$('" + selectID + "Button').button({");
            sb.AppendLine("text: true");
            sb.AppendLine("});");

            //按钮单击事件
            sb.AppendLine("$('" + selectID + "Button').click(function () {");
            sb.AppendFormat(@"AppTreeDialogButtonClick('{0}','{1}','{2}','{3}','{4}','{5}');", selectID, dialogTreeId, pageId, dialogUrl, dialogTitle, dropUrl);
            sb.AppendLine(" });");


            sb.AppendLine("});");
            sb.AppendLine("</script>");

            TagBuilder tg = new TagBuilder("select");
            tg.MergeAttribute("name", name, true);
            tg.MergeAttribute("class", "dialog" + styleTage);
            tg.GenerateId(id);
            sb.AppendLine("<input id='" + id + "Button' value='' style='height:19px;width:16px;margin:0;padding:0;float:left;'/>");
            sb.AppendLine("<div id='" + id + "Dialog'>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(Environment.NewLine + tg.ToString(TagRenderMode.Normal) + sb.ToString());
        }

    }
}