using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.AssetsUses
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsUsesRepository();
        }

        [AppDisplayNameAttribute("AssetsUsesNo")]
        public string AssetsUsesNo { get; set; }
        [AppDisplayNameAttribute("AssetsUsesName")]
        public string AssetsUsesName { get; set; }

    }
}