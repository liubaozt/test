using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Repositorys;

namespace BusinessLogic.AssetsBusiness.Models.AssetsReturn
{
    public class ListModel : ApproveListViewModel
    {

        [AppDisplayNameAttribute("AssetsReturnNo")]
        public string AssetsReturnNo { get; set; }
        [AppDisplayNameAttribute("AssetsReturnName")]
        public string AssetsReturnName { get; set; }

    }
}