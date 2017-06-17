using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.WorkFlow
{
    public class ListModel : ListViewModel
    {
   
        [AppDisplayNameAttribute("WfName")]
        public string WfName { get; set; }

 
    }
}