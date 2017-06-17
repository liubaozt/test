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

namespace BusinessLogic.AssetsBusiness.Models.AssetsCheck
{
    public class EntryModel : EntryQueryModel
    {
      

        public string AssetsCheckId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsCheckNo")]
        public string AssetsCheckNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsCheckName")]
        public string AssetsCheckName { get; set; }

        [AppDisplayNameAttribute("CheckPeople")]
        public string CheckPeople { get; set; }

        [AppDisplayNameAttribute("CheckDate")]
        public DateTime? CheckDate { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public string EntryGridString { get; set; }

        public string UpLoadFileName { get; set; }

      
    }
}