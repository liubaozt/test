using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Models.AssetsPurchase
{
    public class ListModel : ApproveListViewModel
    {
      
        [AppDisplayNameAttribute("AssetsPurchaseNo")]
        public string AssetsPurchaseNo { get; set; }
        [AppDisplayNameAttribute("AssetsPurchaseName")]
        public string AssetsPurchaseName { get; set; }

        [AppDisplayNameAttribute("PurchaseDate1")]
        public string PurchaseDate1 { get; set; }

        [AppDisplayNameAttribute("PurchaseDate2")]
        public string PurchaseDate2 { get; set; }


    }
}