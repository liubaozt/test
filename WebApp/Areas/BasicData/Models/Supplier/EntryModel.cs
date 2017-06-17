using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.Supplier
{
    public class EntryModel : EntryViewModel
    {
        public string SupplierId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("SupplierNo")]
        public string SupplierNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("SupplierName")]
        public string SupplierName { get; set; }

        public SupplierRepository Repository = new SupplierRepository();


    }
}