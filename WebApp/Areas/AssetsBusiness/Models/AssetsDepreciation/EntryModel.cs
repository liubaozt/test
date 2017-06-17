using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsDepreciation
{
    public class EntryModel : EntryViewModel
    {
        public string AssetsDepreciationId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("FiscalYearId")]
        public string FiscalYearId { get; set; }

        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("FiscalPeriodId")]
        public string FiscalPeriodId { get; set; }


        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }

        public string EntryGridId { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }

        public AssetsDepreciationRepository Repository = new AssetsDepreciationRepository();
     
    }
}