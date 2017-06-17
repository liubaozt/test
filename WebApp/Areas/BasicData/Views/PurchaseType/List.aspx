<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.PurchaseType.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="PurchaseTypeListColumn1">
            <%:Html.AppLabelFor(m => m.PurchaseTypeNo, Model.PageId, "PurchaseTypeList")%>
            <%:Html.AppTextBoxFor(m => m.PurchaseTypeNo, Model.PageId, "PurchaseTypeList")%>
        </div>
        <div class="PurchaseTypeListColumn2">
            <%:Html.AppLabelFor(m => m.PurchaseTypeName, Model.PageId, "PurchaseTypeList")%>
            <%:Html.AppTextBoxFor(m => m.PurchaseTypeName, Model.PageId, "PurchaseTypeList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
