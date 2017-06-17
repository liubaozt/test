<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsPurchase.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div class="editor-field">
            <div class="AssetsPurchaseEntryColumn1">
                <%:Html.AppLabelFor(m => m.AssetsPurchaseNo, Model.PageId, "AssetsPurchaseEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsPurchaseNo, Model.PageId, "NoAssetsPurchaseEntry")%>
                <%:Html.AppNormalButton(Model.PageId, "btnAutoNo", "ui-icon-pencil", "autoNoButton")%>
                <%:Html.AppRequiredFlag()%>
            </div>
            <div class="AssetsPurchaseEntryColumn2">
                <%:Html.AppLabelFor(m => m.AssetsPurchaseName, Model.PageId, "AssetsPurchaseEntry")%>
                <%:Html.AppTextBoxFor(m => m.AssetsPurchaseName, Model.PageId, "AssetsPurchaseEntry")%>
            </div>
            <div class="AssetsPurchaseEntryColumn3">
                <%:Html.AppLabelFor(m => m.PurchaseDate, Model.PageId, "AssetsPurchaseEntry")%>
                <%:Html.AppDatePickerFor(m => m.PurchaseDate, Model.PageId, "AssetsPurchaseEntry")%>
            </div>
            <%:Html.ValidationMessageFor(m => m.AssetsPurchaseNo)%>
             <%:Html.ValidationMessageFor(m => m.AssetsPurchaseName)%>
            <%:Html.ValidationMessageFor(m => m.PurchaseDate)%> 
        </div>
        <%:Html.AppHiddenFor(m => m.AssetsPurchaseId, Model.PageId)%>
        <%:Html.AppHiddenFor(m=>m.EntryGridId,Model.PageId)%>
    </fieldset>
    <%:Html.AppEntryGridFor(this.Url, Model.PageId, Model.EntryGridId, Url.Action("EntryGridData", new { formMode = Model.FormMode, primaryKey = Model.AssetsPurchaseId }), Model.EntryGridLayout, 350,false, 0,true,false, "btnSave", "AssetsPurchase")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            var gridId = '<%=Model.EntryGridId %>' + pageId;
            var formMode = '<%=Model.FormMode %>';
            //#endregion 公共变量

            if (formMode == "fix") {
                $('#' + 'btnSave' + pageId).hide();
            }

            loadGridCompleteAssetsPurchase = function () {
                if (formMode == "new") {
                    $('#btnAdd' + gridId, '#t_' + gridId).show();
                    $('#btnDelete' + gridId, '#t_' + gridId).show();
                }

                if (formMode == "fix") {
                    var btnFixed = 'btnFixed' + gridId;
                    $('#t_' + gridId).append("<input id=" + btnFixed + " type='button' value='转固' style='height:20px;font-size:-1;line-height: 0.8;'/>");
                    $('#' + btnFixed, '#t_' + gridId).button({
                        text: true
                    });

                    $('#' + btnFixed, '#t_' + gridId).click(function () {
                        var spageId = pageId + "9";
                        var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                        var rowdata = jQuery('#' + gridId).getRowData(id);
                        var purchaseobj = JSON.stringify(rowdata);

                        var selid = $('#' + gridId).jqGrid('getGridParam', 'selrow');
                        if (!selid) {
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '请选择一行！', 'warning', function () { });
                            return;
                        }

                        var urlStr = '<%=Url.Action("HadApproved", "AssetsPurchase", new { Area = "AssetsBusiness"}) %>';
                        $.ajax({
                            url: urlStr,
                            type: "POST",
                            dataType: "text",
                            data: { purchaseObj: purchaseobj },
                            success: function (data) {
                                if (data == "true") {
                                    var maintab = jQuery('#tabs', '#RightPane');
                                    var st = "#t" + spageId;
                                    if ($(st).html() != null) {
                                        maintab.tabs('select', st);
                                    } else {
                                        maintab.tabs('add', st, "资产转固[" + id + "]");
                                        //$(st,"#tabs").load(treedata.url);
                                        var navurl = '<%:Model.AssetsFixUrl %>';
                                        $.ajax({
                                            url: '<%:Model.AssetsFixUrl %>',
                                            type: "GET",
                                            dataType: "html",
                                            data: { pageId: spageId, formMode: "new2", viewTitle: "资产转固", purchaseObj: purchaseobj },
                                            complete: function (req, err) {
                                                $(st, "#tabs").append(req.responseText);
                                            }
                                        });
                                    }
                                }
                                else {
                                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '审批没有完成不能转固！', 'warning', function () { });
                                }
                            }
                        });
                    });
                }

                $('#btnAdd' + gridId, '#t_' + gridId).click(function () {
                    var spageId = pageId + "dtl";
                    $.ajax({
                        type: "POST",
                        url: '<%:Model.DetailUrl %>',
                        data: { pageId: spageId, detailMode: "add" },
                        datatype: "html",
                        success: function (data) {
                            $("#SelectDialog" + pageId).html(data).dialog({
                                title: '<%=AppMember.AppText["PurchaseDetailInfo"]%>',
                                height: 180,
                                width: 750,
                                modal: true,
                                resizable: true,
                                buttons: {
                                    '<%=AppMember.AppText["BtnConfirm"]%>': function () {
                                        var departmentName = $("#DepartmentId" + spageId + " option:selected").text();
                                        var storeSiteName = $("#StoreSiteId" + spageId + " option:selected").text();
                                        var dataRow = { assetsName: $('#AssetsName' + spageId).val(),
                                            departmentId: $('#DepartmentId' + spageId).val(), departmentName: departmentName,
                                            storeSiteId: $('#StoreSiteId' + spageId).val(), storeSiteName: storeSiteName,
                                            usePeople: $('#UsePeople' + spageId).val(), keeper: $('#Keeper' + spageId).val(),
                                            hasFixed: 'N', hasFixedText: '否',
                                            remark: $('#Remark' + spageId).val(), assetsValue: $('#AssetsValue' + spageId).val()
                                        };
                                        $('#' + gridId).jqGrid('addRowData', 1, dataRow);
                                        $(this).dialog("close");
                                    },
                                    '<%=AppMember.AppText["BtnCancel"]%>': function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                    });
                });


            }

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
                        $('#AssetsPurchaseNo' + pageId).val(data);
                    }
                });
                return false;
            });

            $('#btnSave' + pageId).mousedown(function () {
                var assetsTransferNo = $('#AssetsPurchaseNo' + pageId).val();
                if (!assetsTransferNo) {
                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["AssetsPurchaseNo"]+ AppMember.AppText["Require"]%>', 'error', function () { });

                }
            });

            //#region grid操作
            onCellEditAssetsPurchase = function (id) {
                $('#' + gridId + ' :input').live("change", function () {
                    $('#' + 'btnSave' + pageId).attr('disabled', false);
                    $('#' + 'btnApproveReturn' + pageId).attr('disabled', false);
                })
                $('#' + id + "_" + "ScrapTypeName", '#' + gridId).live("change", function () {
                    var sId = $('#' + id + "_" + "ScrapTypeName", '#' + gridId).val();
                    jQuery('#' + gridId).jqGrid('setRowData', id, { ScrapTypeId: sId });
                })
            }
            $('#btnDelete' + pageId).click(function () {
                var id = jQuery('#' + gridId).jqGrid('getGridParam', 'selrow');
                $('#' + gridId).jqGrid('delRowData', id);
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
<%-- <%if (Model.FormMode != "approve" && !Model.FormMode.Contains("view"))
      { %>
    <%:Html.AppNormalButton(Model.PageId, "btnAdd", AppMember.AppText["BtnAdd"])%>
    <%:Html.AppNormalButton(Model.PageId, "btnDelete", AppMember.AppText["BtnDelete"])%>
    <% } %>--%>
</asp:Content>
