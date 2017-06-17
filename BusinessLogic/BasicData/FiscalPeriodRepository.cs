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
    public class FiscalPeriodRepository : MasterRepository
    {

        public FiscalPeriodRepository()
        {
            DefaulteGridSortField = "fiscalPeriodName";
        }

   

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("fiscalPeriodName") && DataConvert.ToString(paras["fiscalPeriodName"]) != "")
                whereSql += @" and FiscalPeriod.fiscalPeriodName like '%'+@fiscalPeriodName+'%'";
            if (paras.ContainsKey("fiscalYearId") && DataConvert.ToString(paras["fiscalYearId"]) != "")
                whereSql += @" and FiscalPeriod.fiscalYearId = ''+@fiscalYearId+''";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} FiscalPeriod.fiscalPeriodId fiscalPeriodId,
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
                          where 1=1 {1}", DataConvert.ToString(rowSize), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from FiscalPeriod  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetFiscalPeriod(string fiscalPeriodId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("fiscalPeriodId", fiscalPeriodId);
            string sql = @"select * from FiscalPeriod where fiscalPeriodId=@fiscalPeriodId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from FiscalPeriod where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "FiscalPeriod";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["fiscalPeriodId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("fiscalPeriodId", pkValue);
            string sql = @"select * from FiscalPeriod where fiscalPeriodId=@fiscalPeriodId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "FiscalPeriod";
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
            paras.Add("fiscalPeriodId", pkValue);
            string sql = @"select * from FiscalPeriod where fiscalPeriodId=@fiscalPeriodId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "FiscalPeriod";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select fiscalPeriodId,fiscalPeriodName,fiscalYearId from FiscalPeriod  order by fiscalPeriodName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
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
