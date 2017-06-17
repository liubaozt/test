<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.DepreciationRule.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepreciationRuleNo, Model.PageId, "DepreciationRuleEntry")%>
        <%:Html.AppTextBoxFor(m => m.DepreciationRuleNo, Model.PageId, "DepreciationRuleEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.DepreciationRuleNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.TotalMonth, Model.PageId, "DepreciationRuleEntry")%>
        <%:Html.AppTextBoxFor(m => m.TotalMonth, Model.PageId, "DepreciationRuleEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.TotalMonth)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.RemainRate, Model.PageId, "DepreciationRuleEntry")%>
        <%:Html.AppTextBoxFor(m => m.RemainRate, Model.PageId, "DepreciationRuleEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.RemainRate)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepreciationType, Model.PageId, "DepreciationRuleEntry")%>
        <%:Html.AppDropDownListFor(m => m.DepreciationType, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "DepreciationType" }), "DepreciationRuleEntry")%>
        <%:Html.ValidationMessageFor(m => m.DepreciationType)%>
    </div>
    <%:Html.AppHiddenFor(m => m.DepreciationRuleId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
