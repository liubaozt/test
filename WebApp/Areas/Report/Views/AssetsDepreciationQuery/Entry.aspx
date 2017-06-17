<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsDepreciationQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsDepreciationReportColumn1">
            <%:Html.AppLabelFor(m => m.FiscalYearId, Model.PageId, "AssetsDepreciationReport")%>
            <%:Html.AppDropDownListFor(m => m.FiscalYearId, Model.PageId, Url.Action("DropList", "FiscalYear", new { Area = "BasicData" }), "AssetsDepreciationReport")%>
        </div>
        <div class="AssetsDepreciationReportColumn2">
            <%:Html.AppLabelFor(m => m.FiscalPeriodId, Model.PageId, "AssetsDepreciationReport")%>
            <%:Html.AppDropDownListFor(m => m.FiscalPeriodId, Model.PageId, Url.Action("DropList", "FiscalPeriod", new { Area = "BasicData", filterExpression = "fiscalYearId=" + DFT.SQ + Model.FiscalYearId + DFT.SQ }), "AssetsDepreciationReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.FiscalYearId)%>
        <%:Html.ValidationMessageFor(m => m.FiscalPeriodId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsDepreciationReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsDepreciationReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsDepreciationReport")%>
        </div>
        <div class="AssetsDepreciationReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsDepreciationReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsDepreciationReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.EntryGridId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var formId = '<%=Model.FormId %>' + pageId;
            var fiscalYearIdObj = "#FiscalYearId" + pageId;
            var fiscalPeriodIdObj = "#FiscalPeriodId" + pageId;
            //#endregion 公共变量

            //#region 会计期间过滤 
            $(fiscalYearIdObj).change(function () {
                var fiscalYearId = $(fiscalYearIdObj).val();
                if ($.trim(fiscalYearId) == "") {
                    fiscalYearId = "none";
                }
                var urlStr = '<%=Url.Action("DropList", "FiscalPeriod", new { Area = "BasicData"}) %>' + '/?filterExpression=fiscalYearId=' + '<%=DFT.SQ %>' + fiscalYearId + '<%=DFT.SQ %>';
                $.getJSON(urlStr, function (data) {
                    AppAppendSelect2(data, fiscalPeriodIdObj, urlStr);
                });

            });
            //#endregion 会计期间过滤 

            $('#btnQuery' + pageId).click(function () {
                QueryReport();
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>
