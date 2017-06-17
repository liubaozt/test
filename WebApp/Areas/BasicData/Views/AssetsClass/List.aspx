<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AssetsClass.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsClassListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsClassNo, Model.PageId, "AssetsClassList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsClassNo, Model.PageId, "AssetsClassList")%>
        </div>
        <div class="AssetsClassListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsClassName, Model.PageId, "AssetsClassList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsClassName, Model.PageId, "AssetsClassList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
