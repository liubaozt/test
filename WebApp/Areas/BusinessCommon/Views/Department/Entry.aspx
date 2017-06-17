<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Department.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepartmentNo, Model.PageId, "DepartmentEntry")%>
        <%:Html.AppTextBoxFor(m => m.DepartmentNo, Model.PageId, "DepartmentEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.DepartmentNo)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepartmentName, Model.PageId, "DepartmentEntry")%>
        <%:Html.AppTextBoxFor(m => m.DepartmentName, Model.PageId, "DepartmentEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.DepartmentName) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ParentId, Model.PageId, "DepartmentEntry")%>
        <%:Html.AppTreeDialogFor(m => m.ParentId, Model.PageId, Model.ParentUrl, Model.DialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId,Model.AddFavoritUrl,Model.ReplaceFavoritUrl, "DepartmentEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.ParentId)%>
    </div>
    <%:Html.AppHiddenFor(m => m.DepartmentId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
