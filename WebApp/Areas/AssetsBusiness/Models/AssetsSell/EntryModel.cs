using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsSell
{
    public class EntryModel : ApproveEntryViewModel
    {
        public EntryModel()
        {
            BaseRepository = new AssetsSellRepository();
        }

        public string AssetsSellId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsSellNo")]
        public string AssetsSellNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsSellName")]
        public string AssetsSellName { get; set; }

        [AppDisplayNameAttribute("SellDate")]
        public DateTime? SellDate { get; set; }

        public string EntryGridId { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public AssetsSellRepository Repository = new AssetsSellRepository();

    }
}