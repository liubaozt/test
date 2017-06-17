<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsMaintainReport.EntryModel>" %>

<%@ Import Namespace="WebApp.BaseWeb.HtmlHelpers" %>
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
        <%:Html.ValidationMessageFor(m => m.AssetsMaintainNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsMaintainName)%>
    </div>
    <div class="editor-field">
        <div class="AssetsMaintainReportColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <div class="AssetsMaintainReportColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsNo)%>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
    </div>
    <div class="editor-field">
        <div class="AssetsMaintainReportColumn1">
            <%:Html.AppLabelFor(m => m.MaintainDate, Model.PageId, "AssetsMaintainReport")%>
            <%:Html.AppDatePickerFor(m => m.MaintainDate, Model.PageId, "AssetsMaintainReport")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.MaintainDate)%>
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
