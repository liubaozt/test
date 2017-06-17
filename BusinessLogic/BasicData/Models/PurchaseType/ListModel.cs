using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.PurchaseType
{
    public class ListModel : ListViewModel
    {
       
        [AppDisplayNameAttribute("PurchaseTypeNo")]
        public string PurchaseTypeNo { get; set; }
        [AppDisplayNameAttribute("PurchaseTypeName")]
        public string PurchaseTypeName { get; set; }

    }
}