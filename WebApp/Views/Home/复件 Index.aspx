<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
    <link href="../../Content/css/redmond/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/css/redmond/ui.jqgrid.css" rel="stylesheet" type="text/css" />

   

    <script src='<%=Url.Content("~/Scripts/used/jquery-1.7.1.min.js")%>' type="text/javascript" ></script>
    <script src='<%=Url.Content("~/Scripts/used/jquery-ui-1.8.18.custom.min.js")%>' type="text/javascript" ></script>
    <script src='<%=Url.Content("~/Scripts/used/jquery.ui.datepicker-zh-CN.js")%>' type="text/javascript" ></script>
    <script src='<%=Url.Content("~/Scripts/used/grid.locale-cn.js")%>' type="text/javascript" ></script>
    <script src='<%=Url.Content("~/Scripts/used/jquery.jqGrid.js")%>' type="text/javascript" ></script>
      <script type="text/javascript">
          var onCellEdit;
          var lastsel;
          $(document).ready(function () {
              jQuery("#list").jqGrid({
                  url: '<%=Url.Action("EditGridData")%>',
                  datatype: "json",
                  //mtype: 'post',
                  colNames: ['主键', '名称', '编号', '备注'],
                  colModel: [
   		{ name: 'uid', index: 'uid', width: 55, editable: true, sorttype: 'int', editoptions: { dataUrl: '<%=Url.Action("ComboxData")%>' }, sortable: false },
   		{ name: 'name', index: 'name ', sortable: true, width: 100 },
        { name: 'no', index: 'no', editable: true, edittype: 'select', editoptions: { dataUrl: '<%=Url.Action("ComboxData")%>',
            buildSelect: function (data) {
                var jsondata = eval(data);
                var ret = "<select>";
                $.each(jsondata, function (i, item) {
                    ret += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                });
                ret += "</select>";
                return ret;
            }
        }, width: 100, editrules: {}
        },
   		{ name: 'remark', index: 'remark ', width: 100 }
   	],
                  rowNum: 10,
                  rowList: [10, 20, 30],
                  pager: '#pager2',
                  editable: false,
                  autoencode: true,
                  gridview: true,
                  height: 300,
                  editurl: '<%=Url.Action("EditGridData")%>',
                  cellEdit: true,
                  //cellsubmit: "clientArray",
                  cellsubmit: "remote",
                  cellurl: '<%=Url.Action("EditGridData")%>',
                  sortname: 'name',
                  sortable: true,
                  //                onSelectRow: function (id) {
                  //                    if (id && id !== lastsel) {
                  //                        jQuery('#list').jqGrid('restoreRow', lastsel);
                  //                        jQuery('#list').jqGrid('editRow', id, true);
                  //                        lastsel = id;
                  //                    }
                  //                },
                  //                gridComplete: function (id) {
                  //                    if (id && id !== lastsel) {
                  //                        jQuery("#list").jqGrid('saveRow', id);
                  //                        lastsel = id;
                  //                    }
                  //                },
                  viewrecords: true,
                  sortorder: "desc",
                  caption: "Edit Example",
                  width: 600,
                  shrinkToFit: false,
                  autowidth: true,
                  //multiselect: true,
                  modal: true,
                  jqModal: true
              });
              jQuery("#list").jqGrid('navGrid', '#pager', { edit: true, add: false, del: false })
                  ;
              $("#btnedit").click(function () {
                  var griddata = jQuery("#list").jqGrid('getRowData');
                  var griddatastring = '';
                  $.each(griddata, function (n, name) {
                      //回调函数有两个参数,第一个是元素索引,第二个为当前值
                      var griddatarow = griddata[n];
                      $.each(griddatarow, function (key, val) {
                          griddatastring += key + ':' + val + ",";
                      });
                      griddatastring = griddatastring.substring(0, griddatastring.length - 1) + "~";
                  });
                  griddatastring = griddatastring.substring(0, griddatastring.length - 1);
                  $.post("/Test/Test1", { 'AMCJqGridData': griddata });
              });
              $("#btnPdf").click(function () {
                  location.href = '/Test/ExportPdf';
              });

          });
    </script>
 
</head>
<body>
   <div  >
         <table id="list">
        </table>
        <div id="pager">
        </div>
    </div>
</body>
</html>
