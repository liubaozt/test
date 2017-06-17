<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.FiscalPeriod.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="FiscalPeriodListColumn1">
            <%:Html.AppLabelFor(m => m.FiscalPeriodName, Model.PageId, "FiscalPeriodList")%>
            <%:Html.AppTextBoxFor(m => m.FiscalPeriodName, Model.PageId, "FiscalPeriodList")%>
        </div>
        <div class="FiscalPeriodListColumn2">
            <%:Html.AppLabelFor(m => m.FiscalYearId, Model.PageId, "FiscalPeriodList")%>
            <%:Html.AppDropDownListFor(m => m.FiscalYearId, Model.PageId, Url.Action("DropList", "FiscalYear", new { Area = "BasicData" }), "FiscalPeriodList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
