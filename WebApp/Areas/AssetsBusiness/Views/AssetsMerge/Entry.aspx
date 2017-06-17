<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsMerge.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<fieldset>
    <div class="editor-field">
        <div class="AssetsMergeEntryColumn1">
            <%:Html.AppLabelFor(m => m.AssetsMergeNo, Model.PageId, "AssetsMergeEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMergeNo, Model.PageId, "NoAssetsMergeEntry")%>
              <%:Html.AppNormalButton(Model.PageId, "btnAutoNo", "ui-icon-pencil", "autoNoButton")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <div class="AssetsMergeEntryColumn2">
            <%:Html.AppLabelFor(m => m.AssetsMergeName, Model.PageId, "AssetsMergeEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMergeName, Model.PageId, "AssetsMergeEntry")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsMergeNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsMergeName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.AssetsMergeId, Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    <%:Html.AppHiddenFor(m=>m.UpEntryGridId,Model.PageId)%>
    </fieldset>
    <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.UpEntryGridId, Url.Action("UpEntryGridData", new { formMode = Model.FormMode, primaryKey = Model.AssetsMergeId }), Model.UpEntryGridLayout, 100, true, 0, true, false, "btnSave", "AssetsMerge1")%>
    <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId,  Url.Action("EntryGridData", new { formMode = Model.FormMode, primaryKey = Model.AssetsMergeId }), Model.EntryGridLayout, 150, true, 0, true, false, "btnSave", "AssetsMerge2")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var upGridId = '<%=Model.UpEntryGridId %>' + pageId;
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            //#endregion 公共变量

            $('#btnAutoNo' + pageId).click(function () {
                $.ajax({
                    type: "POST",
                    url: '<%=Url.Action("GetAutoNo") %>',
                    datatype: "text",
                    success: function (data) {
                        $('#AssetsMergeNo' + pageId).val(data);
                    }
                });
                return false;
            });


            //#region grid操作
            onCellEditAssetsMerge1 = function (id) {
                $('#' + gridId + ' :input').live("change", function () {
                    $('#' + 'btnSave' + pageId).attr('disabled', false);
                    $('#' + 'btnApproveReturn' + pageId).attr('disabled', false);
                })
            }
            onCellEditAssetsMerge2 = function (id) {
                $('#' + gridId + ' :input').live("change", function () {
                    $('#' + 'btnSave' + pageId).attr('disabled', false);
                    $('#' + 'btnApproveReturn' + pageId).attr('disabled', false);
                })
            }
            $('#btnAdd' + upGridId, '#t_' + upGridId).live("click", function () {
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
                                                var dataRow = { AssetsId: rowdata["assetsId"], AssetsNo: rowdata["assetsNo"], AssetsName: rowdata["assetsName"], AssetsTypeId: rowdata["assetsTypeId"], AssetsClassId: rowdata["assetsClassId"], AssetsValue: rowdata["assetsValue"], Remark: "" };
                                                $('#' + upGridId).jqGrid('addRowData', pk, dataRow);
                                                $('#' + upGridId).jqGrid('editRow', pk, true);
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

            $('#btnAdd' + gridId, '#t_' + gridId).live("click", function () {
                var id = jQuery('#' + upGridId).jqGrid('getGridParam', 'selrow');
                var upData = jQuery('#' + upGridId).getDataIDs();
                var data = jQuery('#' + gridId).getDataIDs();
                var pk = 1;
                if (data.length < 1) {
                    if (id) {
                        var assetsId = jQuery('#' + upGridId).getCell(id, "AssetsId");
                        var assetsName = jQuery('#' + upGridId).getCell(id, "AssetsName");
                        var assetsTypeId = jQuery('#' + upGridId).getCell(id, "AssetsTypeId");
                        var assetsClassId = jQuery('#' + upGridId).getCell(id, "AssetsClassId");
                        var assetsValue = jQuery('#' + upGridId).getCell(id, "AssetsValue");
                        var dataRow = { AssetsId: "", AssetsNo: "", AssetsName: assetsName, AssetsTypeId: assetsTypeId, AssetsClassId: assetsClassId, AssetsValue: assetsValue, Remark: "", OriginalAssetsId: assetsId };
                        $('#' + gridId).jqGrid('addRowData', pk, dataRow);
                        $('#' + gridId).jqGrid('editRow', pk, true);
                    }
                    else {
                        if (upData.length > 0) {
                            var assetsId = jQuery('#' + upGridId).getCell(upData[0], "AssetsId");
                            var assetsName = jQuery('#' + upGridId).getCell(upData[0], "AssetsName");
                            var assetsTypeId = jQuery('#' + upGridId).getCell(upData[0], "AssetsTypeId");
                            var assetsClassId = jQuery('#' + upGridId).getCell(upData[0], "AssetsClassId");
                            var assetsValue = jQuery('#' + upGridId).getCell(upData[0], "AssetsValue");
                            var dataRow = { AssetsId: "", AssetsNo: "", AssetsName: assetsName, AssetsTypeId: assetsTypeId, AssetsClassId: assetsClassId, AssetsValue: assetsValue, Remark: "", OriginalAssetsId: assetsId };
                            $('#' + gridId).jqGrid('addRowData', pk, dataRow);
                            $('#' + gridId).jqGrid('editRow', pk, true);
                        }
                    }
                }
            });
            //#endregion grid操作
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabsLiContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabsDivContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>
