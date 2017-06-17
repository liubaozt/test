using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.Data;
using BusinessCommon.Repositorys;
using BaseCommon.Models;

namespace BusinessCommon.Models.Authority
{
    public class EntryModel : EntryViewModel
    {
        [AppDisplayNameAttribute("AuthorityUser")]
        public string UserNo { get; set; }
        [AppDisplayNameAttribute("GroupId")]
        public string GroupNo { get; set; }
        public string RadioValue { get; set; }
        [AppDisplayNameAttribute("AuthorityUserCheck")]
        public bool IsUser { get; set; }
        public string Tree_HideString { get; set; }
        public string TreeId { get; set; }
        public DataTable AuthorityTree { get; set; }

        public AuthorityRepository Repository = new AuthorityRepository();
    }
}