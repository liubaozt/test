using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.StoreSite
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

        public string AddFavoritUrl { get; set; }

        public string ReplaceFavoritUrl { get; set; }

        [AppRequiredAttribute("CompanyId")]
        [AppDisplayNameAttribute("CompanyId")]
        public string CompanyId { get; set; }
     


    }
}