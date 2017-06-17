using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Data;

namespace BaseCommon.Basic
{
    public class BaseRepository : IUpdate
    {
        //public DataUpdate dbUpdate;

        protected void Create5Field(DataRow dr, string userId, string updatePro)
        {
            dr["createId"] = userId;
            dr["createTime"] = IdGenerator.GetServerDate();
            dr["updateId"] = DBNull.Value;
            dr["updateTime"] = DBNull.Value;
            dr["updatePro"] = updatePro;
        }

        protected void Update5Field(DataTable dt, string userId, string updatePro)
        {
            //dt.Rows[0]["updateId"] = userId;
            //dt.Rows[0]["updateTime"] = IdGenerator.GetServerDate();
            //dt.Rows[0]["updatePro"] = updatePro;
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
