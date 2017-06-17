using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsMerge
{
    public class ListModel : ApproveListViewModel
    {
    
        [AppDisplayNameAttribute("AssetsMergeNo")]
        public string AssetsMergeNo { get; set; }
        [AppDisplayNameAttribute("AssetsMergeName")]
        public string AssetsMergeName { get; set; }

    }
}