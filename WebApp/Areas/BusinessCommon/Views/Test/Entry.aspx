<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Test.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.GroupId, Model.PageId, "width: 80px; float: left; text-align:right;")%>
        <%:Html.AppDropDownListFor(m => m.GroupId, Model.PageId, Url.Action("DropList", "Group", new { Area = "BusinessCommon" }), "width: 80px; float: left;")%>
        <%:Html.ValidationMessageFor(m => m.GroupId)%>
        <%:Html.AppLabelFor(m => m.CreateTime, Model.PageId, "width: 80px; float: left; text-align:right;")%>
        <%:Html.AppDatePickerFor(m => m.CreateTime, Model.PageId, "width: 80px; float: left;")%>
    </div>
      <%:Html.AppFileUpload(Model.PageId, "file_upload", AppMember.AppText["FileView"].ToString(), Url.Action("CheckExisting", "Home", new { Area = "" }) + "/assets",
                            Url.Action("Upload", "Home", new { Area = "" }) + "/assets", Url.Content("~/Content/css/uploadify/uploadify.swf"), "true")%>
  <%--  <%:Html.AppEditGridFor(this.Url, Model.PageId, Model.GridId, AppMember.AppText["UserList"].ToString(), Url.Action("GridData"),  Model.GridLayout,  200,500,true,true,"btnSave","Test")%>--%>
    <%:Html.AppHiddenFor(m=>m.GridId,Model.PageId)%>
    <table style="width:100px">
    <tr></tr>
    </table>
    <div id="myPrintArea"></div>
    <input type="button" id="biuuu_button" value="print"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/jquery-barcode-2.0.2.min.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/jquery.PrintArea.js") %>'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var pageId = '<%=Model.PageId %>';
            var gridId = "#" + '<%=Model.GridId %>' + pageId;
            onCellEditTest = function (id) {
                $('#' + id + "_" + "UserNo", gridId).live("change", function () {
                    alert("111");
                    var uId = $('#' + id + "_" + "UserNo", gridId).val();
                    jQuery(gridId).jqGrid('setRowData', id, { UserId: uId });
                })
                jQuery("#" + id + "_CreateTime", gridId).datepicker({ dateFormat: "yy-mm-dd" });
            }
            $("#myPrintArea").barcode("1234567890128", "code39",{showHRI:false});
            $("#biuuu_button").click(function () {
                $("div#myPrintArea").printArea();
            });
        });
    </script>
</asp:Content>
