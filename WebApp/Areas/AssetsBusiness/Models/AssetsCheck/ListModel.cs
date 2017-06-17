using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsCheck
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsCheckRepository();
        }

        [AppDisplayNameAttribute("AssetsCheckNo")]
        public string AssetsCheckNo { get; set; }
        [AppDisplayNameAttribute("AssetsCheckName")]
        public string AssetsCheckName { get; set; }

    }
}