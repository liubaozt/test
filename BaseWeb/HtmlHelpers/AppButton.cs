using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using BaseCommon.Data;
using BaseCommon.Basic;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppButton
    {
        public static MvcHtmlString AppSubmitButton(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string btnId, string formId, string text, string submitUrl, bool IsDisabled)
        {
            string btnIdAct = btnId + pageId;
            formId = formId + pageId;
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.GenerateId(btnIdAct);
            tagBuilder.MergeAttribute("name", btnIdAct);
            tagBuilder.MergeAttribute("type", "button");
            tagBuilder.MergeAttribute("value", text);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function () {");

            sb.AppendLine("$('#" + btnIdAct + "').button({");
            sb.AppendLine("text: true");
            sb.AppendLine("});");

            if (IsDisabled)
                sb.AppendLine("$('#" + btnIdAct + "').attr('disabled', true);"); 

            sb.AppendLine("$('#" + btnIdAct + "').click(function () {");
            if (text == AppMember.AppText["BtnDelete"].ToString())
            {
                sb.AppendLine("$('#" + btnIdAct + "Dialog' ).html('<p style=\"float:left\"><span class=\"ui-icon ui-icon-alert\" style=\"float:left; margin:0 7px 20px 0;\"></span>" + AppMember.AppText["ConfirmDelete"].ToString() + "</p>').dialog({");
                sb.AppendLine("resizable: false,");
                sb.AppendLine("height:140,");
                sb.AppendLine("modal: true,");
                sb.AppendLine("buttons: {");
                sb.AppendLine("'" + AppMember.AppText["BtnConfirm"].ToString() + "': function() {");
                sb.AppendLine("$( this ).dialog( 'close' );");

                sb.AppendLine("$.ajax({");
                sb.AppendLine("type: 'POST',");
                sb.AppendFormat("url: '{0}',", submitUrl);
                sb.AppendLine("data: $('#" + formId + "').serialize(),");
                sb.AppendLine("success: function (html) {");
                sb.AppendFormat("$('#t{0}', '#tabs').empty();", pageId);
                sb.AppendFormat("$('#t{0}', '#tabs').append(html);", pageId);
                sb.AppendLine("}");
                sb.AppendLine("});");


                sb.AppendLine("},");
                sb.AppendLine("'" + AppMember.AppText["BtnCancel"].ToString() + "': function() {");
                sb.AppendLine("$( this ).dialog( 'close' );");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("});");

            }
            else
            {
                sb.AppendLine("$.ajax({");
                sb.AppendLine("type: 'POST',");
                sb.AppendFormat("url: '{0}',", submitUrl);
                sb.AppendLine("data: $('#" + formId + "').serialize(),");
                sb.AppendLine("success: function (html) {");
                sb.AppendFormat("$('#t{0}', '#tabs').empty();", pageId);
                sb.AppendFormat("$('#t{0}', '#tabs').append(html);", pageId);
                sb.AppendLine("}");
                sb.AppendLine("});");
            }
            sb.AppendLine("});");

            ////如果有message，弹出
            sb.AppendLine("var msg = $('#Message" + pageId + "').val();");
            sb.AppendLine("if (msg) {");
            sb.AppendLine(" AppMessage(" + pageId + ", '" + AppMember.AppText["MessageTitle"] + "', msg , 'error', function () { });");
            sb.AppendLine("}");

            //如果更新正确的话，将按钮设置成不可用
            sb.AppendLine("var err = $('#HasError" + pageId + "').val();");
            sb.AppendLine("if (err == 'false') {");
            sb.AppendLine("$('#" + btnIdAct + "').attr('disabled', true);");
            sb.AppendLine("}");

            sb.AppendLine("});");
            sb.AppendLine("   </script>");

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing) + sb.ToString());
        }

        public static MvcHtmlString AppNormalButton(this HtmlHelper htmlHelper, string pageId, string btnId, string text)
        {
            string btnIdAct = btnId + pageId;
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.GenerateId(btnIdAct);
            tagBuilder.MergeAttribute("name", btnIdAct);
            tagBuilder.MergeAttribute("type", "button");
            tagBuilder.MergeAttribute("value", text);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function () {");

            sb.AppendLine("$('#" + btnIdAct + "').button({");
            sb.AppendLine("text: true");
            sb.AppendLine("});");

            sb.AppendLine("});");
            sb.AppendLine("   </script>");

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing) + sb.ToString());
        }

        public static MvcHtmlString AppListQueryButton(this HtmlHelper htmlHelper, string pageId, string formId, string gridId, string btnId, string text, string styleString = "")
        {
            string btnIdAct = btnId + pageId;
            string gridIdAct = gridId + pageId;
            formId = formId + pageId;
            //TagBuilder tagBuilder = new TagBuilder("input");
            //tagBuilder.GenerateId(btnIdAct);
            //tagBuilder.MergeAttribute("name", btnIdAct);
            //tagBuilder.MergeAttribute("type", "button");
            //tagBuilder.MergeAttribute("value", text);
            //if (styleString != "")
            //    tagBuilder.MergeAttribute("style", styleString);
            //else
            //    tagBuilder.MergeAttribute("style", "height:21px;vertical-align:text-top");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<input id='" + btnIdAct + "' value='Query' style='width:30px; ' />");
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function () {");

            //sb.AppendLine("$('#" + btnIdAct + "').button({");
            //sb.AppendLine("text: false,");
            //sb.AppendLine("icons: {");
            //sb.AppendLine("primary: 'ui-icon-search'");
            //sb.AppendLine("}");
            //sb.AppendLine("});");

            sb.AppendLine("$('#" + btnIdAct + "').button({");
            sb.AppendLine("text: true");
            sb.AppendLine("});");

            sb.AppendLine("$('#" + btnIdAct + "').click(function () {");
            sb.AppendLine(" var obj = $('#" + formId + "').serializeObject();");
            sb.AppendLine("  var formvar = JSON.stringify(obj);");
            sb.AppendLine("$('#" + gridIdAct + "').jqGrid('setPostData', { formVar: formvar });");
            sb.AppendLine("jQuery('#" + gridIdAct + "').trigger('reloadGrid');");
            sb.AppendLine("});");
            sb.AppendLine("});");
            sb.AppendLine("   </script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString AppLinkButton(this HtmlHelper htmlHelper, string pageId, string btnId, string text, string submitUrl, string viewTitle)
        {
            string btnIdAct = btnId + pageId;
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.GenerateId(btnIdAct);
            tagBuilder.MergeAttribute("name", btnIdAct);
            tagBuilder.MergeAttribute("type", "button");
            tagBuilder.MergeAttribute("value", text);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function () {");

            sb.AppendLine("$('#" + btnIdAct + "').button({");
            sb.AppendLine("text: true");
            sb.AppendLine("});");

            sb.AppendLine("$('#" + btnIdAct + "').click(function () {");



            sb.AppendLine("$.ajax({");
            sb.AppendLine("type: 'GET',");
            sb.AppendFormat("url: '{0}',", submitUrl);
            sb.AppendLine("data: { pageId:'" + pageId + "',viewTitle:'" + viewTitle + "' },");
            sb.AppendLine("success: function (html) {");
            sb.AppendLine("var curtab=$(\"#tabs A[href='#t" + pageId + "']\");");
            sb.AppendLine(" curtab.text('" + viewTitle + "')");
            sb.AppendFormat("$('#t{0}', '#tabs').empty();", pageId);
            sb.AppendFormat("$('#t{0}', '#tabs').append(html);", pageId);
            sb.AppendLine("}");
            sb.AppendLine("});");

            sb.AppendLine("});");
            sb.AppendLine("});");
            sb.AppendLine("   </script>");

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing) + sb.ToString());
        }

        public static string AppGridButtonString(string pageId, string formId, string gridId, string btnId, string text, string url, string formMode, string primaryKey, string viewTitle)
        {
            string btnIdAct = btnId + gridId + pageId;
            string gridIdAct = gridId + pageId;
            formId = formId + pageId;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("$('#t_" + gridIdAct + "').append(\"<input id='" + btnIdAct + "' type='button' value='" + text + "' style='height:20px;font-size:-1;line-height: 0.8; ' />\");");
            //sb.AppendLine("$('#t_" + gridIdAct + "').append(\"<button id='" + btnIdAct + "'  style='height:20px; ' >" + text + "</button>\");");

            sb.AppendLine("$('#" + btnIdAct + "', '#t_" + gridIdAct + "').button({");
            sb.AppendLine("text: true,");
            sb.AppendLine("icons: {");
            sb.AppendLine("primary: \"ui-icon-newwin\"");
            sb.AppendLine("}");
            sb.AppendLine("});");

            if (btnId == "query")
            {
                sb.AppendLine("$('#" + btnIdAct + "', '#t_" + gridIdAct + "').click(function () {");
                sb.AppendLine(" var obj = $('#" + formId + "').serializeObject();");
                sb.AppendLine("  var formvar = JSON.stringify(obj);");
                sb.AppendLine("$('#" + gridIdAct + "').jqGrid('setPostData', { formVar: formvar,isQuery:'true' });");
                sb.AppendLine("jQuery('#" + gridIdAct + "').trigger('reloadGrid');");
                sb.AppendLine("});");
            }
            else
            {
                sb.AppendLine("$('#" + btnIdAct + "', '#t_" + gridIdAct + "').click(function () {");
                sb.AppendFormat("var id = jQuery(\"#{0}\").jqGrid('getGridParam','selrow');", gridIdAct);
                sb.AppendLine("var pk; ");
                sb.AppendLine("if (id) { ");
                sb.AppendFormat(" pk=jQuery(\"#{0}\").getCell(id,'{1}');", gridIdAct, primaryKey);
                sb.AppendLine("}");
                sb.AppendLine("else{");
                if (formMode != "new")
                {
                    sb.AppendLine("AppMessage('" + pageId + "', '" + AppMember.AppText["MessageTitle"] + "', '" + AppMember.AppText["NeedSelectRow"] + "', 'warning', function () { });");
                    sb.AppendLine("return;");
                }
                sb.AppendLine("}");

                sb.AppendLine("$.ajax({");
                sb.AppendLine("type: 'Get',");
                sb.AppendLine("dataType: 'html',");
                sb.AppendFormat("url: '{0}',", url);
                sb.AppendLine("data:{ pageId:'" + pageId + "',primaryKey: pk,formMode:'" + formMode + "',viewTitle:'" + viewTitle + "'},");
                sb.AppendLine(" complete: function (req, err) {");
                if (formMode != "new2" && formMode != "approve" && formMode != "reapply")
                {
                    sb.AppendLine("var curtab=$(\"#tabs A[href='#t" + pageId + "']\");");
                    sb.AppendLine(" curtab.text('" + viewTitle + "[" + AppMember.AppText[formMode].ToString() + "]')");
                }
                sb.AppendLine("$('#t" + pageId + "', '#tabs').empty();");
                sb.AppendLine("$('#t" + pageId + "', '#tabs').append(req.responseText);");
                sb.AppendLine("}");
                sb.AppendLine("});");

                sb.AppendLine("});");
            }
            sb.AppendLine("});");
            sb.AppendLine("</script>");
            return sb.ToString();
        }
        public static MvcHtmlString AppGridButton(this HtmlHelper htmlHelper, string pageId, string formId, string gridId, string btnId, string text, string url, string formMode, string primaryKey, string viewTitle)
        {

            return MvcHtmlString.Create(AppGridButtonString(pageId, formId, gridId, btnId, text, url, formMode, primaryKey, viewTitle).ToString());
        }

        public static MvcHtmlString AppGridButtonSet(this HtmlHelper htmlHelper, DataTable dt, string formId, string primaryKey, string viewTitle)
        {
            string sb = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                sb += AppGridButtonString(DataConvert.ToString(dr["pageId"]), formId, DataConvert.ToString(dr["gridId"]),
                    DataConvert.ToString(dr["btnId"]), DataConvert.ToString(dr["text"]),
                    DataConvert.ToString(dr["url"]), DataConvert.ToString(dr["formMode"]), primaryKey, viewTitle).ToString();
            }
            return MvcHtmlString.Create(sb.ToString());
        }


    }
}