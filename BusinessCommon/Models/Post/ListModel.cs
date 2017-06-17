using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.Post
{
    public class ListModel : ListViewModel
    {
        [AppDisplayNameAttribute("PostNo")]
        public string PostNo { get; set; }
        [AppDisplayNameAttribute("PostName")]
        public string PostName { get; set; }

    }
}