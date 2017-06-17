using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsInsure
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsInsureRepository();
        }

        [AppDisplayNameAttribute("AssetsInsureNo")]
        public string AssetsInsureNo { get; set; }
        [AppDisplayNameAttribute("AssetsInsureName")]
        public string AssetsInsureName { get; set; }

    }
}