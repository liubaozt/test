<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsSplit.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsSplitListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsSplitNo, Model.PageId, "AssetsSplitList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsSplitNo, Model.PageId, "AssetsSplitList")%>
        </div>
        <div class="AssetsSplitListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsSplitName, Model.PageId, "AssetsSplitList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsSplitName, Model.PageId, "AssetsSplitList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
