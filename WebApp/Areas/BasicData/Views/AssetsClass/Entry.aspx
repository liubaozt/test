<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AssetsClass.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AssetsClassNo, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppTextBoxFor(m => m.AssetsClassNo, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.AssetsClassNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.AssetsClassName, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppTextBoxFor(m => m.AssetsClassName, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.AssetsClassName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.UnitId, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppDropDownListFor(m => m.UnitId, Model.PageId, Url.Action("DropList", "Unit", new { Area = "BasicData", filterExpression = "unitType=" + DFT.SQ + "N" + DFT.SQ }), "AssetsClassEntry")%>
        <%:Html.ValidationMessageFor(m => m.UnitId)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DurableYears, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppTextBoxFor(m => m.DurableYears, Model.PageId, "AssetsClassEntry")%>
        <%:Html.ValidationMessageFor(m => m.DurableYears)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.RemainRate, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppTextBoxFor(m => m.RemainRate, Model.PageId, "AssetsClassEntry")%>
        <%:Html.ValidationMessageFor(m => m.RemainRate)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.DepreciationType, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppDropDownListFor(m => m.DepreciationType, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "DepreciationType" }), "AssetsClassEntry")%>
        <%:Html.ValidationMessageFor(m => m.DepreciationType)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ParentId, Model.PageId, "AssetsClassEntry")%>
        <%:Html.AppTreeDialogFor(m => m.ParentId, Model.PageId, Model.ParentUrl, Model.DialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId,Model.AddFavoritUrl,Model.ReplaceFavoritUrl,"AssetsClassEntry")%>
        <%:Html.ValidationMessageFor(m => m.ParentId)%>
    </div>
    <%:Html.AppHiddenFor(m => m.AssetsClassId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
