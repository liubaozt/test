<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsMerge.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsMergeListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsMergeNo, Model.PageId, "AssetsMergeList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMergeNo, Model.PageId, "AssetsMergeList")%>
        </div>
        <div class="AssetsMergeListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsMergeName, Model.PageId, "AssetsMergeList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMergeName, Model.PageId, "AssetsMergeList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
