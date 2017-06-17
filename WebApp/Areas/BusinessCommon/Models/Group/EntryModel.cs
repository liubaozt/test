using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.Group
{
    public class EntryModel : EntryViewModel
    {
        public string GroupId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("GroupNo")]
        public string GroupNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("GroupName")]
        public string GroupName { get; set; }

        [AppStringLengthAttribute(250)]
        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }

        public GroupRepository Repository = new GroupRepository();
    }
}