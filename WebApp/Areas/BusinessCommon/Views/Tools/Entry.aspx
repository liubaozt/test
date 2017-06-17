<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Base.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Tools.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <%-- <fieldset  style="padding:6px; margin:6px">--%>
        <div class="editor-field">
            <%:Html.AppNormalButton(Model.PageId, "btnCssMerge", AppMember.AppText["BtnCssMerge"])%>
        </div>
        <div class="editor-field">
        <%:Html.AppNormalButton(Model.PageId, "btnDataDownload", AppMember.AppText["BtnDataDownload"])%>
        </div>
   <%-- </fieldset>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            //#endregion 公共变量

            //#region 画面操作
            $('#btnCssMerge' + pageId).click(function () {
                $.ajax({
                    type: "POST",
                    url: '<%:Model.CssMergeUrl%>',
                    datatype: "text",
                    success: function (data) {
                        if (data == "1")
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["CssMergeSucceed"]%>', 'success', function () { });
                        else
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["CssMergeFailed"]%>', 'error', function () { });
                    }
                });
            });

            $('#btnDataDownload' + pageId).click(function () {
                var url = '<%=Url.Action("DownloadPrintTools", "Tools", new { Area = "BusinessCommon" }) %>';
                location.href = url;
            });

            //#endregion grid操作
        });
    </script>
</asp:Content>
