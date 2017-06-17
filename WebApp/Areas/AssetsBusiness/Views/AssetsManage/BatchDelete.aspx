<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Base.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsManage.BatchDeleteModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("AssetsSelect");%>
    <br />
    <%:Html.AppNormalButton(Model.PageId, "btnQuery", AppMember.AppText["BtnQuery"])%>
    <%:Html.AppNormalButton(Model.PageId, "btnDeletee", AppMember.AppText["BtnDelete"])%>
    <%:Html.AppHiddenFor(m => m.FormMode, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.PageId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.FormId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.GridFormId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.GridId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId%>';
            //#endregion 公共变量
            $('#btnQuery' + pageId).click(function () {
                var obj = $('#SelectForm' + pageId).serializeObject();
                var formvar = JSON.stringify(obj);
                jQuery('#BatchGrid' + pageId).jqGrid('setGridParam', { postData: { formVar: formvar} });
                jQuery('#BatchGrid' + pageId).trigger('reloadGrid');
            });
            $('#btnDeletee' + pageId).click(function () {
                var isCascadeDelete= $("#IsCascadeDelete"+pageId).val();
                var arrRow = jQuery('#BatchGrid' + pageId).jqGrid('getGridParam', 'selarrrow');
                var griddata = new Array();
                var rowdata
                var j = 0;
                var ids = "";
                var rowids = new Array();
                if (arrRow.length > 0) {
                    for (var i = 0; i < arrRow.length; i++) {
                        if (arrRow[i] != undefined) {
                            rowdata = jQuery('#BatchGrid' + pageId).getRowData(arrRow[i]);
                            griddata[j] = rowdata;
                            rowids[j] = arrRow[i];
                            ids += "'" + rowdata["assetsId"] + "',";
                            j++;
                        }
                    }
                }
                var gridstring = JSON.stringify(griddata);
                $.ajax({
                    type: "POST",
                    url: '<%=Url.Action("BatchDelete") %>',
                    data: { pageId: pageId, viewTitle: "AssetsSelect", gridString: ids, isCascadeDelete: isCascadeDelete },
                    datatype: "text",
                    success: function (data) {
                        if (data == "1") {
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["DeleteSucceed"]%>', 'success', function () { });
                            for (var k = 0; k < rowids.length; k++) {
                                jQuery('#BatchGrid' + pageId).jqGrid('delRowData', rowids[k]);
                            }
                        }
                        else
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', data, 'error', function () { });
                    }
                });
            });
        });
    </script>
</asp:Content>
