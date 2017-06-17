<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AssetsType.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsTypeListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsTypeNo, Model.PageId, "AssetsTypeList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsTypeNo, Model.PageId, "AssetsTypeList")%>
        </div>
        <div class="AssetsTypeListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsTypeName, Model.PageId, "AssetsTypeList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsTypeName, Model.PageId, "AssetsTypeList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
