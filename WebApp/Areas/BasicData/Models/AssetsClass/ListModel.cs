using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.AssetsClass
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new AssetsClassRepository();
        }

        [AppDisplayNameAttribute("AssetsClassNo")]
        public string AssetsClassNo { get; set; }
        [AppDisplayNameAttribute("AssetsClassName")]
        public string AssetsClassName { get; set; }
        [AppDisplayNameAttribute("AssetsClassParentId")]
        public string ParentId { get; set; }

    }
}