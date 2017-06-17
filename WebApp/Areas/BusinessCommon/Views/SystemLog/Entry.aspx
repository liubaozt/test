<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessCommon.Models.SystemLog.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="SystemLogColumn1">
            <%:Html.AppLabelFor(m => m.UserName, Model.PageId, "SystemLog")%>
            <%:Html.AppTextBoxFor(m => m.UserName,Model.PageId, "SystemLog")%>
        </div>
        <div class="SystemLogColumn2">
            <%:Html.AppLabelFor(m => m.LogMessage, Model.PageId, "SystemLog")%>
            <%:Html.AppTextBoxFor(m => m.LogMessage, Model.PageId, "SystemLog")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.UserName, "SystemLog")%>
        <%:Html.ValidationMessageFor(m => m.Message, "SystemLog")%>
    </div>
    <div class="editor-field">
          <div class="SystemLogColumn1">
            <%:Html.AppLabelFor(m => m.LogDate1, Model.PageId, "SystemLog")%>
            <%:Html.AppDatePickerFor(m => m.LogDate1, Model.PageId, "SystemLog")%>
        </div>
        <div class="SystemLogColumn2">
            <%:Html.AppLabelFor(m => m.LogDate2, Model.PageId, "SystemLog")%>
            <%:Html.AppDatePickerFor(m => m.LogDate2, Model.PageId, "SystemLog")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.LogDate1)%>
        <%:Html.ValidationMessageFor(m => m.LogDate2)%>
    </div>
    <%:Html.AppHiddenFor(m => m.EntryGridId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   <script type="text/javascript">
       $(document).ready(function () {
           var pageId = '<%=Model.PageId %>';
           $('#btnQuery' + pageId).click(function () {
               QueryReport();
           });
       });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>

