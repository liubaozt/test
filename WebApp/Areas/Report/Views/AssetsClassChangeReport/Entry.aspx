<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsClassChangeReport.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsClassChangeReportColumn1">
            <%:Html.AppLabelFor(m => m.FiscalYearId, Model.PageId, "AssetsClassChangeReport")%>
            <%:Html.AppDropDownListFor(m => m.FiscalYearId, Model.PageId, Url.Action("DropList", "FiscalYear", new { Area = "BasicData" }), "AssetsClassChangeReport")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <div class="AssetsClassChangeReportColumn2">
            <%:Html.AppLabelFor(m => m.FiscalPeriodId, Model.PageId, "AssetsClassChangeReport")%>
            <%:Html.AppDropDownListFor(m => m.FiscalPeriodId, Model.PageId, Url.Action("DropList", "FiscalPeriod", new { Area = "BasicData", filterExpression = "fiscalYearId=" + DFT.SQ + Model.FiscalYearId + DFT.SQ }), "AssetsClassChangeReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.FiscalYearId)%>
        <%:Html.ValidationMessageFor(m => m.FiscalPeriodId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsClassChangeReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "AssetsClassChangeReport")%>
            <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId, Model.AssetsClassAddFavoritUrl, Model.AssetsClassReplaceFavoritUrl, "AssetsClassChangeReport")%>
        </div>
        <div class="AssetsClassChangeReportColumn2">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsClassChangeReport")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsClassChangeReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsClassId)%>
        <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsClassChangeReportColumn1">
            <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "UpcomingGuaranteeReport")%>
            <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "AssetsClassChangeReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.StoreSiteId)%>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var pageId = '<%=Model.PageId %>';
            var formId = '<%=Model.FormId %>' + pageId;
            var fiscalYearIdObj = "#FiscalYearId" + pageId;
            var fiscalPeriodIdObj = "#FiscalPeriodId" + pageId;
            //#region 会计期间过滤 
            $(fiscalYearIdObj).change(function () {
                var fiscalYearId = $(fiscalYearIdObj).val();
                if ($.trim(fiscalYearId) == "") {
                    fiscalYearId = "none";
                }
                var urlStr = '<%=Url.Action("DropList", "FiscalPeriod", new { Area = "BasicData"}) %>' + '/?filterExpression=fiscalYearId=' + '<%=DFT.SQ %>' + fiscalYearId + '<%=DFT.SQ %>';
                $.getJSON(urlStr, function (data) {
                    AppAppendSelect2(data, fiscalPeriodIdObj);
                });

            });
            //#endregion 会计期间过滤 
//            $('#btnQuery' + pageId).click(function () {
//                var fiscalYearId = $(fiscalYearIdObj).val();
//                if (!fiscalYearId || fiscalYearId == "") {
//                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["FiscalYearId"]%>' + '<%=AppMember.AppText["NotNull"]%>', 'error', function () { });
//                }
//                else {
//                    var formobj = $('#' + formId).serializeObject();
//                    var formvar = JSON.stringify(formobj);
//                    window.open("/ReportView/AssetsClassChangeReportView?formVar=" + formvar);
//                }

//            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>

