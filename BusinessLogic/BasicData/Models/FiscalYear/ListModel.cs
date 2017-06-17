using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.FiscalYear
{
    public class ListModel : ListViewModel
    {
      
        [AppDisplayNameAttribute("FiscalYearName")]
        public string FiscalYearName { get; set; }


    }
}