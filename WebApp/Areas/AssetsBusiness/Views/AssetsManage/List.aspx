<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveList.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsManage.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsList")%>
        </div>
        <div class="AssetsListColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsList")%>
        </div>
        <div class="AssetsListColumn3">
            <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "AssetsList")%>
            <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId, Model.AssetsClassAddFavoritUrl, Model.AssetsClassReplaceFavoritUrl, "AssetsList")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="AssetsListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsState, Model.PageId, "AssetsList")%>
            <%:Html.AppDropDownListFor(m => m.AssetsState, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "AssetsState" }), "AssetsList")%>
        </div>
        <div class="AssetsListColumn2">
            <%:Html.AppLabelFor(m => m.Spec, Model.PageId, "AssetsList")%>
            <%:Html.AppTextBoxFor(m => m.Spec, Model.PageId, "AssetsList")%>
        </div>
        <div class="AssetsListColumn3">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsList")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsList")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="AssetsListColumn1">
            <%:Html.AppLabelFor(m => m.UsePeople, Model.PageId, "AssetsList")%>
            <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
              { %>
            <%:Html.AppDropDownEditorFor(m => m.UsePeople, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsList")%>
            <%}
              else
              { %>
            <%:Html.AppAutoCompleteFor(m => m.UsePeople, Model.PageId, "AssetsList", Model.UserSource)%>
            <%}  %>
        </div>
        <div class="AssetsListColumn2">
            <%:Html.AppLabelFor(m => m.Keeper, Model.PageId, "AssetsList")%>
            <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
              { %>
            <%:Html.AppDropDownEditorFor(m => m.Keeper, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsList")%>
            <%}
              else
              { %>
            <%:Html.AppAutoCompleteFor(m => m.Keeper, Model.PageId, "AssetsList", Model.UserSource)%>
            <%}  %>
        </div>
        <div class="AssetsListColumn3">
            <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "AssetsList")%>
            <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "AssetsList")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="AssetsListColumn1">
            <%:Html.AppLabelFor(m => m.CompanyId, Model.PageId, "AssetsList")%>
            <%:Html.AppDropDownListFor(m => m.CompanyId, Model.PageId, Url.Action("DropList", "Company", new { Area = "BusinessCommon" }), "AssetsList")%>
        </div>
        <div class="AssetsListColumn2">
            <%:Html.AppLabelFor(m => m.PurchaseDateFrom, Model.PageId, "AssetsList")%>
            <%:Html.AppDatePickerFor(m => m.PurchaseDateFrom, Model.PageId, "AssetsList")%>
        </div>
        <div class="AssetsListColumn3">
            <%:Html.AppLabelFor(m => m.PurchaseDateTo, Model.PageId, "AssetsList")%>
            <%:Html.AppDatePickerFor(m => m.PurchaseDateTo, Model.PageId, "AssetsList")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="AssetsListColumn1">
            <%:Html.AppLabelFor(m => m.AssetsBarcode, Model.PageId, "AssetsList")%>
            <%:Html.AppTextBoxFor(m => m.AssetsBarcode, Model.PageId, "AssetsList")%>
        </div>
         <div class="AssetsListColumn2">
            <%:Html.AppLabelFor(m => m.Remark, Model.PageId, "AssetsList")%>
            <%:Html.AppTextBoxFor(m => m.Remark, Model.PageId, "AssetsList")%>
        </div>
 <%--       <div class="AssetsListColumn2">
            <%:Html.AppLabelFor(m => m.ProjectManageId, Model.PageId, "AssetsList")%>
            <%:Html.AppDropDownListFor(m => m.ProjectManageId, Model.PageId, Url.Action("DropList", "ProjectManage", new { Area = "BasicData" }), "AssetsList")%>
        </div>--%>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //#region 公共变量
            var pageId = '<%=Model.PageId %>';
            //#endregion 公共变量

            var departmentIdObj = "#DepartmentId" + pageId;
            var usePeopleObj = "#UsePeople" + pageId;
            var keeperObj = "#Keeper" + pageId;
            $(departmentIdObj).change(function () {
                var departmentId = $(departmentIdObj).val();
                if ($.trim(departmentId) == "") {
                    departmentId = "none";
                }
                var urlStr = '<%=Url.Action("DropList", "User", new { Area = "BusinessCommon"}) %>' + '/?filterExpression=departmentId=' + '<%=DFT.SQ %>' + departmentId + '<%=DFT.SQ %>';
                $.getJSON(urlStr, function (data) {
                    AppAppendSelect2(data, usePeopleObj, urlStr);
                    AppAppendSelect2(data, keeperObj, urlStr);
                });
            });

            function abc() {
                document.getElementById('testa').href = '/AssetsBusiness/AssetsManage/ExportCard?pageId=2015&viewTitle=资产管理&primaryKey=T15060700276';
                if (document.all) {
                    // For IE
                    document.getElementById('testa').click();
                } else if (document.createEvent) {
                    //FOR DOM2
                    alert("DOM2");
                    var ev = document.createEvent('MouseEvents');
                    ev.initEvent('click', false, true);
                    document.getElementById('testa').dispatchEvent(ev);
                }
            }

        });
    </script>
</asp:Content>
