using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsManage
{
    public class ListModel : ApproveListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsRepository();
        }
        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }
        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

    }
}