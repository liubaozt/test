using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;

namespace BaseCommon.Basic
{
    public class ListCondition
    {
        public string SortField { get; set; }
        public string SortType { get; set; }
        public int PageIndex { get; set; }
        public int PageRowNum { get; set; }
        public int TotalRowNum { get; set; }
        public string ListModelString { get; set; }
        public string FilterString { get; set; }
        public string SelectMode { get; set; }
        public UserInfo SysUser { get; set; }

    }
}
