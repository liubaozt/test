using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.AssetsMaintainReport
{
    public class EntryModel : QueryEntryViewModel
    {

        [AppDisplayNameAttribute("AssetsMaintainNo")]
        public string AssetsMaintainNo { get; set; }

        [AppDisplayNameAttribute("AssetsMaintainName")]
        public string AssetsMaintainName { get; set; }

        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

        [AppDisplayNameAttribute("MaintainDate")]
        public string MaintainDate { get; set; }

    }
}
