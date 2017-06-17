using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsReturn
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsReturnRepository();
        }
        [AppDisplayNameAttribute("AssetsReturnNo")]
        public string AssetsReturnNo { get; set; }
        [AppDisplayNameAttribute("AssetsReturnName")]
        public string AssetsReturnName { get; set; }

    }
}