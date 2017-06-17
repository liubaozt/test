<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.EquityOwner.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="EquityOwnerListColumn1">
            <%:Html.AppLabelFor(m => m.EquityOwnerNo, Model.PageId, "EquityOwnerList")%>
            <%:Html.AppTextBoxFor(m => m.EquityOwnerNo, Model.PageId, "EquityOwnerList")%>
        </div>
        <div class="EquityOwnerListColumn2">
            <%:Html.AppLabelFor(m => m.EquityOwnerName, Model.PageId, "EquityOwnerList")%>
            <%:Html.AppTextBoxFor(m => m.EquityOwnerName, Model.PageId, "EquityOwnerList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
