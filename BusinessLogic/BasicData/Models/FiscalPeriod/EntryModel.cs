using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.FiscalPeriod
{
    public class EntryModel : EntryViewModel
    {
        public string FiscalPeriodId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(2)]
        [AppRangeAttribute(1, 12)]
        [AppDisplayNameAttribute("FiscalPeriodName")]
        public string FiscalPeriodName { get; set; }

        [AppDisplayNameAttribute("FiscalYearName")]
        public string FiscalYearId { get; set; }


        [AppDisplayNameAttribute("FromDate")]
        public DateTime? FromDate { get; set; }


        [AppDisplayNameAttribute("ToDate")]
        public DateTime? ToDate { get; set; }

    }
}