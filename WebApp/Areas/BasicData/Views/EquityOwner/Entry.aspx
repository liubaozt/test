<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.EquityOwner.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.EquityOwnerNo, Model.PageId, "EquityOwnerEntry")%>
        <%:Html.AppTextBoxFor(m => m.EquityOwnerNo, Model.PageId, "EquityOwnerEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m=>m.EquityOwnerNo) %>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.EquityOwnerName, Model.PageId, "EquityOwnerEntry")%>
        <%:Html.AppTextBoxFor(m => m.EquityOwnerName, Model.PageId, "EquityOwnerEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.EquityOwnerName)%>
    </div>
    <%:Html.AppHiddenFor(m => m.EquityOwnerId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
