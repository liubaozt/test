<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="BaseCommon.Basic" %>
<%@ Import Namespace="BaseCommon.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10;IE=9; IE=8 " />
    <title> <%=AppMember.AppText["SysName"].ToString()%></title>
    <%=TempData["CssBlock"]%>
    <style type="text/css">
        html, body
        {
            margin: 0; /* Remove body margin/padding */
            padding: 0;
            overflow: hidden; /* Remove scroll bars on browser window */
        }
        /*Splitter style */
        
        
        #LeftPane
        {
            /* optional, initial splitbar position */
            overflow: auto;
        }
        /*
         * Right-side element of the splitter.
        */
        
        #RightPane
        {
            padding: 2px;
            overflow: auto;
        }
        .ui-tabs-nav li
        {
            position: relative;
        }
        .ui-tabs-selected a span
        {
            padding-right: 10px;
        }
        .ui-tabs-close
        {
            display: none;
            position: absolute;
            top: 3px;
            right: 0px;
            z-index: 800;
            width: 16px;
            height: 14px;
            font-size: 10px;
            font-style: normal;
            cursor: pointer;
        }
        .ui-tabs-selected .ui-tabs-close
        {
            display: block;
        }
        .ui-layout-west .ui-jqgrid tr.jqgrow td
        {
            border-bottom: 0px none;
        }
        .ui-datepicker
        {
            z-index: 1200;
        }
    </style>
    <%=TempData["ScriptBlock"]%>
    <script type="text/javascript">

        jQuery(document).ready(function () {
            //$('#switcher').themeswitcher();
            var menuUrl = '<%=Url.Action("Menu", "Home") %>';
            $('body').layout({
                resizerClass: 'ui-state-default',
                west__onresize: function (pane, $Pane) {
                    jQuery("#west-grid").jqGrid('setGridWidth', $Pane.innerWidth() - 2);
                }
            });
            $.jgrid.defaults = $.extend($.jgrid.defaults, { loadui: "enable" });
            var maintab = jQuery('#tabs', '#RightPane').tabs({
                add: function (e, ui) {
                    // append close thingy
                    $(ui.tab).parents('li:first')
                .append('<span class="ui-tabs-close ui-icon ui-icon-close" title="Close Tab"></span>')
                .find('span.ui-tabs-close')
				.show()
                .click(function () {
                    maintab.tabs('remove', $('li', maintab).index($(this).parents('li:first')[0]));
                });
                    // select just added tab
                    maintab.tabs('select', '#' + ui.panel.id);
                }
            });
            jQuery("#west-grid").jqGrid({
                //url: "../../Content/tree.xml",
                //datatype: "xml",
                url: menuUrl,
                datatype: "json",
                height: "auto",
                pager: false,
                loadui: "disable",
                colNames: ["id", " ", "url"],
                colModel: [
            { name: "id", width: 1, hidden: true, key: true },
            { name: "menu", width: 150, resizable: false, sortable: false },
            { name: "url", width: 1, hidden: true }
        ],
                treeGrid: true,
                caption: '功能菜单',
                ExpandColumn: "menu",
                autowidth: true,
                //width: 180,
                rowNum: 200,
                ExpandColClick: false,
                treeIcons: { leaf: 'ui-icon-document-b' },
                onSelectRow: function (rowid) {
                    var treedata = $("#west-grid").jqGrid('getRowData', rowid);
                    if (treedata.isLeaf == "true") {
                        //treedata.url
                        var st = "#t" + treedata.id;
                        if ($(st).html() != null) {
                            maintab.tabs('select', st);
                        } else {
                            maintab.tabs('add', st, treedata.menu);
                            //$(st,"#tabs").load(treedata.url);
                            var navurl = treedata.url;
                            $.ajax({
                                url: treedata.url,
                                type: "GET",
                                dataType: "html",
                                data: { pageId: treedata.id, viewTitle: treedata.menu },
                                complete: function (req, err) {
                                    $(st, "#tabs").append(req.responseText);
                                }
                            });
                        }
                    }
                }
            });

            //初始加载资产管理 start
            var st = "#t" + "2015";
            if ($(st).html() != null) {
                maintab.tabs('select', st);
            } else {
                maintab.tabs('add', st, "资产管理");
                var navurl = '<%=Url.Action("List", "AssetsManage",new { Area = "AssetsBusiness"}) %>';
                $.ajax({
                    url: navurl,
                    type: "GET",
                    dataType: "html",
                    data: { pageId: "2015", viewTitle: "资产管理" },
                    complete: function (req, err) {
                        $(st, "#tabs").append(req.responseText);
                    }
                });
            }
            //初始加载资产管理 end

            // end splitter

        });
    </script>


    
</head>
<body>
    <div id="TopPane" class="ui-layout-north indexrightbg">
        <div id="logo" class="indexlogo">
            <img src='<%=Url.Content("~/Content/images/toplogo.png")%>' /></div>
        <br />
        <p id="info_bar">
            公司：
            <%=((UserInfo)ViewData["sysUser"]).CompanyName%>
            <br />
            帐号：
            <%=((UserInfo)ViewData["sysUser"]).UserName%>
            | <a href='<%=Url.Action("LogOut", "Home")%>' class="white">注销</a>
            <br />
            帐套：
             <%=((UserInfo)ViewData["sysUser"]).MySetBooks.SetBooksName%>
        </p>
    </div>
    <div id="LeftPane" class="ui-layout-west ui-widget ui-widget-content">
        <table id="west-grid">
        </table>
    </div>
    <!-- #LeftPane -->
    <div id="RightPane" class="ui-layout-center ui-helper-reset ui-widget-content">
        <!-- Tabs pane -->
        <div id="switcher">
        </div>
        <div id="tabs" class="jqgtabs">
            <ul>
                <li><a href="#tabs-1">主页</a></li>
            </ul>
            <div id="tabs-1" style="font-size: 12px;">
                <br />
                <br />
            </div>
        </div>
    </div>
    <!-- #RightPane -->
</body>
</html>
