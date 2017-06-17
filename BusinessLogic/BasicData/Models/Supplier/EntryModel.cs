using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.Supplier
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

        [AppDisplayNameAttribute("Tel")]
        public string Tel { get; set; }

        [AppDisplayNameAttribute("Email")]
        public string Email { get; set; }

        [AppDisplayNameAttribute("Address")]
        public string Address { get; set; }

        [AppDisplayNameAttribute("Contacts")]
        public string Contacts { get; set; }

    }
}