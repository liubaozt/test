using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.Supplier;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class SupplierRepository : MasterRepository
    {

        public SupplierRepository()
        {
            DefaulteGridSortField = "supplierNo";
            MasterTable = "Supplier";
        }

        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.SupplierNo) != "")
            {
                wcd.Sql += @" and Supplier.supplierNo like '%'+@supplierNo+'%'";
                wcd.DBPara.Add("supplierNo", model.SupplierNo);
            }
            if (DataConvert.ToString(model.SupplierName) != "")
            {
                wcd.Sql += @" and Supplier.supplierName like '%'+@supplierName+'%'";
                wcd.DBPara.Add("supplierName", model.SupplierName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select Supplier.supplierId supplierId,
                                 Supplier.supplierNo supplierNo,
                                 Supplier.supplierName supplierName,
                                 Supplier.tel tel,
                                 Supplier.email email,
                                 Supplier.address address,
                                 Supplier.contacts contacts,
                                 U1.userName createId ,
                                 Supplier.createTime createTime ,
                                 U2.userName updateId ,
                                 Supplier.updateTime updateTime ,
                                 Supplier.updatePro updatePro
                          from Supplier left join AppUser U1 on Supplier.createId=U1.userId
                                    left join AppUser U2 on Supplier.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("supplierId", primaryKey);
                string sql = @"select * from Supplier where supplierId=@supplierId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.SupplierId = primaryKey;
                model.SupplierNo = DataConvert.ToString(dr["supplierNo"]);
                model.SupplierName = DataConvert.ToString(dr["supplierName"]);
                model.Tel = DataConvert.ToString(dr["tel"]);
                model.Email = DataConvert.ToString(dr["email"]);
                model.Address = DataConvert.ToString(dr["address"]);
                model.Contacts = DataConvert.ToString(dr["contacts"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Supplier where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Supplier";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["supplierId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("supplierId", pkValue);
            string sql = @"select * from Supplier where supplierId=@supplierId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Supplier";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("supplierId", pkValue);
            string sql = @"select * from Supplier where supplierId=@supplierId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Supplier";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["supplierNo"] = model.SupplierNo;
            dr["supplierName"] = model.SupplierName;
            dr["tel"] = model.Tel;
            dr["email"] = model.Email;
            dr["address"] = model.Address;
            dr["contacts"] = model.Contacts;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from Supplier  order by supplierName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }


        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
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
