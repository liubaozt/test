<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsDepreciation.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div class="editor-field">
            <div class="AssetsDepreciationEntryColumn1">
                <%:Html.AppLabelFor(m => m.FiscalYearId, Model.PageId, "AssetsDepreciationEntry")%>
                <%:Html.AppDropDownListFor(m => m.FiscalYearId, Model.PageId, Url.Action("DropList", "FiscalYear", new { Area = "BasicData" }), "AssetsDepreciationEntry")%>
              <%:Html.AppRequiredFlag()%>
            </div>
            <div class="AssetsDepreciationEntryColumn2">
                <%:Html.AppLabelFor(m => m.FiscalPeriodId, Model.PageId, "AssetsDepreciationEntry")%>
                <%:Html.AppDropDownListFor(m => m.FiscalPeriodId, Model.PageId, Url.Action("DropList", "FiscalPeriod", new { Area = "BasicData", filterExpression = "fiscalYearId=" + DFT.SQ + Model.FiscalYearId + DFT.SQ }), "AssetsDepreciationEntry")%>
              <%:Html.AppRequiredFlag()%>
            </div>
            <%:Html.ValidationMessageFor(m => m.FiscalYearId)%>
            <%:Html.ValidationMessageFor(m => m.FiscalPeriodId)%>
        </div>
    </fieldset>
    <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId,  Url.Action("EntryGridData", new { formMode = Model.FormMode }), Model.EntryGridLayout, 300, 0, "btnSave", "AssetsDepreciation",true)%>
    <%:Html.AppHiddenFor(m => m.AssetsDepreciationId, Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var formId = '<%=Model.FormId %>' + pageId;
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            var fiscalYearIdObj = "#FiscalYearId" + pageId;
            var fiscalPeriodIdObj = "#FiscalPeriodId" + pageId;
            //#endregion 公共变量

            //#region 初始化
            $('#' + 'btnReturn' + pageId).hide();
            //#endregion 初始化

            //#region 计提 
            $('#btnPickup' + pageId).click(function () {
                var fiscalYearId = $(fiscalYearIdObj).val();
                var fiscalPeriodId = $(fiscalPeriodIdObj).val();
                if (fiscalYearId == "") {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["FiscalYearId"]%>' + '<%=AppMember.AppText["NotNull"]%>', 'error', function () { });
                    return;
                }
                if (fiscalPeriodId == "") {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["FiscalPeriodId"]%>' + '<%=AppMember.AppText["NotNull"]%>', 'error', function () { });
                    return;
                }

                var urlStr = '<%=Url.Action("CheckPeriod", "AssetsDepreciation", new { Area = "AssetsBusiness"}) %>';

                $.ajax({
                    type: 'post',
                    url: urlStr,
                    data: { fiscalYearId: fiscalYearId, fiscalPeriodId: fiscalPeriodId },
                    success: function (jsonObj) {
                        if (jsonObj != "1") {
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', jsonObj, 'error', function () { });
                        }
                        else {
                            var obj = $('#' + formId).serializeObject();
                            var formvar = JSON.stringify(obj);
                            //$('#' + gridId).jqGrid('setPostData', { formVar: formvar, formMode: "pickUp" });
                            $('#' + gridId).jqGrid('setGridParam', { postData: { formVar: formvar, formMode: "pickUp"} })
                            jQuery('#' + gridId).trigger('reloadGrid');
                            $('#' + 'btnSave' + pageId).attr('disabled', false);
                        }
                    }
                });


            });
            //#endregion 计提 

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
            $(fiscalPeriodIdObj).change(function () {
                $('#' + 'btnPickup' + pageId).attr('disabled', false);

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
    <%:Html.AppNormalButton(Model.PageId, "btnPickup", AppMember.AppText["BtnPickup"])%>
</asp:Content>
