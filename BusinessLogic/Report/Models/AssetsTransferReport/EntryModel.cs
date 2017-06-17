using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.AssetsTransferReport
{
    public class EntryModel : QueryEntryViewModel
    {

        [AppDisplayNameAttribute("AssetsTransferNo")]
        public string AssetsTransferNo { get; set; }

        [AppDisplayNameAttribute("AssetsTransferName")]
        public string AssetsTransferName { get; set; }

        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

        [AppDisplayNameAttribute("TransferDate")]
        public string TransferDate { get; set; }

    }
}
