<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BusinessLogic.AssetsBusiness.Models.AssetsManage.SelectModel>" %>
<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<%--<link href='<%=Url.Content("~/Content/css/PageCss/AssetsBusiness/Assets.css") %>'
    rel="stylesheet" type="text/css" />--%>
<script type="text/javascript">
    $(document).ready(function () {
        //#region 公共变量
        var pageId = '<%=Model.PageId%>';
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
        var gridId = '<%=Model.GridId%>';
        if (gridId == "BatchGrid") {
            $("#DivCascadeDelete" + pageId).show();
        }
        else {
            $("#DivCascadeDelete" + pageId).hide();
        }
    });
</script>
<%:Html.AppBeginForm(Model.PageId, Model.FormId)%>
<fieldset>
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
            <%:Html.AppDropDownListFor(m => m.UsePeople, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsList")%>
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
            <%:Html.AppDropDownListFor(m => m.Keeper, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsList")%>
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
   <%--     <div class="AssetsListColumn2">
            <%:Html.AppLabelFor(m => m.ProjectManageId, Model.PageId, "AssetsList")%>
            <%:Html.AppDropDownListFor(m => m.ProjectManageId, Model.PageId, Url.Action("DropList", "ProjectManage", new { Area = "BasicData" }), "AssetsList")%>
        </div>--%>
        <div id="DivCascadeDelete<%=Model.PageId%>" class="AssetsListColumn4">
            <%:Html.AppLabelFor(m => m.IsCascadeDelete, Model.PageId, "AssetsListCascadeDelete")%>
            <%:Html.AppCheckBoxFor(m => m.IsCascadeDelete, Model.PageId, "AssetsList")%>
        </div>
    </div>
</fieldset>
<%:Html.AppEndForm() %>
<%:Html.AppMultiSelectGridFor(this.Url, Model.PageId, Model.GridId, Model.GridUrl, Model.GridLayout, 140, true, Model.GridWidth, Model.IsMultiSelect)%>
