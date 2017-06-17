using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.Department
{
    public class EntryModel : EntryViewModel
    {

        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("DepartmentName")]
        public string DepartmentName { get; set; }

        [AppDisplayNameAttribute("ParentDepartmentId")]
        public string ParentId { get; set; }

        public string ParentUrl { get; set; }

        public string DialogUrl { get; set; }

        public DepartmentRepository Repository = new DepartmentRepository();
    }
}