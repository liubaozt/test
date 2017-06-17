using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.PurchaseType
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new PurchaseTypeRepository();
        }

        [AppDisplayNameAttribute("PurchaseTypeNo")]
        public string PurchaseTypeNo { get; set; }
        [AppDisplayNameAttribute("PurchaseTypeName")]
        public string PurchaseTypeName { get; set; }

    }
}