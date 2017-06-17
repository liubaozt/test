<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsScrapReport.EntryModel>" %>

<%@ Import Namespace="WebApp.BaseWeb.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
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
        <%:Html.ValidationMessageFor(m => m.AssetsScrapNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsScrapName)%>
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
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
    </div>
    <div class="editor-field">
        <div class="AssetsScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsTypeId, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppDropDownListFor(m => m.AssetsTypeId, Model.PageId, Url.Action("AssetsTypeDropList", "DropList", new { Area = "" }), "AssetsScrapReport")%>
        </div>
        <div class="AssetsScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.ScrapTypeId, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppDropDownListFor(m => m.ScrapTypeId, Model.PageId, Url.Action("ScrapTypeDropList", "DropList", new { Area = "" }), "AssetsScrapReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsTypeId)%>
        <%:Html.ValidationMessageFor(m => m.ScrapTypeId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsScrapReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], "tree", "AssetsScrapReport")%>
        </div>
        <div class="AssetsScrapReportColumn2">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsScrapReport")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], "tree", "AssetsScrapReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsClassId)%>
        <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HiddenContent" runat="server">
<%:Html.AppHiddenFor(m => m.EntryGridId, Model.PageId)%>
</asp:Content>
