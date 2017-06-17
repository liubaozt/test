using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.Unit
{
    public class EntryModel : EntryViewModel
    {
        public string UnitId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("UnitNo")]
        public string UnitNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("UnitName")]
        public string UnitName { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(250)]
        [AppDisplayNameAttribute("UnitType")]
        public string UnitType { get; set; }

        public UnitRepository Repository = new UnitRepository();
    }
}