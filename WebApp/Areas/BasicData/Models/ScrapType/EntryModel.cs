using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.ScrapType
{
    public class EntryModel : EntryViewModel
    {
        public string ScrapTypeId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("ScrapTypeNo")]
        public string ScrapTypeNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("ScrapTypeName")]
        public string ScrapTypeName { get; set; }

        public ScrapTypeRepository Repository = new ScrapTypeRepository();
    }
}