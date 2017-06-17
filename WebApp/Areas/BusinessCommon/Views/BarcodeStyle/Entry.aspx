<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.BarcodeStyle.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.BarcodeStyleName, Model.PageId, "BarcodeStyleEntry")%>
        <%:Html.AppTextBoxFor(m => m.BarcodeStyleName, Model.PageId, "BarcodeStyleEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.BarcodeStyleName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Remark, Model.PageId, "BarcodeStyleEntry")%>
        <%:Html.AppTextBoxFor(m => m.Remark, Model.PageId, "BarcodeStyleEntry")%>
        <%:Html.ValidationMessageFor(m => m.Remark)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.IsDefaultStyle, Model.PageId, "BarcodeStyleEntry")%>
        <%:Html.CheckBoxFor(m => m.IsDefaultStyle, "BarcodeStyleEntry")%>
        <%:Html.ValidationMessageFor(m => m.IsDefaultStyle)%>
    </div>
    <div id="BarcodeRadio" class="editor-field">
        <%:Html.RadioButtonFor(m=>m.Sync, "sync", true)%>画面尺寸大小同步到属性
        <%:Html.RadioButtonFor(m => m.Sync, "resync", true)%>尺寸大小属性同步到画面
    </div>
    <%:Html.AppHiddenFor(m => m.BarcodeStyleId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.StyleJson, Model.PageId)%>
    <div class="ui-layout-center ui-helper-reset ui-widget-content">
        <div id="barcodeFlow" class="myflowbg" style="height: 350px">
        </div>
        <div id="mylabel_tools" style="position: absolute; background-color: #fff; width: 70px;
            cursor: default; padding: 3px;" class="ui-widget-content">
            <div id="mylabel_tools_handle" style="text-align: center;" class="ui-widget-header">
                工具集</div>
            <div class="node" id="mylabel_save">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/save.gif") %>' />&nbsp;&nbsp;保存</div>
            <div>
                <hr />
            </div>
            <div class="node state" id="staticField" type="staticField">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/text.png") %>' />&nbsp;&nbsp;文本</div>
            <div class="node state" id="textField" type="textField">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/task_java.png") %>' />&nbsp;&nbsp;字段</div>
            <div class="node state" id="barcode" type="barcode">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/barcode.png") %>' />&nbsp;&nbsp;条码</div>
            <div class="node state" id="imageField" type="imageField">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/picture.png") %>' />&nbsp;&nbsp;图片</div>
            <div class="node state" id="rectangle" type="rectangle">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/task_empty.png") %>' />&nbsp;&nbsp;边框</div>
        </div>
        <div id="mylabel_props" style="position: absolute; background-color: #fff; width: 220px;
            padding: 3px;" class="ui-widget-content">
            <div id="mylabel_props_handle" class="ui-widget-header">
                属性</div>
            <table border="1" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        aaa
                    </td>
                </tr>
                <tr>
                    <td>
                        aaa
                    </td>
                </tr>
            </table>
            <div>
                &nbsp;</div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var workflowImgPath = '<%=Url.Content("~/Scripts/used/workflow/img") %>';
        var imageFieldJson = '<%=Url.Action("DropList", "CodeTable", new { Area = "", filter = "ImageField" }) %>';
        var barcodeFieldJson = '<%=Url.Action("DropList", "CodeTable", new { Area = "" , filter = "BarcodeField"}) %>';
        var textFieldJson = '<%=Url.Action("DropList", "CodeTable", new { Area = "" , filter = "TextField"}) %>';
        var barcodeTypeJson = '<%=Url.Action("DropList", "CodeTable", new { Area = "", filter = "BarcodeType" }) %>';
    </script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/lib/raphael-min.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/mylabel.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/mylabel.barcode.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/mylabel.editors.js") %>'></script>
    <script type="text/javascript">
        $(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId%>';
            var wfJson = "(" + "<%=Model.StyleJson%>" + ")";
            //#endregion 公共变量

            //#region 初始化
            $("#BarcodeRadio :radio")[0].checked = true;
            $('#barcodeFlow')
				.mylabel(
						{
						    basePath: "",
						    restore: eval(wfJson),
						    tools: {
						        save: {
						            onclick: function (data) {
						                $('#StyleJson' + pageId).val(data);
						                AppMessage(pageId, '系统消息', '设计临时保存成功', 'success', function () { });
						                $('#' + 'btnSave' + pageId).attr('disabled', false);
						            }
						        }
						    }
						});
						$('#mylabel_tools').css({ left: 20, top: 180 });
						$('#mylabel_props').css({ right: 20, top: 180 });
            //#endregion 初始化

        });

    </script>
    <style type="text/css">
        .node
        {
            width: 70px;
            text-align: center;
            vertical-align: middle;
            border: 1px solid #fff;
        }
        .mover
        {
            border: 1px solid #ddd;
            background-color: #ddd;
        }
        .selected
        {
            background-color: #ddd;
        }
        .state
        {
        }
        
        #pointer
        {
            background-repeat: no-repeat;
            background-position: center;
        }
        #path
        {
            background-repeat: no-repeat;
            background-position: center;
        }
        #task
        {
            background-repeat: no-repeat;
            background-position: center;
        }
        #state
        {
            background-repeat: no-repeat;
            background-position: center;
        }
    </style>
</asp:Content>
