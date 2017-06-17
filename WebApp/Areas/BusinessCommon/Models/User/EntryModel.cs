using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.User
{
    public class EntryModel : EntryViewModel
    {

        [AppDisplayNameAttribute("UserId")]
        public string UserId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("UserNo")]
        public string UserNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("UserName")]
        public string UserName { get; set; }

        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("GroupId")]
        public string GroupId { get; set; }

        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }

        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("PostId")]
        public string PostId { get; set; }

        public UserRepository Repository = new UserRepository();

    }
}