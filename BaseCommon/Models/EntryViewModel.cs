using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;

namespace BaseCommon.Models
{
    public class EntryViewModel : BaseViewModel
    {

        public string FormMode { get; set; }
        public string SaveUrl { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsDisabled { get; set; }
        public bool CustomClick { get; set; }
        public string PageFlag { get; set; }  

        public string SaveButtonText
        {
            get
            {
                if (FormMode == "delete")
                    return AppMember.AppText["BtnDelete"];
                else if (FormMode == "approve")
                    return AppMember.AppText["ApprovePass"];
                else if (FormMode == "reapply")
                    return AppMember.AppText["ApproveReapply"];
                else 
                    return AppMember.AppText["BtnSave"];
            }
        }
    }
}