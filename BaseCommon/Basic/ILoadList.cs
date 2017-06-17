using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using System.Data;

namespace BaseCommon.Basic
{
    public interface ILoadList
    {
        int GetGridDataCount(ListCondition condition);

        DataTable GetGridDataTable(ListCondition condition);

        DataTable GetAllGridDataTable(ListCondition condition);

    }
}
