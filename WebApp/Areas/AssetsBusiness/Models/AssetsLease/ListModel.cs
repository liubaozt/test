using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsLease
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsLeaseRepository();
        }

        [AppDisplayNameAttribute("AssetsLeaseNo")]
        public string AssetsLeaseNo { get; set; }
        [AppDisplayNameAttribute("AssetsLeaseName")]
        public string AssetsLeaseName { get; set; }

    }
}