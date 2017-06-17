using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.BarcodeStyle
{
    public class ListModel : ListViewModel
    {
        [AppDisplayNameAttribute("BarcodeStyleName")]
        public string BarcodeStyleName { get; set; }

    }
}