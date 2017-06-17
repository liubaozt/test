using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BaseCommon.Basic
{
    public interface IQuery
    {
        DataTable GetReportGridDataTable(ListCondition condition);
    }
}
