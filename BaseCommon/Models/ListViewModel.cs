using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Basic;

namespace BaseCommon.Models
{
    public class ListViewModel : BaseViewModel
    {
        public string GridId { get; set; }
        public string GridUrl { get; set; }
        public string GridDbClickUrl { get; set; }
        public string GridPkField { get; set; }
        public int GridHeight { get; set; }
        public GridLayout GridLayout { get; set; }
        public DataTable AuthorityGridButton { get; set; }
    }
}