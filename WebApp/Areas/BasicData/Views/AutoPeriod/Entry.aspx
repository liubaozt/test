<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.AutoPeriod.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.FromYear, Model.PageId, "AutoPeriodEntry")%>
        <%:Html.AppTextBoxFor(m => m.FromYear, Model.PageId, "AutoPeriodEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.FromYear)%>
    </div>
    <div class="editor-field">
       <%:Html.AppLabelFor(m => m.ToYear, Model.PageId, "AutoPeriodEntry")%>
        <%:Html.AppTextBoxFor(m => m.ToYear, Model.PageId, "AutoPeriodEntry")%>
        <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.ToYear)%>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
      <script type="text/javascript">
          $(document).ready(function () {
              //#region 公共变量
              var pageId = '<%=Model.PageId %>';
              //#endregion 公共变量

              //#region grid操作
              $('#FromYear' + pageId).keydown(function (e) {
                  if (!CheckInputData(e, "#FromYear" + pageId, "Int", true, 0))
                      return false;
              }).focus(function () { this.style.imeMode = 'disabled'; });
              $('#ToYear' + pageId).keydown(function (e) {
                  if (!CheckInputData(e, "#ToYear" + pageId, "Int", true, 0))
                      return false;
              }).focus(function () { this.style.imeMode = 'disabled'; });
              //#endregion grid操作

          });
    </script>
</asp:Content>

