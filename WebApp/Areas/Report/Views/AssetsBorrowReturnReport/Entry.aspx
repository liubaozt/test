<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/QueryEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsBorrowReturnReport.EntryModel>" %>

<%@ Import Namespace="WebApp.BaseWeb.HtmlHelpers" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ButtonContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HiddenContent" runat="server">
    <%:Html.AppHiddenFor(m => m.EntryGridId, Model.PageId)%>
</asp:Content>
