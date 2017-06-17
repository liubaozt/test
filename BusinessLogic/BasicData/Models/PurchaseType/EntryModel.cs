using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.PurchaseType
{
    public class EntryModel : EntryViewModel
    {
        public string PurchaseTypeId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("PurchaseTypeNo")]
        public string PurchaseTypeNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("PurchaseTypeName")]
        public string PurchaseTypeName { get; set; }

        public string IsFixed { get; set; }

    }
}