<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Entry.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.BasicData.Models.FiscalYear.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.FiscalYearName, Model.PageId, "FiscalYearEntry")%>
        <%:Html.AppTextBoxFor(m => m.FiscalYearName, Model.PageId, "FiscalYearEntry")%>
         <%:Html.AppRequiredFlag()%>
        <%:Html.ValidationMessageFor(m => m.FiscalYearName)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.FromDate, Model.PageId, "FiscalYearEntry")%>
        <%:Html.AppDatePickerFor(m => m.FromDate, Model.PageId, "FiscalYearEntry")%>
        <%:Html.ValidationMessageFor(m => m.FromDate)%>
    </div>
    <div class="editor-field">
        <%:Html.AppLabelFor(m => m.ToDate, Model.PageId, "FiscalYearEntry")%>
        <%:Html.AppDatePickerFor(m => m.ToDate, Model.PageId, "FiscalYearEntry")%>
        <%:Html.ValidationMessageFor(m => m.ToDate)%>
    </div>
    <%:Html.AppHiddenFor(m => m.FiscalYearId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
      <script type="text/javascript">
          $(document).ready(function () {
              //#region 公共变量
              var pageId = '<%=Model.PageId %>';
              //#endregion 公共变量

              //#region grid操作
              $('#FiscalYearName' + pageId).keydown(function (e) {
                  if (!CheckInputData(e, "#FiscalYearName" + pageId, "Int", true, 0))
                      return false;
              }).focus(function () { this.style.imeMode = 'disabled'; });
              //#endregion grid操作

          });
    </script>
</asp:Content>

