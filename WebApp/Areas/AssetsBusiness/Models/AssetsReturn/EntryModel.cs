using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsReturn
{
    public class EntryModel : ApproveEntryViewModel
    {
        public EntryModel()
        {
            BaseRepository = new AssetsReturnRepository();
        }
        public string AssetsReturnId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsReturnNo")]
        public string AssetsReturnNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsReturnName")]
        public string AssetsReturnName { get; set; }

 

        [AppDisplayNameAttribute("ReturnPeople")]
        public string ReturnPeople { get; set; }


        [AppDisplayNameAttribute("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        public string EntryGridId { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public AssetsReturnRepository Repository = new AssetsReturnRepository();
    }
}