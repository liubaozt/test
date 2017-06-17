using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.Group
{
    public class ListModel : ListViewModel
    {
        [AppDisplayNameAttribute("GroupNo")]
        public string GroupNo { get; set; }
        [AppDisplayNameAttribute("GroupName")]
        public string GroupName { get; set; }

    }
}