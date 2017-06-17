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

namespace BusinessLogic.AssetsBusiness.Models.AssetsLease
{
    public class EntryModel : ApproveEntryViewModel
    {
      
        public string AssetsLeaseId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsLeaseNo")]
        public string AssetsLeaseNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsLeaseName")]
        public string AssetsLeaseName { get; set; }

        [AppDisplayNameAttribute("LeaseDate")]
        public DateTime? LeaseDate { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public string EntryGridString { get; set; }
 
    }
}