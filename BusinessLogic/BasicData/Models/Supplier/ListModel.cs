using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.Supplier
{
    public class ListModel : ListViewModel
    {

        [AppDisplayNameAttribute("SupplierNo")]
        public string SupplierNo { get; set; }
        [AppDisplayNameAttribute("SupplierName")]
        public string SupplierName { get; set; }

    }
}