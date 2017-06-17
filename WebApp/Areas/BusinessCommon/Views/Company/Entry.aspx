<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Company.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepartmentNo, Model.PageId, "CompanyEntry")%>
        <%:Html.AppTextBoxFor(m => m.DepartmentNo, Model.PageId, "CompanyEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.DepartmentNo)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepartmentName, Model.PageId, "CompanyEntry")%>
        <%:Html.AppTextBoxFor(m => m.DepartmentName, Model.PageId, "CompanyEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.DepartmentName) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.IsHeaderOffice, Model.PageId, "CompanyEntry")%>
        <%:Html.AppCheckBoxFor(m => m.IsHeaderOffice, Model.PageId, "CompanyEntry")%>
        <%:Html.ValidationMessageFor(m => m.IsHeaderOffice)%>
    </div>
    <%:Html.AppHiddenFor(m => m.DepartmentId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
  
</asp:Content>
