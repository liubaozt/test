<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsMaintainQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsMaintainReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsMaintainNo, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMaintainNo, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <div class="AssetsMaintainReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsMaintainName, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsMaintainName, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <div class="AssetsMaintainReportColumn3">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsMaintainNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsMaintainName)%>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
    </div>
    <div class="editor-field">
        <div class="AssetsMaintainReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <div class="AssetsMaintainReportColumn2">
            <%:Html.AppLabelFor(m => m.MaintainDate1, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppDatePickerFor(m => m.MaintainDate1, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <div class="AssetsMaintainReportColumn3">
            <%:Html.AppLabelFor(m => m.MaintainDate2, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppDatePickerFor(m => m.MaintainDate2, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
        <%:Html.ValidationMessageFor(m => m.MaintainDate1)%>
        <%:Html.ValidationMessageFor(m => m.MaintainDate2)%>
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
