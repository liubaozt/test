using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.BarcodeStyle
{
    public class EntryModel : EntryViewModel
    {


        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("BarcodeStyleName")]
        public string BarcodeStyleName { get; set; }


        public string BarcodeStyleId { get; set; }

        [AppDisplayNameAttribute("IsDefaultStyle")]
        public bool IsDefaultStyle { get; set; }

        [AppStringLengthAttribute(250)]
        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }

        public string Sync { get; set; }

        public string StyleJson { get; set; }

        public BarcodeStyleRepository Repository = new BarcodeStyleRepository();
    }
}