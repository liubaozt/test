<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AssetsType.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AssetsTypeNo, Model.PageId, "AssetsTypeEntry")%>
        <%:Html.AppTextBoxFor(m => m.AssetsTypeNo, Model.PageId, "AssetsTypeEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.AssetsTypeNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AssetsTypeName, Model.PageId, "AssetsTypeEntry")%>
        <%:Html.AppTextBoxFor(m => m.AssetsTypeName, Model.PageId, "AssetsTypeEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.AssetsTypeName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.AssetsTypeId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>

