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

namespace BusinessLogic.AssetsBusiness.Models.AssetsPurchase
{
    public class EntryModel : ApproveEntryViewModel
    {
     
        public string AssetsPurchaseId { get; set; }

        [AppRequiredAttribute("AssetsPurchaseNo")]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsPurchaseNo")]
        public string AssetsPurchaseNo { get; set; }

        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsPurchaseName")]
        public string AssetsPurchaseName { get; set; }

        [AppDisplayNameAttribute("PurchaseDate")]
        public DateTime? PurchaseDate { get; set; } 


        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string DetailUrl { get; set; }
        public string EntryGridString { get; set; }

        public string AssetsFixUrl { get; set; } 
    }
}