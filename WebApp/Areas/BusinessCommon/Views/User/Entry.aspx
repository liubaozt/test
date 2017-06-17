<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.User.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="WebCommon.Data" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.UserNo, Model.PageId, "UserEntry")%>
        <%:Html.AppTextBoxFor(m => m.UserNo, Model.PageId, "UserEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.UserNo)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.UserName, Model.PageId, "UserEntry")%>
        <%:Html.AppTextBoxFor(m => m.UserName, Model.PageId, "UserEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.UserName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "UserEntry")%>
        <%:Html.AppTreeDialogMultipleFor(m => m.DepartmentId,Model.DepartmentIdDisplay, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "UserEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
    </div>
  <%--  <div class="editor-field" >
        <%:Html.AppLabelFor(m => m.GroupId, Model.PageId, "UserEntry")%>
        <%:Html.AppDropDownListFor(m => m.GroupId, Model.PageId, Url.Action("DropList", "Group", new { Area = "BusinessCommon" }), "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.GroupId)%>
    </div >--%>
   <div class="editor-field">
        <%:Html.AppLabelFor(m => m.GroupId, Model.PageId, "UserEntry")%>
        <%:Html.AppDropDownListMultipleFor(m => m.GroupId,Model.GroupIdDisplay, Model.PageId, Url.Action("Select", "Group", new { Area = "BusinessCommon" }),Model.GroupId, "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.GroupId)%>
    </div>
 <%--   <div class="editor-field">
        <%:Html.AppLabelFor(m => m.PostId, Model.PageId, "UserEntry")%>
        <%:Html.AppDropDownListFor(m => m.PostId, Model.PageId, Url.Action("DropList", "Post", new { Area = "BusinessCommon" }), "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.PostId)%>
    </div>--%>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Tel, Model.PageId, "UserEntry")%>
        <%:Html.AppTextBoxFor(m => m.Tel, Model.PageId, "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.Tel)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Email, Model.PageId, "UserEntry")%>
        <%:Html.AppTextBoxFor(m => m.Email, Model.PageId, "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.Email)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.Address, Model.PageId, "UserEntry")%>
        <%:Html.AppTextBoxFor(m => m.Address, Model.PageId, "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.Address)%>
    </div>

     <div class="editor-field">
        <%:Html.AppLabelFor(m => m.HasApproveAuthority, Model.PageId, "UserEntry")%>
        <%:Html.AppCheckBoxFor(m => m.HasApproveAuthority, Model.PageId, "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.HasApproveAuthority)%>
    </div>
    <div id="UserRadio" class="editor-field">
        <%:Html.AppLabelFor(m => m.Sex, Model.PageId, "UserEntry")%>
        <%:Html.RadioButtonFor(m=>m.Sex, "M", true)%><%=AppMember.AppText["Man"].ToString()%>
        <%:Html.RadioButtonFor(m => m.Sex, "W", true)%><%=AppMember.AppText["Women"].ToString()%>
    </div>
    <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
      { %>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.IsSysUser, Model.PageId, "UserEntry")%>
        <%:Html.AppCheckBoxFor(m => m.IsSysUser, Model.PageId, "UserEntry")%>
        <%:Html.ValidationMessageFor(m => m.IsSysUser)%>
    </div>
    <%} %>
    <div id="AccessLevelGroup" class="editor-field">
        <%:Html.AppLabelFor(m => m.AccessLevel, Model.PageId, "UserEntry")%>
        <%:Html.RadioButtonFor(m => m.AccessLevel, "A", true)%><%=AppMember.AppText["AllLevel"].ToString()%>
        <%:Html.RadioButtonFor(m => m.AccessLevel, "C", true)%><%=AppMember.AppText["CompanyLevel"].ToString()%>
        <%:Html.RadioButtonFor(m => m.AccessLevel, "D", true)%><%=AppMember.AppText["DepartmentLevel"].ToString()%>
        <%:Html.RadioButtonFor(m => m.AccessLevel, "M", true)%><%=AppMember.AppText["MyLevel"].ToString()%>
    </div>
    <%:Html.AppHiddenFor(m => m.IsFixed, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.UserId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            //#region 公共变量
            var pageId = '<%=Model.PageId%>';
            var isSysUser = '<%=Model.IsSysUser%>';
            //#endregion 公共变量
            if (isSysUser == "True")
                $("#AccessLevelGroup").show();
            else
                $("#AccessLevelGroup").hide();


            //#region 用户操作
            $("#IsSysUser" + pageId).click(function () {
                if ($("#IsSysUser" + pageId).attr("checked")) {
                    $("#AccessLevelGroup").show();
                }
                else {
                    $("#AccessLevelGroup").hide();
                }
            });

           
            //#endregion 用户操作

        });
    </script>
</asp:Content>
