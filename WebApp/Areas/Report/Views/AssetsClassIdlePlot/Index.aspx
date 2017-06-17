<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Plot.Master" Inherits="System.Web.Mvc.ViewPage<BusinessLogic.Report.Models.AssetsClassPlot.EntryModel>" %>

<%@ Import Namespace="BaseControl.HtmlHelpers" %>
<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<%@ Import Namespace="WebCommon.Data" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <div id="chart<%=Model.PageId %>" style="width: 900px; height: 360px;">
        </div>
        <div id="image<%=Model.PageId %>" style="width: 900px;">
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ConditonContent" runat="server">
    <div class="editor-field">
        <div class="PlotReportColumn1">
            <%:Html.AppLabelFor(m => m.CompanyId, Model.PageId, "PlotReport")%>
            <%:Html.AppDropDownListFor(m => m.CompanyId, Model.PageId, Url.Action("DropList", "Company", new { Area = "BusinessCommon" }), "AssetsList")%>
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
                        $('#chart' + pageId).width(chartWeight);
                        $.jqplot('chart' + pageId, [line1, line2], {
                            title: '',
                            series: [{
                                renderer: $.jqplot.BarRenderer,
                                showMarker: true,
                                rendererOptions: {
                                    barPadding: 15,      //设置同一分类两个柱状条之间的距离（px）  
                                    barMargin: 15,      //设置不同分类的两个柱状条之间的距离（px）（同一个横坐标表点上）  
                                    barDirection: 'vertical', //设置柱状图显示的方向：垂直显示和水平显示  
                                    //，默认垂直显示 vertical or horizontal.  
                                    barWidth: 25     // 设置柱状图中每个柱状条的宽度  
                                    //shadowOffset: 2,    // 同grid相同属性设置  
                                    //shadowDepth: 5,     // 同grid相同属性设置  
                                    //shadowAlpha: 0.8,   // 同grid相同属性设置  
                                }
                            },
                        { xaxis: 'x2axis', yaxis: 'y2axis', showLine: false,
                            pointLabels: {
                                show: false,
                                location: 's', //数据标签显示在数据点附近的方位
                                ypadding: 2 //数据标签距数据点在纵轴方向上的距离
                            }
                        }],
                            axesDefaults: {
                                tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                                tickOptions: {
                                    //fontFamily: 'Georgia',
                                    angle: -20,
                                    fontSize: '10pt'
                                    //labelPosition: 'start'
                                }
                            },
                            axes: {
                                xaxis: {
                                    renderer: $.jqplot.CategoryAxisRenderer
                                },
                                yaxis: {
                                    autoscale: true
                                },
                                x2axis: {
                                    show: false,
                                    showTicks: false,
                                    showTickMarks: false,
                                    renderer: $.jqplot.CategoryAxisRenderer //指定X轴显示分类
                                },
                                y2axis: {
                                    autoscale: true
                                }
                            },
                            legend: {
                                show: true, //设置是否出现分类名称框（即所有分类的名称出现在图的某个位置） 
                                location: 'ne', // 分类名称框出现位置, nw, n, ne, e, se, s, sw, w. 
                                labels: ['闲置数量', '闲置率'],
                                xoffset: 12, // 分类名称框距图表区域上边框的距离（单位px） 
                                yoffset: 12 // 分类名称框距图表区域左边框的距离(单位px) 
                            }
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
