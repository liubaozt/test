<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.BarcodePrint.Entry2Model>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="float: left; height: 600px">
      <% if (Model.FormMode == "new2")
           { %>
           <fieldset style="padding-right:2; margin-right:2">
        <div class="editor-field">
            <div class="BarcodePrintEntryColumn1">
                <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "BarcodePrintEntry")%>
            </div>
            <div class="BarcodePrintEntryColumn2">
                <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "BarcodePrintEntry")%>
            </div>
            <div class="BarcodePrintEntryColumn3">
                <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId, Model.AssetsClassAddFavoritUrl, Model.AssetsClassReplaceFavoritUrl, "BarcodePrintEntry")%>
            </div>
        </div>
        <div class="editor-field">
            <div class="BarcodePrintEntryColumn1">
                <%:Html.AppLabelFor(m => m.AssetsState, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppDropDownListFor(m => m.AssetsState, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "AssetsState" }), "BarcodePrintEntry")%>
            </div>
            <div class="BarcodePrintEntryColumn2">
                <%:Html.AppLabelFor(m => m.AssetsBarcode, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsBarcode, Model.PageId, "BarcodePrintEntry")%>
            </div>
            <div class="BarcodePrintEntryColumn3">
                <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "BarcodePrintEntry")%>
            </div>
        </div>
        <div class="editor-field">
            <div class="BarcodePrintEntryColumn1">
                <%:Html.AppLabelFor(m => m.UsePeople, Model.PageId, "BarcodePrintEntry")%>
                <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
                  { %>
                <%:Html.AppDropDownListFor(m => m.UsePeople, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "BarcodePrintEntry")%>
                <%}
                  else
                  { %>
                <%:Html.AppAutoCompleteFor(m => m.UsePeople, Model.PageId, "BarcodePrintEntry", Model.UserSource)%>
                <%}  %>
            </div>
            <div class="BarcodePrintEntryColumn2">
                <%:Html.AppLabelFor(m => m.Keeper, Model.PageId, "BarcodePrintEntry")%>
                <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
                  { %>
                <%:Html.AppDropDownListFor(m => m.Keeper, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "BarcodePrintEntry")%>
                <%}
                  else
                  { %>
                <%:Html.AppAutoCompleteFor(m => m.Keeper, Model.PageId, "BarcodePrintEntry", Model.UserSource)%>
                <%}  %>
            </div>
            <div class="BarcodePrintEntryColumn3">
                <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "BarcodePrintEntry")%>
            </div>
        </div>
        <div class="editor-field">
            <div class="BarcodePrintEntryColumn1">
                <%:Html.AppLabelFor(m => m.PurchaseDateFrom, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppDatePickerFor(m => m.PurchaseDateFrom, Model.PageId, "BarcodePrintEntry")%>
            </div>
            <div class="BarcodePrintEntryColumn2">
                <%:Html.AppLabelFor(m => m.PurchaseDateTo, Model.PageId, "BarcodePrintEntry")%>
                <%:Html.AppDatePickerFor(m => m.PurchaseDateTo, Model.PageId, "BarcodePrintEntry")%>
            </div>
            <%:Html.AppNormalButton(Model.PageId, "btnQuery", AppMember.AppText["BtnQuery"], false)%>
        </div>
        </fieldset>
           <% } %>
        <% if (Model.FormMode == "new2")
           { %>
        <div style="float: left">
            <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId, Url.Action("EntryGridData"), Model.EntryGridLayout, 200, true,650, false, false, "btnPreView", "BarcodePrint")%>
        </div>
        <% }
           else if (Model.FormMode == "storeSite")
           { %>
        <div style="float: left">
            <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId, Url.Action("StoreSiteGridData"), Model.StoreSiteGridLayout, 250, true,600, false, false, "btnPreView", "BarcodePrint")%>
        </div>
        <% } %>
    </div>
      <fieldset>
    <div class="editor-field" style="float: left">
     <div class="BarcodePrintEntryColumn1">
        <%:Html.AppLabelFor(m => m.BarcodeStyleId, Model.PageId, "BarcodePrintStyle")%>
        <%:Html.AppDropDownListFor(m => m.BarcodeStyleId, Model.PageId, Url.Action("DropList", "BarcodePrint", new { Area = "AssetsBusiness", formMode = Model.FormMode }), "BarcodePrintEntry", false)%>
        <%:Html.ValidationMessageFor(m => m.BarcodeStyleId)%>
        </div>
           <div class="BarcodePrintEntryColumn2">
        <%:Html.AppLabelFor(m => m.PrinterName, Model.PageId, "BarcodePrintStyle")%>
        <%:Html.AppTextBoxFor(m => m.PrinterName, Model.PageId, "BarcodePrintEntry")%>
          </div>
    </div>
    <div style="z-index: 0">
        <object id="LabelDefine<%=Model.PageId%>" classid="CLSID:C91CF236-57C1-4A3C-9FAA-90C343970A15"
            codebase="LabelDefine.CAB#version=2,1,0,4">
        </object>
    </div>
    <div id="divDesign<%=Model.PageId%>" class="editor-field" style="float: left">
        <%:Html.AppNormalButton(Model.PageId, "btnNew", AppMember.AppText["BtnStyleNew"], false)%>
        <%:Html.AppNormalButton(Model.PageId, "btnSave2", AppMember.AppText["BtnSave"], false)%>
        <%-- <%:Html.AppNormalButton(Model.PageId, "btnModified", AppMember.AppText["BtnStyleModified"], false)%>--%>
        <%:Html.AppNormalButton(Model.PageId, "btnStyleDelete", AppMember.AppText["BtnStyleDelete"], false)%>
        <%:Html.AppNormalButton(Model.PageId, "btnCancel", AppMember.AppText["BtnCancel"], false)%>
    </div>
    <div id='divGenal' class="editor-field">
        <%:Html.AppNormalButton(Model.PageId, "btnDesign", AppMember.AppText["BtnStyleDesign"], false)%>
        <%:Html.AppNormalButton(Model.PageId, "btnPrint", AppMember.AppText["BtnPrint"], false)%>
        <%:Html.AppNormalButton(Model.PageId, "btnPrintSingle", AppMember.AppText["BtnPrintSingle"], false)%>
        <%:Html.AppNormalButton(Model.PageId, "btnPrintPreView", AppMember.AppText["BtnPreView"], false)%>
        <%--   <%:Html.AppNormalButton(Model.PageId, "btnLock", AppMember.AppText["BtnLock"], false)%>
        <%:Html.AppNormalButton(Model.PageId, "btnUnLock", AppMember.AppText["BtnUnLock"], false)%>--%>
    </div>
   
    <br />
    <br />
     </fieldset>
    <br />
    <br />
    <br />
    <br />
       <br />
    <div id="SelectDialog<%=Model.PageId%>" style="z-index: 99999">
    </div>
    <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.DefaultAssetsId,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.XmlString,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.BarcodeStyleName,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.StoreSiteGridId,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.LabelType,Model.PageId)%>
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
            var formMode = '<%=Model.FormMode %>';
            var labelType = '<%=Model.LabelType %>';
            //#endregion 公共变量

            //#region 初始化
            $('#btnSave' + pageId).hide();
            $('#btnReturn' + pageId).hide();
            document.getElementById("divDesign" + pageId).style.display = "none";
            LoadDefaultLabelStyle();
            //#endregion 初始化

            //#region 用户操作
            $('#BarcodeStyleId' + pageId).change(function () {
                var barcodeStyleId = $('#BarcodeStyleId' + pageId).val();
                var barcodeStyleUrl = '<%=Url.Action("LoadLabelStyle") %>';
                $.ajax({
                    type: 'POST',
                    url: barcodeStyleUrl,
                    data: { pageId: pageId, barcodeStyleId: barcodeStyleId },
                    success: function (xml) {
                        var ld = document.getElementById("LabelDefine" + pageId);

                        if (!ld) return;

                        while (xml.indexOf("&lt;") >= 0)
                            xml = xml.replace("&lt;", "<");
                        while (xml.indexOf("&gt;") >= 0)
                            xml = xml.replace("&gt;", ">");
                        ld.LoadData(xml);
                    }
                });
            });

            function LoadDefaultLabelStyle() {
                var barcodeStyleUrl = '<%=Url.Action("LoadDefaultLabelStyle") %>';
                $.ajax({
                    type: 'POST',
                    url: barcodeStyleUrl,
                    data: { pageId: pageId, formMode: formMode },
                    success: function (xml) {
                        var ld = document.getElementById("LabelDefine" + pageId);
                        if (!ld) return;

                        while (xml.indexOf("&lt;") >= 0)
                            xml = xml.replace("&lt;", "<");
                        while (xml.indexOf("&gt;") >= 0)
                            xml = xml.replace("&gt;", ">");
                        ld.LoadData(xml);
                    }
                });
            }

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
                    data: { pageId: spageId, selectMode: "AssetsSelect", formName: "BarcodePrint" },
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
                                        var curGridData = $('#' + gridId).getDataIDs();
                                        var arrGridData = new Array();
                                        var count = 0;
                                        for (var i = 0; i < arrRow.length; i++) {
                                            rowdata = jQuery('#list' + spageId).getRowData(arrRow[i]);
                                            var pk = rowdata["assetsId"];
                                            var hasPk = false;
                                            for (j = 0; j < curGridData.length; j++) {
                                                if (pk == curGridData[j])
                                                    hasPk = true;
                                            }
                                            if (hasPk == false && pk) {
                                                //                                                var dataRow = { AssetsId: rowdata["assetsId"], AssetsNo: rowdata["assetsNo"],
                                                //                                                    AssetsName: rowdata["assetsName"], AssetsClassName: rowdata["assetsClassId"],
                                                //                                                    ImgDefault: rowdata["imgDefault"], AssetsBarcode: rowdata["assetsBarcode"],
                                                //                                                    AssetsUsesName: rowdata["assetsUsesId"], CompanyName: rowdata["companyId"],
                                                //                                                    DepartmentName: rowdata["departmentName"], StoreSiteName: rowdata["storeSiteName"],
                                                //                                                    UsePeopleName: rowdata["usePeopleName"], KeeperName: rowdata["keeperName"],
                                                //                                                    AssetsValue: rowdata["assetsValue"], PurchaseDate: rowdata["purchaseDate"],
                                                //                                                    EquityOwnerName: rowdata["equityOwnerName"], Spec: rowdata["spec"],
                                                //                                                    Remark: rowdata["remark"], ProjectManageName: rowdata["projectManageName"]
                                                //                                                };
                                                var dataRow = { id: pk, cell: [rowdata["assetsId"], rowdata["assetsNo"],
                                                     rowdata["assetsName"], rowdata["assetsClassId"],
                                                     rowdata["imgDefault"], rowdata["assetsBarcode"],
                                                     rowdata["assetsUsesId"], rowdata["companyId"],
                                                     rowdata["departmentName"], rowdata["storeSiteName"],
                                                     rowdata["usePeopleName"], rowdata["keeperName"],
                                                     rowdata["assetsValue"], rowdata["purchaseDate"],
                                                     rowdata["equityOwnerName"], rowdata["spec"],
                                                     rowdata["remark"], rowdata["projectManageName"]]
                                                };
                                                arrGridData.push(dataRow);
                                                count++;
                                                //$('#' + gridId).jqGrid('addRowData', pk, dataRow);
                                                //$('#' + gridId).jqGrid('editRow', pk, true);
                                                //$('#' + 'btnPreView' + pageId).attr('disabled', false);
                                            }
                                        }
                                        var resultData = { total: 1, page: 1, records: count, rows: arrGridData };
                                        $('#' + gridId)[0].addJSONData(resultData);
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

            //#region 打印操作
            function styleDesign()  // 样式设计
            {
                event.srcElement.disabled = true;
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                if (formMode == "new2") {
                    //ld.SetBindingFields("资产编号|资产名称|资产分类|资产条码|部门|存放地点|规格|产权单位|入库日期|保管人|使用人|公司|字段终结点");
                    ld.SetBindingFields("资产编号|资产名称|资产分类|资产条码|部门|存放地点|序列号|产权单位|入库日期|到期日期|保管人|使用人|公司|字段终结点");
                }
                else if (formMode == "storeSite") {
                    ld.SetBindingFields("存放地点编码|存放地点名称|上级地点|公司|字段终结点");
                }
                ld.SettingMode = true;
                document.getElementById("divDesign" + pageId).style.display = "block";
                $("#btnNew" + pageId).attr('disabled', false);
                $("#btnModified" + pageId).attr('disabled', false);
                $("#btnDelete" + pageId).attr('disabled', false);
                $("#btnSave2" + pageId).attr('disabled', false);
                $("#btnCancel" + pageId).attr('disabled', false);
            }
            function cancel()  // 取消
            {
                $("#btnDesign" + pageId).attr('disabled', false);
                document.getElementById("divDesign" + pageId).style.display = "none";
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                ld.SettingMode = false;
                //var cfg = document.getElementById("hdnConfigFile").value;
                //if (cfg) ld.LoadData(cfg);
            }
            function barcodePrint()  // 打印
            {
                event.srcElement.disabled = true;
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                var printerName = $("#PrinterName" + pageId).val();
                if (!printerName) {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["SetPrinterName"]%>', 'error', function () { });
                    return;
                }
                ld.PrinterName = printerName;
                var rowdatas = jQuery('#' + gridId).getRowData();
                for (j = 0; j < rowdatas.length; j++) {
                    var rowdata = rowdatas[j];
                    var assets = "";
                    if (formMode == "new2") {
                        assets += rowdata["AssetsNo"] + "|" + rowdata["AssetsName"] + "|" + rowdata["AssetsClassName"] + "|" + rowdata["AssetsBarcode"]
                             + "|" + rowdata["DepartmentName"] + "|" + rowdata["StoreSiteName"] + "|" + rowdata["Spec"] + "|" + rowdata["EquityOwnerName"]
                             + "|" + rowdata["PurchaseDate"] + "|" + rowdata["DuetoDate"] + "|" + rowdata["UsePeopleName"] + "|" + rowdata["KeeperName"] + "|" + rowdata["CompanyName"] + "|0";
                    }
                    else if (formMode == "storeSite") {
                        assets += rowdata["storeSiteNo"] + "|" + rowdata["storeSiteName"] + "|" + rowdata["parentId"] + "|" + rowdata["companyId"] + "|0";
                    }
                    ld.SetDataSource(assets);
                    ld.PrintLabel();
                }
                if (rowdatas.length < 1) {
                    ld.PrintLabel();
                }
                $("#btnPrint" + pageId).attr('disabled', false);
            }
            function printSingle()  // 单条打印
            {
                event.srcElement.disabled = true;
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                var printerName = $("#PrinterName" + pageId).val();
                if (!printerName) {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["SetPrinterName"]%>', 'error', function () { });
                    return;
                }
                ld.PrinterName = printerName;
                var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                if (id) {
                    var rowdata = jQuery('#' + gridId).getRowData(id);
                    var assets = "";
                    if (formMode == "new2") {
                        assets += rowdata["AssetsNo"] + "|" + rowdata["AssetsName"] + "|" + rowdata["AssetsClassName"] + "|" + rowdata["AssetsBarcode"]
                             + "|" + rowdata["DepartmentName"] + "|" + rowdata["StoreSiteName"] + "|" + rowdata["Spec"] + "|" + rowdata["EquityOwnerName"]
                             + "|" + rowdata["PurchaseDate"] + "|" + rowdata["DuetoDate"] + "|" + rowdata["UsePeopleName"] + "|" + rowdata["KeeperName"] + "|" + rowdata["CompanyName"] + "|0";
                    }
                    else if (formMode == "storeSite") {
                        assets += rowdata["storeSiteNo"] + "|" + rowdata["storeSiteName"] + "|" + rowdata["parentId"] + "|" + rowdata["companyId"] + "|0";
                    }
                    ld.SetDataSource(assets);
                    ld.PrintLabel();
                    $("#btnPrintSingle" + pageId).attr('disabled', false);
                }
            }
            function preview()  // 预览
            {
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                var rowdata = jQuery('#' + gridId).getRowData(id);
                var assets = "";
                if (formMode == "new2") {
                    assets += rowdata["AssetsNo"] + "|" + rowdata["AssetsName"] + "|" + rowdata["AssetsClassName"] + "|" + rowdata["AssetsBarcode"]
                             + "|" + rowdata["DepartmentName"] + "|" + rowdata["StoreSiteName"] + "|" + rowdata["Spec"] + "|" + rowdata["EquityOwnerName"]
                             + "|" + rowdata["PurchaseDate"] + "|" + rowdata["DuetoDate"] + "|" + rowdata["UsePeopleName"] + "|" + rowdata["KeeperName"] + "|" + rowdata["CompanyName"] + "|0";
                }
                else if (formMode == "storeSite") {
                    assets += rowdata["storeSiteNo"] + "|" + rowdata["storeSiteName"] + "|" + rowdata["parentId"] + "|" + rowdata["companyId"] + "|0";
                }
                ld.SetDataSource(assets);
            }
            function lock()  // 锁定
            {
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                ld.SetDragMode(false);
            }
            function opterateLabel() {
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                var dt = ld.GetXml();
                while (dt.indexOf("<") >= 0)
                    dt = dt.replace("<", "&lt;");
                while (dt.indexOf(">") >= 0)
                    dt = dt.replace(">", "&gt;");
                $("#XmlString" + pageId).val(dt);
            }
            function unlock()  // 解锁
            {
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                ld.SetDragMode(true);
            }
            $("#btnDesign" + pageId).click(function () {
                styleDesign();
            });
            $("#btnNew" + pageId).click(function () {
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                ld.AddNewXML();
                ld.SetDragMode(true);
                $("#btnModified" + pageId).attr('disabled', true);
                $("#btnStyleDelete" + pageId).attr('disabled', true);
                $("#FormMode" + pageId).val("new");
            });
            //            $("#btnModified" + pageId).click(function () {
            //                $("#FormMode" + pageId).val("modified");
            //                $("#btnNew" + pageId).attr('disabled', true);
            //                $("#btnStyleDelete" + pageId).attr('disabled', true);
            //                opterateLabel();
            //                $("#btnSave" + pageId).click();
            //            });
            $("#btnStyleDelete" + pageId).click(function () {
                $("#FormMode" + pageId).val("delete");
                $("#btnNew" + pageId).attr('disabled', true);
                $("#btnModified" + pageId).attr('disabled', true);
                opterateLabel();
                $("#btnSave" + pageId).click();
            });
            $("#btnPrint" + pageId).click(function () {
                barcodePrint();
            });
            $("#btnPrintSingle" + pageId).click(function () {
                printSingle();
            });
            $("#btnPrintPreView" + pageId).click(function () {
                preview();
            });
            $("#btnLock" + pageId).click(function () {
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                var xml = ld.GetXml();
                $("#ssxml").val(xml);
                lock();
            });
            $("#btnUnLock" + pageId).click(function () {
                unlock();
            });
            $("#btnCancel" + pageId).click(function () {
                cancel();
            });
            $("#btnSave2" + pageId).click(function () {
                var formMode = $("#FormMode" + pageId).val();
                if (formMode == 'new') {
                    var currentLabelName = window.prompt("请输入名称", "");
                    $("#BarcodeStyleName" + pageId).val(currentLabelName);
                }
                else {
                    $("#FormMode" + pageId).val("modified");
                }
                var ld = document.getElementById("LabelDefine" + pageId);
                if (!ld) return;
                opterateLabel();
                $("#btnSave" + pageId).click();

            });

            //#endregion 打印操作

            $('#btnQuery' + pageId).click(function () {
                var obj = $('#' + formId).serializeObject();
                var formvar = JSON.stringify(obj);
                jQuery('#' + gridId).jqGrid('setGridParam', { postData: { formVar: formvar} });
                jQuery('#' + gridId).trigger('reloadGrid');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>
