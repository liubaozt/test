<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AccountSubject.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AccountSubjectNo, Model.PageId, "AccountSubjectEntry")%>
        <%:Html.AppTextBoxFor(m => m.AccountSubjectNo, Model.PageId, "AccountSubjectEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.AccountSubjectNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AccountSubjectName, Model.PageId, "AccountSubjectEntry")%>
        <%:Html.AppTextBoxFor(m => m.AccountSubjectName, Model.PageId, "AccountSubjectEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.AccountSubjectName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ParentId, Model.PageId, "AccountSubjectEntry")%>
        <%:Html.AppTreeDialogFor(m => m.ParentId, Model.PageId, Model.ParentUrl, Model.DialogUrl, AppMember.AppText["AccountSubjectSelect"], "tree",Model.AddFavoritUrl,Model.ReplaceFavoritUrl, "AccountSubjectEntry")%>
        <%:Html.ValidationMessageFor(m => m.ParentId)%>
    </div>
    
    <%:Html.AppHiddenFor(m => m.AccountSubjectId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
