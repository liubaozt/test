<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsMaintain.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsMaintainListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsMaintainNo, Model.PageId, "AssetsMaintainList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMaintainNo, Model.PageId, "AssetsMaintainList")%>
        </div>
        <div class="AssetsMaintainListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsMaintainName, Model.PageId, "AssetsMaintainList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMaintainName, Model.PageId, "AssetsMaintainList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
