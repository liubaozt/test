using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.Company
{
    public class ListModel : ListViewModel
    {

        [AppDisplayNameAttribute("CompanyName")]
        public string DepartmentName { get; set; }

       

     

    }
}