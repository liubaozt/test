using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsTransfer
{
    public class ListModel : ApproveListViewModel
    {

        [AppDisplayNameAttribute("AssetsTransferNo")]
        public string AssetsTransferNo { get; set; }

        [AppDisplayNameAttribute("AssetsTransferName")]
        public string AssetsTransferName { get; set; }

        [AppDisplayNameAttribute("TransferDate1")]
        public string TransferDate1 { get; set; }

        [AppDisplayNameAttribute("TransferDate2")]
        public string TransferDate2 { get; set; }
    }
}