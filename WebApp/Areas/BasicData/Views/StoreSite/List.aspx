<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.StoreSite.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="StoreSiteListColumn1">
            <%:Html.AppLabelFor(m => m.StoreSiteNo, Model.PageId, "StoreSiteList")%>
            <%:Html.AppTextBoxFor(m => m.StoreSiteNo, Model.PageId, "StoreSiteList")%>
        </div>
        <div class="StoreSiteListColumn2">
            <%:Html.AppLabelFor(m => m.StoreSiteName, Model.PageId, "StoreSiteList")%>
            <%:Html.AppTextBoxFor(m => m.StoreSiteName, Model.PageId, "StoreSiteList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
