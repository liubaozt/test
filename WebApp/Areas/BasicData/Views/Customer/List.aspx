<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.Customer.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="CustomerListColumn1">
            <%:Html.AppLabelFor(m => m.CustomerNo, Model.PageId, "CustomerList")%>
            <%:Html.AppTextBoxFor(m => m.CustomerNo, Model.PageId, "CustomerList")%>
        </div>
        <div class="CustomerListColumn2">
            <%:Html.AppLabelFor(m => m.CustomerName, Model.PageId, "CustomerList")%>
            <%:Html.AppTextBoxFor(m => m.CustomerName, Model.PageId, "CustomerList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
