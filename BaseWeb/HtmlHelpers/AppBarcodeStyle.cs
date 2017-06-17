using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace WebApp.BaseWeb.HtmlHelpers
{
    public static class AppBarcodeStyle
    {
        public static MvcHtmlString AppBarcodeStyleFor(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, string styleId, string assetsId)
        {
            return MvcHtmlString.Create(BarcodeStyle(urlHelper, pageId, styleId, assetsId));
        }

        public static string BarcodeStyle(UrlHelper urlHelper, string pageId, string styleId, string assetsId)
        {
            StringBuilder sb = new StringBuilder();
            string tagStr = "";
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", styleId);
            string sql = string.Format(@"select BarcodeStyleDetail.* from BarcodeStyle,BarcodeStyleDetail 
                        where BarcodeStyle.barcodeStyleId=BarcodeStyleDetail.barcodeStyleId 
                        and BarcodeStyle.barcodeStyleId=@barcodeStyleId ");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];

            DataRow[] drRectangle = dtGrid.Select("nodeType='rectangle'");
            string rectangleWidth = DataConvert.ToString(drRectangle[0]["width"]);
            string rectangleHeight = DataConvert.ToString(drRectangle[0]["height"]);
            string rectangleX = DataConvert.ToString(drRectangle[0]["x"]);
            string rectangleY = DataConvert.ToString(drRectangle[0]["y"]);

            paras.Add("assetsId", assetsId);
            string sqlAssets = string.Format(@"select * from Assets where assetsId=@assetsId ");
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sqlAssets, paras).Tables[0];

            DataRow[] drOther = dtGrid.Select("nodeType<>'rectangle'");
            foreach (DataRow dr in drOther)
            {
                if (DataConvert.ToString(dr["nodeType"]) == "textField")
                {
                    string refField = DataConvert.ToString(dr["refField"]);
                    TagBuilder tg = new TagBuilder("label");
                    tg.GenerateId(refField + "_textField" + pageId);
                    tg.InnerHtml = DataConvert.ToString(dtAssets.Rows[0][refField]);
                    tg.MergeAttribute("style", "width:" + DataConvert.ToString(dr["width"]) + "px; height: " + DataConvert.ToString(dr["height"]) + "px; position: absolute;");
                    tagStr += tg.ToString(TagRenderMode.Normal);
                    string x = DataConvert.ToString(DataConvert.ToInt32(dr["x"]) - DataConvert.ToInt32(rectangleX));
                    string y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("$(function(){");
                    sb.AppendLine("var offset = $('#BarcodeStyleDiv" + pageId + "').position();");
                    sb.AppendLine("$('#" + refField + "_textField" + pageId + "').css({ left: offset.left +  " + x + ", top: offset.top + " + y + " });");
                    sb.AppendLine("});");
                    sb.AppendLine("</script>");
                }
                if (DataConvert.ToString(dr["nodeType"]) == "imageField")
                {
                    string refField = DataConvert.ToString(dr["refField"]);
                    TagBuilder tg = new TagBuilder("img");
                    if (refField == "imgSystem")
                        tg.MergeAttribute("src", urlHelper.Content("~/Content/uploads/assets/sys.jpg"));
                    else
                        tg.MergeAttribute("src", urlHelper.Content("~/Content/uploads/assets/") + DataConvert.ToString(dtAssets.Rows[0][refField]));
                    tg.GenerateId(refField + "_imageField" + pageId);
                    tg.MergeAttribute("style", "width:" + DataConvert.ToString(dr["width"]) + "px; height: " + DataConvert.ToString(dr["height"]) + "px; position: absolute;");
                    tagStr += tg.ToString(TagRenderMode.Normal);
                    string x = DataConvert.ToString(DataConvert.ToInt32(dr["x"]) - DataConvert.ToInt32(rectangleX));
                    string y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("$(function(){");
                    sb.AppendLine("var offset = $('#BarcodeStyleDiv" + pageId + "').position();");
                    sb.AppendLine("$('#" + refField + "_imageField" + pageId + "').css({ left: offset.left +  " + x + ", top: offset.top + " + y + " });");
                    sb.AppendLine("});");
                    sb.AppendLine("</script>");
                }
                if (DataConvert.ToString(dr["nodeType"]) == "barcode")
                {
                    string x = DataConvert.ToString(DataConvert.ToInt32(dr["x"]) - DataConvert.ToInt32(rectangleX));
                    string y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    string refField = DataConvert.ToString(dr["refField"]);
                    sb.AppendLine("<div id='" + refField + "barcode" + pageId + "' style='position: absolute;'></div>");
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("$(function(){");
                    sb.AppendLine("var offset = $('#BarcodeStyleDiv" + pageId + "').position();");
                    sb.AppendLine("$('#" + refField + "barcode" + pageId + "').barcode('" + DataConvert.ToString(dtAssets.Rows[0][refField]) + "', 'code39',{showHRI:false});");
                    sb.AppendLine("$('#" + refField + "barcode" + pageId + "').css({ left: offset.left +  " + x + ", top: offset.top + " + y + " });");
                    sb.AppendLine("});");
                    sb.AppendLine("</script>");
                }
            }


            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(function(){");
            sb.AppendLine("$('#BarcodeStyleDiv" + pageId + "').width(" + rectangleWidth + ")");
            sb.AppendLine("$('#BarcodeStyleDiv" + pageId + "').height(" + rectangleHeight + ")");
            sb.AppendLine("$('#BarcodeStyleFieldSet" + pageId + "').width(" + rectangleWidth + ")");
            sb.AppendLine("$('#BarcodeStyleFieldSet" + pageId + "').height(" + rectangleHeight + ")");
            sb.AppendLine("});");
            sb.AppendLine("</script>");


            return sb.ToString() + Environment.NewLine + tagStr;
        }

    }
}