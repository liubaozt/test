using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsTransfer
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsTransferRepository();
        }

        [AppDisplayNameAttribute("AssetsTransferNo")]
        public string AssetsTransferNo { get; set; }
        [AppDisplayNameAttribute("AssetsTransferName")]
        public string AssetsTransferName { get; set; }

    }
}