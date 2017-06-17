<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsBorrowReturnQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsBorrowReturnReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsBorrowReturnReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsBorrowReturnReport")%>
        </div>
        <div class="AssetsBorrowReturnReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsBorrowReturnReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsBorrowReturnReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
    </div>
      <div class="editor-field">
        <div class="AssetsTransferReportColumn1">
            <%:Html.AppLabelFor(m => m.BorrowDate1, Model.PageId, "AssetsBorrowReturnReport")%>
            <%:Html.AppDatePickerFor(m => m.BorrowDate1, Model.PageId, "AssetsBorrowReturnReport")%>
        </div>
        <div class="AssetsTransferReportColumn2">
            <%:Html.AppLabelFor(m => m.BorrowDate2, Model.PageId, "AssetsBorrowReturnReport")%>
            <%:Html.AppDatePickerFor(m => m.BorrowDate2, Model.PageId, "AssetsBorrowReturnReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.BorrowDate1)%>
        <%:Html.ValidationMessageFor(m => m.BorrowDate2)%>
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

