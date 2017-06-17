using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.SetBooks
{
    public class ListModel : ListViewModel
    {
        [AppDisplayNameAttribute("SetBooksNo")]
        public string SetBooksNo { get; set; }
        [AppDisplayNameAttribute("SetBooksName")]
        public string SetBooksName { get; set; }

    }
}