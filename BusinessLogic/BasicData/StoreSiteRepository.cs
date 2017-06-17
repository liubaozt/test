using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;

namespace BusinessLogic.BasicData
{
    public class StoreSiteRepository : MasterRepository
    {

        public StoreSiteRepository()
        {
            DefaulteGridSortField = "storeSiteNo";
        }

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("storeSiteNo") && DataConvert.ToString(paras["storeSiteNo"]) != "")
                whereSql += @" and StoreSite.storeSiteNo like '%'+@storeSiteNo+'%'";
            if (paras.ContainsKey("storeSiteName") && DataConvert.ToString(paras["storeSiteName"]) != "")
                whereSql += @" and StoreSite.storeSiteName like '%'+@storeSiteName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} StoreSite.storeSiteId storeSiteId,
                                 StoreSite.storeSiteNo storeSiteNo,
                                 StoreSite.storeSiteName storeSiteName,
                                 T.storeSiteName parentId,
                                 U1.userName createId ,
                                 StoreSite.createTime createTime ,
                                 U2.userName updateId ,
                                 StoreSite.updateTime updateTime ,
                                 StoreSite.updatePro updatePro
                          from StoreSite left join AppUser U1 on StoreSite.createId=U1.userId
                                    left join AppUser U2 on StoreSite.updateId=U2.userId 
                                    left join StoreSite T on StoreSite.parentId=T.storeSiteId
                          where 1=1 {1}", DataConvert.ToString(rowSize),WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from StoreSite  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetStoreSite(string storeSiteId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("storeSiteId", storeSiteId);
            string sql = @"select * from StoreSite where storeSiteId=@storeSiteId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from StoreSite where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "StoreSite";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["storeSiteId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("storeSiteId", pkValue);
            string sql = @"select * from StoreSite where storeSiteId=@storeSiteId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "StoreSite";
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dt.Rows[0][kv.Key] = kv.Value;
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return dbUpdate.Update(dt);
        }

        protected override int DeleteData(Dictionary<string, object> objs,UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("storeSiteId", pkValue);
            string sql = @"select * from StoreSite where storeSiteId=@storeSiteId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "StoreSite";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public DataTable GetStoreSiteTree()
        {
            string sql = @"select storeSiteId,parentId,storeSiteName,1 isOpen ,'false' checked from StoreSite ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select storeSiteId,storeSiteName,parentId from StoreSite  order by storeSiteName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["storeSiteId"]);
                dropList.Text = DataConvert.ToString(dr["storeSiteName"]);
                dropList.Filter = DataConvert.ToString(dr["parentId"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
