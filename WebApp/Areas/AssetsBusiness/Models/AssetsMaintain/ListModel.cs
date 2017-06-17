using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsMaintain
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsMaintainRepository();
        }

        [AppDisplayNameAttribute("AssetsMaintainNo")]
        public string AssetsMaintainNo { get; set; }
        [AppDisplayNameAttribute("AssetsMaintainName")]
        public string AssetsMaintainName { get; set; }

    }
}