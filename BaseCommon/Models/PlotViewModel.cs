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
    public class PlotViewModel : BaseViewModel
    {
        public string FormVar { get; set; }
        public string ExportUrl { get; set; }
        public string FormMode { get; set; }

    }
}