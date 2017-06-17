<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsBorrow.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsBorrowListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsBorrowNo, Model.PageId, "AssetsBorrowList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsBorrowNo, Model.PageId, "AssetsBorrowList")%>
        </div>
        <div class="AssetsBorrowListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsBorrowName, Model.PageId, "AssetsBorrowList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsBorrowName, Model.PageId, "AssetsBorrowList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
