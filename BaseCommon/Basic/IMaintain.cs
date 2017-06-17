using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Models;

namespace BaseCommon.Basic
{
    public interface IMaintain : IUpdate
    {
        int Update(EntryViewModel model, UserInfo sysUser, string viewTitle);
        DataRow GetModel();
    }
}
