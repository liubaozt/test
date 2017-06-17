using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.Department
{
    public class ListModel : ListViewModel
    {
        [AppDisplayNameAttribute("DepartmentNo")]
        public string DepartmentNo { get; set; }
     
        [AppDisplayNameAttribute("DepartmentName")]
        public string DepartmentName { get; set; }

        [AppDisplayNameAttribute("ParentDepartmentId")]
        public string ParentId { get; set; }

        public string ParentUrl { get; set; }

        public string DialogUrl { get; set; }

        public string AddFavoritUrl { get; set; }

        public string ReplaceFavoritUrl { get; set; }

    }
}