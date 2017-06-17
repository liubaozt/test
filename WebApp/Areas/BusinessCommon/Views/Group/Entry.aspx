<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Group.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.GroupNo, Model.PageId, "GroupEntry")%>
        <%:Html.AppTextBoxFor(m=>m.GroupNo,Model.PageId,"GroupEntry") %>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.GroupNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.GroupName, Model.PageId, "GroupEntry")%>
        <%:Html.AppTextBoxFor(m => m.GroupName, Model.PageId, "GroupEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.GroupName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Remark, Model.PageId, "GroupEntry")%>
        <%:Html.AppTextBoxFor(m => m.Remark, Model.PageId, "GroupEntry")%>
        <%:Html.ValidationMessageFor(m => m.Remark)%>
    </div>
      <%:Html.AppHiddenFor(m => m.IsFixed, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.GroupId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>
