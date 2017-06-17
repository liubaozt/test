<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsReturn.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsReturnListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsReturnNo, Model.PageId, "AssetsReturnList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsReturnNo, Model.PageId, "AssetsReturnList")%>
        </div>
        <div class="AssetsReturnListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsReturnName, Model.PageId, "AssetsReturnList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsReturnName, Model.PageId, "AssetsReturnList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
