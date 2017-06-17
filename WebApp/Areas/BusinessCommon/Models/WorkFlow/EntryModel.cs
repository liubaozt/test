using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.WorkFlow
{
    public class EntryModel : EntryViewModel
    {


        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("WfName")]
        public string WfName { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("ApproveTable")]
        public string ApproveTable { get; set; }

        [AppStringLengthAttribute(250)]
        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }

        public string WorkFlowJson { get; set; }

        public WorkFlowRepository Repository = new WorkFlowRepository();
    }
}