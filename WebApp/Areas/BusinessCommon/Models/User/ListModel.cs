using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.User
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new UserRepository();
        }

        [AppDisplayNameAttribute("UserNo")]
        public string UserNo { get; set; }

        [AppDisplayNameAttribute("UserName")]
        public string UserName { get; set; }

        [AppDisplayNameAttribute("GroupId")]
        public string GroupId { get; set; }

    

    }
}