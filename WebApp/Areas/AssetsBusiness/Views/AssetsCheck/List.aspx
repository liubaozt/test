<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsCheck.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsCheckListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsCheckNo, Model.PageId, "AssetsCheckList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsCheckNo, Model.PageId, "AssetsCheckList")%>
        </div>
        <div class="AssetsCheckListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsCheckName, Model.PageId, "AssetsCheckList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsCheckName, Model.PageId, "AssetsCheckList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
