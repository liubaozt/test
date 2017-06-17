using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;

namespace BaseCommon.Basic
{
    public interface IRepositoryUpdate
    {
        int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle);

        int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode);

        int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue);
    }
}
