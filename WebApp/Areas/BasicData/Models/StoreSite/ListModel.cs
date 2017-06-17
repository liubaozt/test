using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.StoreSite
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new StoreSiteRepository();
        }

        [AppDisplayNameAttribute("StoreSiteNo")]
        public string StoreSiteNo { get; set; }
        [AppDisplayNameAttribute("StoreSiteName")]
        public string StoreSiteName { get; set; }
        [AppDisplayNameAttribute("StoreSiteParentId")]
        public string ParentId { get; set; }

    }
}