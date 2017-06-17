using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.AccountSubject
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new AccountSubjectRepository();
        }

        [AppDisplayNameAttribute("AccountSubjectNo")]
        public string AccountSubjectNo { get; set; }
        [AppDisplayNameAttribute("AccountSubjectName")]
        public string AccountSubjectName { get; set; }
        [AppDisplayNameAttribute("AccountParentId")]
        public string ParentId { get; set; }

    }
}