<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsInsure.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsInsureListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsInsureNo, Model.PageId, "AssetsInsureEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsInsureNo, Model.PageId, "AssetsInsureEntry")%>
        </div>
        <div class="AssetsInsureListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsInsureName, Model.PageId, "AssetsInsureEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsInsureName, Model.PageId, "AssetsInsureEntry")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
