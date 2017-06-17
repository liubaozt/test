using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.Department
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new DepartmentRepository();
        }


        [AppDisplayNameAttribute("DepartmentName")]
        public string DepartmentName { get; set; }

        [AppDisplayNameAttribute("ParentDepartmentId")]
        public string ParentId { get; set; }

     

    }
}