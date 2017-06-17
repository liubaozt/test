<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsCheck.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%if (Model.FormMode == "new" || Model.FormMode == "new2")
  {%>
     <fieldset style="background-color:#DEDEDE">
        <div class="editor-field">
            <div class="AssetsCheckEntryColumn1">
                <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsCheckEntry")%>
                <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsCheckEntry")%>
            </div>
            <div class="AssetsCheckEntryColumn2">
                <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "AssetsCheckEntry")%>
                <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "AssetsCheckEntry")%>
            </div>
            <div class="AssetsCheckEntryColumn3">
                <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "AssetsCheckEntry")%>
                <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId, Model.AssetsClassAddFavoritUrl, Model.AssetsClassReplaceFavoritUrl, "AssetsCheckEntry")%>
            </div>
            <%:Html.AppNormalButton(Model.PageId, "btnQuery", AppMember.AppText["BtnQuery"], false)%>
        </div>
    </fieldset>
   <%} %>
    <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId, Url.Action("EntryGridData", new { formMode = Model.FormMode, primaryKey = Model.AssetsCheckId }), Model.EntryGridLayout, 350, 0, "btnSave", "AssetsCheck",true)%>
     <fieldset >
        <div class="editor-field" >
            <div class="AssetsCheckEntryColumn1">
                <%:Html.AppLabelFor(m => m.AssetsCheckNo, Model.PageId, "AssetsCheckEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsCheckNo, Model.PageId, "NoAssetsCheckEntry")%>
                <%:Html.AppNormalButton(Model.PageId, "btnAutoNo", "ui-icon-pencil", "autoNoButton")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <div class="AssetsCheckEntryColumn2">
                <%:Html.AppLabelFor(m => m.AssetsCheckName, Model.PageId, "AssetsCheckEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsCheckName, Model.PageId, "AssetsCheckEntry")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <div class="AssetsCheckEntryColumn3">
                <%:Html.AppLabelFor(m => m.CheckDate, Model.PageId, "AssetsCheckEntry")%>
                <%:Html.AppDatePickerFor(m => m.CheckDate, Model.PageId, "AssetsCheckEntry")%>
            </div>
            <%:Html.ValidationMessageFor(m => m.AssetsCheckNo)%>
            <%:Html.ValidationMessageFor(m => m.AssetsCheckName)%>
            <%:Html.ValidationMessageFor(m => m.CheckDate)%>
        </div>
        <div id="grid<%=Model.PageId %>Dialog">
        </div>
        <%:Html.AppHiddenFor(m => m.AssetsCheckId, Model.PageId)%>
        <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    </fieldset>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            var formMode = '<%=Model.FormMode %>';
            var formId = '<%=Model.FormId %>' + pageId;
            //#endregion 公共变量
      

            $('#btnAutoNo' + pageId).click(function () {
                $.ajax({
                    type: "POST",
                    url: '<%=Url.Action("GetAutoNo") %>',
                    datatype: "text",
                    success: function (data) {
                        $('#AssetsCheckNo' + pageId).val(data);
                    }
                });
                return false;
            });

            $('#btnQuery' + pageId).click(function () {
                var obj = $('#' + formId).serializeObject();
                var formvar = JSON.stringify(obj);
                jQuery('#' + gridId).jqGrid('setGridParam', { postData: { formVar: formvar} });
                jQuery('#' + gridId).trigger('reloadGrid');
            });

            //#region grid操作

            loadGridCompleteAssetsCheck = function () {
                setTimeout(function () {
                    var ids = $('#' + gridId).getDataIDs();
                    for (i = 0; i < ids.length; i++) {
                        var sid = $('#' + ids[i] + '_ActualStoreSiteName', '#' + gridId).val();
                        var vid = $('#' + ids[i] + '_ActualStoreSiteId', '#' + gridId).val();
                        var txt = $('#' + ids[i] + '_ActualStoreSiteText', '#' + gridId).val();
                        if ($.trim(sid).length < 1) {
                            $("<option></option>")
                            .val(vid)
                            .text(txt)
                            .appendTo($('#' + ids[i] + "_" + "ActualStoreSiteName", '#' + gridId));
                            $('#' + ids[i] + "_" + "ActualStoreSiteName", '#' + gridId).val(vid);
                        }
                    }
                }, 2000);
            }

            onCellEditAssetsCheck = function (id) {
                $('#' + gridId + ' :input').live("change", function () {
                    $('#' + 'btnSave' + pageId).attr('disabled', false);
                    $('#' + 'btnApproveReturn' + pageId).attr('disabled', false);
                });
                jQuery("#" + id + "_ActualCheckDate", '#' + gridId).datepicker({ dateFormat: "yy-mm-dd" });

                $('#' + id + "_" + "ActualStoreSiteName", '#' + gridId).live("change", function () {
                    var sId = $('#' + id + "_" + "ActualStoreSiteName", '#' + gridId).val();
                    jQuery('#' + gridId).jqGrid('setRowData', id, { ActualStoreSiteId: sId });
                });
                $('#' + id + "_" + "ActualStoreSiteBtn", '#' + gridId).button({
                    text: true
                });
                $('#' + id + "_" + "ActualStoreSiteBtn", '#' + gridId).height(21);
                $('#' + id + "_" + "ActualStoreSiteBtn", '#' + gridId).click(function () {
                    AppTreeDialogButtonClick2('#' + id + "_" + "ActualStoreSiteName", '#' + gridId, '<%=TreeId.StoreSiteTreeId %>' + pageId, pageId, '<%=Url.Action("Select", "StoreSite", new { Area = "BasicData" }) %>', '<%=AppMember.AppText["StoreSiteSelect"] %>', '<%= Url.Action("DropList", "StoreSite", new { Area = "BasicData"})%>', '<%=Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" }) %>', '<%= Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" }) %>');
                });
                $('#' + id + "_" + "CheckResultName", '#' + gridId).live("change", function () {
                    var sId = $('#' + id + "_" + "CheckResultName", '#' + gridId).val();
                    jQuery('#' + gridId).jqGrid('setRowData', id, { CheckResultId: sId });
                });
            }
            $('#btnDelete' + pageId).click(function () {
                var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                $('#' + gridId).jqGrid('delRowData', id);
            });
            $('#btnQuery' + pageId).click(function () {
                var obj = $('#EntryForm' + pageId).serializeObject();
                var formvar = JSON.stringify(obj);
                $('#' + gridId).jqGrid('setGridParam', { postData: { formVar: formvar} });
                $('#' + gridId).trigger('reloadGrid');
            });
            $('#btnAdd' + pageId).click(function () {
                var spageId = pageId + "s";
                $.ajax({
                    type: "POST",
                    url: '<%:Model.SelectUrl %>',
                    data: { pageId: spageId, selectMode: "AssetsSelect", assetsState: "A,B,X,W,F,RI,SI,MI,TI,WI,BI,HI,J" },
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
                                    var arrGridData = new Array();
                                    var count = 0;
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
                                                //var dataRow = { AssetsId: rowdata["assetsId"], AssetsNo: rowdata["assetsNo"], AssetsName: rowdata["assetsName"], AssetsTypeId: rowdata["assetsTypeId"], AssetsClassId: rowdata["assetsClassId"], CompanyId: rowdata["companyId"], DepartmentId: rowdata["departmentId"], DepartmentName: rowdata["departmentName"], StoreSiteId: rowdata["storeSiteId"], StoreSiteName: rowdata["storeSiteName"], Remark: "" };
                                                //$('#' + gridId).jqGrid('addRowData', pk, dataRow);
                                                //$('#' + gridId).jqGrid('editRow', pk, true);
                                                var dataRow = { id: pk, cell: [rowdata["assetsId"], rowdata["assetsNo"], rowdata["assetsName"], rowdata["assetsTypeId"], rowdata["assetsClassId"], rowdata["companyId"], rowdata["departmentId"], rowdata["departmentName"], rowdata["storeSiteId"], rowdata["storeSiteName"], rowdata["assetsBarcode"]] };
                                                arrGridData.push(dataRow);
                                                count++;
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



        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabsLiContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabsDivContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ButtonContent" runat="server">
 <%--   <% if (Model.FormMode != "approve" && Model.FormMode != "actual" && !Model.FormMode.Contains("view") && Model.FormMode != "delete")
       { %>
    <%:Html.AppNormalButton(Model.PageId, "btnAdd", AppMember.AppText["BtnAdd"])%>
    <%:Html.AppNormalButton(Model.PageId, "btnDelete", AppMember.AppText["BtnDelete"])%>
    <% } %>--%>
</asp:Content>
