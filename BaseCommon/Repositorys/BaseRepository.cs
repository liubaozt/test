using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Data;
using BaseCommon.Basic;

namespace BaseCommon.Repositorys
{
    public class BaseRepository : IUpdate
    {

        protected void Create5Field(DataTable dt, string userId, string updatePro)
        {
            DateTime time = IdGenerator.GetServerDate();
            foreach (DataRow dr in dt.Rows)
            {
                dr["createId"] = userId;
                dr["createTime"] = time;
                dr["updateId"] = userId;
                dr["updateTime"] = time;
                dr["updatePro"] = updatePro;
            }
        }

        protected void Update5Field(DataTable dt, string userId, string updatePro)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr["updateId"] = userId;
                dr["updateTime"] = IdGenerator.GetServerDate();
                dr["updatePro"] = updatePro;
            }
        }

        #region IUpdate 成员

        public DataUpdate DbUpdate
        {
            get;
            set;
        }

        #endregion
    }
}
