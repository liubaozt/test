<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.BarcodePrint.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.BarcodeStyleId, Model.PageId, "BarcodePrintEntry")%>
        <%:Html.AppDropDownListFor(m => m.BarcodeStyleId, Model.PageId, Url.Action("DropList", "BarcodeStyle", new { Area = "BusinessCommon" }), "BarcodePrintEntry", false)%>
        <%:Html.ValidationMessageFor(m => m.BarcodeStyleId)%>
    </div>
     <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId, Url.Action("EntryGridData"), Model.EntryGridLayout, 200, true, 500, true, false, "btnPreView", "BarcodePrint")%>
    <%--<%:Html.AppNormalGridFor(this.Url, Model.PageId, Model.EntryGridId, Url.Action("EntryGridData"), Model.EntryGridLayout, 150, true, 600, true, false, true, "BarcodePrint")%>--%>
    <div class="editor-field">
        <%:Html.AppNormalButton(Model.PageId, "btnPreView", AppMember.AppText["BtnPreView"],true)%>
    </div>
    <div id="SelectDialog<%=Model.PageId%>">
    </div>
    <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.DefaultAssetsId,Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <%=TempData["CssBlock"]%>
    <%=TempData["ScriptBlock"]%>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/jquery-barcode-2.0.2.min.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/jquery.jqprint.js") %>'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var formId = '<%=Model.FormId %>' + pageId;
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            //#endregion 公共变量

            //#region 初始化
            $('#btnSave' + pageId).hide();
            $('#btnReturn' + pageId).hide();
            $('#' + formId).attr('target', '_blank');
            $('#' + formId).attr('method', 'post');
            $('#' + formId).attr('action', ' <%=Url.Action("Print")%> ');
            //#endregion 初始化



            //#region 用户操作
            $('#BarcodeStyleId' + pageId).change(function () {
                var barcodeStyleId = $('#BarcodeStyleId' + pageId).val();
                var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                var assetsId = "";
                if (id) {
                    assetsId = jQuery('#' + gridId).getCell(id, 'AssetsId');
                }
                var barcodeStyleUrl = '<%=Url.Action("ChangeBarcodeStyle") %>';
                $.ajax({
                    type: 'POST',
                    url: barcodeStyleUrl,
                    data: { pageId: pageId, barcodeStyleId: barcodeStyleId, assetsId: assetsId },
                    success: function (jsonObj) {
                        $('#BarcodeStyleDiv' + pageId).empty();
                        $('#BarcodeStyleDiv' + pageId).append(jsonObj);
                    }
                });
            });

            $("#btnPreView" + pageId).click(function () {
                //var urls = '<%= Url.Action("Print")%>' + "?pageId=" + pageId;
                //window.open(urls, "_blank")
                var barcodeStyleId = $('#BarcodeStyleId' + pageId).val();
                if (barcodeStyleId == "") {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["NoneBarcodeStyle"]%>', 'warning', function () { });
                    return;
                }
                var gridIds = $('#' + gridId).getDataIDs();
                if (gridIds.length < 1) {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["NoneAssets"]%>', 'warning', function () { });
                    return;
                }
                else {
                    $('#' + formId).submit();
                }
            });

            //设置网页打印的页眉页脚为空   
            function pagesetup_null() {
                var hkey_root, hkey_path, hkey_key;
                hkey_root = "HKEY_CURRENT_USER";
                hkey_path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";
                try {
                    var RegWsh = new ActiveXObject("WScript.Shell");
                    hkey_key = "header";
                    RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
                    hkey_key = "footer";
                    RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
                } catch (e) { }
            }
            //#endregion 用户操作

            //#region grid操作
            onClickRowBarcodePrint = function (id) {
                var barcodeStyleId = $('#BarcodeStyleId' + pageId).val();
                var urlStr = '<%=Url.Action("GridClick") %>';
                var pk;
                if (id) {
                    pk = jQuery("#" + gridId).getCell(id, 'AssetsId');
                }
                $.ajax({
                    type: 'POST',
                    url: urlStr,
                    data: { pageId: pageId, gridPkValue: pk, barcodeStyleId: barcodeStyleId },
                    success: function (jsonObj) {
                        $("#BarcodeStyleDiv" + pageId).empty();
                        $("#BarcodeStyleDiv" + pageId).append(jsonObj);
                    }
                });
            };
            $('#btnDelete' + pageId).click(function () {
                var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                $('#' + gridId).jqGrid('delRowData', id);
            });
            $('#btnAdd' + gridId, '#t_' + gridId).live("click", function () {
                var spageId = pageId + "s";
                $.ajax({
                    type: "POST",
                    url: '<%:Model.SelectUrl %>',
                    data: { pageId: spageId, selectMode: "AssetsSelect" },
                    datatype: "html",
                    success: function (data) {
                        $("#SelectDialog" + pageId).html(data).dialog({
                            title: '<%=AppMember.AppText["AssetsSelect"]%>',
                            height: 470,
                            width: 800,
                            modal: true,
                            resizable: true,
                            buttons: {
                                '<%=AppMember.AppText["BtnQuery"]%>': function () {
                                    var obj = $('#SelectForm' + spageId).serializeObject();
                                    var formvar = JSON.stringify(obj);
                                    jQuery('#list' + spageId).jqGrid('setGridParam', { postData: { formVar: formvar} });
                                    jQuery('#list' + spageId).trigger('reloadGrid');

                                },
                                '<%=AppMember.AppText["BtnConfirm"]%>': function () {
                                    var arrRow = jQuery('#list' + spageId).jqGrid('getGridParam', 'selarrrow');
                                    if (arrRow.length > 0) {
                                        var rowdata;
                                        for (var i = 0; i < arrRow.length; i++) {
                                            rowdata = jQuery('#list' + spageId).getRowData(arrRow[i]);
                                            var pk = rowdata["assetsId"];
                                            var hasPk = false;
                                            var curGridData = $('#' + gridId).getDataIDs();
                                            for (j = 0; j < curGridData.length; j++) {
                                                if (pk == curGridData[j])
                                                    hasPk = true;
                                            }
                                            if (hasPk == false && pk) {
                                                var dataRow = { AssetsId: rowdata["assetsId"], AssetsNo: rowdata["assetsNo"],
                                                    AssetsName: rowdata["assetsName"], AssetsClassName: rowdata["assetsClassId"],
                                                    ImgDefault: rowdata["imgDefault"], AssetsBarcode: rowdata["assetsBarcode"],
                                                    AssetsUsesName: rowdata["assetsUsesId"], CompanyName: rowdata["companyId"],
                                                    DepartmentName: rowdata["departmentName"], StoreSiteName: rowdata["storeSiteName"],
                                                    UsePeopleName: rowdata["usePeopleName"], KeeperName: rowdata["keeperName"],
                                                    AssetsValue: rowdata["assetsValue"], PurchaseDate: rowdata["purchaseDate"],
                                                    EquityOwnerName: rowdata["equityOwnerName"], Spec: rowdata["spec"],
                                                    Remark: rowdata["remark"], ProjectManageName: rowdata["projectManageName"]
                                                };
                                                $('#' + gridId).jqGrid('addRowData', pk, dataRow);
                                                $('#' + gridId).jqGrid('editRow', pk, true);
                                                $('#' + 'btnPreView' + pageId).attr('disabled', false);
                                            }
                                        }
                                    }
                                    $(this).dialog("close");
                                },
                                '<%=AppMember.AppText["BtnCancel"]%>': function () {
                                    $(this).dialog("close");
                                }
                            },
                            close: function () {
                                //allFields.val("").removeClass("ui-state-error");
                            }
                        });
                    }
                });
            });
            //#endregion grid操作
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>
