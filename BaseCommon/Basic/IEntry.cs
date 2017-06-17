using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Models;

namespace BaseCommon.Basic
{
    public interface IEntry : IUpdate
    {
        int Update(EntryViewModel model, UserInfo sysUser, string mode, string pkValue, string viewTitle);
        DataTable GetDropListSource();
        List<DropListSource> DropList(DataTable dt, string filterExpression);
    }
}
