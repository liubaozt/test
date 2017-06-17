using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.AccountSubject
{
    public class ListModel : ListViewModel
    {
       
        [AppDisplayNameAttribute("AccountSubjectNo")]
        public string AccountSubjectNo { get; set; }
        [AppDisplayNameAttribute("AccountSubjectName")]
        public string AccountSubjectName { get; set; }
        [AppDisplayNameAttribute("AccountParentId")]
        public string ParentId { get; set; }

    }
}