using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppUpload
    {
        public static MvcHtmlString AppFileUpload(this HtmlHelper htmlHelper, string pageId, string fileInputId, string btntext,string checkUrl,string uploadUrl,string filePath,string isauto)
        {
            fileInputId = fileInputId + pageId;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<input id='" + fileInputId + "' name='" + fileInputId + "'  type='file'  />");
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("$('#" + fileInputId + "').uploadify({");
            //sb.AppendLine("'buttonText': '"+btntext+"',");
            //sb.AppendLine("'checkExisting': '/Home/CheckExisting/" + folder + "',");
            //sb.AppendLine("'swf': '/Content/css/uploadify/uploadify.swf',");
            //sb.AppendLine("'uploader': '/Home/Upload/" + folder + "',");
            sb.AppendLine("'checkExisting': '" + checkUrl + "',");
            sb.AppendLine("'buttonImage' : '" + filePath + "browse-btn.png',");
            sb.AppendLine("'swf': '" + filePath + "uploadify.swf',");
            sb.AppendLine("'uploader': '" + uploadUrl + "',");
            //sb.AppendLine("'height': 26,");
            //sb.AppendLine("'width': 80,");
            sb.AppendLine("'auto': " + isauto + ",");
            sb.AppendLine("'onUploadSuccess' : function(file, data, response) { ");
            sb.AppendLine(" AppUploadComplete(file, data, response); ");
            sb.AppendLine("} ");
            sb.AppendLine(" });");

            sb.AppendLine(" });");
            sb.AppendLine("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}