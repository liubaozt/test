<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.UpcomingGuaranteeQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="UpcomingGuaranteeReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "UpcomingGuaranteeReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "UpcomingGuaranteeReport")%>
        </div>
        <div class="UpcomingGuaranteeReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "UpcomingGuaranteeReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "UpcomingGuaranteeReport")%>
        </div>
        <div class="UpcomingGuaranteeReportColumn3">
            <%:Html.AppLabelFor(m => m.RemindDays, Model.PageId, "UpcomingGuaranteeReport")%>
            <%:Html.AppTextBoxFor(m => m.RemindDays, Model.PageId, "UpcomingGuaranteeReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
        <%:Html.ValidationMessageFor(m => m.RemindDays)%>
    </div>
    <div class="editor-field">
        <div class="UpcomingScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "UpcomingGuaranteeReport")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "UpcomingGuaranteeReport")%>
        </div>
        <div class="UpcomingScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "UpcomingGuaranteeReport")%>
            <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "UpcomingGuaranteeReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
        <%:Html.ValidationMessageFor(m => m.StoreSiteId)%>
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
