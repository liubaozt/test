<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Base.Master" Inherits="System.Web.Mvc.ViewPage<BaseCommon.Models.QueryEntryViewModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%:Html.AppNormalButton(Model.PageId, "btnPrint", AppMember.AppText["BtnPrint"])%>
    <div id="ReportDiv">
        <%:Html.AppReportPrintFor(Model.EntryGridData, Model.EntryGridLayout) %>
    </div>
    <%:Html.AppHiddenFor(m => m.PageId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.FormId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.Message, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.HasError, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.ViewTitle,Model.PageId) %>
    <%:Html.AppHiddenFor(m => m.ExportUrl, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.FormVar, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <title>报表打印</title>
    <%=TempData["CssBlock"]%>
    <%=TempData["ScriptBlock"]%>
    <style type="text/css">
        BODY
        {
            margin: 1pt,1pt;
            padding: 1pt,1pt;
        }
        .#page1 DIV
        {
            position: absolute;
        }
    </style>
     <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/jquery.jqprint.js") %>'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var formId = '<%=Model.FormId %>' + pageId;
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            //#endregion 公共变量

            //设置网页打印的页眉页脚为空   
            function pagesetup_null() {
                var hkey_root, hkey_path, hkey_key;
                hkey_root = "HKEY_CURRENT_USER";
                hkey_path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";
                try {
                    var RegWsh = new ActiveXObject("WScript.Shell");
                    hkey_key = "header";
                    RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
                    hkey_key = "footer";
                    RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
                } catch (e) { }
            }
            //#endregion 用户操作

            $("#btnPrint" + pageId).click(function () {
                pagesetup_null();
                $("#ReportDiv").jqprint();
            });


        });
    </script>
</asp:Content>
