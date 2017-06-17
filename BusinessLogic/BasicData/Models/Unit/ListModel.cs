using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.Unit
{
    public class ListModel : ListViewModel
    {

        [AppDisplayNameAttribute("UnitNo")]
        public string UnitNo { get; set; }
        [AppDisplayNameAttribute("UnitName")]
        public string UnitName { get; set; }
        [AppDisplayNameAttribute("UnitType")]
        public string UnitType { get; set; }

    }
}