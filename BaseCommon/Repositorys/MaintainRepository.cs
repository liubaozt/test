using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using System.Reflection;
using System.Data;
using System.Data.Common;
using BaseCommon.Basic;
using BaseCommon.Models;

namespace BaseCommon.Repositorys
{
    public abstract class MaintainRepository : BaseRepository, IMaintain
    {

        public virtual int Update(EntryViewModel model, UserInfo sysUser,  string viewTitle)
        {
            throw new NotImplementedException();
        }

        public virtual DataRow GetModel()
        {
            throw new NotImplementedException();
        }

    }
}
