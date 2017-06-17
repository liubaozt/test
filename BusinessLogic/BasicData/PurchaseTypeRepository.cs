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
    public class PurchaseTypeRepository : MasterRepository
    {

        public PurchaseTypeRepository()
        {
            DefaulteGridSortField = "purchaseTypeNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("purchaseTypeNo") && DataConvert.ToString(paras["purchaseTypeNo"]) != "")
                whereSql += @" and PurchaseType.purchaseTypeNo like '%'+@purchaseTypeNo+'%'";
            if (paras.ContainsKey("purchaseTypeName") && DataConvert.ToString(paras["purchaseTypeName"]) != "")
                whereSql += @" and PurchaseType.purchaseTypeName like '%'+@purchaseTypeName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} PurchaseType.purchaseTypeId purchaseTypeId,
                                 PurchaseType.purchaseTypeNo purchaseTypeNo,
                                 PurchaseType.purchaseTypeName purchaseTypeName,
                                 U1.userName createId ,
                                 PurchaseType.createTime createTime ,
                                 U2.userName updateId ,
                                 PurchaseType.updateTime updateTime ,
                                 PurchaseType.updatePro updatePro
                          from PurchaseType left join AppUser U1 on PurchaseType.createId=U1.userId
                                    left join AppUser U2 on PurchaseType.updateId=U2.userId 
                          where 1=1 {1}", DataConvert.ToString(rowSize),WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from PurchaseType  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetPurchaseType(string purchaseTypeId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("purchaseTypeId", purchaseTypeId);
            string sql = @"select * from PurchaseType where purchaseTypeId=@purchaseTypeId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from PurchaseType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "PurchaseType";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["purchaseTypeId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("purchaseTypeId", pkValue);
            string sql = @"select * from PurchaseType where purchaseTypeId=@purchaseTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "PurchaseType";
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
            paras.Add("purchaseTypeId", pkValue);
            string sql = @"select * from PurchaseType where purchaseTypeId=@purchaseTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "PurchaseType";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select purchaseTypeId,purchaseTypeName from PurchaseType  order by purchaseTypeName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["purchaseTypeId"]);
                dropList.Text = DataConvert.ToString(dr["purchaseTypeName"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
