using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.ScrapType
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new ScrapTypeRepository();
        }

        [AppDisplayNameAttribute("ScrapTypeNo")]
        public string ScrapTypeNo { get; set; }
        [AppDisplayNameAttribute("ScrapTypeName")]
        public string ScrapTypeName { get; set; }

    }
}