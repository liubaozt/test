<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AssetsUses.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AssetsUsesNo, Model.PageId, "AssetsUsesEntry")%>
        <%:Html.AppTextBoxFor(m=>m.AssetsUsesNo,Model.PageId, "AssetsUsesEntry") %>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.AssetsUsesNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AssetsUsesName, Model.PageId, "AssetsUsesEntry")%>
        <%:Html.AppTextBoxFor(m => m.AssetsUsesName, Model.PageId, "AssetsUsesEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.AssetsUsesName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.AssetsUsesId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
