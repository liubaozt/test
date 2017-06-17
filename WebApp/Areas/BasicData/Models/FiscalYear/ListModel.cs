using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.FiscalYear
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new FiscalYearRepository();
        }

        [AppDisplayNameAttribute("FiscalYearName")]
        public string FiscalYearName { get; set; }


    }
}