using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.Customer
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new CustomerRepository();
        }

        [AppDisplayNameAttribute("CustomerNo")]
        public string CustomerNo { get; set; }
        [AppDisplayNameAttribute("CustomerName")]
        public string CustomerName { get; set; }


    }
}