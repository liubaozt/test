using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Models;
using BaseCommon.Data;
using System.Data;

namespace BaseCommon.Models
{
    public class QueryEntryViewModel : BaseViewModel
    {
        public string EntryGridId { get; set; }
        public AdvanceGridLayout EntryGridLayout { get; set; }
        public string EntryGridString { get; set; }
        public string ExportUrl { get; set; }
        public int GridHeight { get; set; }
        public string FormMode { get; set; }
        public string FormVar { get; set; }
        public DataTable EntryGridData { get; set; }
    }
}