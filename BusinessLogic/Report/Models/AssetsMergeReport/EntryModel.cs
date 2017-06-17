using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.AssetsMergeReport
{
    public class EntryModel : QueryEntryViewModel
    {

        [AppDisplayNameAttribute("AssetsMergeNo")]
        public string AssetsMergeNo { get; set; }

        [AppDisplayNameAttribute("AssetsMergeName")]
        public string AssetsMergeName { get; set; }

        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

       
    }
}
