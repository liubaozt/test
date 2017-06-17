using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.Supplier
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new SupplierRepository();
        }
        [AppDisplayNameAttribute("SupplierNo")]
        public string SupplierNo { get; set; }
        [AppDisplayNameAttribute("SupplierName")]
        public string SupplierName { get; set; }

    }
}