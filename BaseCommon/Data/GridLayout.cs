using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCommon.Data
{
    public class GridLayout
    {
        public string GridTitle { get; set; }

        public Dictionary<string, GridInfo> GridLayouts { get; set; }
    }
}
