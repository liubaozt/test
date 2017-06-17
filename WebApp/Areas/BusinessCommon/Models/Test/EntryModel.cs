using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.BaseWeb.Common;
using BaseCommon.Data;
using BaseCommon.Basic;

namespace WebApp.Areas.BusinessCommon.Models.Test
{
    public class EntryModel : EntryViewModel
    {
        public string GridId { get; set; }

        [AppDisplayNameAttribute("GroupNo")]
        public string GroupId { get; set; }

        [AppDisplayNameAttribute("CreateTime")]
        public DateTime CreateTime { get; set; }

        public Dictionary<string, GridInfo> GridLayout { get; set; }
    }
}