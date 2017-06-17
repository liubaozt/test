<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BusinessCommon.Models.Department.SelectModel>" %>
<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="WebCommon.Data" %>
<script type="text/javascript">
    $(document).ready(function () {
        var pageId = '<%=Model.PageId%>';
        $("#PYDepartmentSearch" + pageId).keydown(function (event) {
            if (event.keyCode == 13) {
                var pySearch = $("#PYDepartmentSearch" + pageId).val();
                var urlStr = '<%=Url.Action("SearchTree", "Department", new { Area = "BusinessCommon"}) %>';
                $.ajax({
                    type: 'POST',
                    url: urlStr,
                    data: { pageId: pageId, pySearch: pySearch },
                    success: function (jsonObj) {
                        if (jsonObj != "0") {
                            $("#DivDepartmentTree" + pageId).empty();
                            $("#DivDepartmentTree" + pageId).append(jsonObj);
                        } 
                    }
                });
            }
        });
    });
</script>
<div class="editor-field">
    <%:Html.AppTextBoxFor(m => m.PYDepartmentSearch, Model.PageId, "DepartmentEntry")%>
</div>
<div id="DivDepartmentTree<%=Model.PageId %>">
    <%:Html.AppTreeViewFor(Model.PageId, TreeId.DepartmentTreeId, Model.DepartmentTree, Model.ShowCheckBox,Model.DepartmentId)%>
</div>
