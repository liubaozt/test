<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.ScrapType.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ScrapTypeNo, Model.PageId, "ScrapTypeEntry")%>
        <%:Html.AppTextBoxFor(m => m.ScrapTypeNo, Model.PageId, "ScrapTypeEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.ScrapTypeNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ScrapTypeName, Model.PageId, "ScrapTypeEntry")%>
        <%:Html.AppTextBoxFor(m => m.ScrapTypeName, Model.PageId, "ScrapTypeEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.ScrapTypeName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.ScrapTypeId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   
</asp:Content>
