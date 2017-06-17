using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using BaseCommon.Data;
using BaseCommon.Basic;
using System.Text.RegularExpressions;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppGrid
    {

        public static MvcHtmlString AppGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string formId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, string primaryKey, string viewUrlStr, string viewTitle, int gridHeight)
        {
            string inputName = gridId + AppMember.HideString;
            gridId = gridId + pageId;
            formId = formId + pageId;
            string pagerId = gridId + "pager";
            StringBuilder sbGrid = new StringBuilder();
            sbGrid.AppendLine("<script type=\"text/javascript\">");
            sbGrid.AppendLine("$(function(){");
            #region grid设置
            sbGrid.AppendLine("$('#%%GRIDID%%').jqGrid({".Replace("%%GRIDID%%", gridId));
            sbGrid.AppendFormat("url:'{0}',", url);
            sbGrid.AppendFormat("mtype:'{0}',", "post");
            sbGrid.AppendLine(@"datatype: ""json"",");
            //sbGrid.AppendLine("postData: $('form').serialize(),");

            sbGrid.AppendFormat("colNames:[{0}],", GetColNames(gridLayout));
            sbGrid.AppendFormat("colModel:[{0}],", GetColModel(urlHelper, gridLayout));

            SetGridProperty(gridId, gridTitle, pagerId, sbGrid, gridHeight - 110, true);
            SetGridEvent(pageId, formId, gridId, sbGrid, primaryKey, viewUrlStr, viewTitle);

            sbGrid.Length--;
            sbGrid.AppendLine("});");
            SetGridPage(gridId, pagerId, sbGrid);
            sbGrid.AppendLine("});");

            #region 窗体改变大小时,重新设定grid的宽度和高度
            //gridResize(gridId, sbGrid);
            #endregion

            sbGrid.AppendLine("</script>");
            #endregion
            GenerateGridHtml(gridId, pagerId, inputName, sbGrid);
            return MvcHtmlString.Create(sbGrid.ToString());
        }



        private static string EditGrid(UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, string submitBtn, string primaryKey, bool hasToolbar, bool needAddEvent, int gridHeight, int gridWidth, bool isMultiSelect, string editFlag, bool hasClickEvent, string clickFlag)
        {
            string actGridId = gridId + pageId;
            submitBtn = submitBtn + pageId;
            string pagerId = actGridId + "pager";
            string inputId = actGridId + AppMember.HideString;
            string inputName = gridId + AppMember.HideString;
            StringBuilder sbGrid = new StringBuilder();
            sbGrid.AppendLine("<script type=\"text/javascript\">");
            sbGrid.AppendLine("var onCellEdit" + editFlag + ";");
            sbGrid.AppendLine("var lastsel;");
            //sbGrid.AppendLine("var onClickRow = function(id){return id+'qq';}; ");
            sbGrid.AppendLine("$(function(){");
            #region grid设置
            sbGrid.AppendLine("$('#%%GRIDID%%').jqGrid({".Replace("%%GRIDID%%", actGridId));
            sbGrid.AppendFormat("url:'{0}',", url);
            sbGrid.AppendLine("mtype:'post',");
            sbGrid.AppendLine(@"datatype: ""json"",");

            sbGrid.AppendFormat("colNames:[{0}],", GetColNames(gridLayout));
            sbGrid.AppendFormat("colModel:[{0}],", GetColModel(urlHelper, gridLayout));

            SetGridProperty(actGridId, gridTitle, pagerId, sbGrid, gridHeight, hasToolbar, true, gridWidth, isMultiSelect);
            SetEditGridEvent(pageId, actGridId, sbGrid, isMultiSelect, editFlag, hasClickEvent, clickFlag);

            sbGrid.Length--;
            sbGrid.AppendLine("});");
            SetGridPage(actGridId, pagerId, sbGrid);
            sbGrid.AppendLine("});");
            sbGrid.AppendLine("</script>");
            #endregion
            GenerateGridHtml(actGridId, pagerId, inputName, sbGrid, true);
            SetEditGridMethod(actGridId, sbGrid, gridLayout, primaryKey, inputId, submitBtn, hasToolbar, needAddEvent, isMultiSelect);
            return sbGrid.ToString();
        }




        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, "", "", false, false, gridHeight, 0, false, "", false, "");
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight, int gridWidth)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, "", "", false, false, gridHeight, gridWidth, false, "", false, "");
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight, int gridWidth, bool isMultiSelect)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, "", "", false, false, gridHeight, gridWidth, isMultiSelect, "", false, "");
            return MvcHtmlString.Create(str);
        }


        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight, int gridWidth, bool hasClickEvent, string clickFlag)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, "", "", false, false, gridHeight, gridWidth, false, "", hasClickEvent, clickFlag);
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight, int gridWidth, string submitBtn, string editFlag)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, submitBtn, "", false, false, gridHeight, gridWidth, false, editFlag, false, "");
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight, int gridWidth, bool hasToolbar, bool needAddEvent)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, "", "", hasToolbar, needAddEvent, gridHeight, gridWidth, false, "", false, "");
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight, int gridWidth, bool hasToolbar, bool needAddEvent, bool hasClickEvent, string clickFlag)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, "", "", hasToolbar, needAddEvent, gridHeight, gridWidth, false, "", hasClickEvent, clickFlag);
            return MvcHtmlString.Create(str);
        }


        public static MvcHtmlString AppEditGridFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string gridId, string gridTitle, string url, Dictionary<string, GridInfo> gridLayout, int gridHeight, int gridWidth, bool hasToolbar, bool needAddEvent, string submitBtn, string editFlag)
        {
            string str = EditGrid(urlHelper, pageId, gridId, gridTitle, url, gridLayout, submitBtn, "", hasToolbar, needAddEvent, gridHeight, gridWidth, false, editFlag, false, "");
            return MvcHtmlString.Create(str);
        }


        private static string GetColNames(Dictionary<string, GridInfo> gridLayout)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, GridInfo> dic in gridLayout)
            {
                sb.AppendFormat("'{0}',", dic.Value.Caption);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static string GetColModel(UrlHelper urlHelper, Dictionary<string, GridInfo> gridLayout)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, GridInfo> cdic in gridLayout)
            {

                sb.Append("{");
                sb.AppendFormat("name:'{0}',index:'{0}',", cdic.Value.Name);
                if (cdic.Value.Width != null && cdic.Value.Width != "")
                    sb.AppendFormat("width:{0},", cdic.Value.Width);
                if (cdic.Value.Align != null && cdic.Value.Align != "")
                    sb.AppendFormat("align:'{0}',", cdic.Value.Align);


                if (cdic.Value.Editable != null && cdic.Value.Editable != "")
                    sb.AppendFormat("editable:{0},", cdic.Value.Editable);


                if (cdic.Value.Edittype == "select")
                    sb.AppendLine("editrules:{},");

                if (cdic.Value.Formatter != null && cdic.Value.Formatter == "date")
                {
                    sb.AppendLine("formatter:'date',");
                    sb.AppendLine("formatoptions:{srcformat:'Y-m-d H:i:s',newformat:'Y-m-d'},");
                }
                if (cdic.Value.Edittype == "image")
                    sb.AppendLine("formatter:function(cellvalue, options, rowObject){ var imgsrc='" + urlHelper.Content("") + "'+cellvalue; var temp = '<img src='+imgsrc+' border=\"0\"/>'; return temp;},");


                if (cdic.Value.Datasourceurl != null && cdic.Value.Datasourceurl != "")
                {
                    string[] routes = cdic.Value.Datasourceurl.Split('/');
                    if (routes.Length == 2)
                    {
                        string control = routes[0];
                        string action = routes[1];
                        sb.AppendLine("editoptions:{ dataUrl:'" + urlHelper.Action(action, control, new { Area = "" }) + "', buildSelect: function (data) {var jsondata = eval(data); var ret = \"<select>\";ret += \"<option value=' '> </option>\"; $.each(jsondata, function (i, item) { ret += \"<option value='\" + item.Value + \"'>\" + item.Text + \"</option>\";}); ret += \"</select>\";return ret;}},");
                    }
                    else if (routes.Length == 3)
                    {
                        string area = routes[0];
                        string control = routes[1];
                        string action = routes[2];
                        sb.AppendLine("editoptions:{ dataUrl:'" + urlHelper.Action(action, control, new { Area = area }) + "', buildSelect: function (data) {var jsondata = eval(data); var ret = \"<select>\";ret += \"<option value=' '> </option>\"; $.each(jsondata, function (i, item) { ret += \"<option value='\" + item.Value + \"'>\" + item.Text + \"</option>\";}); ret += \"</select>\";return ret;}},");
                    }

                }

                if (cdic.Value.Edittype != null && cdic.Value.Edittype != "")
                    sb.AppendFormat("edittype:'{0}', ", cdic.Value.Edittype);

                if (cdic.Value.Hidden != null && cdic.Value.Hidden != "")
                    sb.AppendFormat("hidden:{0},", cdic.Value.Hidden);

                sb.Length--;
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static void SetGridProperty(string gridID, string gridTitle, string pager, StringBuilder sbGrid, int gridHeight, bool hasToolbar, bool editGrid = false, int gridWidth = 0, bool isMultiSelect = false)
        {
            sbGrid.AppendFormat(" rowNum: 20,");
            sbGrid.AppendLine("rownumbers: true,");
            //sbGrid.AppendFormat("rowList: [10, 50, 100],");
            sbGrid.AppendFormat("pager: '#{0}',", pager);
            if (editGrid && !isMultiSelect)
                sbGrid.AppendLine("sortable:false,");
            else
                sbGrid.AppendLine("sortable:true,");
            sbGrid.AppendLine("viewrecords: true,");
            sbGrid.AppendLine("sortorder: \"desc\",");
            sbGrid.AppendFormat("caption: '{0}',", gridTitle);
            sbGrid.AppendFormat("height: {0},", gridHeight);
            sbGrid.AppendLine("shrinkToFit: false,");

            if (isMultiSelect)
            {
                sbGrid.AppendLine("multiselect: true,");
                //sbGrid.AppendLine("multikey: 'ctrlKey',");
            }
            if (editGrid && !isMultiSelect)
            {
                //sbGrid.AppendLine("cellEdit:true,");
                //sbGrid.AppendLine("cellsubmit: 'clientArray',");
                sbGrid.AppendFormat("editurl: '{0}',", "clientArray");
                //sbGrid.AppendLine(" autowidth:false,");
            }
            if (gridWidth != 0)
                sbGrid.AppendFormat("width: {0},", gridWidth);
            else
                sbGrid.AppendLine(" autowidth:true,");
            if (hasToolbar)
                sbGrid.AppendLine("toolbar: [true,\"top\"],");
        }

        private static void gridResize(string gridID, StringBuilder sbGrid)
        {
            sbGrid.AppendLine("$(window).resize(function(){");
            sbGrid.AppendFormat("var width=$(\"#{0}\").closest('.ui-jqgrid').parent().outerWidth();", gridID);
            sbGrid.AppendFormat(@" jQuery(""#{0}"").setGridWidth(width); ", gridID);
            sbGrid.AppendFormat("var height=$(\"#{0}\").closest('.ui-jqgrid').parent().outerHeight()-10;", gridID);
            sbGrid.AppendLine();
            sbGrid.AppendFormat(@" jQuery(""#{0}"").setGridHeight(height);", gridID);
            sbGrid.AppendLine("});");
        }

        private static void SetGridEvent(string pageId, string formId, string gridId, StringBuilder sbGrid, string primaryKey, string dblUrlStr, string viewTitle)
        {
            if (primaryKey != "")
            {
                string str = dblUrlStr.Split('?')[1];
                string str2 = Regex.Split(str, "formMode=", RegexOptions.IgnoreCase)[1];
                string formMode = str2.Split('&')[0];
                sbGrid.AppendLine("ondblClickRow: function (rowid, iRow, iCol, e) {");
                sbGrid.AppendLine("var pk; ");
                sbGrid.AppendLine("if (rowid) { ");
                sbGrid.AppendFormat(" pk=jQuery(\"#{0}\").getCell(rowid,'{1}');", gridId, primaryKey);
                sbGrid.AppendLine("}");
                sbGrid.AppendLine("$.ajax({");
                sbGrid.AppendLine("type: 'Get',");
                sbGrid.AppendLine("dataType: 'html',");
                sbGrid.AppendFormat("url: '{0}',", dblUrlStr);
                sbGrid.AppendLine("data:{ pageId:'" + pageId + "',primaryKey: pk,viewTitle:'" + viewTitle + "'},");
                sbGrid.AppendLine(" complete: function (req, err) {");
                if (formMode != "new2" && formMode != "approve" && formMode != "reapply")
                {
                    sbGrid.AppendLine("var curtab=$(\"#tabs A[href='#t" + pageId + "']\");");
                    sbGrid.AppendLine(" curtab.text('" + viewTitle + "[" + AppMember.AppText[formMode].ToString() + "]')");
                }
                sbGrid.AppendLine("$('#t" + pageId + "', '#tabs').empty();");
                sbGrid.AppendLine("$('#t" + pageId + "', '#tabs').append(req.responseText);");
                sbGrid.AppendLine("}");
                sbGrid.AppendLine("});");
                sbGrid.AppendLine("},");
            }
            sbGrid.AppendLine("onPaging: function (pgButton) {");
            sbGrid.AppendLine("var obj = $('#" + formId + "').serializeObject()");
            sbGrid.AppendLine("var formvar = JSON.stringify(obj)");
            sbGrid.AppendLine("$('#" + gridId + "').jqGrid('setPostData', { formVar: formvar })");
            sbGrid.AppendLine("},");

            sbGrid.AppendFormat(@"onRightClickRow: function (rowid, iRow, iCol, e) {{
                                jQuery(""{0}"").jqGrid('setSelection', rowid);
                            }},", gridId);

        }

        private static void SetEditGridEvent(string pageId, string gridId, StringBuilder sbGrid, bool isMultiSelect, string editFlag, bool hasClickEvent, string clickFlag)
        {
            sbGrid.AppendFormat(@"onRightClickRow: function (rowid, iRow, iCol, e) {{
                                jQuery(""{0}"").jqGrid('setSelection', rowid);
                            }},", gridId);
            string clickEvent = "";
            if (hasClickEvent)
            {
                clickEvent = " onClickRow" + clickFlag + "(id); ";
            }
            if (isMultiSelect)
            {
                sbGrid.AppendFormat(@"onSelectRow: function(id){{
                                         if(id && id!=lastsel){{ 
                                            jQuery('#{0}').saveRow(lastsel); 
                                            lastsel=id; 
                                         }}
                                         jQuery('#{0}').editRow(id,true);
                                         {1} 
                                    }},", gridId, clickEvent);
            }
            else
            {
                sbGrid.AppendFormat(@"onSelectRow: function(id){{
                                         {0}
                                    }},", clickEvent);
                sbGrid.AppendFormat(@"gridComplete: function(){{
                                        var data = $('#{0}').getDataIDs();
                                        for(i=0;i<data.length;i++){{
                                            jQuery('#{0}').jqGrid('editRow',data[i],true,onCellEdit{1}); 
                                        }}
                                     }},", gridId, editFlag);
            }

        }


        private static void SetGridPage(string gridID, string pagerID, StringBuilder sbGrid)
        {
            sbGrid.AppendFormat(@"$('#{0}').jqGrid('navGrid','#{1}',
                                    {{edit: false, add: false, del: false,search :false,refresh:false}}
                                );", gridID, pagerID);
        }

        private static void SetEditGridMethod(string gridID, StringBuilder sbGrid, Dictionary<string, GridInfo> gridLayout, string primaryKey, string inputId, string submitBtn, bool hasToolbar, bool needAddEvent, bool isMultiSelect)
        {
            sbGrid.AppendLine();
            sbGrid.AppendLine("<script type=\"text/javascript\">");
            sbGrid.AppendLine("$(document).ready(function () {");
            if (!isMultiSelect && hasToolbar)
            {
                if (needAddEvent)
                {
                    sbGrid.AppendLine("$('#btnAdd" + gridID + "', '#t_" + gridID + "').click(function () {");
                    sbGrid.AppendLine("var dataRow = {");
                    foreach (KeyValuePair<string, GridInfo> dic in gridLayout)
                    {

                        sbGrid.AppendLine(dic.Value.Name + ":'',");
                    }
                    sbGrid.Remove(sbGrid.Length - 3, 3);
                    sbGrid.AppendLine(" };");
                    sbGrid.AppendLine(" var d =new Date();  ");
                    sbGrid.AppendLine("var s= '';");
                    sbGrid.AppendLine(" s += d.getHours().toString();  ");
                    sbGrid.AppendLine(" s += d.getMinutes().toString();  ");
                    sbGrid.AppendLine(" s += d.getSeconds().toString();  ");
                    sbGrid.AppendLine(" s += d.getMilliseconds().toString(); ");
                    sbGrid.AppendLine(" s += Math.floor(Math.random()*100).toString();");
                    sbGrid.AppendLine("$('#" + gridID + "').jqGrid('addRowData', s, dataRow);");
                    sbGrid.AppendLine("$('#" + gridID + "').jqGrid('editRow', s,true);");
                    sbGrid.AppendLine("  });");
                }

                sbGrid.AppendLine("$('#btnDelete" + gridID + "', '#t_" + gridID + "').click(function () {");
                sbGrid.AppendLine("var id = jQuery('#" + gridID + "').jqGrid('getGridParam','selrow');");
                sbGrid.AppendFormat(" var pk=jQuery(\"#{0}\").getCell(id,'{1}');", gridID, primaryKey);
                sbGrid.AppendLine("  var oldval=$('#" + gridID + AppMember.DeletePK + "').val();");
                sbGrid.AppendLine("$('#" + gridID + AppMember.DeletePK + "').attr('value',oldval+','+pk);");
                sbGrid.AppendLine("$('#" + gridID + "').jqGrid('delRowData', id);");
                sbGrid.AppendLine("  });");
            }
            string ss = sbGrid.ToString();
            //sbGrid.AppendLine(" $('#" + btnSubmit + "').click(function () {");
            sbGrid.AppendLine("$('#" + submitBtn + "').mouseup(function() {");
            sbGrid.AppendFormat(@"var data = $('#{0}').getDataIDs();
                                                    for(i=0;i<data.length;i++){{
                                                        $('#{0}').jqGrid('saveRow',data[i]); 
                                                    }}
                                ", gridID);
            //sbGrid.AppendLine("});");
            sbGrid.AppendLine(" var griddata = '';");
            if (!isMultiSelect)
                sbGrid.AppendLine(" griddata = jQuery('#" + gridID + "').jqGrid('getRowData');");
            else
            {
                sbGrid.AppendLine(" var  arrRow = jQuery('#" + gridID + "').jqGrid('getGridParam','selarrrow');");
                sbGrid.AppendFormat(@"if(arrRow.length > 0)
                                        {{
                                            griddata = new Array();
                                            var rowdata
                                            var j=0;
                                            for(var i=0;i<arrRow.length;i++)
                                            {{
                                                if(arrRow[i]!=undefined){{
                                                    rowdata=jQuery('#{0}').getRowData(arrRow[i]);
                                                    griddata[j] = rowdata;
                                                    j++;             
                                                }}
                                            }}
                                        }}", gridID);
            }
            sbGrid.AppendFormat("$(\"#{0}\").attr(\"value\",JSON.stringify(griddata));", inputId);
            //sbGrid.AppendLine(" var griddata1 =JSON.stringify(griddata);");
            //sbGrid.AppendLine(" alert(griddata1);");
            sbGrid.AppendLine(" });");
            sbGrid.AppendLine("   });");
            sbGrid.AppendLine("</script>");

        }

        private static void GenerateGridHtml(string gridID, string pagerID, string inputName, StringBuilder sbGrid, bool isEditGrid = false)
        {

            sbGrid.AppendFormat("<table id=\"{0}\"></table>", gridID);
            sbGrid.AppendFormat("<div id=\"{0}\"></div>", pagerID);
            sbGrid.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{1}\" ></input>", gridID + AppMember.HideString, inputName);
            sbGrid.AppendLine();
            if (isEditGrid)
            {
                sbGrid.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" ></input>", gridID + AppMember.DeletePK);
                sbGrid.AppendLine("<script type=\"text/javascript\">");
                sbGrid.AppendLine("$(document).ready(function () {");
                sbGrid.AppendLine("$('#t_" + gridID + "').append(\"<input id='btnAdd" + gridID + "' type='button' value='" + AppMember.AppText["BtnAdd"] + "' style='height:20px;font-size:-1;line-height: 0.8;'/>\");");
                sbGrid.AppendLine("$('#t_" + gridID + "').append(\"<input id='btnDelete" + gridID + "' type='button' value='" + AppMember.AppText["BtnDelete"] + "' style='height:20px;font-size:-1;line-height: 0.8;'/>\");");

                sbGrid.AppendLine("$('#btnAdd" + gridID + "', '#t_" + gridID + "').button({");
                sbGrid.AppendLine("text: true");
                sbGrid.AppendLine("});");

                sbGrid.AppendLine("$('#btnDelete" + gridID + "', '#t_" + gridID + "').button({");
                sbGrid.AppendLine("text: true");
                sbGrid.AppendLine("});");

                sbGrid.AppendLine("});");
                sbGrid.AppendLine("</script>");
            }
        }

    }
}