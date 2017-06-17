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

namespace BusinessLogic.AssetsBusiness.Models.AssetsScrap
{
    public class EntryModel : ApproveEntryViewModel
    {
     
        public string AssetsScrapId { get; set; }

        [AppRequiredAttribute("AssetsScrapNo")]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsScrapNo")]
        public string AssetsScrapNo { get; set; }

        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsScrapName")]
        public string AssetsScrapName { get; set; }

        [AppDisplayNameAttribute("ScrapDate")]
        public DateTime? ScrapDate { get; set; }


        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }
        public string EntryGridString { get; set; }
    }
}