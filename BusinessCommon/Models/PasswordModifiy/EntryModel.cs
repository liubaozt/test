using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.Data;
using BusinessCommon.Repositorys;
using BaseCommon.Models;

namespace BusinessCommon.Models.PasswordModifiy
{
    public class EntryModel : EntryViewModel
    {
         [AppRequiredAttribute()]
        [AppDisplayNameAttribute("OldPassword")]
        public string OldPassword { get; set; }

         [AppRequiredAttribute()]
        [AppDisplayNameAttribute("NewPassword")]
        public string NewPassword { get; set; }

         [AppRequiredAttribute()]
        [AppDisplayNameAttribute("NewPassword2")]
        public string NewPassword2 { get; set; }

        public PasswordModifiyRepository Repository = new PasswordModifiyRepository();
    }
}