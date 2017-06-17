<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.PurchaseType.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.PurchaseTypeNo, Model.PageId, "PurchaseTypeEntry")%>
        <%:Html.AppTextBoxFor(m => m.PurchaseTypeNo, Model.PageId, "PurchaseTypeEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.PurchaseTypeNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.PurchaseTypeName, Model.PageId, "PurchaseTypeEntry")%>
        <%:Html.AppTextBoxFor(m => m.PurchaseTypeName, Model.PageId, "PurchaseTypeEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.PurchaseTypeName)%>
    </div>
     <%:Html.AppHiddenFor(m => m.IsFixed, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.PurchaseTypeId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
     
</asp:Content>
