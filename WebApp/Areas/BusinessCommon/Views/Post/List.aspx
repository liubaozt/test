<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Post.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="PostListColumn1">
            <%:Html.AppLabelFor(m => m.PostNo, Model.PageId, "PostList")%>
            <%:Html.AppTextBoxFor(m => m.PostNo, Model.PageId, "PostList")%>
        </div>
        <div class="PostListColumn2">
            <%:Html.AppLabelFor(m => m.PostName, Model.PageId, "PostList")%>
            <%:Html.AppTextBoxFor(m => m.PostName, Model.PageId, "PostList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
