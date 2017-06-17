<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsSplitQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsSplitReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsSplitNo, Model.PageId, "AssetsSplitReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsSplitNo, Model.PageId, "AssetsSplitReport")%>
        </div>
        <div class="AssetsSplitReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsSplitName, Model.PageId, "AssetsSplitReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsSplitName, Model.PageId, "AssetsSplitReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsSplitNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsSplitName)%>
    </div>
    <div class="editor-field">
        <div class="AssetsSplitReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsSplitReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsSplitReport")%>
        </div>
        <div class="AssetsSplitReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsSplitReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsSplitReport")%>
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
