<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.UpcomingScrapQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="UpcomingScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "UpcomingScrapReport")%>
        </div>
        <div class="UpcomingScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "UpcomingScrapReport")%>
        </div>
        <div class="UpcomingScrapReportColumn3">
            <%:Html.AppLabelFor(m => m.AssetsState, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppDropDownListFor(m => m.AssetsState, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "AssetsState" }), "UpcomingScrapReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
        <%:Html.ValidationMessageFor(m => m.AssetsState)%>
    </div>
    <div class="editor-field">
        <div class="UpcomingScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId, Model.AssetsClassAddFavoritUrl, Model.AssetsClassReplaceFavoritUrl, "UpcomingScrapReport")%>
        </div>
        <div class="UpcomingScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "UpcomingScrapReport")%>
        </div>
        <div class="UpcomingScrapReportColumn3">
            <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "UpcomingScrapReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsClassId)%>
        <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
        <%:Html.ValidationMessageFor(m => m.StoreSiteId)%>
    </div>
    <div class="editor-field">
        <div class="UpcomingScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.DepreciationEndDate1, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppDatePickerFor(m => m.DepreciationEndDate1, Model.PageId, "UpcomingScrapReport")%>
        </div>
        <div class="UpcomingScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.DepreciationEndDate2, Model.PageId, "UpcomingScrapReport")%>
            <%:Html.AppDatePickerFor(m => m.DepreciationEndDate2, Model.PageId, "UpcomingScrapReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.DepreciationEndDate1)%>
        <%:Html.ValidationMessageFor(m => m.DepreciationEndDate2)%>
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
