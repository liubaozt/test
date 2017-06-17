using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.PurchaseType;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Repositorys
{
    public class PurchaseTypeRepository : MasterRepository
    {

        public PurchaseTypeRepository()
        {
            DefaulteGridSortField = "purchaseTypeNo";
            MasterTable = "PurchaseType";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.PurchaseTypeNo) != "")
            {
                wcd.Sql += @" and PurchaseType.purchaseTypeNo like '%'+@purchaseTypeNo+'%'";
                wcd.DBPara.Add("purchaseTypeNo", model.PurchaseTypeNo);
            }
            if (DataConvert.ToString(model.PurchaseTypeName) != "")
            {
                wcd.Sql += @" and PurchaseType.purchaseTypeName like '%'+@purchaseTypeName+'%'";
                wcd.DBPara.Add("purchaseTypeName", model.PurchaseTypeName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  PurchaseType.purchaseTypeId purchaseTypeId,
                                 PurchaseType.purchaseTypeNo purchaseTypeNo,
                                 PurchaseType.purchaseTypeName purchaseTypeName,
                                 (select CodeTable.codeName from CodeTable where PurchaseType.isFixed=CodeTable.codeNo and CodeTable.codeType='BoolVal' and CodeTable.languageVer='{0}' ) isFixed,
                                 U1.userName createId ,
                                 PurchaseType.createTime createTime ,
                                 U2.userName updateId ,
                                 PurchaseType.updateTime updateTime ,
                                 PurchaseType.updatePro updatePro
                          from PurchaseType left join AppUser U1 on PurchaseType.createId=U1.userId
                                    left join AppUser U2 on PurchaseType.updateId=U2.userId 
                          where 1=1 ", AppMember.AppLanguage.ToString());
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("purchaseTypeId", primaryKey);
                string sql = @"select * from PurchaseType where purchaseTypeId=@purchaseTypeId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.PurchaseTypeId = primaryKey;
                model.PurchaseTypeNo = DataConvert.ToString(dr["purchaseTypeNo"]);
                model.PurchaseTypeName = DataConvert.ToString(dr["purchaseTypeName"]);
                model.IsFixed = DataConvert.ToString(dr["isFixed"]); 
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from PurchaseType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "PurchaseType";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["purchaseTypeId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("purchaseTypeId", pkValue);
            string sql = @"select * from PurchaseType where purchaseTypeId=@purchaseTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "PurchaseType";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("purchaseTypeId", pkValue);
            string sql = @"select * from PurchaseType where purchaseTypeId=@purchaseTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "PurchaseType";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["purchaseTypeNo"] = model.PurchaseTypeNo;
            dr["purchaseTypeName"] = model.PurchaseTypeName;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from PurchaseType  order by isFixed desc, purchaseTypeNo";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }


        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
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
