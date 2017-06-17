using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.StoreSite
{
    public class EntryModel : EntryViewModel
    {
        public string StoreSiteId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("StoreSiteNo")]
        public string StoreSiteNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("StoreSiteName")]
        public string StoreSiteName { get; set; }

        [AppDisplayNameAttribute("StoreSiteParentId")]
        public string ParentId { get; set; }

        public string ParentUrl { get; set; }

        public string DialogUrl { get; set; }

        public StoreSiteRepository Repository = new StoreSiteRepository();

    }
}