<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.StoreSite.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.StoreSiteNo, Model.PageId, "StoreSiteEntry")%>
        <%:Html.AppTextBoxFor(m => m.StoreSiteNo, Model.PageId, "StoreSiteEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.StoreSiteNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.StoreSiteName, Model.PageId, "StoreSiteEntry")%>
        <%:Html.AppTextBoxFor(m => m.StoreSiteName, Model.PageId, "StoreSiteEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.StoreSiteName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.CompanyId, Model.PageId, "StoreSiteEntry")%>
        <%:Html.AppDropDownListFor(m => m.CompanyId, Model.PageId, Url.Action("DropList", "Company", new { Area = "BusinessCommon" }), "StoreSiteEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.StoreSiteName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ParentId, Model.PageId, "StoreSiteEntry")%>
        <%:Html.AppTreeDialogFor(m => m.ParentId, Model.PageId, Model.ParentUrl, Model.DialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.AddFavoritUrl,Model.ReplaceFavoritUrl,"StoreSiteEntry")%>
        <%:Html.ValidationMessageFor(m => m.ParentId)%>
    </div>
    <%:Html.AppHiddenFor(m => m.StoreSiteId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
