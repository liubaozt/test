using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BaseCommon.Models
{
    public class ApproveEntryViewModel : EntryViewModel
    {
        public string ApproveReturnUrl { get; set; }

        [AppDisplayNameAttribute("ApproveMind")]
        public string ApproveMind { get; set; }

        public string ApproveLevel { get; set; }

        public string ApproveNode { get; set; }

        public string ApprovePkField { get; set; }

        public string ApprovePkValue { get; set; }

        public string ApproveTableName { get; set; }

        public GridLayout ApproveGridLayout { get; set; }

    }
}