using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsMerge
{
    public class EntryModel : ApproveEntryViewModel
    {
        public EntryModel()
        {
            BaseRepository = new AssetsMergeRepository();
        }
        public string AssetsMergeId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsMergeNo")]
        public string AssetsMergeNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsMergeName")]
        public string AssetsMergeName { get; set; }

        public string UpEntryGridId { get; set; }
        public string EntryGridId { get; set; }
        public Dictionary<string, GridInfo> UpEntryGridLayout { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }
        public string SelectUrl { get; set; }

        public AssetsMergeRepository Repository = new AssetsMergeRepository();
    }
}