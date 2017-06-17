<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.User.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="UserListColumn1">
            <%:Html.AppLabelFor(m => m.UserNo, Model.PageId, "UserList")%>
            <%:Html.AppTextBoxFor(m => m.UserNo, Model.PageId, "UserList")%>
        </div>
        <div class="UserListColumn2">
            <%:Html.AppLabelFor(m => m.UserName, Model.PageId, "UserList")%>
            <%:Html.AppTextBoxFor(m => m.UserName, Model.PageId, "UserList")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="UserListColumn1">
            <%:Html.AppLabelFor(m => m.GroupId, Model.PageId, "UserList")%>
            <%:Html.AppDropDownListFor(m => m.GroupId, Model.PageId, Url.Action("DropList", "Group", new { Area = "BusinessCommon" }), "UserList")%>
        </div>
        <div class="UserListColumn2">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "UserList")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "UserList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
