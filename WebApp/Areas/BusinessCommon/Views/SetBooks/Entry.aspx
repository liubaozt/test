<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.SetBooks.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.SetBooksNo, Model.PageId, "SetBooksEntry")%>
        <%:Html.AppTextBoxFor(m=>m.SetBooksNo,Model.PageId,"SetBooksEntry") %>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.SetBooksNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.SetBooksName, Model.PageId, "SetBooksEntry")%>
        <%:Html.AppTextBoxFor(m => m.SetBooksName, Model.PageId, "SetBooksEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.SetBooksName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Remark, Model.PageId, "SetBooksEntry")%>
        <%:Html.AppTextBoxFor(m => m.Remark, Model.PageId, "SetBooksEntry")%>
        <%:Html.ValidationMessageFor(m => m.Remark)%>
    </div>
    <%:Html.AppHiddenFor(m => m.IsFixed, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.SetBooksId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>
