using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsSplit
{
    public class ListModel : ApproveListViewModel
    {

        [AppDisplayNameAttribute("AssetsSplitNo")]
        public string AssetsSplitNo { get; set; }
        [AppDisplayNameAttribute("AssetsSplitName")]
        public string AssetsSplitName { get; set; }

    }
}