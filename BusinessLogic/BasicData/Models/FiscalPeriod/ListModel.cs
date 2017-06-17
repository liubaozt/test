using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.FiscalPeriod
{
    public class ListModel : ListViewModel
    {
     

        [AppDisplayNameAttribute("FiscalPeriodName")]
        public string FiscalPeriodName { get; set; }

        [AppDisplayNameAttribute("FiscalYearName")]
        public string FiscalYearId { get; set; }


    }
}