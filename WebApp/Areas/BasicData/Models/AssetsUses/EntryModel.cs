using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.AssetsUses
{
    public class EntryModel : EntryViewModel
    {
        public string AssetsUsesId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsUsesNo")]
        public string AssetsUsesNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsUsesName")]
        public string AssetsUsesName { get; set; }

        public AssetsUsesRepository Repository = new AssetsUsesRepository();
    }
}