using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.Customer
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

        public CustomerRepository Repository = new CustomerRepository();

    }
}