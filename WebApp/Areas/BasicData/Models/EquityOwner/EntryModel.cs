using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.EquityOwner
{
    public class EntryModel : EntryViewModel
    {
        public string EquityOwnerId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("EquityOwnerNo")]
        public string EquityOwnerNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("EquityOwnerName")]
        public string EquityOwnerName { get; set; }

        public EquityOwnerRepository Repository = new EquityOwnerRepository();
    }
}