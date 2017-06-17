using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.DepreciationRule
{
    public class EntryModel : EntryViewModel
    {
        public string DepreciationRuleId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(50)]
        [AppDisplayNameAttribute("DepreciationRuleNo")]
        public string DepreciationRuleNo { get; set; }

        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("TotalMonth")]
        public int TotalMonth { get; set; }

        [AppDisplayNameAttribute("RemainRate")]
        public double RemainRate { get; set; }

        [AppDisplayNameAttribute("DepreciationType")]
        public string DepreciationType { get; set; } 

    }
}