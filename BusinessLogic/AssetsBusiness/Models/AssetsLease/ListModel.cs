using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsLease
{
    public class ListModel : ApproveListViewModel
    {
        [AppDisplayNameAttribute("AssetsLeaseNo")]
        public string AssetsLeaseNo { get; set; }
        [AppDisplayNameAttribute("AssetsLeaseName")]
        public string AssetsLeaseName { get; set; }

    }
}