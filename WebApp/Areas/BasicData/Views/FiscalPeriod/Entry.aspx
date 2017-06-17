<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.FiscalPeriod.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.FiscalPeriodName, Model.PageId, "FiscalPeriodEntry")%>
        <%:Html.AppTextBoxFor(m => m.FiscalPeriodName, Model.PageId, "FiscalPeriodEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.FiscalPeriodName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.FiscalYearId, Model.PageId, "FiscalPeriodEntry")%>
        <%:Html.AppDropDownListFor(m => m.FiscalYearId, Model.PageId, Url.Action("DropList", "FiscalYear", new { Area = "BasicData" }), "FiscalPeriodEntry")%>
        <%:Html.ValidationMessageFor(m => m.FiscalYearId)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.FromDate, Model.PageId, "FiscalPeriodEntry")%>
        <%:Html.AppDatePickerFor(m => m.FromDate, Model.PageId, "FiscalPeriodEntry")%>
        <%:Html.ValidationMessageFor(m => m.FromDate)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ToDate, Model.PageId, "FiscalPeriodEntry")%>
        <%:Html.AppDatePickerFor(m => m.ToDate, Model.PageId, "FiscalPeriodEntry")%>
        <%:Html.ValidationMessageFor(m => m.ToDate)%>
    </div>
    <%:Html.AppHiddenFor(m => m.FiscalPeriodId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
 <script type="text/javascript">
     $(document).ready(function () {
         //#region 公共变量
         var pageId = '<%=Model.PageId %>';
         //#endregion 公共变量

         //#region grid操作
         $('#FiscalPeriodName' + pageId).keydown(function (e) {
             if (!CheckInputData(e, "#FiscalPeriodName" + pageId, "Int", true, 0))
                 return false;
         }).focus(function () { this.style.imeMode = 'disabled'; });
         //#endregion grid操作

     });
    </script>
</asp:Content>

