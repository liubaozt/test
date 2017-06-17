using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.Customer
{
    public class EntryModel : EntryViewModel
    {
        public string CustomerId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("CustomerNo")]
        public string CustomerNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("CustomerName")]
        public string CustomerName { get; set; }

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