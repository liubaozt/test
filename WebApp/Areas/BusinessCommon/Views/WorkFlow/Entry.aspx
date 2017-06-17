<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.WorkFlow.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="WorkFlowEntryColumn1">
            <%:Html.AppLabelFor(m => m.WfName, Model.PageId,"WorkFlowEntry")%>
            <%:Html.AppTextBoxFor(m => m.WfName, Model.PageId, "WorkFlowEntry")%>
            <%:Html.AppRequiredFlag()%>
            <%:Html.ValidationMessageFor(m => m.WfName)%>
        </div>
        <div class="WorkFlowEntryColumn2">
            <%:Html.AppLabelFor(m => m.ApproveTable, Model.PageId, "WorkFlowEntry")%>
            <%:Html.AppDropDownListFor(m => m.ApproveTable, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "ApproveTable" }), "WorkFlowEntry")%>
            <%:Html.AppRequiredFlag()%>
            <%:Html.ValidationMessageFor(m => m.ApproveTable)%>
        </div>
        <div class="WorkFlowEntryColumn3">
            <%:Html.AppLabelFor(m => m.Remark, Model.PageId, "WorkFlowEntry")%>
            <%:Html.AppTextBoxFor(m => m.Remark, Model.PageId, "WorkFlowEntry")%>
            <%:Html.ValidationMessageFor(m => m.Remark)%>
        </div>
    </div>
    <%:Html.AppHiddenFor(m => m.WorkFlowJson, Model.PageId)%>
    <div class="ui-layout-center ui-helper-reset ui-widget-content">
        <div id="myflow" class="myflowbg" style="height: 700px">
        </div>
        <div id="myflow_tools" style="position: absolute; background-color: #fff; width: 70px;
            cursor: default; padding: 3px;" class="ui-widget-content">
            <div id="myflow_tools_handle" style="text-align: center;" class="ui-widget-header">
                工具集</div>
            <div class="node" id="myflow_save">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/save.gif") %>' />&nbsp;&nbsp;保存</div>
            <div>
                <hr />
            </div>
            <div class="node selectable" id="pointer">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/select16.gif") %>' />&nbsp;&nbsp;选择</div>
            <div class="node selectable" id="path">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/flow_sequence.png") %>' />&nbsp;&nbsp;转换</div>
            <div>
                <hr />
            </div>
            <div class="node state" id="start" type="start">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/start_event_empty.png") %>' />&nbsp;&nbsp;开始</div>
            <div class="node state" id="task" type="task">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/task_empty.png") %>' />&nbsp;&nbsp;任务</div>
            <div class="node state" id="fork" type="fork">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/gateway_parallel.png") %>' />&nbsp;&nbsp;分支</div>
          <%--  <div class="node state" id="join" type="join">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/gateway_parallel.png") %>' />&nbsp;&nbsp;合并</div>--%>
            <div class="node state" id="end" type="end">
                <img src='<%=Url.Content("~/Scripts/used/workflow/img/16/end_event_terminate.png") %>' />&nbsp;&nbsp;结束</div>
        </div>
        <div id="myflow_props" style="position: absolute; background-color: #fff; width: 220px;
            padding: 3px;" class="ui-widget-content">
            <div id="myflow_props_handle" class="ui-widget-header">
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
        //var userGroupJson = '<%=Url.Action("DropList", "Group", new { Area = "BusinessCommon" }) %>';
        var userGroupJson = '<%=Url.Action("Select", "Group", new { Area = "BusinessCommon",showCheckbox="true" }) %>' + "&PageId=" + '<%=Model.PageId%>';
        var postJson = '<%=Url.Action("DropList", "CodeTable", new {Area = "", filter = "ApproveRange" }) %>';
    </script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/lib/raphael-min.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/myflow.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/myflow.jpdl4.js") %>'></script>
    <script type="text/javascript" src='<%=Url.Content("~/Scripts/used/workflow/myflow.editors.js") %>'></script>
    <script type="text/javascript">
        $(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId%>';
            var wfJson = "(" + "<%=Model.WorkFlowJson%>" + ")";
            //#endregion 公共变量 
            //alert(wfJson);
            //#region 初始化
            $('#myflow')
				.myflow(
						{
						    basePath: "",
						    restore: eval(wfJson),
						    util: { arrow: function (l, k, d) {
						        //alert(l);
						        //alert(k);
						    }
						    },
						    tools: {
						        save: {
						            onclick: function (data) {
						                $('#WorkFlowJson' + pageId).val(data);
						                AppMessage(pageId, '系统消息', '流程设计临时保存成功', 'success', function () { });
						                $('#' + 'btnSave' + pageId).attr('disabled', false);
						            }
						        }
						    }
						});
            $('#myflow_tools').css({ left: 20, top: 90 });
            $('#myflow_props').css({ right: 20, top: 90 });
            //#endregion 初始化

        });

        var beforeSaveWorkFlow = function () {
            var formMode = '<%=Model.FormMode%>';
            if (formMode != "delete") {
                var mymessage = confirm("是否保存了流程设计？未保存请先点击工具集上的保存按钮。");
                if (mymessage == true) {
                    return "true";
                }
                else if (mymessage == false) {
                    return "false";
                }
            }
        };
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
