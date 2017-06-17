using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Models.AssetsBorrow
{
    public class ListModel : ApproveListViewModel
    {
        [AppDisplayNameAttribute("AssetsBorrowNo")]
        public string AssetsBorrowNo { get; set; }
        [AppDisplayNameAttribute("AssetsBorrowName")]
        public string AssetsBorrowName { get; set; }

    }
}