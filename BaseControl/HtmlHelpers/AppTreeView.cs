using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using BaseCommon.Data;
using BaseCommon.Basic;

namespace BaseControl.HtmlHelpers
{
    public static class AppTreeView
    {
        public static MvcHtmlString AppTreeViewFor(this HtmlHelper htmlHelper, string pageId, string treeViewId, DataTable dtTree, string btnSubmit)
        {
            //return TreeViewMvcHtmlString(pageId, treeViewId, dtTree, btnSubmit, true);
            return MvcHtmlString.Create(TreeViewString(pageId, treeViewId, dtTree, btnSubmit, true,""));
        }
        public static MvcHtmlString AppTreeViewFor(this HtmlHelper htmlHelper, string pageId, string treeViewId, DataTable dtTree, bool checkbox,string selectIds="")
        {
            //return TreeViewMvcHtmlString(pageId, treeViewId, dtTree, "", checkbox);
            return MvcHtmlString.Create(TreeViewString(pageId, treeViewId, dtTree, "", checkbox,selectIds));
        }

        public static string TreeViewString(string pageId, string treeViewId, DataTable dtTree, string btnSubmit, bool checkbox, string selectIds="")
        {
            treeViewId = treeViewId + pageId;
            btnSubmit = btnSubmit + pageId;
            string inputHidden = treeViewId + AppMember.HideString;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var setting = {");
            if (checkbox)
                sb.AppendLine(" check: { enable: true},");
            else
                sb.AppendLine(" check: { enable: false},");
            sb.AppendLine(" data: { simpleData: {enable: true}},");
            sb.AppendLine(" async: { enable: true},");
            sb.AppendLine(" view: {showIcon: false}");
            sb.AppendLine(" };");
            sb.AppendFormat(" var zNodes = [{0}];", SetTreeNode(dtTree,selectIds));

            sb.AppendLine("  function setCheck() {");
            sb.AppendLine("  var zTree = $.fn.zTree.getZTreeObj('" + treeViewId + "');");
            sb.AppendLine(" zTree.setting.check.chkboxType = { 'Y' : 'ps', 'N' : 'ps' };");
            sb.AppendLine("  }");

            sb.AppendLine(" $(document).ready(function () {");
            sb.AppendLine("  $.fn.zTree.init($('#" + treeViewId + "'), setting, zNodes);");
            sb.AppendLine(" setCheck();");
            sb.AppendLine("  });");

            sb.AppendLine("</script>");
            sb.AppendLine("<div class='content_wrap'>");
            sb.AppendLine("<div class='zTreeDemoBackground left'>");
            sb.AppendLine("<ul id='" + treeViewId + "' class='ztree'></ul>	  ");
            sb.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" ></input>", inputHidden);
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            if (btnSubmit != "")
                SetTreeView(treeViewId, btnSubmit, inputHidden, sb);
            return sb.ToString();
        }



        //设置树时的datatable必须第一列是id,第二列是父类id,第三项是名称,第四项是是否打开的bool值,第五项是是否选中
        private static string SetTreeNode(DataTable dtTree, string selectIds)
        {
            List<string> checkIds=new List<string>();
            if (DataConvert.ToString(selectIds) != "")
            {
                string[] ids = selectIds.Split(',');
                checkIds=ids.ToList();
            }
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dtTree.Rows)
            {
                sb.Append("{");
                sb.AppendFormat("id:'{0}',", dr[0]);
                sb.AppendFormat("pId:'{0}',", dr[1]);
                sb.AppendFormat("name:'{0}', ", dr[2]);
                sb.AppendFormat("open:{0}, ", DataConvert.ToString(dr[3]).ToLower());
                if(checkIds.Contains(DataConvert.ToString(dr[0])))
                     sb.AppendFormat("checked:{0}", "true");
                else
                     sb.AppendFormat("checked:{0}", DataConvert.ToString(dr[4]).ToLower());
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static MvcHtmlString SetTreeView(string treeViewId, string btn, string inputHidden, StringBuilder sb)
        {
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("$('#" + btn + "').mouseup(function () {");
            sb.AppendLine("  var nodestring = '';");
            sb.AppendLine(" var treeObj = $.fn.zTree.getZTreeObj('" + treeViewId + "');");
            sb.AppendLine("var nodes = treeObj.getCheckedNodes(true);");
            sb.AppendLine("  $.each(nodes, function (n, vlaue) {");
            sb.AppendFormat(" var node = nodes[n];");
            sb.AppendLine("   $.each(node, function (i, name) {");
            sb.AppendLine("  if (i == 'id') {");
            sb.AppendLine("  nodestring = nodestring + name + ',';");
            sb.AppendLine("  } }); });");
            sb.AppendFormat("$(\"#{0}\").val(nodestring);", inputHidden);


            //sb.AppendLine("  alert(JSON.stringify(nodes));");
            //sb.AppendFormat("$(\"#{0}\").attr(\"value\",JSON.stringify(nodes));", inputHidden);

            sb.AppendLine("  }); });");
            sb.AppendLine("   </script>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}