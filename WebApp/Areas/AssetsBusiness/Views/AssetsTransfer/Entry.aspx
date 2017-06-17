<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsTransfer.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div class="editor-field">
            <div class="AssetsTransferEntryColumn1">
                <%:Html.AppLabelFor(m => m.AssetsTransferNo, Model.PageId, "AssetsTransferEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsTransferNo, Model.PageId, "NoAssetsTransferEntry")%>
                <%:Html.AppNormalButton(Model.PageId, "btnAutoNo", "ui-icon-pencil", "autoNoButton")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <div class="AssetsTransferEntryColumn2">
                <%:Html.AppLabelFor(m => m.TransferDate, Model.PageId, "AssetsTransferEntry")%>
                <%:Html.AppDatePickerFor(m => m.TransferDate, Model.PageId, "AssetsTransferEntry")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <%:Html.ValidationMessageFor(m => m.AssetsTransferNo)%>
            <%:Html.ValidationMessageFor(m => m.TransferDate)%>
        </div>
        <div class="editor-field">
            <div class="AssetsTransferEntryColumn1">
                <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsTransferEntry")%>
                <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsTransferEntry")%>
            </div>
            <div class="AssetsTransferEntryColumn1">
                <%:Html.AppLabelFor(m => m.UsePeople, Model.PageId, "AssetsTransferEntry")%>
                <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
                  { %>
                <%:Html.AppDropDownEditorFor(m => m.UsePeople, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsTransferEntry")%>
                <%}
                  else
                  { %>
                <%:Html.AppAutoCompleteFor(m => m.UsePeople, Model.PageId, "AssetsTransferEntry", Model.UserSource)%>
                <%}  %>
            </div>
            <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
            <%:Html.ValidationMessageFor(m => m.UsePeople)%>
        </div>
        <div class="editor-field">
            <div class="AssetsTransferEntryColumn2">
                <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "AssetsTransferEntry")%>
                <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "AssetsTransferEntry")%>
            </div>
            <div class="AssetsTransferEntryColumn2">
                <%:Html.AppLabelFor(m => m.Keeper, Model.PageId, "AssetsTransferEntry")%>
                <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
                  { %>
                <%:Html.AppDropDownEditorFor(m => m.Keeper, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsTransferEntry")%>
                <%}
                  else
                  { %>
                <%:Html.AppAutoCompleteFor(m => m.Keeper, Model.PageId, "AssetsTransferEntry", Model.UserSource)%>
                <%}  %>
            </div>
            <%:Html.ValidationMessageFor(m => m.StoreSiteId)%>
            <%:Html.ValidationMessageFor(m => m.Keeper)%>
        </div>
        <%:Html.AppHiddenFor(m => m.AssetsTransferId, Model.PageId)%>
        <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    </fieldset>
    <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId,  Url.Action("EntryGridData", new { formMode = Model.FormMode, primaryKey = Model.AssetsTransferId }), Model.EntryGridLayout, 400, 0, "btnSave", "AssetsTransfer")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            var departmentIdObj = "#DepartmentId" + pageId;
            var usePeopleObj = "#UsePeople" + pageId;
            var keeperObj = "#Keeper" + pageId;
            var formMode = '<%=Model.FormMode %>';
            //#endregion 公共变量
    
            if (formMode == "approveinfo") {
                $('#' + 'btnAdd' + pageId).hide();
                $('#' + 'btnDelete' + pageId).hide();
            }

            $('#btnAutoNo' + pageId).click(function () {
                $.ajax({
                    type: "POST",
                    url: '<%=Url.Action("GetAutoNo") %>',
                    datatype: "text",
                    success: function (data) {
                        $('#AssetsTransferNo' + pageId).val(data);
                    }
                });
                return false;
            });

            $('#btnSave' + pageId).mousedown(function () {
                var assetsTransferNo = $('#AssetsTransferNo' + pageId).val();
                if (!assetsTransferNo) {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["AssetsTransferNo"]+ AppMember.AppText["Require"]%>', 'error', function () { });
          
                }
                var transferDate = $('#TransferDate' + pageId).val();
                if (!transferDate) {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["TransferDate"]+ AppMember.AppText["Require"]%>', 'error', function () { });
 
                }
            });

            //#region grid操作
            onCellEditAssetsTransfer = function (id) {
                $('#' + gridId + ' :input').live("change", function () {
                    $('#' + 'btnSave' + pageId).attr('disabled', false);
                    $('#' + 'btnApproveReturn' + pageId).attr('disabled', false);
                })
                $('#' + id + "_" + "AssetsStateName", '#' + gridId).live("change", function () {
                    var sId = $('#' + id + "_" + "AssetsStateName", '#' + gridId).val();
                    jQuery('#' + gridId).jqGrid('setRowData', id, { AssetsStateId: sId });
                })
            }
            $('#btnDelete' + pageId).click(function () {
                var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                $('#' + gridId).jqGrid('delRowData', id);
            });
            $('#btnAdd' + pageId).click(function () {
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
                                    //jQuery('#list' + spageId).jqGrid('setPostData', { formVar: formvar });
                                    jQuery('#list' + spageId).jqGrid('setGridParam', { postData: { formVar: formvar} });
                                    jQuery('#list' + spageId).trigger('reloadGrid');
                                },
                                '<%=AppMember.AppText["BtnConfirm"]%>': function () {
                                    var arrRow = jQuery('#list' + spageId).jqGrid('getGridParam', 'selarrrow');
                                    var curGridData = $('#' + gridId).getDataIDs();
                                    if (arrRow.length > 0) {
                                        var rowdata;
                                        for (var i = 0; i < arrRow.length; i++) {
                                            rowdata = jQuery('#list' + spageId).getRowData(arrRow[i]);
                                            var pk = rowdata["assetsId"];
                                            var hasPk = false;

                                            for (j = 0; j < curGridData.length; j++) {
                                                if (pk == curGridData[j])
                                                    hasPk = true;
                                            }
                                            if (hasPk == false && pk) {
                                                var dataRow = { AssetsId: rowdata["assetsId"], AssetsNo: rowdata["assetsNo"], AssetsName: rowdata["assetsName"], AssetsTypeId: rowdata["assetsTypeId"], AssetsClassId: rowdata["assetsClassId"],
                                                    CompanyId: rowdata["companyId"], OriginalDepartmentId: rowdata["departmentId"], OriginalDepartmentName: rowdata["departmentName"], OriginalStoreSiteId: rowdata["storeSiteId"], OriginalStoreSiteName: rowdata["storeSiteName"],
                                                    OriginalKeeper: rowdata["keeper"], OriginalKeeperName: rowdata["keeperName"], OriginalUsePeople: rowdata["usePeople"], OriginalUsePeopleName: rowdata["usePeopleName"], spec: rowdata["spec"], assetsQty: rowdata["assetsQty"], Remark: ""
                                                };
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
            //#endregion grid变量

            //#region 保管人使用人过滤
            $(departmentIdObj).change(function () {
                var departmentId = $(departmentIdObj).val();
                if ($.trim(departmentId) == "") {
                    departmentId = "none";
                }
                var urlStr = '<%=Url.Action("DropList", "User", new { Area = "BusinessCommon"}) %>' + '/?filterExpression=departmentId=' + '<%=DFT.SQ %>' + departmentId + '<%=DFT.SQ %>';
                $.getJSON(urlStr, function (data) {
                    AppAppendSelect2(data, usePeopleObj, urlStr);
                    AppAppendSelect2(data, keeperObj, urlStr);
                });
            });
            //#endregion 保管人使用人过滤
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabsLiContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabsDivContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ButtonContent" runat="server">
    <% if (Model.FormMode != "approve" && !Model.FormMode.Contains("view"))
       { %>
    <%:Html.AppNormalButton(Model.PageId, "btnAdd", AppMember.AppText["BtnAdd"])%>
    <%:Html.AppNormalButton(Model.PageId, "btnDelete", AppMember.AppText["BtnDelete"])%>
    <% } %>
</asp:Content>
