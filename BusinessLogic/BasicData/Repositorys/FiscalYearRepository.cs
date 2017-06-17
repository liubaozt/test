using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.FiscalYear;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class FiscalYearRepository : MasterRepository
    {

        public FiscalYearRepository()
        {
            DefaulteGridSortField = "fiscalYearName";
            MasterTable = "FiscalYear";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.FiscalYearName) != "")
            {
                wcd.Sql += @" and FiscalYear.fiscalYearName like '%'+@fiscalYearName+'%'";
                wcd.DBPara.Add("fiscalYearName", model.FiscalYearName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  FiscalYear.fiscalYearId fiscalYearId,
                                 FiscalYear.fiscalYearName fiscalYearName,
                                   convert(nvarchar(100),FiscalYear.fromDate,23)   fromDate,
                                 convert(nvarchar(100), FiscalYear.toDate,23) toDate,
                                 U1.userName createId ,
                                 FiscalYear.createTime createTime ,
                                 U2.userName updateId ,
                                 FiscalYear.updateTime updateTime ,
                                 FiscalYear.updatePro updatePro
                          from FiscalYear left join AppUser U1 on FiscalYear.createId=U1.userId
                                       left join AppUser U2 on FiscalYear.updateId=U2.userId
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("fiscalYearId", primaryKey);
                string sql = @"select * from FiscalYear where fiscalYearId=@fiscalYearId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.FiscalYearId = primaryKey;
                model.FiscalYearName = DataConvert.ToString(dr["fiscalYearName"]);
                model.FromDate = DataConvert.ToDateTime(dr["fromDate"]);
                model.ToDate = DataConvert.ToDateTime(dr["toDate"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from FiscalYear where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "FiscalYear";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["fiscalYearId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("fiscalYearId", pkValue);
            string sql = @"select * from FiscalYear where fiscalYearId=@fiscalYearId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "FiscalYear";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("fiscalYearId", pkValue);
            string sql = @"select * from FiscalYear where fiscalYearId=@fiscalYearId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "FiscalYear";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["fiscalYearName"] = model.FiscalYearName;
            dr["fromDate"] = DataConvert.ToDBObject(model.FromDate);
            dr["toDate"] = DataConvert.ToDBObject(model.ToDate);
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from FiscalYear  order by fiscalYearName  ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["fiscalYearId"]);
                dropList.Text = DataConvert.ToString(dr["fiscalYearName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
