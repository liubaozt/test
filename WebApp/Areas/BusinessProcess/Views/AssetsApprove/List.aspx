<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<WebApp.Areas.BusinessProcess.Models.AssetsApprove.ListModel>" %>

<%@ Import Namespace="WebApp.BaseWeb.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset style="height: 22px">
        <div class="editor-field">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId)%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId)%>
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId)%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId)%>
        </div>
    </fieldset>
    <%:Html.AppGridFor(this.Url, Model.PageId, Model.FormId, Model.GridId,Model.ApproveListTitle,
                                                    Url.Action("GridData", "AssetsApprove", new { Area = "BusinessProcess", approveMode = Model.ApproveMode }), Model == null ? null : Model.GridLayout, "assetsId", 
    Url.Action("Entry"), Model.ViewTitle, 
    DataConvert.ToInt32(Request.Cookies["HeighCookie"].Value)-22)%>
    <%:Html.AppGridButtonSet(Model.AuthorityGridButton, Model.FormId, "assetsId", Model.ViewTitle)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

     <script type="text/javascript">
         $(document).ready(function () {
             var pageId = '<%=Model.PageId %>';
             var mainContent = "#MainContent" + pageId;
             $(mainContent + " label").css({ "width": "80px", "float": "left", "text-align": "left" });
             $(mainContent + " .inputtext").css({ "width": "120px", "float": "left", "text-align": "left" });
             $(mainContent + " .inputselect").css({ "width": "125px", "float": "left", "text-align": "left" });
             $(mainContent + " .inputdialog").css({ "width": "99px", "float": "left", "text-align": "left" });
         });
    </script>
</asp:Content>