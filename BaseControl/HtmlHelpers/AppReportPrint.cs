using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;

namespace BaseControl.HtmlHelpers
{
   public static class AppReportPrint
    {

       public static MvcHtmlString AppReportPrintFor(this HtmlHelper htmlHelper,DataTable dt,AdvanceGridLayout layout)
       {
           StringBuilder sb = ExcelHelper.CreateExcel(dt, layout);
           return MvcHtmlString.Create(sb.ToString());
       }
    }
}
