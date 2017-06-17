<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Post.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.PostNo, Model.PageId, "PostEntry")%>
        <%:Html.AppTextBoxFor(m => m.PostNo, Model.PageId, "PostEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.PostNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.PostName, Model.PageId, "PostEntry")%>
        <%:Html.AppTextBoxFor(m => m.PostName, Model.PageId, "PostEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.PostName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.PostId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
