<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.DepreciationRule.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="DepreciationRuleListColumn1">
            <%:Html.AppLabelFor(m => m.DepreciationRuleNo, Model.PageId, "DepreciationRuleList")%>
            <%:Html.AppTextBoxFor(m => m.DepreciationRuleNo, Model.PageId, "DepreciationRuleList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
