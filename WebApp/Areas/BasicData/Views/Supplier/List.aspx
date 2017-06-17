<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.Supplier.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="SupplierListColumn1">
            <%:Html.AppLabelFor(m => m.SupplierNo, Model.PageId, "SupplierList")%>
            <%:Html.AppTextBoxFor(m => m.SupplierNo, Model.PageId, "SupplierList")%>
        </div>
        <div class="SupplierListColumn2">
            <%:Html.AppLabelFor(m => m.SupplierName, Model.PageId, "SupplierList")%>
            <%:Html.AppTextBoxFor(m => m.SupplierName, Model.PageId, "SupplierList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
