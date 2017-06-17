<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BusinessLogic.BasicData.Models.StoreSite.SelectModel>" %>
<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="WebCommon.Data" %>
<script type="text/javascript">
    $(document).ready(function () {
        var pageId = '<%=Model.PageId%>';
        var pySearch = "";
        $("#PYStoreSiteSearch" + pageId).keydown(function (event) {
            if (event.keyCode == 13) {
                pySearch = $("#PYStoreSiteSearch" + pageId).val();
                var urlStr = '<%=Url.Action("SearchTree", "StoreSite", new { Area = "BasicData"}) %>';
                $.ajax({
                        type: 'POST',
                        url: urlStr,
                        data: { pageId: pageId, pySearch: pySearch },
                        success: function (jsonObj) {
//                            setTimeout(function () { if (jsonObj != "0") {
//                                $("#DivStoreSiteTree" + pageId).empty();
//                                $("#DivStoreSiteTree" + pageId).append(jsonObj);
                            //                             }}, 1000);
                            if (jsonObj != "0") {
                                $("#DivStoreSiteTree" + pageId).empty();
                                $("#DivStoreSiteTree" + pageId).append(jsonObj);
                            }
                        }
                    });

            }
        });
      
    });
</script>
<div class="editor-field">
    <%:Html.AppTextBoxFor(m => m.PYStoreSiteSearch, Model.PageId, "StoreSiteEntry")%>
</div>
<div id="DivStoreSiteTree<%=Model.PageId %>">
<%:Html.AppTreeViewFor(Model.PageId,TreeId.StoreSiteTreeId, Model.StoreSiteTree,false)%>
</div>
