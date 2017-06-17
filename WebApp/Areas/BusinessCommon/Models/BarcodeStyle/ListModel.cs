using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.BarcodeStyle
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new BarcodeStyleRepository();
        }

        [AppDisplayNameAttribute("BarcodeStyleName")]
        public string BarcodeStyleName { get; set; }

    }
}