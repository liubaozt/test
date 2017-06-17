<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.ProjectManage.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="ProjectManageListColumn1">
            <%:Html.AppLabelFor(m => m.ProjectManageNo, Model.PageId, "ProjectManageList")%>
            <%:Html.AppTextBoxFor(m => m.ProjectManageNo, Model.PageId, "ProjectManageList")%>
        </div>
        <div class="ProjectManageListColumn2">
            <%:Html.AppLabelFor(m => m.ProjectManageName, Model.PageId, "ProjectManageList")%>
            <%:Html.AppTextBoxFor(m => m.ProjectManageName, Model.PageId, "ProjectManageList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
