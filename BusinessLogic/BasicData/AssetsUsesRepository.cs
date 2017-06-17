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
    public class AssetsUsesRepository : MasterRepository
    {

        public AssetsUsesRepository()
        {
            DefaulteGridSortField = "assetsUsesNo";
        }

        

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsUsesNo") && DataConvert.ToString(paras["assetsUsesNo"]) != "")
                whereSql += @" and AssetsUses.assetsUsesNo like '%'+@assetsUsesNo+'%'";
            if (paras.ContainsKey("assetsUsesName") && DataConvert.ToString(paras["assetsUsesName"]) != "")
                whereSql += @" and AssetsUses.assetsUsesName like '%'+@assetsUsesName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AssetsUses.assetsUsesId assetsUsesId,
                                 AssetsUses.assetsUsesNo assetsUsesNo,
                                 AssetsUses.assetsUsesName assetsUsesName,
                                 U1.userName createId ,
                                 AssetsUses.createTime createTime ,
                                 U2.userName updateId ,
                                 AssetsUses.updateTime updateTime ,
                                 AssetsUses.updatePro updatePro
                          from AssetsUses left join AppUser U1 on AssetsUses.createId=U1.userId
                                    left join AppUser U2 on AssetsUses.updateId=U2.userId 
                          where 1=1 {1}", DataConvert.ToString(rowSize),WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsUses  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsUses(string assetsUsesId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsUsesId", assetsUsesId);
            string sql = @"select * from AssetsUses where assetsUsesId=@assetsUsesId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsUses where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsUses";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["assetsUsesId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsUsesId", pkValue);
            string sql = @"select * from AssetsUses where assetsUsesId=@assetsUsesId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsUses";
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
            paras.Add("assetsUsesId", pkValue);
            string sql = @"select * from AssetsUses where assetsUsesId=@assetsUsesId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsUses";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select assetsUsesId,assetsUsesName from AssetsUses  order by assetsUsesName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["assetsUsesId"]);
                dropList.Text = DataConvert.ToString(dr["assetsUsesName"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
