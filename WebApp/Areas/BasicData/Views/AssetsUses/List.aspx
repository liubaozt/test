<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AssetsUses.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsUsesListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsUsesNo, Model.PageId, "AssetsUsesList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsUsesNo, Model.PageId, "AssetsUsesList")%>
        </div>
        <div class="AssetsUsesListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsUsesName, Model.PageId, "AssetsUsesList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsUsesName, Model.PageId, "AssetsUsesList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
