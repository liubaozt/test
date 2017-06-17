<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsSell.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsSellListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsSellNo, Model.PageId, "AssetsSellList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsSellNo, Model.PageId, "AssetsSellList")%>
        </div>
        <div class="AssetsSellListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsSellName, Model.PageId, "AssetsSellList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsSellName, Model.PageId, "AssetsSellList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
