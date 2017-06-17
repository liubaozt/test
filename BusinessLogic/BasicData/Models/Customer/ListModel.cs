using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.Customer
{
    public class ListModel : ListViewModel
    {
 
        [AppDisplayNameAttribute("CustomerNo")]
        public string CustomerNo { get; set; }
        [AppDisplayNameAttribute("CustomerName")]
        public string CustomerName { get; set; }


    }
}