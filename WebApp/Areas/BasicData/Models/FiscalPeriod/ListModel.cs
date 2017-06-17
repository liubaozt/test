using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.FiscalPeriod
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new FiscalPeriodRepository();
        }

        [AppDisplayNameAttribute("FiscalPeriodName")]
        public string FiscalPeriodName { get; set; }

        [AppDisplayNameAttribute("FiscalYearName")]
        public string FiscalYearId { get; set; }


    }
}