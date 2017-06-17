using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;

namespace WebApp.Areas.Report.Models.AssetsClassChangeReport
{
    public class EntryModel : ReportEntryViewModel
    {
        [AppRequiredAttribute("AssetsClassId")]
        [AppDisplayNameAttribute("AssetsClassId")]
        public string AssetsClassId { get; set; }
        public string AssetsClassUrl { get; set; }
        public string AssetsClassDialogUrl { get; set; }

        [AppRequiredAttribute("DepartmentId")]
        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }

    }
}