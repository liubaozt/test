<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.ScrapType.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="ScrapTypeListColumn1">
            <%:Html.AppLabelFor(m => m.ScrapTypeNo, Model.PageId, "ScrapTypeList")%>
            <%:Html.AppTextBoxFor(m => m.ScrapTypeNo, Model.PageId, "ScrapTypeList")%>
        </div>
        <div class="ScrapTypeListColumn2">
            <%:Html.AppLabelFor(m => m.ScrapTypeName, Model.PageId, "ScrapTypeList")%>
            <%:Html.AppTextBoxFor(m => m.ScrapTypeName, Model.PageId, "ScrapTypeList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
