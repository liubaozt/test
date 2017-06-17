<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsCheckQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsCheckReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsCheckNo, Model.PageId, "AssetsCheckReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsCheckNo, Model.PageId, "AssetsCheckReport")%>
        </div>
        <div class="AssetsCheckReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsCheckName, Model.PageId, "AssetsCheckReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsCheckName, Model.PageId, "AssetsCheckReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsCheckNo, "AssetsCheckReport")%>
        <%:Html.ValidationMessageFor(m => m.AssetsCheckName, "AssetsCheckReport")%>
    </div>
    <div class="editor-field">
        <div class="AssetsCheckReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsCheckReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsCheckReport")%>
        </div>
        <div class="AssetsCheckReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsCheckReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsCheckReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
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

