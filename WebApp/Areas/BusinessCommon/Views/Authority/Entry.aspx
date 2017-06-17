<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.Authority.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div id="AuthorityRadio" class="editor-field">
            <%:Html.AppCheckBoxFor(m => m.IsUser, Model.PageId, "AuthorityEntry")%>
            <%:Html.AppLabelFor(m => m.IsUser, Model.PageId, "IsUserAuthorityEntry")%>
        </div>
        <div class="editor-field">
            <div class="AuthorityEntryColumn1">
                <%:Html.AppLabelFor(m => m.GroupNo, Model.PageId, "AuthorityEntry")%>
                <%:Html.AppDropDownListFor(m => m.GroupNo, Model.PageId, Url.Action("DropList", "Group", new { Area = "BusinessCommon" }), "AuthorityEntry")%>
            </div>
            <div class="AuthorityEntryColumn2">
                <%:Html.AppLabelFor(m => m.UserNo, Model.PageId, "AuthorityEntry")%>
                <%:Html.AppDropDownListFor(m => m.UserNo, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filter = "none" }), "AuthorityEntry")%>
            </div>
        </div>
    </fieldset>
    <fieldset>
        <%:Html.AppTreeViewFor(Model.PageId,Model.TreeId, Model.AuthorityTree, "btnSave")%>
    </fieldset>
    <%:Html.AppHiddenFor(m => m.TreeId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.RadioValue, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            //#region 公共变量
            var pageId = '<%=Model.PageId%>';
            var treeIdObj = '<%=Model.TreeId%>' + pageId;
            var groupIdObj = '#GroupNo' + pageId;
            var userIdObj = '#UserNo' + pageId;
            var userIdLblObj = '#UserNo' + pageId + 'Label';
            var refreshUrl = '<%=Url.Action("RefreshTree", "Authority", new { Area = "BusinessCommon" }) %>';
            //#endregion 公共变量

            $(userIdObj).hide();
            $(userIdLblObj).hide();
            $("#RadioValue" + pageId).val("group");


            //#region 用户操作
            $("#IsUser" + pageId).click(function () {
                if ($("#IsUser" + pageId).attr("checked")) {
                    $(userIdObj).show();
                    $(userIdLblObj).show();
                    $("#RadioValue" + pageId).val("user");
                }
                else {
                    $(userIdObj).hide();
                    $(userIdLblObj).hide();
                    $("#RadioValue" + pageId).val("group");
                }
            });
            $(groupIdObj).change(function () {
                var groupid = $(groupIdObj).val();
                if ($("#IsUser" + pageId).attr("checked")) {
                    if ($.trim(groupid) == "") {
                        groupid = "none";
                    }
                    var urlStr = '<%=Url.Action("DropList", "User", new { Area = "BusinessCommon" }) %>' + '/?filterExpression=groupId=' + '<%=DFT.SQ %>' + groupid + '<%=DFT.SQ %>';
                    $.getJSON(urlStr, function (data) {
                        AppAppendSelect2(data, userIdObj, urlStr);
                    });
                }
                else {
                    $.ajax({
                        type: 'POST',
                        url: refreshUrl,
                        data: { groupId: groupid, userId: '' },
                        success: function (jsonObj) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            treeObj.checkAllNodes(false);
                            for (var i = 0; i < jsonObj.menuIds.length; i++) {
                                var node = treeObj.getNodeByParam("id", jsonObj.menuIds[i].menuId, null);
                                treeObj.checkNode(node, true, false);
                            }
                        }
                    });
                }
            });
            $(userIdObj).change(function () {
                    var userid = $(userIdObj).val();
                    $.ajax({
                        type: 'POST',
                        url: refreshUrl,
                        data: { groupId: '', userId: userid },
                        success: function (jsonObj) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            treeObj.checkAllNodes(false);
                            for (var i = 0; i < jsonObj.menuIds.length; i++) {
                                var node = treeObj.getNodeByParam("id", jsonObj.menuIds[i].menuId, null);
                                treeObj.checkNode(node, true, false);
                            }
                        }
                    });
            });
            //#endregion 用户操作

        });
    </script>
</asp:Content>
