using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;

namespace BaseCommon.Basic
{
    public interface IUpdate
    {
        DataUpdate DbUpdate { get; set; }
    }
}
