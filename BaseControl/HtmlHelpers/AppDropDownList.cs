using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;
using BaseCommon.Data;

namespace BaseControl.HtmlHelpers
{
    public static class AppDropDownList
    {
        public static MvcHtmlString AppDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string dataSourceUrl, string styleTage, bool firstItemIsNull = true)
        {
            string str = AppDropDownListForStr(htmlHelper, expression, pageId, dataSourceUrl, styleTage, firstItemIsNull);
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString AppDropDownEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string dataSourceUrl, string styleTage, bool firstItemIsNull = true)
        {
            string str = AppDropDownListForStr(htmlHelper, expression, pageId, dataSourceUrl, styleTage, firstItemIsNull,true);

            
            return MvcHtmlString.Create(str );
        }

        private static string AppDropDownListForStr<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pageId, string dataSourceUrl, string styleTage, bool firstItemIsNull = true,bool isEdit=false)
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
                    AppAppendSelect(data,""{1}"",'{0}');
                    $(""{1}"").attr(""value"",'{2}');
                }}); ", dataSourceUrl, selectID, data == null ? string.Empty : data.ToString());
            sb.AppendLine("});");

            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine(" $('" + selectID + "Label').click(function () {");
            if (firstItemIsNull)
                sb.AppendFormat(@"
                $.getJSON(""{0}"", function(data) {{
                    AppAppendSelect2(data,""{1}"",'{0}');
                    $(""{1}"").attr(""value"",'{2}');
                }}); ", dataSourceUrl, selectID, data == null ? string.Empty : data.ToString());
            else
                sb.AppendFormat(@"
                $.getJSON(""{0}"", function(data) {{
                    AppAppendSelect(data,""{1}"",'{0}');
                    $(""{1}"").attr(""value"",'{2}');
                }}); ", dataSourceUrl, selectID, data == null ? string.Empty : data.ToString());
            sb.AppendLine("});");
            sb.AppendLine("});");

            sb.AppendLine("</script>");

            string tag = "";
            if (isEdit)
            {
                tag =string.Format(@"<DIV style='POSITION: absolute'>
                            <TABLE cellSpacing=0 cellPadding=0 border=0>
                                <TBODY>
                                    <TR>
                                        <TD>
                                            <SELECT id='{0}' name='{1}' class='{4}' style=' TOP: 0px;font-size:13px; CLIP: rect(0px auto auto 80px); POSITION: absolute' onchange=""document.getElementById('{2}').value=this.options[this.selectedIndex].text"">
                                            </SELECT>
                                            <INPUT id='{2}' name='{3}'  class='{5}' style=' TOP: 0px; POSITION: absolute'>
                                        </TD>
                                    </TR>
                                </TBODY>
                            </TABLE>
                        </DIV>", id, name, id + "Editor", name + "Editor", "editor" + styleTage , "editorInput" + styleTage);

                sb.AppendLine("<script type=\"text/javascript\">");
                sb.AppendLine("$(function(){");
                //改变输入框值改变下拉框数据源 start
                sb.AppendLine(" $('#" + id + "Editor" + "').change(function () {");
                sb.AppendLine(" var inVal= $('#" + id + "Editor" + "').val();");
                //sb.AppendLine(" alert(inVal);");
                sb.AppendLine(" var oldurl= $('#" + id + "Url" + "').val();");
                //sb.AppendLine("  alert(oldurl);");
                sb.AppendLine("var dataSourceUrl;");
                sb.AppendLine("if(oldurl.indexOf('?')>=0)");
                sb.AppendLine("  dataSourceUrl=oldurl+'&pySearch='+inVal");
                sb.AppendLine("else");
                sb.AppendLine("  dataSourceUrl=oldurl+'?pySearch='+inVal");
                //sb.AppendLine("  alert(dataSourceUrl);");
                if (firstItemIsNull)
                    sb.AppendFormat(@"
                    $.getJSON(dataSourceUrl, function(data) {{
                        AppAppendSelect2(data,""{0}"",oldurl);
                        $(""{0}"").attr(""value"",'{1}');
                    }}); ", selectID, data == null ? string.Empty : data.ToString());
                else
                    sb.AppendFormat(@"
                    $.getJSON(dataSourceUrl, function(data) {{
                        AppAppendSelect(data,""{0}"",oldurl);
                        $(""{0}"").attr(""value"",'{1}');
                    }}); ", selectID, data == null ? string.Empty : data.ToString());
                sb.AppendLine("});");
                //改变输入框值改变下拉框数据源 end
                sb.AppendLine("});");
                sb.AppendLine("</script>");
            }
            else
            {
                TagBuilder tg = new TagBuilder("select");
                tg.MergeAttribute("name", name, true);
                tg.MergeAttribute("class", "select" + styleTage);
                tg.GenerateId(id);
                tag = tg.ToString(TagRenderMode.Normal);
            }

            //记录下拉数据源的url start
            TagBuilder tgUrl = new TagBuilder("input");
            tgUrl.MergeAttribute("name", name+"Url", true);
            tgUrl.GenerateId(id + "Url");
            tgUrl.MergeAttribute("type", "hidden");
            //记录下拉数据源的url end

            return sb.ToString() + Environment.NewLine + tag +  Environment.NewLine+ tgUrl.ToString(TagRenderMode.Normal);
        }


        public static MvcHtmlString AppDropDownListMultipleFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string display, string pageId, string dropUrl,string selectVal, string styleTage)
        {
            string name = ExpressionHelper.GetExpressionText(expression); 
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;
            string id = name + pageId;
            string inputID = name + pageId;
            if (inputID.Substring(0, 1) != "#")
                inputID = "#" + inputID;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(function(){");

            sb.AppendLine("$('" + inputID + "').val('" + DataConvert.ToString(data) + "');");
            sb.AppendLine("$('" + inputID + "Display').val('" + DataConvert.ToString(display) + "');");


            //按钮单击事件
            sb.AppendFormat(@"AppDropMultipleDiv('{0}','{1}','{2}','{3}');", inputID, pageId, dropUrl, selectVal);

            sb.AppendLine("$('" + inputID + "Display').click(function (){;");
            sb.AppendLine(@"$('#"+id+"DropDiv').show();");
            sb.AppendLine("});");

            //sb.AppendLine("$('" + inputID + "DropDiv').focusout(function (){;");
            //sb.AppendLine(@"$('#" + id + "DropDiv').hide();");
            //sb.AppendLine("});");

            sb.AppendLine("});");
            sb.AppendLine("</script>");

            TagBuilder tg = new TagBuilder("input");
            tg.MergeAttribute("name", name + "Display", true);
            tg.MergeAttribute("class", "text" + styleTage);
            tg.GenerateId(id + "Display");

            //弹出的框
            sb.AppendLine("<div id='" + id + "DropDiv' class='ui-widget-content' style='z-index:999999;position: absolute;display:none; background-color: #fff;' >");
            sb.AppendLine("</div>");
            //隐藏的实际值
            sb.AppendFormat("<input type='hidden' id='{0}' name='{1}'></input>", id, name);

            return MvcHtmlString.Create(Environment.NewLine + tg.ToString(TagRenderMode.Normal) + sb.ToString());
        }
    
    }
}