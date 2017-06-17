using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.ProjectManage
{
    public class EntryModel : EntryViewModel
    {
        public string ProjectManageId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("ProjectManageNo")]
        public string ProjectManageNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("ProjectManageName")]
        public string ProjectManageName { get; set; }

    }
}