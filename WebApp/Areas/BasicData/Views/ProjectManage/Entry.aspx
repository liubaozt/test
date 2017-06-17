<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.ProjectManage.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ProjectManageNo, Model.PageId, "ProjectManageEntry")%>
        <%:Html.AppTextBoxFor(m => m.ProjectManageNo, Model.PageId, "ProjectManageEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.ProjectManageNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ProjectManageName, Model.PageId, "ProjectManageEntry")%>
        <%:Html.AppTextBoxFor(m => m.ProjectManageName, Model.PageId, "ProjectManageEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.ProjectManageName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.ProjectManageId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
     
</asp:Content>
