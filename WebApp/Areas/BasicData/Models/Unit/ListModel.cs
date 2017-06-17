using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.Unit
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new UnitRepository();
        }
        [AppDisplayNameAttribute("UnitNo")]
        public string UnitNo { get; set; }
        [AppDisplayNameAttribute("UnitName")]
        public string UnitName { get; set; }


    }
}