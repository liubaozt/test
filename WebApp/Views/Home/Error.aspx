<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Error</title>
</head>
<body>
    <div>
     <h2>登录超时,请重新<a  href='<%=Url.Action("Login", "Home")%>' style="color:#F00">登录</a>!</h2>
    </div>
</body>
</html>
