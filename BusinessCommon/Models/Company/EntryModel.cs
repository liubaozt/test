using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Models;

namespace BusinessCommon.Models.Company
{
    public class EntryModel : EntryViewModel
    {

        [AppDisplayNameAttribute("CompanyId")]
        public string DepartmentId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("CompanyNo")]
        public string DepartmentNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("CompanyName")]
        public string DepartmentName { get; set; }

        [AppDisplayNameAttribute("IsHeaderOffice")]
        public bool IsHeaderOffice { get; set; }

    }
}