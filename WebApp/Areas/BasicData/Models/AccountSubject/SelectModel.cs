using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.AccountSubject
{
    public class SelectModel : EntryViewModel
    {
        public string Tree_HideString { get; set; }
        public string TreeId { get; set; }
        public DataTable AccountSubjectTree { get; set; }

        public AccountSubjectRepository Repository = new AccountSubjectRepository();
    }
}