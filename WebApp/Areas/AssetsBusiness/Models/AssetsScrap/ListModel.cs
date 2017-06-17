using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsScrap
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsScrapRepository();
        }
        [AppDisplayNameAttribute("AssetsScrapNo")]
        public string AssetsScrapNo { get; set; }
        [AppDisplayNameAttribute("AssetsScrapName")]
        public string AssetsScrapName { get; set; }

    }
}