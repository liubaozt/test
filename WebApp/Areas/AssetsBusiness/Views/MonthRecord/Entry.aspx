<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.MonthRecord.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div class="editor-field">
            <div class="MonthRecordEntryColumn1">
                <%:Html.AppLabelFor(m => m.FiscalYearId, Model.PageId, "MonthRecordEntry")%>
                <%:Html.AppDropDownListFor(m => m.FiscalYearId, Model.PageId, Url.Action("DropList", "FiscalYear", new { Area = "BasicData" }), "MonthRecordEntry")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <div class="MonthRecordEntryColumn2">
                <%:Html.AppLabelFor(m => m.FiscalPeriodId, Model.PageId, "MonthRecordEntry")%>
                <%:Html.AppDropDownListFor(m => m.FiscalPeriodId, Model.PageId, Url.Action("DropList", "FiscalPeriod", new { Area = "BasicData", filterExpression = "fiscalYearId=" + DFT.SQ + Model.FiscalYearId + DFT.SQ }), "MonthRecordEntry")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <%:Html.ValidationMessageFor(m => m.FiscalYearId)%>
            <%:Html.ValidationMessageFor(m => m.FiscalPeriodId)%>
        </div>
        <br />
        <fieldset>
        <p>每个会计期间结束后，请选择该会计期间，点击保存，将会生成该期间的资产汇总数据。<br />
        注：可规定在下一个会计期间的开始做上一个会计期间的月次更新。
        </p>
        </fieldset>
        <br />
        <%:Html.AppHiddenFor(m => m.IsUpdateAgain, Model.PageId)%>
    </fieldset>
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

            //#region 初始化
            $('#' + 'btnReturn' + pageId).hide();
            //#endregion 初始化

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

            function submitSave() {
                $.ajax({
                    type: 'POST',
                    url: '<%=Url.Action("Entry") %>',
                    data: $('#' + formId).serialize(),
                    success: function (html) {
                        $('#t' + pageId, '#tabs').empty();
                        $('#t' + pageId, '#tabs').append(html);
                    }
                });
                ////如果有message，弹出
                var msg = $('#Message' + pageId).val();
                if (msg) {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"] %>', msg, 'error', function () { });
                }

                //如果更新正确的话，将按钮设置成不可用
                var err = $('#HasError' + pageId).val();
                if (err == 'false') {
                    $('#btnSave' + pageId).attr('disabled', true);
                }
            }


            //#region 保存 
            $('#btnSave' + pageId).click(function () {
                $.ajax({
                    type: 'POST',
                    url: '<%=Url.Action("Check") %>',
                    data: $('#' + formId).serialize(),
                    success: function (jsonObj) {
                        if (jsonObj == "1") {
                            $('#MessageDialog' + pageId).html('<p style="float:left"><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span><%=AppMember.AppText["ThisMonthHadUpdate"]%></p>').dialog({
                                title: '<%=AppMember.AppText["MessageTitle"]%>',
                                height: 140,
                                modal: true,
                                resizable: false,
                                buttons: {
                                    '<%=AppMember.AppText["BtnConfirm"]%>': function () {
                                        $(this).dialog("close");
                                        $('#IsUpdateAgain' + pageId).val("true");
                                        submitSave();
                                    },
                                    '<%=AppMember.AppText["BtnCancel"]%>': function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                        else if (jsonObj == "0") {
                            $('#IsUpdateAgain' + pageId).val("false");
                            submitSave();
                        }
                    }
                });
            });
            //#endregion 保存 

        });
    </script>
</asp:Content>
