<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WebApp.Models.LoginModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="WebCommon.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=AppMember.AppText["SysName"].ToString()%></title>
    <%=TempData["CssBlock"]%>
    <link href="../../Content/css/login.css" rel="stylesheet" type="text/css" />
    <%=TempData["ScriptBlock"]%>
    <script type="text/javascript">
        //=============================切换验证码======================================
        function ToggleCode(obj, codeurl) {
            $("#txtCode").val("");
            $("#" + obj).attr("src", codeurl + "?time=" + Math.random());
        }
        $(document).ready(function () {
            var msg = '<%=ViewData["Message"]%>';
            if (msg != "") {
                AppMessage("Login", '系统消息', msg, 'error', function () { });
            }
        });
    </script>
</head>
<body style="padding-top: 167px">
    <div class="form">
        <% using (Html.BeginForm())
           {%>
        <div class="boxLogin">
            <dl>
                <dd>
                    <div class="s1">
                        帐&nbsp;&nbsp;&nbsp;号：</div>
                    <div class="s2">
                        <%:Html.TextBoxFor(m => m.UserNo, new { @style = "width:122px;" ,@class="txt" })%>
                        <%:Html.ValidationMessageFor(m => m.UserNo)%>
                    </div>
                </dd>
                <dd>
                    <div class="s3">
                        密&nbsp;&nbsp;&nbsp;码：</div>
                    <div class="s4">
                        <%:Html.PasswordFor(m => m.UserPwd, new { @style = "width:122px;", @class = "txt" })%>
                        <%:Html.ValidationMessageFor(m => m.UserPwd)%>
                    </div>
                </dd>
                <dd>
                    <div class="s11">
                        帐&nbsp;&nbsp;&nbsp;套：</div>
                    <div class="s12">
                        <%:Html.AppDropDownListFor(m => m.SetBooks, "", Url.Action("DropList", "SetBooks", new { Area = "BusinessCommon" }), "SetBooks",false)%>
                        <%:Html.ValidationMessageFor(m => m.UserPwd)%>
                    </div>
                </dd>
                <dd>
                    <div class="s5">
                        验证码：</div>
                    <div class="s6">
                        <%:Html.TextBoxFor(m => m.TxtCheckCode, new { @style = "width:48px;", @class = "txt" })%>
                        <img src='<%=Url.Action("CheckCode", "Home") %>' id="Verify_codeImag" width="70"
                            height="22" alt="点击切换验证码" title="点击切换验证码" style="margin-top: 0px; vertical-align: top;
                            cursor: pointer;" onclick="ToggleCode(this.id, '<%=Url.Action("CheckCode", "Home") %>');return false;" />
                        <%:Html.ValidationMessageFor(m => m.TxtCheckCode)%>
             
                    </div>
                </dd>
                <dd>
                    <div class="load">
                        <img src='<%= Url.Content("~/Content/images/Login/loading.gif")%>' /></div>
                </dd>
            </dl>
            <div class="s8">
                <input id="Log_Submit" type="submit" value="" class="sign" />
            </div>
        </div>
        <div class="copyright">
            <p id="cp">
                Copyright &copy; 2011 - 2021
            </p>
        </div>
        <% } %>
    </div>
    <div id="MessageDialogLogin">
    </div>
</body>
</html>
