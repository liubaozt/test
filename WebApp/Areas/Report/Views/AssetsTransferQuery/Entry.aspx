<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsTransferQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsTransferReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsTransferNo, Model.PageId, "AssetsTransferReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsTransferNo, Model.PageId, "AssetsTransferReport")%>
        </div>
        <div class="AssetsTransferReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsTransferName, Model.PageId, "AssetsTransferReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsTransferName, Model.PageId, "AssetsTransferReport")%>
        </div>
        <div class="AssetsTransferReportColumn3">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsTransferReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsTransferReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsTransferNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsTransferName)%>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
    </div>
    <div class="editor-field">
        <div class="AssetsTransferReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsTransferReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsTransferReport")%>
        </div>
        <div class="AssetsTransferReportColumn2">
            <%:Html.AppLabelFor(m => m.TransferDate1, Model.PageId, "AssetsTransferReport")%>
            <%:Html.AppDatePickerFor(m => m.TransferDate1, Model.PageId, "AssetsTransferReport")%>
        </div>
        <div class="AssetsTransferReportColumn3">
            <%:Html.AppLabelFor(m => m.TransferDate2, Model.PageId, "AssetsTransferReport")%>
            <%:Html.AppDatePickerFor(m => m.TransferDate2, Model.PageId, "AssetsTransferReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
        <%:Html.ValidationMessageFor(m => m.TransferDate1)%>
        <%:Html.ValidationMessageFor(m => m.TransferDate2)%>
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
