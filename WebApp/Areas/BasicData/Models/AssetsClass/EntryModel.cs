using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.AssetsClass
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

        public string ParentUrl { get; set; }

        public string DialogUrl { get; set; }

        public AssetsClassRepository Repository = new AssetsClassRepository();

    }
}