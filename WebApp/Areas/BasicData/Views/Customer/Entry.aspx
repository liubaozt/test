<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.Customer.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.CustomerNo, Model.PageId, "CustomerEntry")%>
        <%:Html.AppTextBoxFor(m => m.CustomerNo, Model.PageId, "CustomerEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.CustomerNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.CustomerName, Model.PageId, "CustomerEntry")%>
        <%:Html.AppTextBoxFor(m => m.CustomerName, Model.PageId, "CustomerEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.CustomerName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Tel, Model.PageId, "CustomerEntry")%>
        <%:Html.AppTextBoxFor(m => m.Tel, Model.PageId, "CustomerEntry")%>
        <%:Html.ValidationMessageFor(m => m.Tel)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Email, Model.PageId, "CustomerEntry")%>
        <%:Html.AppTextBoxFor(m => m.Email, Model.PageId, "CustomerEntry")%>
        <%:Html.ValidationMessageFor(m => m.Email)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Address, Model.PageId, "CustomerEntry")%>
        <%:Html.AppTextBoxFor(m => m.Address, Model.PageId, "CustomerEntry")%>
        <%:Html.ValidationMessageFor(m => m.Address)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Contacts, Model.PageId, "CustomerEntry")%>
        <%:Html.AppTextBoxFor(m => m.Contacts, Model.PageId, "CustomerEntry")%>
        <%:Html.ValidationMessageFor(m => m.Contacts)%>
    </div>
    <%:Html.AppHiddenFor(m => m.CustomerId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
  
</asp:Content>
