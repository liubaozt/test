using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Models.AssetsScrap
{
    public class ListModel : ApproveListViewModel
    {
      
        [AppDisplayNameAttribute("AssetsScrapNo")]
        public string AssetsScrapNo { get; set; }
        [AppDisplayNameAttribute("AssetsScrapName")]
        public string AssetsScrapName { get; set; }

        [AppDisplayNameAttribute("ScrapDate1")]
        public string ScrapDate1 { get; set; }

        [AppDisplayNameAttribute("ScrapDate2")]
        public string ScrapDate2 { get; set; }


    }
}