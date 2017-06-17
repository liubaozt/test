using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.AssetsCheckReport
{
    public class EntryModel : QueryEntryViewModel
    {

        [AppDisplayNameAttribute("AssetsCheckNo")]
        public string AssetsCheckNo { get; set; }

        [AppDisplayNameAttribute("AssetsCheckName")]
        public string AssetsCheckName { get; set; }

        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }


    }
}
