using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.AssetsMaintainQuery
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

        [AppDisplayNameAttribute("MaintainDate1")]
        public string MaintainDate1 { get; set; }

        [AppDisplayNameAttribute("MaintainDate2")]
        public string MaintainDate2 { get; set; }
    }
}
