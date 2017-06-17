using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.UpcomingScrapQuery
{
    public class EntryModel : QueryEntryViewModel
    {


        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }


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

        [AppDisplayNameAttribute("AssetsState")]
        public string AssetsState { get; set; }

        [AppDisplayNameAttribute("DepreciationEndDate1")]
        public string DepreciationEndDate1 { get; set; }

        [AppDisplayNameAttribute("DepreciationEndDate2")]
        public string DepreciationEndDate2 { get; set; }
        
    }
}
