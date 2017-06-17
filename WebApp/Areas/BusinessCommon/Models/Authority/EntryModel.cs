using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.Data;
using System.Web.Mvc;
using BusinessCommon.AppMng;
using WebApp.BaseWeb.Common;

namespace WebApp.Areas.BusinessCommon.Models.Authority
{
    public class EntryModel : EntryViewModel
    {
        [AppDisplayNameAttribute("UserId")]
        public string UserNo { get; set; }
        [AppDisplayNameAttribute("GroupId")]
        public string GroupNo { get; set; }
        public string RadioValue { get; set; }
        public string Tree_HideString { get; set; }
        public string TreeId { get; set; }
        public DataTable AuthorityTree { get; set; }

        public AuthorityRepository Repository = new AuthorityRepository();
    }
}