using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using log4net;
using System.Reflection;
using System.Data;
using System.Data.Common;

namespace BaseCommon.Basic
{
    public abstract class MaintainRepository : BaseRepository
    {
        public int Save(Dictionary<string, object> objs, UserInfo sysUser, string mode, string pkValue, string viewTitle)
        {
            try
            {
                switch (mode)
                {
                    case "new":
                        return AddData(objs, sysUser, viewTitle);
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected abstract int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle);
    }
}
