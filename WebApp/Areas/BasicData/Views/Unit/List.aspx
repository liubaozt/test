<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.Unit.ListModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="UnitListColumn1">
            <%:Html.AppLabelFor(m => m.UnitNo, Model.PageId, "UnitList")%>
            <%:Html.AppTextBoxFor(m => m.UnitNo, Model.PageId, "UnitList")%>
        </div>
        <div class="UnitListColumn2">
            <%:Html.AppLabelFor(m => m.UnitName, Model.PageId, "UnitList")%>
            <%:Html.AppTextBoxFor(m => m.UnitName, Model.PageId, "UnitList")%>
        </div>
    </div>
    <div class="editor-field">
        <div class="UnitListColumn1">
            <%:Html.AppLabelFor(m => m.UnitType, Model.PageId, "UnitList")%>
            <%:Html.AppDropDownListFor(m => m.UnitType, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "UnitType" }), "UnitList")%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
