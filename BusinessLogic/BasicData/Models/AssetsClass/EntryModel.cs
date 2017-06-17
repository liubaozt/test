using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.AssetsClass
{
    public class EntryModel : EntryViewModel
    {
        public string AssetsClassId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsClassNo")]
        public string AssetsClassNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsClassName")]
        public string AssetsClassName { get; set; }

        [AppDisplayNameAttribute("AssetsClassParentId")]
        public string ParentId { get; set; }

        [AppDisplayNameAttribute("DepreciationType")]
        public string DepreciationType { get; set; }


        [AppDisplayNameAttribute("AcountUnit")]
        public string UnitId { get; set; }

        [AppRegularExpression(AppMember.IntReg, "IntOnly")]
        [AppDisplayNameAttribute("DurableYears")]
        public int? DurableYears { get; set; }

        [AppRegularExpression(AppMember.DoubleReg, "DoubleOnly")]
        [AppRangeAttribute(0, 1)]
        [AppDisplayNameAttribute("RemainRate")]
        public double? RemainRate { get; set; }

        public string ParentUrl { get; set; }

        public string DialogUrl { get; set; }

        public string AddFavoritUrl { get; set; }

        public string ReplaceFavoritUrl { get; set; }



    }
}