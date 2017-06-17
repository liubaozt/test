using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.BaseWeb.Common;
using BaseCommon.Basic;

namespace WebApp.Areas.BusinessProcess.Models.AssetsApprove
{
    public class ListModel : ApproveListViewModel
    {
        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }
        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }
    }
}