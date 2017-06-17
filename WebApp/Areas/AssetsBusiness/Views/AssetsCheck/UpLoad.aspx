<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsCheck.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%:Html.AppFileUpload(Model.PageId, "SqliteUpload", AppMember.AppText["FileView"].ToString(), Url.Action("CheckExisting", "Home", new { Area = "" }) + "/sqlite",
                                                        Url.Action("SqliteUpload") + "/sqlite", Url.Content("~/Content/css/uploadify/"), "true")%>
    <%:Html.AppHiddenFor(m => m.UpLoadFileName,Model.PageId) %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var pageId = '<%=Model.PageId %>';
            AppUploadCompleteSqliteUpload = function (file, data, response) {
                $('#UpLoadFileName' + pageId).val(file.name);
                $('#' + 'btnSave' + pageId).attr('disabled', false);
                //AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["ImportSucceed"]%>', 'success', function () { });
            }
        });
    </script>
</asp:Content>
