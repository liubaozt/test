using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WebApp.BaseWeb.Common;
using BaseCommon.Basic;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.Department
{
    public class SelectModel : EntryViewModel
    {
        [AppDisplayNameAttribute("DepartmentName")]
        public string DepartmentName1 { get; set; }

        public string Tree_HideString { get; set; }
        public string TreeId { get; set; }
        public DataTable DepartmentTree { get; set; }

        public DepartmentRepository Repository = new DepartmentRepository();
    }
}