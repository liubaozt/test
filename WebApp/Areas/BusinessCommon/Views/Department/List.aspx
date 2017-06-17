<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Department.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
      <div class="DepartmentListColumn1">
            <%:Html.AppLabelFor(m => m.DepartmentNo, Model.PageId, "DepartmentList")%>
            <%:Html.AppTextBoxFor(m => m.DepartmentNo, Model.PageId, "DepartmentList")%>
        </div>
        <div class="DepartmentListColumn2">
            <%:Html.AppLabelFor(m => m.DepartmentName, Model.PageId, "DepartmentList")%>
            <%:Html.AppTextBoxFor(m => m.DepartmentName, Model.PageId, "DepartmentList")%>
        </div>
        <div class="DepartmentListColumn3">
            <%:Html.AppLabelFor(m => m.ParentId, Model.PageId, "DepartmentList")%>
          <%:Html.AppTreeDialogFor(m => m.ParentId, Model.PageId, Model.ParentUrl, Model.DialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.AddFavoritUrl, Model.ReplaceFavoritUrl, "DepartmentList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
