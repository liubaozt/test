<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.Supplier.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.SupplierNo, Model.PageId, "SupplierEntry")%>
        <%:Html.AppTextBoxFor(m => m.SupplierNo, Model.PageId, "SupplierEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.SupplierNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.SupplierName, Model.PageId, "SupplierEntry")%>
        <%:Html.AppTextBoxFor(m => m.SupplierName, Model.PageId, "SupplierEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.SupplierName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Tel, Model.PageId, "SupplierEntry")%>
        <%:Html.AppTextBoxFor(m => m.Tel, Model.PageId, "SupplierEntry")%>
        <%:Html.ValidationMessageFor(m => m.Tel)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Email, Model.PageId, "SupplierEntry")%>
        <%:Html.AppTextBoxFor(m => m.Email, Model.PageId, "SupplierEntry")%>
        <%:Html.ValidationMessageFor(m => m.Email)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Address, Model.PageId, "SupplierEntry")%>
        <%:Html.AppTextBoxFor(m => m.Address, Model.PageId, "SupplierEntry")%>
        <%:Html.ValidationMessageFor(m => m.Address)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Contacts, Model.PageId, "SupplierEntry")%>
        <%:Html.AppTextBoxFor(m => m.Contacts, Model.PageId, "SupplierEntry")%>
        <%:Html.ValidationMessageFor(m => m.Contacts)%>
    </div>
    <%:Html.AppHiddenFor(m => m.SupplierId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
