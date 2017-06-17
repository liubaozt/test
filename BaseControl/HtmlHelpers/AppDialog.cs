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
            sb.AppendLine(" AppAppendSelect(data, '" + selectID + "','" + urlString + "');");
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

        public static MvcHtmlString AppTreeDialogFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string dropUrl, string dialogUrl, string dialogTitle, string dialogTreeId, string addFavoritUrl, string replaceFavoritUrl, string styleTage)
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
                sb.AppendLine(" AppAppendSelect2(data, '" + selectID + "','" + dropUrl + "');");
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

            sb.AppendLine("var sheight=$('" + selectID + "').height();");
            sb.AppendLine("var borderheight=$('" + selectID + "').css('borderTopWidth');");
            sb.AppendLine("var borderheight=borderheight.replace('px', '');");

            sb.AppendLine("var myheight=21;");
            sb.AppendLine("if(sheight+parseInt(borderheight)<10) {");
            sb.AppendLine(" myheight=18;");
            sb.AppendLine("} else {");
            sb.AppendLine("myheight=sheight+parseInt(borderheight)");
            sb.AppendLine("}");

            //sb.AppendLine("$('" + selectID + "Button').height(sheight+parseInt(borderheight));");
            sb.AppendLine("$('" + selectID + "Button').height(myheight);");

            //按钮单击事件
            sb.AppendLine("$('" + selectID + "Button').click(function () {");
            sb.AppendFormat(@"AppTreeDialogButtonClick('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", selectID, dialogTreeId, pageId, dialogUrl, dialogTitle, dropUrl, addFavoritUrl, replaceFavoritUrl);
            sb.AppendLine(" });");


            sb.AppendLine("});");
            sb.AppendLine("</script>");

            TagBuilder tg = new TagBuilder("select");
            tg.MergeAttribute("name", name, true);
            tg.MergeAttribute("class", "dialog" + styleTage);
            tg.GenerateId(id);
            sb.AppendLine("<input id='" + id + "Button' value='' style='width:16px;margin:0;padding:0;float:left;'/>");
            sb.AppendLine("<div id='" + id + "Dialog'>");
            sb.AppendLine("</div>");

            return MvcHtmlString.Create(Environment.NewLine + tg.ToString(TagRenderMode.Normal) + sb.ToString());
        }

        public static MvcHtmlString AppTreeDialogMultipleFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string display, string pageId, string dropUrl, string dialogUrl, string dialogTitle, string dialogTreeId, string addFavoritUrl, string replaceFavoritUrl, string styleTage)
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
            //sb.AppendFormat(@"$(""{0}"").empty();", selectID);
            //sb.AppendLine("$('<option></option>').val('').text('').appendTo($('" + selectID + "'));");

            //if (DataConvert.ToString(dropUrl).Contains("filterExpression"))
            //{
            //sb.AppendLine("$.getJSON('" + dropUrl + "', function (data) {");
            //sb.AppendLine(" AppAppendSelect2(data, '" + selectID + "','" + dropUrl + "');");
            //sb.AppendLine("$('" + selectID + "').val('" + DataConvert.ToString(data) + "');");
            //sb.AppendLine("});");
            //}

            sb.AppendLine("$('" + selectID + "').val('" + DataConvert.ToString(data) + "');");
            sb.AppendLine("$('" + selectID + "Display').val('" + DataConvert.ToString(display) + "');");

            //按钮设置样式
            sb.AppendLine("$('" + selectID + "Button').button({");
            sb.AppendLine("text: true");
            sb.AppendLine("});");

            sb.AppendLine("var sheight=$('" + selectID + "').height();");
            sb.AppendLine("var borderheight=$('" + selectID + "').css('borderTopWidth');");
            sb.AppendLine("var borderheight=borderheight.replace('px', '');");

            sb.AppendLine("var myheight=21;");
            sb.AppendLine("if(sheight+parseInt(borderheight)<10) {");
            sb.AppendLine(" myheight=18;");
            sb.AppendLine("} else {");
            sb.AppendLine("myheight=sheight+parseInt(borderheight)");
            sb.AppendLine("}");

            //sb.AppendLine("$('" + selectID + "Button').height(sheight+parseInt(borderheight));");
            sb.AppendLine("$('" + selectID + "Button').height(myheight);");

            //按钮单击事件
            sb.AppendLine("$('" + selectID + "Button').click(function () {");
            sb.AppendFormat(@"AppTreeDialogButtonClick3('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');", selectID, dialogTreeId, pageId, dialogUrl, dialogTitle, dropUrl, addFavoritUrl, replaceFavoritUrl);
            sb.AppendLine(" });");


            sb.AppendLine("});");
            sb.AppendLine("</script>");

            TagBuilder tg = new TagBuilder("input");
            tg.MergeAttribute("name", name + "Display", true);
            tg.MergeAttribute("class", "dialog" + styleTage);
            tg.GenerateId(id+"Display");

            //按钮
            sb.AppendLine("<input id='" + id + "Button' value='' style='width:16px;margin:0;padding:0;float:left;'/>");
            //弹出的框
            sb.AppendLine("<div id='" + id + "Dialog'>");
            sb.AppendLine("</div>");
            //隐藏的实际值
            sb.AppendFormat("<input type='hidden' id='{0}' name='{1}'></input>", id,name);

            return MvcHtmlString.Create(Environment.NewLine + tg.ToString(TagRenderMode.Normal) + sb.ToString());
        }

    }
}