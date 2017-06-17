using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Models;

namespace BusinessCommon.Models.WorkFlow
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

    }
}