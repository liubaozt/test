using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Models;

namespace BusinessCommon.Models.Group
{
    public class EntryModel : EntryViewModel
    {
        public string GroupId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(50)]
        [AppDisplayNameAttribute("GroupNo")]
        public string GroupNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(50)]
        [AppDisplayNameAttribute("GroupName")]
        public string GroupName { get; set; }

        [AppStringLengthAttribute(250)]
        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }

        public string IsFixed { get; set; }

    }
}