using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsCheck
{
    public class ListModel : ApproveListViewModel
    {
 
        [AppDisplayNameAttribute("AssetsCheckNo")]
        public string AssetsCheckNo { get; set; }
        [AppDisplayNameAttribute("AssetsCheckName")]
        public string AssetsCheckName { get; set; }

    }
}