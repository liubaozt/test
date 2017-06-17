using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsMerge
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsMergeRepository();
        }
        [AppDisplayNameAttribute("AssetsMergeNo")]
        public string AssetsMergeNo { get; set; }
        [AppDisplayNameAttribute("AssetsMergeName")]
        public string AssetsMergeName { get; set; }

    }
}