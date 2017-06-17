<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsScrapQuery.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsScrapNo, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsScrapNo, Model.PageId, "AssetsScrapReport")%>
        </div>
        <div class="AssetsScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsScrapName, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsScrapName, Model.PageId, "AssetsScrapReport")%>
        </div>
        <div class="AssetsScrapReportColumn3">
            <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId, Model.AssetsClassAddFavoritUrl, Model.AssetsClassReplaceFavoritUrl, "AssetsScrapReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsScrapNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsScrapName)%>
        <%:Html.ValidationMessageFor(m => m.AssetsClassId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsScrapReport")%>
        </div>
        <div class="AssetsScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsScrapReport")%>
        </div>
        <div class="AssetsScrapReportColumn3">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsScrapReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
        <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.ScrapTypeId, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppDropDownListFor(m => m.ScrapTypeId, Model.PageId, Url.Action("DropList", "ScrapType", new { Area = "BasicData" }), "AssetsScrapReport")%>
        </div>
         <div class="AssetsScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.ScrapDate1, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppDatePickerFor(m => m.ScrapDate1, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <div class="AssetsScrapReportColumn3">
            <%:Html.AppLabelFor(m => m.ScrapDate2, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppDatePickerFor(m => m.ScrapDate2, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.ScrapTypeId)%>
         <%:Html.ValidationMessageFor(m => m.ScrapDate1)%>
        <%:Html.ValidationMessageFor(m => m.ScrapDate2)%>
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
