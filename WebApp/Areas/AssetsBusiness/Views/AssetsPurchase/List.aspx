<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsPurchase.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsPurchaseListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsPurchaseNo, Model.PageId, "AssetsPurchaseList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsPurchaseNo, Model.PageId, "AssetsPurchaseList")%>
        </div>
        <div class="AssetsPurchaseListColumn2">
            <%:Html.AppLabelFor(m => m.PurchaseDate1, Model.PageId, "AssetsPurchaseList")%>
            <%:Html.AppDatePickerFor(m => m.PurchaseDate1, Model.PageId, "AssetsPurchaseList")%>
        </div>
        <div class="AssetsPurchaseListColumn3">
            <%:Html.AppLabelFor(m => m.PurchaseDate2, Model.PageId, "AssetsPurchaseList")%>
            <%:Html.AppDatePickerFor(m => m.PurchaseDate2, Model.PageId, "AssetsPurchaseList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
