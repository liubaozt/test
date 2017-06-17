using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsInsure
{
    public class ListModel : ApproveListViewModel
    {

        [AppDisplayNameAttribute("AssetsInsureNo")]
        public string AssetsInsureNo { get; set; }
        [AppDisplayNameAttribute("AssetsInsureName")]
        public string AssetsInsureName { get; set; }

    }
}