using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.FiscalPeriod;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class FiscalPeriodRepository : MasterRepository
    {

        public FiscalPeriodRepository()
        {
            DefaulteGridSortField = "fiscalYearId,fiscalPeriodName";
            MasterTable = "FiscalPeriod";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.FiscalPeriodName) != "")
            {
                wcd.Sql += @" and FiscalPeriod.fiscalPeriodName like '%'+@fiscalPeriodName+'%'";
                wcd.DBPara.Add("fiscalPeriodName", model.FiscalPeriodName);
            }
            if (DataConvert.ToString(model.FiscalYearId) != "")
            {
                wcd.Sql += @" and FiscalPeriod.fiscalYearId = ''+@fiscalYearId+''";
                wcd.DBPara.Add("fiscalYearId", model.FiscalYearId);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  FiscalPeriod.fiscalPeriodId fiscalPeriodId,
                                 FiscalPeriod.fiscalPeriodName fiscalPeriodName,
                                 FiscalYear.fiscalYearName fiscalYearId,
                                 FiscalPeriod.fromDate fromDate,
                                 FiscalPeriod.toDate toDate,
                                 U1.userName createId ,
                                 FiscalPeriod.createTime createTime ,
                                 U2.userName updateId ,
                                 FiscalPeriod.updateTime updateTime ,
                                 FiscalPeriod.updatePro updatePro
                          from FiscalPeriod left join AppUser U1 on FiscalPeriod.createId=U1.userId
                                       left join AppUser U2 on FiscalPeriod.updateId=U2.userId
                                       left join FiscalYear on FiscalYear.fiscalYearId=FiscalPeriod.fiscalYearId 
                          where 1=1 ");
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("fiscalPeriodId", primaryKey);
                string sql = @"select * from FiscalPeriod where fiscalPeriodId=@fiscalPeriodId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.FiscalPeriodId = primaryKey;
                model.FiscalPeriodName = DataConvert.ToString(dr["fiscalPeriodName"]);
                model.FiscalYearId = DataConvert.ToString(dr["fiscalYearId"]);
                model.FromDate = DataConvert.ToDateTime(dr["fromDate"]);
                model.ToDate = DataConvert.ToDateTime(dr["toDate"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from FiscalPeriod where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "FiscalPeriod";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["fiscalPeriodId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("fiscalPeriodId", pkValue);
            string sql = @"select * from FiscalPeriod where fiscalPeriodId=@fiscalPeriodId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "FiscalPeriod";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("fiscalPeriodId", pkValue);
            string sql = @"select * from FiscalPeriod where fiscalPeriodId=@fiscalPeriodId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "FiscalPeriod";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["fiscalPeriodName"] = model.FiscalPeriodName;
            dr["fiscalYearId"] = model.FiscalYearId;
            dr["fromDate"] = DataConvert.ToDBObject(model.FromDate);
            dr["toDate"] = DataConvert.ToDBObject(model.ToDate);
        }


        public override DataTable GetDropListSource()
        {
            string sql = @"select * from FiscalPeriod  order by fiscalPeriodName ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public  DataTable GetFiscalPeriod(DateTime dtTime)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("dtTime", dtTime.ToShortDateString());
            string sql = @"select * from FiscalPeriod where fromDate<= @dtTime and toDate>=@dtTime ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["fiscalPeriodId"]);
                dropList.Text = DataConvert.ToString(dr["fiscalPeriodName"]);
                dropList.Filter = DataConvert.ToString(dr["fiscalYearId"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
