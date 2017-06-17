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
    public class AssetsClassRepository : MasterRepository
    {

        public AssetsClassRepository()
        {
            DefaulteGridSortField = "assetsClassNo";
        }

        

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsClassNo") && DataConvert.ToString(paras["assetsClassNo"]) != "")
                whereSql += @" and AssetsClass.assetsClassNo like '%'+@assetsClassNo+'%'";
            if (paras.ContainsKey("assetsClassName") && DataConvert.ToString(paras["assetsClassName"]) != "")
                whereSql += @" and AssetsClass.assetsClassName like '%'+@assetsClassName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AssetsClass.assetsClassId assetsClassId,
                                 AssetsClass.assetsClassNo assetsClassNo,
                                 AssetsClass.assetsClassName assetsClassName,
                                 T.assetsClassName parentId,
                                 U1.userName createId ,
                                 AssetsClass.createTime createTime ,
                                 U2.userName updateId ,
                                 AssetsClass.updateTime updateTime ,
                                 AssetsClass.updatePro updatePro
                          from AssetsClass left join AppUser U1 on AssetsClass.createId=U1.userId
                                    left join AppUser U2 on AssetsClass.updateId=U2.userId 
                                    left join AssetsClass T on AssetsClass.parentId=T.assetsClassId
                          where 1=1 {1}", DataConvert.ToString(rowSize), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsClass  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsClass(string assetsClassId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsClassId", assetsClassId);
            string sql = @"select * from AssetsClass where assetsClassId=@assetsClassId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }


        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsClass where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsClass";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["assetsClassId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsClassId", pkValue);
            string sql = @"select * from AssetsClass where assetsClassId=@assetsClassId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsClass";
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dt.Rows[0][kv.Key] = kv.Value;
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return dbUpdate.Update(dt);
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsClassId", pkValue);
            string sql = @"select * from AssetsClass where assetsClassId=@assetsClassId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsClass";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public DataTable GetAssetsClassTree()
        {
            string sql = @"select assetsClassId,parentId,assetsClassName,1 isOpen ,'false' checked from AssetsClass ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select assetsClassId,assetsClassName,parentId from AssetsClass  order by assetsClassName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["assetsClassId"]);
                dropList.Text = DataConvert.ToString(dr["assetsClassName"]);
                dropList.Filter = DataConvert.ToString(dr["parentId"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
