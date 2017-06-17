using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.Customer;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Repositorys
{
    public class CustomerRepository : MasterRepository
    {

        public CustomerRepository()
        {
            DefaulteGridSortField = "customerNo";
            MasterTable = "Customer";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.CustomerNo) != "")
            {
                wcd.Sql += @" and Customer.customerNo like '%'+@customerNo+'%'";
                wcd.DBPara.Add("customerNo", model.CustomerNo);
            }
            if (DataConvert.ToString(model.CustomerName) != "")
            {
                wcd.Sql += @" and Customer.customerName like '%'+@customerName+'%'";
                wcd.DBPara.Add("customerName", model.CustomerName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  Customer.customerId customerId,
                                 Customer.customerNo customerNo,
                                 Customer.customerName customerName,
                                 Customer.tel tel,
                                 Customer.email email,
                                 Customer.address address,
                                 Customer.contacts contacts,
                                 U1.userName createId ,
                                 Customer.createTime createTime ,
                                 U2.userName updateId ,
                                 Customer.updateTime updateTime ,
                                 Customer.updatePro updatePro
                          from Customer left join AppUser U1 on Customer.createId=U1.userId
                                    left join AppUser U2 on Customer.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("customerId", primaryKey);
                string sql = @"select * from Customer where customerId=@customerId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.CustomerId = primaryKey;
                model.CustomerNo = DataConvert.ToString(dr["customerNo"]);
                model.CustomerName = DataConvert.ToString(dr["customerName"]);
                model.Tel = DataConvert.ToString(dr["tel"]);
                model.Email = DataConvert.ToString(dr["email"]);
                model.Address = DataConvert.ToString(dr["address"]);
                model.Contacts = DataConvert.ToString(dr["contacts"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Customer where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Customer";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["customerId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("customerId", pkValue);
            string sql = @"select * from Customer where customerId=@customerId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Customer";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("customerId", pkValue);
            string sql = @"select * from Customer where customerId=@customerId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Customer";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["customerNo"] = model.CustomerNo;
            dr["customerName"] = model.CustomerName;
            dr["tel"] = model.Tel;
            dr["email"] = model.Email;
            dr["address"] = model.Address;
            dr["contacts"] = model.Contacts;
        }


        public override DataTable GetDropListSource()
        {
            string sql = @"select * from Customer  order by customerName  ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["customerId"]);
                dropList.Text = DataConvert.ToString(dr["customerName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
