<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.SetBooks.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="SetBooksListColumn1">
            <%:Html.AppLabelFor(m => m.SetBooksNo, Model.PageId, "SetBooksList")%>
            <%:Html.AppTextBoxFor(m => m.SetBooksNo, Model.PageId,"SetBooksList")%>
        </div>
        <div class="SetBooksListColumn2">
            <%:Html.AppLabelFor(m => m.SetBooksName, Model.PageId, "SetBooksList")%>
            <%:Html.AppTextBoxFor(m => m.SetBooksName, Model.PageId,"SetBooksList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
