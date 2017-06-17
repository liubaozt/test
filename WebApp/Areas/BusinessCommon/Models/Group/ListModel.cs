using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.Group
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new GroupRepository();
        }

        [AppDisplayNameAttribute("GroupNo")]
        public string GroupNo { get; set; }
        [AppDisplayNameAttribute("GroupName")]
        public string GroupName { get; set; }

    }
}