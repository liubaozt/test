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

namespace BaseControl.HtmlHelpers
{


    public static class AppBarcodeStyle
    {
        public static MvcHtmlString AppBarcodeStyleFor<T>(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, DataTable barcodeStyle, T t)
        {
            string str = "";
            str += " <div id='BarcodeStyleDiv" + pageId + "' style='width: 210mm; height: 140mm; position: relative'>";
            str += BarcodeStyle(urlHelper, pageId, barcodeStyle, t);
            str += "</div>";
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString AppBarcodeStyleAllFor<T>(this HtmlHelper htmlHelper, UrlHelper urlHelper, string pageId, DataTable barcodeStyle, List<T> gridData)
        {
            string str = "";
            int i = 1;
            foreach (T t in gridData)
            {
                //str += "<fieldset id='BarcodeStyleFieldSet" + i.ToString() + "_" + pageId + "'>";
                str += " <div id='BarcodeStyleDiv" + i.ToString() + "_" + pageId + "' style='width: 210mm; height: 140mm; position: relative'>";
                str += BarcodeStyle(urlHelper,  i.ToString() + "_"+pageId, barcodeStyle, t, i);
                str += "<div style='page-break-before:always;'></div>";
                str += "</div>";
                //str += "</fieldset>";
                i++;
            }
            return MvcHtmlString.Create(str);
        }

        public static string BarcodeStyle<T>(UrlHelper urlHelper, string pageId, DataTable dtGrid, T t, int i = 1)
        {
            StringBuilder sb = new StringBuilder();
            string tagStr = "";
            //            Dictionary<string, object> paras = new Dictionary<string, object>();
            //            paras.Add("barcodeStyleId", styleId);
            //            string sql = string.Format(@"select BarcodeStyleDetail.* from BarcodeStyle,BarcodeStyleDetail 
            //                        where BarcodeStyle.barcodeStyleId=BarcodeStyleDetail.barcodeStyleId 
            //                        and BarcodeStyle.barcodeStyleId=@barcodeStyleId ");
            //            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];

            DataRow[] drRectangle = dtGrid.Select("nodeType='rectangle'");
            string rectangleWidth = DataConvert.ToString(drRectangle[0]["width"]);
            string rectangleHeight = DataConvert.ToString(drRectangle[0]["height"]);
            string rectangleX = DataConvert.ToString(drRectangle[0]["x"]);
            string rectangleY = DataConvert.ToString(drRectangle[0]["y"]);

            //            paras.Add("assetsId", assetsId);
            //            string sqlAssets = string.Format(@"select Assets.*,
            //                                        AppDepartment.departmentName departmentName,  
            //                                        StoreSite.storeSiteName storeSiteName,  
            //                                        A.userName usePeopleName,  
            //                                        B.userName keeperName,  
            //                                        AssetsUses.assetsUsesName assetsUsesName,  
            //                                        AssetsClass.assetsClassName assetsClassName
            //                                        from Assets
            //                                        left join AppDepartment on AppDepartment.departmentId=Assets.departmentId
            //                                        left join StoreSite on StoreSite.storeSiteId=Assets.storeSiteId 
            //                                        left join AssetsUses on AssetsUses.assetsUsesId=Assets.assetsUsesId 
            //                                        left join AssetsClass on AssetsClass.assetsClassId=Assets.assetsClassId
            //                                        left join AppUser A on A.userId=Assets.usePeople 
            //                                        left join AppUser B on B.userId=Assets.keeper 
            //                                        where assetsId=@assetsId");
            //          DataTable dtAssets = AppMember.DbHelper.GetDataSet(sqlAssets, paras).Tables[0];

            DataRow[] drOther = dtGrid.Select("nodeType<>'rectangle'");
            foreach (DataRow dr in drOther)
            {
                if (DataConvert.ToString(dr["nodeType"]) == "staticField")
                {
                    string nodeText = DataConvert.ToString(dr["nodeText"]);
                    string nodeId = DataConvert.ToString(dr["nodeId"]);
                    TagBuilder tg = new TagBuilder("label");
                    tg.GenerateId(nodeId + "_staticField" + pageId);
                    tg.InnerHtml = DataConvert.ToString(nodeText);
                    tg.MergeAttribute("style", "width:" + DataConvert.ToString(dr["width"]) + "px; height: " + DataConvert.ToString(dr["height"]) + "px; position: absolute;font-family:'黑体';font-size:20px;font-weight:900;");
                    tagStr += tg.ToString(TagRenderMode.Normal);
                    string x = DataConvert.ToString(DataConvert.ToInt32(dr["x"]) - DataConvert.ToInt32(rectangleX));
                    string y = "";
                    if (i > 1)
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY) - (DataConvert.ToInt32(rectangleHeight) + 10) * (i - 1));
                    else
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    //if (DataConvert.ToInt32(x) > 15)
                    //    x = DataConvert.ToString(DataConvert.ToInt32(x) - 15);
                    //if (DataConvert.ToInt32(y) > 20)
                    //    y = DataConvert.ToString(DataConvert.ToInt32(y) - 20);
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("$(function(){");
                    sb.AppendLine("var offset = $('#BarcodeStyleDiv" + pageId + "').position();");
                    //sb.AppendLine("var offset = $('#page1').position();");
                    //sb.AppendLine("alert(offset.left);alert(offset.top);"); //test
                    sb.AppendLine("$('#" + nodeId + "_staticField" + pageId + "').css({ left: offset.left +  " + x + ", top: offset.top + " + y + " });");
                    sb.AppendLine("});");
                    sb.AppendLine("</script>");
                }
                if (DataConvert.ToString(dr["nodeType"]) == "textField")
                {
                    string refField = DataConvert.ToString(dr["refField"]);
                    string nodeId = DataConvert.ToString(dr["nodeId"]);
                    TagBuilder tg = new TagBuilder("label");
                    tg.GenerateId(nodeId + "_textField" + pageId);
                    string data = ObjectPropertyDeal.GetObjectPropertyValue<T>(t, refField);
                    tg.InnerHtml = DataConvert.ToString(data);
                    tg.MergeAttribute("style", "width:" + DataConvert.ToString(dr["width"]) + "px; height: " + DataConvert.ToString(dr["height"]) + "px; position: absolute;font-family:'黑体';font-size:20px;font-weight:900;");
                    tagStr += tg.ToString(TagRenderMode.Normal);
                    string x = DataConvert.ToString(DataConvert.ToInt32(dr["x"]) - DataConvert.ToInt32(rectangleX));
                    //string y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    string y = "";
                    if (i > 1)
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY) - (DataConvert.ToInt32(rectangleHeight) + 10) * (i - 1));
                    else
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    //if (DataConvert.ToInt32(x) > 15)
                    //    x = DataConvert.ToString(DataConvert.ToInt32(x) - 15);
                    //if (DataConvert.ToInt32(y) > 20)
                    //    y = DataConvert.ToString(DataConvert.ToInt32(y) - 20);
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("$(function(){");
                    sb.AppendLine("var offset = $('#BarcodeStyleDiv" + pageId + "').position();");
                    //sb.AppendLine("var offset = $('#page1').position();");
                    //sb.AppendLine("alert(offset.left);alert(offset.top);"); //test
                    sb.AppendLine("$('#" + nodeId + "_textField" + pageId + "').css({ left: offset.left +  " + x + ", top: offset.top + " + y + " });");
                    sb.AppendLine("});");
                    sb.AppendLine("</script>");
                }
                if (DataConvert.ToString(dr["nodeType"]) == "imageField")
                {
                    string refField = DataConvert.ToString(dr["refField"]);
                    string nodeId = DataConvert.ToString(dr["nodeId"]);
                    TagBuilder tg = new TagBuilder("img");
                    string data = ObjectPropertyDeal.GetObjectPropertyValue<T>(t, refField);
                    if (refField == "ImgSystem")
                        tg.MergeAttribute("src", urlHelper.Content("~/Content/uploads/assets/sys.jpg"));
                    else
                        tg.MergeAttribute("src", urlHelper.Content("~/Content/uploads/assets/") + data);
                    tg.GenerateId(nodeId + "_imageField" + pageId);
                    tg.MergeAttribute("style", "width:" + DataConvert.ToString(dr["width"]) + "px; height: " + DataConvert.ToString(dr["height"]) + "px; position: absolute;");
                    tagStr += tg.ToString(TagRenderMode.Normal);
                    string x = DataConvert.ToString(DataConvert.ToInt32(dr["x"]) - DataConvert.ToInt32(rectangleX));
                    //string y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    string y = "";
                    if (i > 1)
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY) - (DataConvert.ToInt32(rectangleHeight) + 10) * (i - 1));
                    else
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    //if (DataConvert.ToInt32(x) > 15)
                    //    x = DataConvert.ToString(DataConvert.ToInt32(x) - 15);
                    //if (DataConvert.ToInt32(y) > 20)
                    //    y = DataConvert.ToString(DataConvert.ToInt32(y) - 20);
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("$(function(){");
                    sb.AppendLine("var offset = $('#BarcodeStyleDiv" + pageId + "').position();");
                    //sb.AppendLine("var offset = $('#page1').position();");
                    sb.AppendLine("$('#" + nodeId + "_imageField" + pageId + "').css({ left: offset.left +  " + x + ", top: offset.top + " + y + " });");
                    sb.AppendLine("});");
                    sb.AppendLine("</script>");
                }
                if (DataConvert.ToString(dr["nodeType"]) == "barcode")
                {
                    string x = DataConvert.ToString(DataConvert.ToInt32(dr["x"]) - DataConvert.ToInt32(rectangleX));
                    //string y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    string y = "";
                    if (i > 1)
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY) - (DataConvert.ToInt32(rectangleHeight) + 10) * (i - 1));
                    else
                        y = DataConvert.ToString(DataConvert.ToInt32(dr["y"]) - DataConvert.ToInt32(rectangleY));
                    //if (DataConvert.ToInt32(x) > 15)
                    //    x = DataConvert.ToString(DataConvert.ToInt32(x) - 15);
                    //if (DataConvert.ToInt32(y) > 20)
                     //   y = DataConvert.ToString(DataConvert.ToInt32(y) - 20);
                    string barcodeType = DataConvert.ToString(dr["barcodeType"]);
                    string barWidth = DataConvert.ToString(dr["barWidth"]);
                    string barHeight = DataConvert.ToString(dr["height"]);
                    string refField = DataConvert.ToString(dr["refField"]);
                    string data = ObjectPropertyDeal.GetObjectPropertyValue<T>(t, refField);
                    string nodeId = DataConvert.ToString(dr["nodeId"]);
                    sb.AppendLine("<div id='" + nodeId + "barcode" + pageId + "' style='position: absolute;'></div>");
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine("$(function(){");
                    sb.AppendLine("var offset = $('#BarcodeStyleDiv" + pageId + "').position();");
                    //sb.AppendLine("var offset = $('#page1').position();");
                    //sb.AppendLine("alert(offset.left);alert(offset.top);"); //test
                    sb.AppendLine("$('#" + nodeId + "barcode" + pageId + "').barcode('" + data + "', '" + barcodeType + "',{showHRI:false,barWidth:" + barWidth + ", barHeight:" + barHeight + "});");
                    sb.AppendLine("$('#" + nodeId + "barcode" + pageId + "').css({ left: offset.left +  " + x + ", top: offset.top + " + y + " });");
                    sb.AppendLine("});");
                    sb.AppendLine("</script>");
                }
            }


            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(function(){");
            sb.AppendLine("$('#BarcodeStyleDiv" + pageId + "').width(" + rectangleWidth + ")");
            sb.AppendLine("$('#BarcodeStyleDiv" + pageId + "').height(" + rectangleHeight + ")");
            //sb.AppendLine("$('#page1').width(" + rectangleWidth + ")");
            //sb.AppendLine("$('#page1').height(" + rectangleHeight + ")");
            sb.AppendLine("$('#BarcodeStyleFieldSet" + pageId + "').width(" + rectangleWidth + ")");
            sb.AppendLine("$('#BarcodeStyleFieldSet" + pageId + "').height(" + rectangleHeight + ")");
            sb.AppendLine("});");
            sb.AppendLine("</script>");


            return sb.ToString() + Environment.NewLine + tagStr;
        }

    }
}