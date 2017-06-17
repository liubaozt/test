using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;

namespace BaseCommon.Models
{
    public class ApproveListViewModel : ListViewModel
    {
        public string ListMode { get; set; }

        public string ApproveListTitle
        {
            get
            {
                if (ListMode == "reapply")
                    return AppMember.AppText["ReturnApproveList"];
                else if (ListMode == "approve")
                    return AppMember.AppText["PreApproveList"];
                else
                    return AppMember.AppText["PreApproveList"];
            }
        }

    }
}