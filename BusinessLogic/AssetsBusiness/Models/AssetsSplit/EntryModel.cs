using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Repositorys;

namespace BusinessLogic.AssetsBusiness.Models.AssetsSplit
{
    public class EntryModel : ApproveEntryViewModel
    {
     
        public string AssetsSplitId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsSplitNo")]
        public string AssetsSplitNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsSplitName")]
        public string AssetsSplitName { get; set; }

        public string UpEntryGridId { get; set; }
        public string EntryGridId { get; set; }
        public GridLayout UpEntryGridLayout { get; set; }
        public GridLayout EntryGridLayout { get; set; }
        public string SelectUrl { get; set; }
        public string EntryGridString { get; set; }
    }
}