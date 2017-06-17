using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsBorrow
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsBorrowRepository();
        }

        [AppDisplayNameAttribute("AssetsBorrowNo")]
        public string AssetsBorrowNo { get; set; }
        [AppDisplayNameAttribute("AssetsBorrowName")]
        public string AssetsBorrowName { get; set; }

    }
}