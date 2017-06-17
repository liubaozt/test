using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsSell
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsSellRepository();
        }
        [AppDisplayNameAttribute("AssetsSellNo")]
        public string AssetsSellNo { get; set; }
        [AppDisplayNameAttribute("AssetsSellName")]
        public string AssetsSellName { get; set; }

    }
}