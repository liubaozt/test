using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.AssetsScrapQuery
{
    public class EntryModel : QueryEntryViewModel
    {

        [AppDisplayNameAttribute("AssetsScrapNo")]
        public string AssetsScrapNo { get; set; }

        [AppDisplayNameAttribute("AssetsScrapName")]
        public string AssetsScrapName { get; set; }

        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

        [AppDisplayNameAttribute("AssetsTypeId")]
        public string AssetsTypeId { get; set; }

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

        [AppDisplayNameAttribute("ScrapTypeId")]
        public string ScrapTypeId { get; set; }

        [AppDisplayNameAttribute("ScrapDate1")]
        public string ScrapDate1 { get; set; }

        [AppDisplayNameAttribute("ScrapDate2")]
        public string ScrapDate2 { get; set; }


        
    }
}
