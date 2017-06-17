using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Models.AutoPeriod
{
    public class EntryModel : EntryViewModel
    {
        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("FromYear")]
        public string FromYear { get; set; }

        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("ToYear")]
        public string ToYear { get; set; }
    }
}
