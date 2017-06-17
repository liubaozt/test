<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ApproveEntry.Master"
    Inherits="System.Web.Mvc.ViewPage<BusinessLogic.AssetsBusiness.Models.AssetsManage.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.AssetsNo, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsNo, Model.PageId, "NoAssetsEntry")%>
            <%:Html.AppNormalButton(Model.PageId, "btnAutoNo", "ui-icon-pencil", "autoNoButton")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.AssetsName, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsName, Model.PageId, "AssetsEntry")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <%:Html.ValidationMessageFor(m=>m.AssetsNo) %>
        <%:Html.ValidationMessageFor(m => m.AssetsName)%>
    </div>
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.AssetsClassId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTreeDialogFor(m => m.AssetsClassId, Model.PageId, Model.AssetsClassUrl, Model.AssetsClassDialogUrl, AppMember.AppText["AssetsClassSelect"], TreeId.AssetsClassTreeId, Model.AssetsClassAddFavoritUrl, Model.AssetsClassReplaceFavoritUrl, "AssetsEntry")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "AssetsEntry")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsClassId)%>
        <%:Html.ValidationMessageFor(m => m.DepartmentId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.StoreSiteId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTreeDialogFor(m => m.StoreSiteId, Model.PageId, Model.StoreSiteUrl, Model.StoreSiteDialogUrl, AppMember.AppText["StoreSiteSelect"], TreeId.StoreSiteTreeId, Model.StoreSiteAddFavoritUrl, Model.StoreSiteReplaceFavoritUrl, "AssetsEntry")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.PurchaseTypeId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.PurchaseTypeId, Model.PageId, Url.Action("DropList", "PurchaseType", new { Area = "BasicData" }), "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.StoreSiteId)%>
        <%:Html.ValidationMessageFor(m => m.PurchaseTypeId)%>
    </div>
   
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.UsePeople, Model.PageId, "AssetsEntry")%>
            <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
              { %>
            <%:Html.AppDropDownEditorFor(m => m.UsePeople, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsEntry")%>
            <%}
              else
              { %>
            <%:Html.AppAutoCompleteFor(m => m.UsePeople, Model.PageId, "AssetsEntry", Model.UserSource)%>
            <%}  %>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.Keeper, Model.PageId, "AssetsEntry")%>
            <%if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
              { %>
            <%:Html.AppDropDownEditorFor(m => m.Keeper, Model.PageId, Url.Action("DropList", "User", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + DataConvert.ToString(Model.DepartmentId) + DFT.SQ }), "AssetsEntry")%>
            <%}
              else
              { %>
            <%:Html.AppAutoCompleteFor(m => m.Keeper, Model.PageId, "AssetsEntry", Model.UserSource)%>
            <%}  %>
        </div>
        <%:Html.ValidationMessageFor(m => m.UsePeople)%>
        <%:Html.ValidationMessageFor(m => m.Keeper)%>
    </div>
  
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.PurchaseDate, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDatePickerFor(m => m.PurchaseDate, Model.PageId, "AssetsEntry")%>
            <%:Html.AppRequiredFlag()%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.PurchaseDate2, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDatePickerFor(m => m.PurchaseDate2, Model.PageId, "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.PurchaseDate)%>
        <%:Html.ValidationMessageFor(m => m.PurchaseDate2)%>
    </div>
    <%if (!AppMember.DepreciationRuleOpen)
      { %>
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.DurableYears, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.DurableYears, Model.PageId, "AssetsEntry")%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.RemainRate, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.RemainRate, Model.PageId, "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.DurableYears)%>
        <%:Html.ValidationMessageFor(m => m.RemainRate)%>
    </div>
    <%} %>
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.AssetsValue, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsValue, Model.PageId, "AssetsEntry")%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.Remark, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.Remark, Model.PageId, "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsValue)%>
        <%:Html.ValidationMessageFor(m => m.Remark)%>
    </div>
      <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.AssetsBarcode, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.AssetsBarcode, Model.PageId, "AssetsEntry")%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.Spec, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.Spec, Model.PageId, "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsBarcode)%>
        <%:Html.ValidationMessageFor(m => m.Spec)%>
    </div>
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.GuaranteeDays, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.GuaranteeDays, Model.PageId, "AssetsEntry")%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.MaintainDays, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.MaintainDays, Model.PageId, "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.GuaranteeDays)%>
        <%:Html.ValidationMessageFor(m => m.MaintainDays)%>
    </div>
     <div class="editor-field">
        <div class="AssetsEntryColumn2">
            <%if (AppMember.DepreciationRuleOpen)
              { %>
            <%:Html.AppLabelFor(m => m.DepreciationRule, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.DepreciationRule, Model.PageId, Url.Action("DropList", "DepreciationRule", new { Area = "BasicData" }), "AssetsEntry")%>
            <%}
              else
              { %>
            <%:Html.AppLabelFor(m => m.DepreciationType, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.DepreciationType, Model.PageId, Url.Action("DropList", "CodeTable", new { Area = "", filter = "DepreciationType" }), "AssetsEntry")%>
            <%}  %>
        </div>
        <%if (AppMember.DepreciationRuleOpen)
          { %>
        <%:Html.ValidationMessageFor(m => m.DepreciationRule)%>
        <%}
          else
          { %>
        <%:Html.ValidationMessageFor(m => m.DepreciationType)%>
        <%}  %>
     <%--   <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.AssetsUsesId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.AssetsUsesId, Model.PageId, Url.Action("DropList", "AssetsUses", new { Area = "BasicData" }), "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.AssetsUsesId)%>--%>
    </div>
    <%-- <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.InvoiceNo, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.InvoiceNo, Model.PageId, "AssetsEntry")%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.PurchaseNo, Model.PageId, "AssetsEntry")%>
            <%:Html.AppTextBoxFor(m => m.PurchaseNo, Model.PageId, "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.InvoiceNo)%>
        <%:Html.ValidationMessageFor(m => m.PurchaseNo)%>
    </div>
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.SupplierId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.SupplierId, Model.PageId, Url.Action("DropList", "Supplier", new { Area = "BasicData" }), "AssetsEntry")%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.UnitId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.UnitId, Model.PageId, Url.Action("DropList", "Unit", new { Area = "BasicData", filterExpression = "unitType=" + DFT.SQ + "N" + DFT.SQ }), "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.SupplierId)%>
        <%:Html.ValidationMessageFor(m => m.UnitId)%>
    </div>
    <div class="editor-field">
        <div class="AssetsEntryColumn1">
            <%:Html.AppLabelFor(m => m.EquityOwnerId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.EquityOwnerId, Model.PageId, Url.Action("DropList", "EquityOwner", new { Area = "BasicData" }), "AssetsEntry")%>
        </div>
        <div class="AssetsEntryColumn2">
            <%:Html.AppLabelFor(m => m.ProjectManageId, Model.PageId, "AssetsEntry")%>
            <%:Html.AppDropDownListFor(m => m.ProjectManageId, Model.PageId, Url.Action("DropList", "ProjectManage", new { Area = "BasicData" }), "AssetsEntry")%>
        </div>
        <%:Html.ValidationMessageFor(m => m.EquityOwnerId)%>
        <%:Html.ValidationMessageFor(m => m.ProjectManageId)%>
    </div>--%>
    <%:Html.AppHiddenFor(m => m.AssetsId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.AssetsPurchaseDetailId, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.AssetsPurchaseId, Model.PageId)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            //#region 公共变量
            var pageId = '<%=Model.PageId%>';
            var formMode = '<%=Model.FormMode%>';
            //#endregion 公共变量

            //#region 初始化处理
            $('#ullist' + pageId).show();
            $('#tabs' + pageId).tabs();
            //#endregion 初始化处理

            //#region 字段输入校验 
            $('#AssetsValue' + pageId).keydown(function (e) {
                if (!CheckInputData(e, "#AssetsValue" + pageId, "Numerical", true, 0))
                    return false;
            }).focus(function () { this.style.imeMode = 'disabled'; });
            $('#DurableYears' + pageId).keydown(function (e) {
                if (!CheckInputData(e, "#DurableYears" + pageId, "Int", true, 0))
                    return false;
            }).focus(function () { this.style.imeMode = 'disabled'; });
            $('#RemainRate' + pageId).keydown(function (e) {
                if (!CheckInputData(e, "#RemainRate" + pageId, "Numerical", true, 0))
                    return false;
            }).focus(function () { this.style.imeMode = 'disabled'; });
            $('#GuaranteeDays' + pageId).keydown(function (e) {
                if (!CheckInputData(e, "#GuaranteeDays" + pageId, "Int", true, 0))
                    return false;
            }).focus(function () { this.style.imeMode = 'disabled'; });
            $('#MaintainDays' + pageId).keydown(function (e) {
                if (!CheckInputData(e, "#MaintainDays" + pageId, "Int", false, 0))
                    return false;
            }).focus(function () { this.style.imeMode = 'disabled'; });
            //#endregion 字段输入校验 

            //#region 基本信息画面处理
            var departmentIdObj = "#DepartmentId" + pageId;
            var usePeopleObj = "#UsePeople" + pageId;
            var keeperObj = "#Keeper" + pageId;
            var assetsClassIdObj = "#AssetsClassId" + pageId;
            $(departmentIdObj).change(function () {
                var departmentId = $(departmentIdObj).val();
                if ($.trim(departmentId) == "") {
                    departmentId = "none";
                }
                var urlStr = '<%=Url.Action("DropList", "User", new { Area = "BusinessCommon"}) %>' + '/?filterExpression=departmentId=' + '<%=DFT.SQ %>' + departmentId + '<%=DFT.SQ %>';
                $.getJSON(urlStr, function (data) {
                    AppAppendSelect2(data, usePeopleObj, urlStr);
                    AppAppendSelect2(data, keeperObj, urlStr);
                });
            });
            $(assetsClassIdObj).change(function () {
                var assetsClassId = $(assetsClassIdObj).val();
                var urlStr = '<%=Url.Action("GetDefaultByAssetsClass", "AssetsClass", new { Area = "BasicData"}) %>' + '/?assetsClassId=' + assetsClassId;
                $.getJSON(urlStr, function (data) {
                    $('#DepreciationType' + pageId).val(data["depreciationType"]);
                    $('#UnitId' + pageId).val(data["unitId"]);
                    $('#DurableYears' + pageId).val(data["durableYears"]);
                    $('#RemainRate' + pageId).val(data["remainRate"]);
                    $('#AssetsBarcode' + pageId).val(data["assetsBarcode"]);
                });
            });

            $('#btnAutoNo' + pageId).click(function () {
                $.ajax({
                    type: "POST",
                    url: '<%=Url.Action("GetAutoNo") %>',
                    datatype: "text",
                    success: function (data) {
                        $('#AssetsNo' + pageId).val(data);
                    }
                });
                return false;
            });

            //#endregion 基本信息画面处理

            //#region 附加信息画面处理
            var imgPath = '<%= Url.Content("~/Content/uploads/assets/")%>';
            if (formMode == "approve") {
                $('#btnDefault' + pageId).hide();
                $('#btnDelete' + pageId).hide();
            }
            AppUploadCompleteAssetsUpload = function (file, data, response) {
                //file.name
                $('#assetsImg' + pageId).attr("src", imgPath + data);
                var fileContainer = $('#ImgFileContainer' + pageId).val();
                $('#ImgFileContainer' + pageId).val(fileContainer + data + ";");
                $('#ImgFileCurrent' + pageId).val(data);
                var defaultimg = $('#ImgFileDefault' + pageId).val();
                if (!defaultimg || defaultimg == "") {
                    $('#ImgFileDefault' + pageId).val(data);
                }

                fileContainer = $('#ImgFileContainer' + pageId).val();
                var fileArray = fileContainer.split(";");
                if (fileArray.length - 1 > 1) {
                    $('#btnPrevious' + pageId).attr('disabled', false);
                }
                $('#' + 'btnSave' + pageId).attr('disabled', false);
            }

            $('#btnPrevious' + pageId).click(function () {
                var fileContainer = $('#ImgFileContainer' + pageId).val();
                var fileArray = fileContainer.split(";");
                var fileCurrent = $('#ImgFileCurrent' + pageId).val();
                var currentIndex = 0;
                for (var i = 0; i < fileArray.length - 1; i++) {
                    if (fileArray[i] == fileCurrent) {
                        currentIndex = i;
                        break;
                    }
                }
                if (currentIndex <= 0) {
                    $('#btnPrevious' + pageId).attr('disabled', true);
                    return;
                }
                $('#btnNext' + pageId).attr('disabled', false);
                $('#assetsImg' + pageId).attr("src", imgPath + fileArray[currentIndex - 1]);
                $('#ImgFileCurrent' + pageId).val(fileArray[currentIndex - 1]);
            });
            $('#btnNext' + pageId).click(function () {
                var fileContainer = $('#ImgFileContainer' + pageId).val();
                var fileArray = fileContainer.split(";");
                var fileCurrent = $('#ImgFileCurrent' + pageId).val();
                var currentIndex = 0;
                for (var i = 0; i < fileArray.length - 1; i++) {
                    if (fileArray[i] == fileCurrent) {
                        currentIndex = i;
                        break;
                    }
                }
                if (currentIndex >= fileArray.length - 2) {
                    $('#btnNext' + pageId).attr('disabled', true);
                    return;
                }
                $('#btnPrevious' + pageId).attr('disabled', false);
                $('#assetsImg' + pageId).attr("src", imgPath + fileArray[currentIndex + 1]);
                $('#ImgFileCurrent' + pageId).val(fileArray[currentIndex + 1]);

            });
            $('#btnDelete' + pageId).click(function () {
                $('#MessageDialog' + pageId).html('<p style="float:left"><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span><%=AppMember.AppText["ConfirmDelete"]%></p>').dialog({
                    title: '<%=AppMember.AppText["MessageTitle"]%>',
                    height: 140,
                    modal: true,
                    resizable: false,
                    buttons: {
                        '<%=AppMember.AppText["BtnConfirm"]%>': function () {
                            $(this).dialog("close");
                            var filecurrent = $('#ImgFileCurrent' + pageId).val();
                            var deleteUrl = '<%=Url.Action("DeleteImg", "Home", new { Area = "" }) %>';
                            $.ajax({
                                type: 'POST',
                                url: deleteUrl,
                                data: { fileCurrent: filecurrent, fileDirectory: 'assets' },
                                success: function (jsonObj) {
                                    var fileContainer = $('#ImgFileContainer' + pageId).val();
                                    fileContainer = fileContainer.replace(filecurrent + ";", "");
                                    $('#ImgFileContainer' + pageId).val(fileContainer);
                                    AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["ImageDeleteSucceed"]%>', 'success', function () { });
                                    var fileArray = fileContainer.split(";");
                                    if (fileArray.length > 1) {
                                        $('#assetsImg' + pageId).attr("src", imgPath + fileArray[0]);
                                        $('#ImgFileCurrent' + pageId).val(fileArray[0]);
                                    }
                                    else {
                                        $('#assetsImg' + pageId).attr("src", "");
                                        $('#ImgFileCurrent' + pageId).val("");
                                    }
                                }
                            });
                        },
                        '<%=AppMember.AppText["BtnCancel"]%>': function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });
            $('#btnDefault' + pageId).click(function () {
                var filecurrent = $('#ImgFileCurrent' + pageId).val();
                $('#ImgFileDefault' + pageId).val(filecurrent);
                AppMessage(pageId, '<%=AppMember.AppText["MessageTitle"]%>', '<%=AppMember.AppText["SetDefaultSucceed"]%>', 'success', function () { });
            });
            //#endregion 附加信息画面处理

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TabsLiContent" runat="server">
    <li id="addition<%=Model.PageId%>"><a href="#tabs-addition-<%=Model.PageId%>">
        <%=AppMember.AppText["AdditionInfo"]%></a></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TabsDivContent" runat="server">
    <div id='tabs-addition-<%=Model.PageId%>' style="height: 330px;">
        <div class="editor-field" style="height: 250px;">
            <div class="fieldwidth3">
                <%:Html.AppLabelFor(m => m.ImgFileDefault, Model.PageId,"AssetsEntry")%>
            </div>
            <div class="fieldwidth4" style="width: 500px; height: 240px">
                <fieldset>
                    <img id="assetsImg<%=Model.PageId%>" alt="" style="width: 400px; height: 240px" src='<%= Url.Content(DataConvert.ToString( Model.ImgFileDefault)==""?"~/Content/images/imgnull.jpg":"~/Content/uploads/assets/"+Model.ImgFileDefault)%>' />
                </fieldset>
            </div>
        </div>
        <div class="editor-field" style="height: 30px;">
           
            <div class="AssetsEntryColumn4">
                <div class="AssetsEntryColumn5">
                    <%if (Model.FormMode != "approve")
                      {%>
                    <%:Html.AppFileUpload(Model.PageId, "AssetsUpload", AppMember.AppText["FileView"].ToString(), Url.Action("CheckExisting", "Home", new { Area = "" }) + "/assets",
                                Url.Action("Upload", "Home", new { Area = "" }) + "/assets", Url.Content("~/Content/css/uploadify/"), "true")%>
                    <%} %>
                </div>
                <div class="AssetsEntryColumn6">
                    <%:Html.AppNormalButton(Model.PageId, "btnDelete", AppMember.AppText["BtnDelete"])%>
                    <%:Html.AppNormalButton(Model.PageId, "btnDefault", AppMember.AppText["BtnDefault"])%>
                    <%:Html.AppNormalButton(Model.PageId, "btnPrevious", AppMember.AppText["BtnPrevious"])%>
                    <%:Html.AppNormalButton(Model.PageId, "btnNext", AppMember.AppText["BtnNext"])%>
                </div>
            </div>
        </div>
    </div>
    <%:Html.AppHiddenFor(m => m.ImgFileContainer, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.ImgFileCurrent, Model.PageId)%>
    <%:Html.AppHiddenFor(m => m.ImgFileDefault, Model.PageId)%>
</asp:Content>
