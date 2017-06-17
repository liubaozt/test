<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WebApp.Models.AppRegisterModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>软件注册</title>
    <%=TempData["CssBlock"]%>
    <%=TempData["ScriptBlock"]%>
</head>
<body>
    <div>
        <% using (Html.BeginForm())
           {%>
        <h2>
            请您注册后使用!</h2>
        <div class="editor-field">
            <%:Html.AppLabelFor(m => m.RegisterNo, "", "GroupEntry")%>
            <%:Html.TextBoxFor(m => m.RegisterNo, new { @style = "width:150px; height:21px" })%>
            <%:Html.ValidationMessageFor(m => m.RegisterNo)%>
        </div>
        <div class="editor-field">
            <%:Html.AppLabelFor(m => m.CompanyName, "", "GroupEntry")%>
            <%:Html.TextBoxFor(m => m.CompanyName, new { @style = "width:150px; height:21px" })%>
            <%:Html.ValidationMessageFor(m => m.CompanyName)%>
        </div>
        <div class="submit">
            <button class="submitBd" id="btnOk" type="submit">
                注 册</button>
        </div>
        <p>
            如没有注册码，请<a href='<%=Url.Action("GetMachineInfo", "Home")%>'>点击这里</a>获取信息提供给供应商，谢谢！</p>
        <% } %>
    </div>
</body>
</html>
