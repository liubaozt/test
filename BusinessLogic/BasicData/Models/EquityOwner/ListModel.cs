using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.EquityOwner
{
    public class ListModel : ListViewModel
    {
      
        [AppDisplayNameAttribute("EquityOwnerNo")]
        public string EquityOwnerNo { get; set; }
        [AppDisplayNameAttribute("EquityOwnerName")]
        public string EquityOwnerName { get; set; }

    }
}