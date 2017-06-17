<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BusinessLogic.AssetsBusiness.Models.AssetsPurchase.PurchaseModel>" %>
<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<%:Html.AppBeginForm(Model.PageId, Model.FormId)%>

    <div class="editor-field">
        <div class="AssetsPurchaseEntryColumn1">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsPurchaseEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsPurchaseEntry")%>
        </div>
        <div class="AssetsPurchaseEntryColumn2">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsPurchaseEntry")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsPurchaseEntry")%>
        </div>
        <div class="AssetsPurchaseEntryColumn3">
            <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "AssetsPurchaseEntry")%>
            <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "AssetsPurchaseEntry")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="AssetsPurchaseEntryColumn1">
            <%:Html.AppLabelFor(m => m.UsePeople, Model.PageId, "AssetsPurchaseEntry")%>
            <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
              { %>
            <%:Html.AppDropDownListFor(m => m.UsePeople, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsPurchaseEntry")%>
            <%}
              else
              { %>
            <%:Html.AppAutoCompleteFor(m => m.UsePeople, Model.PageId, "AssetsPurchaseEntry", Model.UserSource)%>
            <%}  %>
        </div>
        <div class="AssetsPurchaseEntryColumn2">
            <%:Html.AppLabelFor(m => m.Keeper, Model.PageId, "AssetsPurchaseEntry")%>
            <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
              { %>
            <%:Html.AppDropDownListFor(m => m.Keeper, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsPurchaseEntry")%>
            <%}
              else
              { %>
            <%:Html.AppAutoCompleteFor(m => m.Keeper, Model.PageId, "AssetsPurchaseEntry", Model.UserSource)%>
            <%}  %>
        </div>
        <div class="AssetsPurchaseEntryColumn3">
            <%:Html.AppLabelFor(m => m.AssetsValue, Model.PageId, "AssetsPurchaseEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsValue, Model.PageId, "AssetsPurchaseEntry")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="AssetsPurchaseEntryColumn1">
            <%:Html.AppLabelFor(m => m.Remark, Model.PageId, "AssetsPurchaseEntry")%>
            <%:Html.AppTextBoxFor(m => m.Remark, Model.PageId, "AssetsPurchaseEntry")%>
        </div>
    </div>
<%:Html.AppEndForm() %>
