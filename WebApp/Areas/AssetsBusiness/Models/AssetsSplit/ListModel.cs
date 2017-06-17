using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsSplit
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsSplitRepository();
        }

        [AppDisplayNameAttribute("AssetsSplitNo")]
        public string AssetsSplitNo { get; set; }
        [AppDisplayNameAttribute("AssetsSplitName")]
        public string AssetsSplitName { get; set; }

    }
}