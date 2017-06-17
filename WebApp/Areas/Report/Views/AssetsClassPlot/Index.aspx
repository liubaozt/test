<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Plot.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsClassPlot.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div id="chart<%=Model.PageId %>" style="width: 600px; height: 400px;">
        </div>
        <div id="image<%=Model.PageId %>" style="width: 600px;">
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ConditonContent" runat="server">
    <div class="editor-field">
        <div class="PlotReportColumn1">
            <%:Html.AppLabelFor(m => m.CompanyId, Model.PageId, "PlotReport")%>
            <%:Html.AppDropDownListFor(m => m.CompanyId, Model.PageId, Url.Action("DropList", "Company", new { Area = "BusinessCommon" }), "PlotReport")%>
        </div>
        <div class="PlotReportColumn2">
            <%:Html.AppLabelFor(m => m.DepartmentId, Model.PageId, "PlotReport")%>
            <%:Html.AppTreeDialogFor(m => m.DepartmentId, Model.PageId, Model.DepartmentUrl, Model.DepartmentDialogUrl, AppMember.AppText["DepartmentSelect"], TreeId.DepartmentTreeId, Model.DepartmentAddFavoritUrl, Model.DepartmentReplaceFavoritUrl, "PlotReport")%>
        </div>
        <%:Html.AppNormalButton(Model.PageId, "btnQuery", AppMember.AppText["BtnQuery"])%>
          <%:Html.AppNormalButton(Model.PageId, "btnPrint", AppMember.AppText["BtnExportImage"])%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var pageId = '<%=Model.PageId %>';
            var formId = '<%=Model.FormId %>';
            QueryPlot(pageId, "");
            $("#btnPrint" + pageId).click(function () {
                var chart = $('#chart' + pageId);
                //chart.jqplotViewImage();
                $('#image' + pageId).show();
                var str = chart.jqplotToImageElem({ backgroundColor: chart.css('background-color') });
                $('#image' + pageId).html(str);
                $('#chart' + pageId).hide();
            });
            function QueryPlot(pageId, formvar) {
                $.ajax({
                    type: 'GET',
                    url: '<%=Url.Action("Search") %>',
                    data: { pageId: pageId, formVar: formvar },
                    success: function (html) {
                        jQuery.jqplot.config.enablePlugins = true;
                        //            var line1 = [['办公设备', 7], ['存储设备', 9], ['电脑/笔记本', 36],
                        //                         ['电脑/台式机', 80], ['服务器', 3],
                        //                         ['机房设备', 6], ['手机', 28], ['网络设备', 18]];
                        //  
                        //                    var line1 = [7, 8, 10, 29, 30, 12];
                        //                    var ticks = ['充值', '提款', '应收', '销售', '退票', '验证'];
                        //                    var line1 = [[7,'办公设备'], [9,'存储设备'], [30,'电脑/笔记本'],
                        //                                    [80,'电脑/台式机'], [3,'服务器'],
                        //                                    [6,'机房设备'], [28,'手机'], [18,'网络设备']];                 
                        $('#chart' + pageId).height(chartHeight);
                        $.jqplot('chart' + pageId, [line1], {
                            title: '',
                            series: [{
                                renderer: $.jqplot.BarRenderer,
                                showMarker: true,
                                rendererOptions: {
                                    //barPadding: 8,      //设置同一分类两个柱状条之间的距离（px）  
                                    //barMargin: 10,      //设置不同分类的两个柱状条之间的距离（px）（同一个横坐标表点上）  
                                    barDirection: 'horizontal', //设置柱状图显示的方向：垂直显示和水平显示  
                                    //，默认垂直显示 vertical or horizontal.  
                                    barWidth: 25     // 设置柱状图中每个柱状条的宽度  
                                    //shadowOffset: 2,    // 同grid相同属性设置  
                                    //shadowDepth: 5,     // 同grid相同属性设置  
                                    //shadowAlpha: 0.8,   // 同grid相同属性设置  
                                }
                            }],
                            axesDefaults: {
                                tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                                tickOptions: {
                                    //fontFamily: '宋体',
                                    angle: 0,
                                    fontSize: '10pt'
                                }
                            },
                            axes: {
                                xaxis: {
                                // renderer: $.jqplot.CategoryAxisRenderer
                            },
                            yaxis: {
                                //ticks: ticks,
                                //label: "数量" ,
                                renderer: $.jqplot.CategoryAxisRenderer
                            }
                        }
                        //                        legend: {
                        //                            show: true, //设置是否出现分类名称框（即所有分类的名称出现在图的某个位置） 
                        //                            location: 'ne', // 分类名称框出现位置, nw, n, ne, e, se, s, sw, w. 
                        //                            labels: ['资产分类'],
                        //                            xoffset: 12, // 分类名称框距图表区域上边框的距离（单位px） 
                        //                            yoffset: 12 // 分类名称框距图表区域左边框的距离(单位px) 
                        //                        }
                    });
                }
            });
        }

        $('#btnQuery' + pageId).click(function () {
            $('#image' + pageId).hide();
            $('#chart' + pageId).show();
            $('#chart' + pageId).empty();
            var obj = $('#' + formId + pageId).serializeObject();
            var formvar = JSON.stringify(obj);
            QueryPlot(pageId, formvar);
        });

    });
    </script>
</asp:Content>
