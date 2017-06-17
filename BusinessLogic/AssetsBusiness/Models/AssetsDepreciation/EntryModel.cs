using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Repositorys;

namespace BusinessLogic.AssetsBusiness.Models.AssetsDepreciation
{
    public class EntryModel : EntryViewModel
    {
        public string AssetsDepreciationId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("FiscalYearId")]
        public string FiscalYearId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("FiscalPeriodId")]
        public string FiscalPeriodId { get; set; }


        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }
        public string EntryGridString { get; set; }

    }
}