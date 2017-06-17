using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Models;

namespace BusinessCommon.Models.Department
{
    public class EntryModel : EntryViewModel
    {

        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("DepartmentNo")]
        public string DepartmentNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("DepartmentName")]
        public string DepartmentName { get; set; }

         [AppRequiredAttribute()]
        [AppDisplayNameAttribute("ParentDepartmentId")]
        public string ParentId { get; set; }

        public string ParentUrl { get; set; }

        public string DialogUrl { get; set; }

        public string AddFavoritUrl { get; set; }

        public string ReplaceFavoritUrl { get; set; }

    }
}