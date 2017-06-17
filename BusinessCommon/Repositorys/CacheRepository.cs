using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Repositorys;

namespace BusinessCommon.Repositorys
{
    public class CacheRepository:BaseRepository
    {
        public int AddFavorit(string pkValue, string userId, string tableName)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            paras.Add("tableName", tableName);
            paras.Add("pkValue", pkValue);
            string sql = @"select * from AppCache where userId=@userId and tableName=@tableName and pkValue=@pkValue ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppCache";
            if (dt.Rows.Count < 1)
            {
                DataRow dr = dt.NewRow();
                dr["userId"] = userId;
                dr["tableName"] = tableName;
                dr["pkValue"] = pkValue;
                dt.Rows.Add(dr);
                return DbUpdate.Update(dt);
            }
            return 1;
        }

        public int ReplaceFavorit(string pkValue, string userId, string tableName)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            paras.Add("tableName", tableName);
            string sql = @"select * from AppCache where userId=@userId and tableName=@tableName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppCache";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            DbUpdate.Update(dt);
            DataRow drNew = dt.NewRow();
            drNew["userId"] = userId;
            drNew["tableName"] = tableName;
            drNew["pkValue"] = pkValue;
            dt.Rows.Add(drNew);
            DbUpdate.Update(dt);
            return 1;
        }
    }
}
