using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.FiscalYear
{
    public class EntryModel : EntryViewModel
    {
        public string FiscalYearId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(4)]
        [AppRangeAttribute(1990, 2999)]
        [AppDisplayNameAttribute("FiscalYearName")]
        public string FiscalYearName { get; set; }


        [AppDisplayNameAttribute("FromDate")]
        public DateTime? FromDate { get; set; }


        [AppDisplayNameAttribute("ToDate")]
        public DateTime? ToDate { get; set; }

       
    }
}