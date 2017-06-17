using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsSplit
{
    public class EntryModel : ApproveEntryViewModel
    {
        public EntryModel()
        {
            BaseRepository = new AssetsSplitRepository();
        }

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
        public Dictionary<string, GridInfo> UpEntryGridLayout { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }
        public string SelectUrl { get; set; }

        public AssetsSplitRepository Repository = new AssetsSplitRepository();
    }
}