using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsSell
{
    public class ListModel : ApproveListViewModel
    {
        [AppDisplayNameAttribute("AssetsSellNo")]
        public string AssetsSellNo { get; set; }
        [AppDisplayNameAttribute("AssetsSellName")]
        public string AssetsSellName { get; set; }

    }
}