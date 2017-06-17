using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.AssetsType
{
    public class EntryModel : EntryViewModel
    {
        public string AssetsTypeId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsTypeNo")]
        public string AssetsTypeNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsTypeName")]
        public string AssetsTypeName { get; set; }

    


    }
}