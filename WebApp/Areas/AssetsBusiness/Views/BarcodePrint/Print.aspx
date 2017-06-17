<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.BarcodePrint.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="BusinessLogic.AssetsBusiness.Repositorys" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="BarcodeStyleFieldSet<%=Model.PageId%>" style="float: left">
        <div id="BarcodeAllAssets" style="float: left">
            <%-- <div id="page1">--%>
            <%-- <div id="BarcodeStyleDiv<%=Model.PageId %>" style='width: 210mm; height: 140mm; position: relative'>
                <%:Html.AppBarcodeStyleFor<AssetsPrint>(this.Url,Model.PageId, Model.BarcodeStyle,Model.CurrentAssets)%>               
            </div>--%>
            <%:Html.AppBarcodeStyleAllFor<AssetsPrint>(this.Url, Model.PageId, Model.BarcodeStyle, Model.AllAssets)%>
            <%-- </div>--%>
        </div>
    </fieldset>
    <br />
    <div class="editor-field">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <%-- <label id="AssetsCountAndIndex">
        </label>--%>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <%-- <%:Html.AppNormalButton(Model.PageId, "btnPrevious", AppMember.AppText["BtnPrevious"])%>
        <%:Html.AppNormalButton(Model.PageId, "btnNext", AppMember.AppText["BtnNext"])%>--%>
        <%:Html.AppNormalButton(Model.PageId, "btnPrint", AppMember.AppText["BtnPrint"])%>
        <%-- <%:Html.AppNormalButton(Model.PageId, "btnPrintAll", AppMember.AppText["BtnPrintAll"])%>--%>
    </div>
    <div id="SelectDialog<%=Model.PageId%>">
    </div>
    <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.DefaultAssetsId,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.BarcodeStyleId,Model.PageId) %>
    <%:Html.AppHiddenFor(m=>m.AssetsCount,Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <title>固定资产条码打印</title>
    <%=TempData["CssBlock"]%>
    <%=TempData["ScriptBlock"]%>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/jquery-barcode-2.0.2.min.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/jquery.jqprint.js") %>'></script>
    <style type="text/css">
        BODY
        {
            margin: 0pt;
        }
        .#page1 DIV
        {
            position: absolute;
        }
         .#BarcodeAllAssets DIV
        {
            margin:0 ;
            padding:0;
        }
    </style>
    <object id="jatoolsPrinter" classid="CLSID:B43D3361-D075-4BE2-87FE-057188254255"
        codebase='<%= Url.Content("~/Content/jatoolsPrinter/")%>hjatoolsPrinter.cab#version=8,6,0,0'
        height='0' width='0'>
    </object>
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var formId = '<%=Model.FormId %>' + pageId;
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            var assetsIndex = 1;
            //#endregion 公共变量

            //#region 初始化
            $('#btnSave' + pageId).hide();
            $('#btnReturn' + pageId).hide();
            $('#AssetsCountAndIndex').html(assetsIndex + "/" + '<%=Model.AssetsCount %>');
            //#endregion 初始化

            function PrintLable(irow, needRefreshCount) {
                if (needRefreshCount == 1) {
                    assetsIndex = irow;
                    $('#AssetsCountAndIndex').html(assetsIndex + "/" + '<%=Model.AssetsCount %>');
                }
                var myDoc = {
                    settings: {
                        paperWidth: 2100,   //指定纸张的宽度单位是十分之一毫米
                        paperHeight: 1400  //指定纸张的高度
                    },
                    documents: document,    // 打印页面(div)们在本文档中
                    marginIgnored: true,  //零边距打印
                    copyrights: '杰创软件拥有版权  www.jatools.com'         // 版权声明必须
                };
                jatoolsPrinter.print(myDoc, false);
            }

            //#region 用户操作
            $("#btnPrint" + pageId).click(function () {
                pagesetup_null();
                $("#BarcodeAllAssets").jqprint();

                //PrintLable(assetsIndex,0);
            });

            $("#btnPrintAll" + pageId).click(function () {
                var urlStr = '<%=Url.Action("PrintAll") %>';
                $.ajax({
                    type: 'POST',
                    url: urlStr,
                    data: { pageId: pageId },
                    success: function (jsonObj) {
                        if (jsonObj != "0") {
                            $("#BarcodeStyleDiv" + pageId).empty();
                            $("#BarcodeStyleDiv" + pageId).append(jsonObj);
                            setTimeout(PrintLable(1, 1), 800);

                        }
                    }
                });

                var acount = parseInt('<%=Model.AssetsCount %>');
                for (i = 2; i <= acount; i++) {
                    urlStr = '<%=Url.Action("NextAssets") %>';
                    $.ajax({
                        type: 'POST',
                        url: urlStr,
                        data: { pageId: pageId },
                        success: function (jsonObj) {
                            if (jsonObj != "0") {
                                $("#BarcodeStyleDiv" + pageId).empty();
                                $("#BarcodeStyleDiv" + pageId).append(jsonObj);
                                setTimeout(PrintLable(assetsIndex + 1, 1), 800);
                            }
                        }
                    });

                }

            });


            $("#btnPrevious" + pageId).click(function () {
                var urlStr = '<%=Url.Action("PreviousAssets") %>';
                $.ajax({
                    type: 'POST',
                    url: urlStr,
                    data: { pageId: pageId },
                    success: function (jsonObj) {
                        if (jsonObj != "0") {
                            $("#BarcodeStyleDiv" + pageId).empty();
                            $("#BarcodeStyleDiv" + pageId).append(jsonObj);
                            assetsIndex -= 1;
                            $('#AssetsCountAndIndex').html(assetsIndex + "/" + '<%=Model.AssetsCount %>');
                        }
                    }
                });
            });

            $("#btnNext" + pageId).click(function () {
                var urlStr = '<%=Url.Action("NextAssets") %>';
                $.ajax({
                    type: 'POST',
                    url: urlStr,
                    data: { pageId: pageId },
                    success: function (jsonObj) {
                        if (jsonObj != "0") {
                            $("#BarcodeStyleDiv" + pageId).empty();
                            $("#BarcodeStyleDiv" + pageId).append(jsonObj);
                            assetsIndex += 1;
                            $('#AssetsCountAndIndex').html(assetsIndex + "/" + '<%=Model.AssetsCount %>');
                        }
                    }
                });
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
                $.ajax({
                    type: "POST",
                    url: '<%:Model.SelectUrl %>',
                    data: { pageId: pageId },
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
                                    var obj = $('#SelectForm' + pageId).serializeObject();
                                    var formvar = JSON.stringify(obj);
                                    jQuery('#list' + spageId).jqGrid('setGridParam', { postData: { formVar: formvar} });
                                    jQuery('#list' + pageId).trigger('reloadGrid');
                                },
                                '<%=AppMember.AppText["BtnConfirm"]%>': function () {
                                    var arrRow = jQuery('#list' + pageId).jqGrid('getGridParam', 'selarrrow');
                                    if (arrRow.length > 0) {
                                        var rowdata;
                                        for (var i = 0; i < arrRow.length; i++) {
                                            rowdata = jQuery('#list' + pageId).getRowData(arrRow[i]);
                                            var pk = rowdata["assetsId"];
                                            var hasPk = false;
                                            var curGridData = $('#' + gridId).getDataIDs();
                                            for (j = 0; j < curGridData.length; j++) {
                                                if (pk == curGridData[j])
                                                    hasPk = true;
                                            }
                                            if (hasPk == false && pk) {
                                                var dataRow = { AssetsId: rowdata["assetsId"], AssetsNo: rowdata["assetsNo"], AssetsName: rowdata["assetsName"], AssetsTypeId: rowdata["assetsTypeId"], AssetsClassId: rowdata["assetsClassId"] };
                                                $('#' + gridId).jqGrid('addRowData', pk, dataRow);
                                                $('#' + gridId).jqGrid('editRow', pk, true);
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
