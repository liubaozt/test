using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCommon.Basic
{
    public class ApproveListCondition : ListCondition
    {
        public string ListMode { get; set; }
        public string Approver { get; set; }
    }
}
