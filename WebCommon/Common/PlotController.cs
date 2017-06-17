using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCommon.Common
{
    public abstract class PlotController : BaseController
    {
      
        private void InitPlotCssAndJs()
        {
            string layoutContentPath = "~/Content/css/";

            string[] layoutCss = new string[]{
                "jquery.jqplot.min.css"
                };
            StringBuilder sb = new StringBuilder();
            foreach (var item in layoutCss)
            {
                sb.Append(@"<link type=""text/css"" rel=""stylesheet"" href=""");
                sb.Append(Url.Content(layoutContentPath) + item);
                sb.AppendLine(@"""/>");
            }

            TempData["PlotCssBlock"] = sb.ToString();
            sb.Clear();
            string jsPath = "~/Scripts/used/jqplot/";
            string[] jqScript = new string[]{
                "excanvas.min.js",
                "jquery.jqplot.min.js",
                "jqplot.canvasAxisTickRenderer.min.js",
                "jqplot.canvasTextRenderer.js",
                "jqplot.canvasAxisLabelRenderer.js",
                "jqplot.categoryAxisRenderer.min.js",
                "jqplot.barRenderer.min.js"
            };
            foreach (var item in jqScript)
            {
                sb.Append(@"<script type=""text/javascript"" src=""");
                sb.Append(Url.Content(jsPath + item));
                sb.AppendLine(@" ""></script>");
            }
            TempData["PlotScriptBlock"] = sb.ToString();
        }
    }
}
