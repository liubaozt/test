<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.DataImport.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%:Html.AppFileUpload(Model.PageId, "ExcelUpload", AppMember.AppText["FileView"].ToString(), Url.Action("CheckExisting", "Home", new { Area = "" }) + "/excel",
                                        Url.Action("OriginUpload", "Home", new { Area = "" }) + "/excel", Url.Content("~/Content/css/uploadify/"), "true")%>
    <div id='excellist<%=Model.PageId %>'>
        <%:Html.AppEntryGridFor(this.Url, Model.PageId, "listExcelFile", Url.Action("EntryGridData"), Model.EntryGridLayout, 375, 0, "btnSave", "DataImport")%>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            //#endregion 公共变量
            $('#' + 'btnSave' + pageId).attr('disabled', false);
            var err = $('#ProcessDialog' + pageId).val();
            if (err == 'false') {
                $('#ProcessDialog' + pageId).html("");
            }

            AppUploadCompleteExcelUpload = function (file, data, response) {
                //$("#excellist" + pageId).append(data);
                var d = new Date();
                var s = '';
                s += d.getHours().toString();
                s += d.getMinutes().toString();
                s += d.getMilliseconds().toString();
                s += Math.floor(Math.random() * 100).toString();
                var sortNO = '';
                if (data == "公司.xls")
                    sortNO = 100;
                else if (data == "部门.xls")
                    sortNO = 105;
                else if (data == "购置方式.xls")
                    sortNO = 110;
                else if (data == "帐套.xls")
                    sortNO = 115;
                else if (data == "产权归属.xls")
                    sortNO = 120;
                else if (data == "报废方式.xls")
                    sortNO = 125;
                else if (data == "资产用途.xls")
                    sortNO = 130;
                else if (data == "存放地点.xls")
                    sortNO = 135;
                else if (data == "用户.xls")
                    sortNO = 140;
                else if (data == "供应商.xls")
                    sortNO = 145;
                else if (data == "资产分类.xls")
                    sortNO = 150;
                else if (data.substring(0, 3) == "资产_")
                    sortNO = 155;
                var dataRow = { seqno: s, fileName: data, sortNo: sortNO };
                $('#listExcelFile' + pageId).jqGrid('addRowData', s, dataRow);
                $('#listExcelFile' + pageId).jqGrid('editRow', s, true);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>
