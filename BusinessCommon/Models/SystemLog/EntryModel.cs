using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using BaseCommon.Models;

namespace BusinessCommon.Models.SystemLog
{
    public class EntryModel : QueryEntryViewModel
    {

        [AppDisplayNameAttribute("UserName")]
        public string UserName { get; set; }

        [AppDisplayNameAttribute("Message")]
        public string LogMessage { get; set; }

        [AppDisplayNameAttribute("LogDate1")]
        public string LogDate1 { get; set; }

        [AppDisplayNameAttribute("LogDate2")]
        public string LogDate2 { get; set; }



    }
}
