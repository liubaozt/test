using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.ProjectManage
{
    public class ListModel : ListViewModel
    {
       
        [AppDisplayNameAttribute("ProjectManageNo")]
        public string ProjectManageNo { get; set; }
        [AppDisplayNameAttribute("ProjectManageName")]
        public string ProjectManageName { get; set; }

    }
}