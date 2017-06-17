<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsBorrow.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div class="editor-field">
            <div class="AssetsBorrowEntryColumn1">
                <%:Html.AppLabelFor(m => m.AssetsBorrowNo, Model.PageId, "AssetsBorrowEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsBorrowNo, Model.PageId, "NoAssetsBorrowEntry")%>
                <%:Html.AppNormalButton(Model.PageId, "btnAutoNo", "ui-icon-pencil", "autoNoButton")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <div class="AssetsBorrowEntryColumn2">
                <%:Html.AppLabelFor(m => m.AssetsBorrowName, Model.PageId, "AssetsBorrowEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsBorrowName, Model.PageId, "AssetsBorrowEntry")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <%:Html.ValidationMessageFor(m => m.AssetsBorrowNo, "AssetsBorrowEntry")%>
            <%:Html.ValidationMessageFor(m => m.AssetsBorrowName, "AssetsBorrowEntry")%>
        </div>
        <div class="editor-field">
            <div class="AssetsBorrowEntryColumn1">
                <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsBorrowEntry")%>
                <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsBorrowEntry")%>
            </div>
            <div class="AssetsBorrowEntryColumn2">
                <%:Html.AppLabelFor(m => m.BorrowPeople, Model.PageId, "AssetsBorrowEntry")%>
                <%:Html.AppDropDownEditorFor(m => m.BorrowPeople, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsBorrowEntry")%>
            </div>
            <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
            <%:Html.ValidationMessageFor(m => m.BorrowPeople)%>
        </div>
        <div class="editor-field">
            <div class="AssetsBorrowEntryColumn1">
                <%:Html.AppLabelFor(m => m.BorrowDate, Model.PageId, "AssetsBorrowEntry")%>
                <%:Html.AppDatePickerFor(m => m.BorrowDate, Model.PageId, "AssetsBorrowEntry")%>
                <%:Html.ValidationMessageFor(m => m.BorrowDate)%>
            </div>
        </div>
        <%:Html.AppHiddenFor(m => m.AssetsBorrowId, Model.PageId)%>
        <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    </fieldset>
    <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId, Url.Action("EntryGridData", new { formMode = Model.FormMode, primaryKey = Model.AssetsBorrowId }), Model == null ? null : Model.EntryGridLayout, 375, 0, "btnSave", "AssetsBorrow")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            //#endregion 公共变量

            
            $('#btnAutoNo' + pageId).click(function () {
                $.ajax({
                    type: "POST",
                    url: '<%=Url.Action("GetAutoNo") %>',
                    datatype: "text",
                    success: function (data) {
                        $('#AssetsBorrowNo' + pageId).val(data);
                    }
                });
                return false;
            });

            //#region grid操作
            onCellEditAssetsBorrow = function (id) {
                $('#' + gridId + ' :input').live("change", function () {
                    $('#' + 'btnSave' + pageId).attr('disabled', false);
                    $('#' + 'btnApproveReturn' + pageId).attr('disabled', false);
                });
                jQuery("#" + id + "_PlanReturnDate", '#' + gridId).datepicker({ dateFormat: "yy-mm-dd" });
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
                                                var dataRow = { AssetsId: rowdata["assetsId"], AssetsNo: rowdata["assetsNo"], AssetsName: rowdata["assetsName"],
                                                    AssetsTypeId: rowdata["assetsTypeId"], CompanyId: rowdata["companyId"], DepartmentId: rowdata["departmentName"], UsePeople: rowdata["usePeopleName"],
                                                    Keeper: rowdata["keeperName"], AssetsClassId: rowdata["assetsClassId"], Remark: ""
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
            //#endregion grid操作

            //#region 使用人保管人过滤
            var departmentIdObj = "#DepartmentId" + pageId;
            var borrowPeopleObj = "#BorrowPeople" + pageId;
            $(departmentIdObj).change(function () {
                var departmentId = $(departmentIdObj).val();
                if ($.trim(departmentId) == "") {
                    departmentId = "none";
                }
                var urlStr = '<%=Url.Action("DropList", "User", new { Area = "BusinessCommon"}) %>' + '/?filterExpression=departmentId=' + '<%=DFT.SQ %>' + departmentId + '<%=DFT.SQ %>';
                $.getJSON(urlStr, function (data) {
                    AppAppendSelect2(data, borrowPeopleObj, urlStr);
                });
            });
            //#endregion 使用人保管人过滤

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabsLiContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabsDivContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ButtonContent" runat="server">
    <%  if (Model.FormMode != "approve" && !Model.FormMode.Contains("view"))
        { %>
    <%:Html.AppNormalButton(Model.PageId, "btnAdd", AppMember.AppText["BtnAdd"])%>
    <%:Html.AppNormalButton(Model.PageId, "btnDelete", AppMember.AppText["BtnDelete"])%>
    <% } %>
</asp:Content>
