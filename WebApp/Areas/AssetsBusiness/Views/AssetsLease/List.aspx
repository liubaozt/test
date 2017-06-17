<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsLease.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsLeaseListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsLeaseNo, Model.PageId, "AssetsLeaseList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsLeaseNo, Model.PageId, "AssetsLeaseList")%>
        </div>
        <div class="AssetsLeaseListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsLeaseName, Model.PageId, "AssetsLeaseList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsLeaseName, Model.PageId, "AssetsLeaseList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
