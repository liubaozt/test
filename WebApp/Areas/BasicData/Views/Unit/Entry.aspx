<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.Unit.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.UnitNo, Model.PageId, "UnitEntry")%>
        <%:Html.AppTextBoxFor(m => m.UnitNo, Model.PageId, "UnitEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.UnitNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.UnitName, Model.PageId, "UnitEntry")%>
        <%:Html.AppTextBoxFor(m => m.UnitName, Model.PageId, "UnitEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.UnitName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.UnitType, Model.PageId, "UnitEntry")%>
        <%:Html.AppDropDownListFor(m => m.UnitType, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "UnitType" }), "UnitEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.UnitType)%>
    </div>
    <%:Html.AppHiddenFor(m => m.UnitId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
