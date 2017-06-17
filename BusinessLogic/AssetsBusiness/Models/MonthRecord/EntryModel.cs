using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Models.MonthRecord
{
    public class EntryModel : EntryViewModel
    {
        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("FiscalYearId")]
        public string FiscalYearId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("FiscalPeriodId")]
        public string FiscalPeriodId { get; set; }

        [AppDisplayNameAttribute("AssetsClassId")]
        public string AssetsClassId { get; set; }
        public string AssetsClassUrl { get; set; }
        public string AssetsClassDialogUrl { get; set; }
        public string AssetsClassAddFavoritUrl { get; set; }
        public string AssetsClassReplaceFavoritUrl { get; set; }

        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }
        public string DepartmentAddFavoritUrl { get; set; }
        public string DepartmentReplaceFavoritUrl { get; set; }

        [AppDisplayNameAttribute("StoreSiteId")]
        public string StoreSiteId { get; set; }
        public string StoreSiteUrl { get; set; }
        public string StoreSiteDialogUrl { get; set; }
        public string StoreSiteAddFavoritUrl { get; set; }
        public string StoreSiteReplaceFavoritUrl { get; set; }

        public string IsUpdateAgain { get; set; }
    }
}
