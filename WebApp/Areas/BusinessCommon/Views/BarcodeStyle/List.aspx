<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.BarcodeStyle.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="BarcodeStyleListColumn1">
            <%:Html.AppLabelFor(m => m.BarcodeStyleName, Model.PageId, "BarcodeStyleList")%>
            <%:Html.AppTextBoxFor(m => m.BarcodeStyleName, Model.PageId, "BarcodeStyleList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
