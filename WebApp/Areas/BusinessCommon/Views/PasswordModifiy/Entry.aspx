<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.PasswordModifiy.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.OldPassword, Model.PageId, "PasswordModifiyEntry")%>
        <%:Html.AppTextBoxFor(m => m.OldPassword, Model.PageId, "PasswordModifiyEntry", TextType.Password)%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.OldPassword)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.NewPassword, Model.PageId, "PasswordModifiyEntry")%>
        <%:Html.AppTextBoxFor(m => m.NewPassword, Model.PageId, "PasswordModifiyEntry", TextType.Password)%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.NewPassword)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.NewPassword2, Model.PageId, "PasswordModifiyEntry")%>
        <%:Html.AppTextBoxFor(m => m.NewPassword2, Model.PageId, "PasswordModifiyEntry", TextType.Password)%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.NewPassword2)%>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            //#region 公共变量
            var pageId = '<%=Model.PageId%>';
            var checkOldPasswordUrl = '<%=Url.Action("CheckOldPassword", "PasswordModifiy", new { Area = "BusinessCommon" }) %>';
            var checkNewPasswordUrl = '<%=Url.Action("CheckNewPassword", "PasswordModifiy", new { Area = "BusinessCommon" }) %>';
            //#endregion 公共变量


            //#region 用户操作
            $("#OldPassword" + pageId).change(function () {
                var oldPassword = $("#OldPassword" + pageId).val();
                $.ajax({
                    type: 'POST',
                    url: checkOldPasswordUrl,
                    data: { oldPassword: oldPassword },
                    success: function (data) {
                        if (data != "1") {
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>',
                             '<%=AppMember.AppText["OldPasswordError"]%>', 'error',
                             function () {
                                 $("#OldPassword" + pageId).val("");
                                 $("#OldPassword" + pageId).focus();
                             });
                        }
                    }
                });
            });
            $("#NewPassword2" + pageId).change(function () {
                var newPassword = $("#NewPassword" + pageId).val();
                var newPassword2 = $("#NewPassword2" + pageId).val();
                $.ajax({
                    type: 'POST',
                    url: checkNewPasswordUrl,
                    data: { newPassword: newPassword, newPassword2: newPassword2 },
                    success: function (data) {
                        if (data != "1") {
                            AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>',
                          '<%=AppMember.AppText["NewPasswordError"]%>', 'error',
                          function () {
                              $("#NewPassword2" + pageId).val("");
                              $("#NewPassword2" + pageId).focus();
                          });

                        }
                    }
                });
            });
            //#endregion 用户操作

        });
    </script>
</asp:Content>
