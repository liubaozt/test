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

namespace BusinessLogic.AssetsBusiness.Models.AssetsInsure
{
    public class EntryModel : ApproveEntryViewModel
    {
        
        public string AssetsInsureId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsInsureNo")]
        public string AssetsInsureNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsInsureName")]
        public string AssetsInsureName { get; set; }

        [AppDisplayNameAttribute("InsureDate")]
        public DateTime? InsureDate { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public string EntryGridString { get; set; }

    }
}