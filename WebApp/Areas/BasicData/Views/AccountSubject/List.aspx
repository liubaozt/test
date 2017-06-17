<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AccountSubject.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AccountSubjectListColumn1">
            <%:Html.AppLabelFor(m => m.AccountSubjectNo, Model.PageId, "AccountSubjectList")%>
            <%:Html.AppTextBoxFor(m => m.AccountSubjectNo, Model.PageId, "AccountSubjectList")%>
        </div>
        <div class="AccountSubjectListColumn2">
            <%:Html.AppLabelFor(m => m.AccountSubjectName, Model.PageId, "AccountSubjectList")%>
            <%:Html.AppTextBoxFor(m => m.AccountSubjectName, Model.PageId, "AccountSubjectList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
