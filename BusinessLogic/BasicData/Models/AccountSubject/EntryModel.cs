using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.AccountSubject
{
    public class EntryModel : EntryViewModel
    {
        public string AccountSubjectId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AccountSubjectNo")]
        public string AccountSubjectNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AccountSubjectName")]
        public string AccountSubjectName { get; set; }

       
        [AppDisplayNameAttribute("AccountParentId")]
        public string ParentId { get; set; }

        public string ParentUrl { get; set; }

        public string DialogUrl { get; set; }

        public string AddFavoritUrl { get; set; }

        public string ReplaceFavoritUrl { get; set; }

 
    }
}