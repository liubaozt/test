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
    public class SupplierRepository : MasterRepository
    {

        public SupplierRepository()
        {
            DefaulteGridSortField = "supplierNo";
        }

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("supplierNo") && DataConvert.ToString(paras["supplierNo"]) != "")
                whereSql += @" and Supplier.supplierNo like '%'+@supplierNo+'%'";
            if (paras.ContainsKey("supplierName") && DataConvert.ToString(paras["supplierName"]) != "")
                whereSql += @" and Supplier.supplierName like '%'+@supplierName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} Supplier.supplierId supplierId,
                                 Supplier.supplierNo supplierNo,
                                 Supplier.supplierName supplierName,
                                 U1.userName createId ,
                                 Supplier.createTime createTime ,
                                 U2.userName updateId ,
                                 Supplier.updateTime updateTime ,
                                 Supplier.updatePro updatePro
                          from Supplier left join AppUser U1 on Supplier.createId=U1.userId
                                    left join AppUser U2 on Supplier.updateId=U2.userId 
                          where 1=1 {1}", DataConvert.ToString(rowSize),WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from Supplier  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetSupplier(string supplierId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("supplierId", supplierId);
            string sql = @"select * from Supplier where supplierId=@supplierId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Supplier where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Supplier";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["supplierId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("supplierId", pkValue);
            string sql = @"select * from Supplier where supplierId=@supplierId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Supplier";
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
            paras.Add("supplierId", pkValue);
            string sql = @"select * from Supplier where supplierId=@supplierId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Supplier";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select supplierId,supplierName from Supplier  order by supplierName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["supplierId"]);
                dropList.Text = DataConvert.ToString(dr["supplierName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
