using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsScrap
{
    public class EntryModel : ApproveEntryViewModel
    {
        public EntryModel()
        {
            BaseRepository = new AssetsScrapRepository();
        }

        public string AssetsScrapId { get; set; }

        [AppRequiredAttribute("AssetsScrapNo")]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsScrapNo")]
        public string AssetsScrapNo { get; set; }

        [AppRequiredAttribute("AssetsScrapName")]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsScrapName")]
        public string AssetsScrapName { get; set; }

        public string EntryGridId { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public AssetsScrapRepository Repository = new AssetsScrapRepository();
    }
}