<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WebApp.Models.MessageModel>" %>
<p>
    <span id='MessageIco' class="ui-icon ui-icon-circle-close" style="float: left; margin: 0 7px 50px 0;">
    </span>
    <%:Html.DisplayFor(m=>m.MessageContent)%>
    <%:Html.HiddenFor(m=>m.MessageType) %>
    <%:Html.HiddenFor(m=>m.PageId) %>
</p>
<script type="text/javascript">
    $(document).ready(function () {
        var messageType = '<%=Model.MessageType %>';
        if (messageType == "error") {
            $('#MessageIco').removeClass();
            $('#MessageIco').addClass("ui-icon ui-icon-circle-close");
        }
        else if (messageType == "success") {
            $('#MessageIco').removeClass();
            $('#MessageIco').addClass("ui-icon ui-icon-circle-check");
        }
        else if (messageType == "confirm") {
            $('#MessageIco').removeClass();
            $('#MessageIco').addClass("ui-icon ui-icon-alert");
        }
        else {
            $('#MessageIco').removeClass();
            $('#MessageIco').addClass("ui-icon ui-icon-alert");
        }
    });
</script>
