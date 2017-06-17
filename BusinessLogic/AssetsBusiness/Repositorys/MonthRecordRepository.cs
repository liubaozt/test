using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Models.MonthRecord;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AutoMonthUpdate
    {
        public static void ExcuteAutoUpdate()
        {
            System.Timers.Timer myTimer = new System.Timers.Timer(60000 * 30); //每30分钟进行一次检查
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            myTimer.Interval = 60000 * 30;
            myTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            MonthRecordRepository rep = new MonthRecordRepository();
            DataUpdate dbUpdate = new DataUpdate();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                rep.UpdateAuto(AppMember.AppText["MonthAutoUpdate"]);
                dbUpdate.Commit();
            }
            catch (Exception ex)
            {
                dbUpdate.Rollback();
                throw ex;
            }
            finally
            {
                dbUpdate.Close();
            }
        }
    }

    public class MonthRecordRepository : MaintainRepository
    {

        public string Check(EntryModel model)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("fiscalPeriodId", model.FiscalPeriodId);
            paras.Add("fiscalYearId", model.FiscalYearId);
            string sql = @"select * from MonthRecord where fiscalYearId=@fiscalYearId and fiscalPeriodId=@fiscalPeriodId ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dt.Rows.Count > 0)
                return "1";
            else
                return "0";
        }

        public override int Update(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            EntryModel myModel = model as EntryModel;
            if (myModel.IsUpdateAgain == "true")
            {
                Dictionary<string, object> paras1 = new Dictionary<string, object>();
                paras1.Add("fiscalPeriodId", myModel.FiscalPeriodId);
                paras1.Add("fiscalYearId", myModel.FiscalYearId);
                string sql1 = @"select * from MonthRecord where fiscalYearId=@fiscalYearId and fiscalPeriodId=@fiscalPeriodId ";
                DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras1, DbUpdate.cmd).Tables[0];
                dt1.TableName = "MonthRecord";
                foreach (DataRow dr in dt1.Rows)
                {
                    dr.Delete();
                }
                DbUpdate.Update(dt1);
            }
            string sql = @"select * from MonthRecord where 1<>1";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "MonthRecord";
            SetRecord(myModel, dt, sysUser);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            return 1;
        }

        public int UpdateAuto( string viewTitle)
        {
            DateTime nowTime = IdGenerator.GetServerDate();
            if (nowTime.Hour <= 1 ||  nowTime.Hour >= 5)
                return 0;
            Dictionary<string, object> paras1 = new Dictionary<string, object>();
            paras1.Add("nowTime", nowTime.AddMonths(-1).ToString("yyyy-MM-dd"));
            string sql1 = @"select fiscalYearId,fiscalPeriodId from FiscalPeriod 
            where fromDate<=@nowTime and toDate>=@nowTime";
            DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras1, DbUpdate.cmd).Tables[0];

            Dictionary<string, object> paras2 = new Dictionary<string, object>();
            paras2.Add("fiscalPeriodId", DataConvert.ToString(dt1.Rows[0]["fiscalPeriodId"]));
            paras2.Add("fiscalYearId", DataConvert.ToString(dt1.Rows[0]["fiscalYearId"]));
            string sql2 = @"select * from MonthRecord where fiscalYearId=@fiscalYearId and fiscalPeriodId=@fiscalPeriodId ";
            DataTable dt2 = AppMember.DbHelper.GetDataSet(sql2, paras2, DbUpdate.cmd).Tables[0];
            dt2.TableName = "MonthRecord";
            if (dt2.Rows.Count > 0)
                return 0;
            string sql = @"select * from MonthRecord where 1<>1";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "MonthRecord";

            EntryModel model = new EntryModel();
            model.FiscalPeriodId = DataConvert.ToString(dt1.Rows[0]["fiscalPeriodId"]);
            model.FiscalYearId = DataConvert.ToString(dt1.Rows[0]["fiscalYearId"]);
            SetRecord(model, dt, null);
            Create5Field(dt, "", viewTitle);
            DbUpdate.Update(dt);
            return 1;
        }

        protected void SetRecord(EntryModel model, DataTable dt, UserInfo sysUser)
        {
            Dictionary<string, object> paras1 = new Dictionary<string, object>();
            paras1.Add("fiscalPeriodId", model.FiscalPeriodId);
            paras1.Add("fiscalYearId", model.FiscalYearId);
            string sql1 = @"select fromDate,toDate from FiscalPeriod 
            where fiscalYearId=@fiscalYearId and fiscalPeriodId=@fiscalPeriodId";
            DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras1, DbUpdate.cmd).Tables[0];
            DateTime fromDate = DataConvert.ToDateTime(dt1.Rows[0]["fromDate"]);
            DateTime toDate = DataConvert.ToDateTime(dt1.Rows[0]["toDate"]);

            DataTable dtBeginPeriod = GetBeginPeriod(fromDate, toDate,model);
            DataTable dtCurrentPeriodAdd = GetCurrentPeriodAdd(fromDate, toDate);
            DataTable dtCurrentPeriodReduce = GetCurrentPeriodReduce(fromDate, toDate,model);
            DataTable dtEndPeriod = GetEndPeriod(fromDate, toDate,model);

            SetDetail(model, dt, dtBeginPeriod, dtCurrentPeriodAdd, dtCurrentPeriodReduce, dtEndPeriod);

        }

        protected void SetDetail( EntryModel model,DataTable dt, DataTable dtBeginPeriod, DataTable dtCurrentPeriodAdd, DataTable dtCurrentPeriodReduce, DataTable dtEndPeriod)
        {
            string sql6 = @"select distinct assetsClassId,departmentId,storeSiteId,setBooksId from assets
            order by assetsClassId,departmentId,storeSiteId,setBooksId";
            DataTable dt6 = AppMember.DbHelper.GetDataSet(sql6, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dt6.Rows)
            {
                DataRow drNew = dt.NewRow();
                drNew["fiscalYearId"] = model.FiscalYearId;
                drNew["fiscalPeriodId"] = model.FiscalPeriodId;
                string assetsClassId = DataConvert.ToString(dr["assetsClassId"]);
                string departmentId = DataConvert.ToString(dr["departmentId"]);
                string storeSiteId = DataConvert.ToString(dr["storeSiteId"]);
                string setBooksId = DataConvert.ToString(dr["setBooksId"]);
                drNew["assetsClassId"] = assetsClassId;
                drNew["departmentId"] = departmentId;
                drNew["storeSiteId"] = storeSiteId;
                drNew["setBooksId"] = setBooksId;
                DataRow[] drSel = dtBeginPeriod.Select(" assetsClassId='" + assetsClassId + "' and departmentId='"
                           + departmentId + "' and storeSiteId='" + storeSiteId + "' and setBooksId='" + setBooksId + "'");
                if (drSel.Length > 0)
                {
                    drNew["beginPeriodNum"] = DataConvert.ToInt32(drSel[0]["beginPeriodNum"]);
                    drNew["beginPeriodAmt"] = DataConvert.ToDouble(drSel[0]["beginPeriodAmt"]);
                }
                else
                {
                    drNew["beginPeriodNum"] = 0;
                    drNew["beginPeriodAmt"] = 0;
                }
                drSel = dtCurrentPeriodAdd.Select(" assetsClassId='" + assetsClassId + "' and departmentId='"
                           + departmentId + "' and storeSiteId='" + storeSiteId + "' and setBooksId='" + setBooksId + "'");
                if (drSel.Length > 0)
                {
                    drNew["currentPeriodAddNum"] = DataConvert.ToInt32(drSel[0]["currentPeriodAddNum"]);
                    drNew["currentPeriodAddAmt"] = DataConvert.ToDouble(drSel[0]["currentPeriodAddAmt"]);
                }
                else
                {
                    drNew["currentPeriodAddNum"] = 0;
                    drNew["currentPeriodAddAmt"] = 0;
                }
                drSel = dtCurrentPeriodReduce.Select(" assetsClassId='" + assetsClassId + "' and departmentId='"
                          + departmentId + "' and storeSiteId='" + storeSiteId + "' and setBooksId='" + setBooksId + "'");
                if (drSel.Length > 0)
                {
                    drNew["currentPeriodReduceNum"] = DataConvert.ToInt32(drSel[0]["currentPeriodReduceNum"]);
                    drNew["currentPeriodReduceAmt"] = DataConvert.ToDouble(drSel[0]["currentPeriodReduceAmt"]);
                }
                else
                {
                    drNew["currentPeriodReduceNum"] = 0;
                    drNew["currentPeriodReduceAmt"] = 0;
                }
                drSel = dtEndPeriod.Select(" assetsClassId='" + assetsClassId + "' and departmentId='"
                          + departmentId + "' and storeSiteId='" + storeSiteId + "' and setBooksId='" + setBooksId + "'");
                if (drSel.Length > 0)
                {
                    drNew["endPeriodNum"] = DataConvert.ToInt32(drSel[0]["endPeriodNum"]);
                    drNew["endPeriodAmt"] = DataConvert.ToDouble(drSel[0]["endPeriodAmt"]);
                }
                else
                {
                    drNew["endPeriodNum"] = 0;
                    drNew["endPeriodAmt"] = 0;
                }

                dt.Rows.Add(drNew);
            }
        }

        protected DataTable GetBeginPeriod(DateTime fromDate, DateTime toDate,EntryModel model)
        {
            string sql = @"select FiscalYear.fiscalYearName,FiscalPeriod.fiscalPeriodName ,FiscalPeriod.fiscalYearId, FiscalPeriod.fiscalPeriodId 
                    from FiscalPeriod,FiscalYear
                     where FiscalYear.fiscalYearId=FiscalPeriod.fiscalYearId
                    order by  FiscalYear.fiscalYearName,FiscalPeriod.fiscalPeriodName";
            DataTable dtFiscalPeriod = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            string fiscalYearIdPre = "";
            string fiscalPeriodIdPre = "";
            for (int i = 0; i < dtFiscalPeriod.Rows.Count; i++)
            {
                if (DataConvert.ToString(dtFiscalPeriod.Rows[i]["fiscalYearId"]) == model.FiscalYearId
                    && DataConvert.ToString(dtFiscalPeriod.Rows[i]["fiscalPeriodId"]) == model.FiscalPeriodId)
                {
                    if (i > 1)
                    {
                        fiscalYearIdPre = DataConvert.ToString(dtFiscalPeriod.Rows[i - 1]["fiscalYearId"]);
                        fiscalPeriodIdPre = DataConvert.ToString(dtFiscalPeriod.Rows[i - 1]["fiscalPeriodId"]);
                        break;
                    }
                }
            }
            Dictionary<string, object> paras2 = new Dictionary<string, object>();
            paras2.Add("fromDate", fromDate);
            paras2.Add("toDate", toDate.ToString("yyyy-MM-dd 23:59:59"));
            paras2.Add("fiscalPeriodId", fiscalPeriodIdPre);
            paras2.Add("fiscalYearId",fiscalYearIdPre );
            string sql2 = @"select Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId,
                    count(Assets.assetsId) beginPeriodNum,sum(isnull(AssetsDepreciation.assetsNetValue,Assets.assetsValue)) beginPeriodAmt 
                    from Assets left join AssetsDepreciation 
                    on ( Assets.assetsId=AssetsDepreciation.assetsId and AssetsDepreciation.fiscalYearId=@fiscalYearId and AssetsDepreciation.fiscalPeriodId=@fiscalPeriodId )
                    where  ((Assets.addDate<@fromDate and Assets.assetsState not in ('O','S','M','L')) or 
                    (Assets.smDate>=@fromDate and Assets.smDate<=@toDate and Assets.assetsState  in ('S','M','L')))
                    group by Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId";
            DataTable dt2 = AppMember.DbHelper.GetDataSet(sql2, paras2, DbUpdate.cmd).Tables[0];
            return dt2;
        }

        protected DataTable GetCurrentPeriodAdd(DateTime fromDate, DateTime toDate)
        {
            Dictionary<string, object> paras3 = new Dictionary<string, object>();
            paras3.Add("fromDate", fromDate);
            paras3.Add("toDate", toDate.ToString("yyyy-MM-dd 23:59:59"));
            string sql3 = @"select assetsClassId,departmentId,storeSiteId,setBooksId,count(assetsId) currentPeriodAddNum,sum(assetsNetValue) currentPeriodAddAmt 
            from Assets where addDate>=@fromDate and addDate<=@toDate 
            and Assets.assetsState not in ('O')
            group by assetsClassId,departmentId,storeSiteId,setBooksId";
            DataTable dt3 = AppMember.DbHelper.GetDataSet(sql3, paras3, DbUpdate.cmd).Tables[0];
            return dt3;
        }

        protected DataTable GetCurrentPeriodReduce(DateTime fromDate, DateTime toDate, EntryModel model)
        {
            Dictionary<string, object> paras4 = new Dictionary<string, object>();
            paras4.Add("fiscalPeriodId", model.FiscalPeriodId);
            paras4.Add("fiscalYearId", model.FiscalYearId);
            paras4.Add("fromDate", fromDate);
            paras4.Add("toDate", toDate.ToString("yyyy-MM-dd 23:59:59"));
     
            string sql4 = @"select assetsClassId,departmentId,storeSiteId,setBooksId
                    ,sum(currentPeriodReduceNum) currentPeriodReduceNum,sum(currentPeriodReduceAmt) currentPeriodReduceAmt
                    from (";

            //折旧
             sql4 += @"select Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId,0 currentPeriodReduceNum,sum(AssetsDepreciation.depreciationAmount) currentPeriodReduceAmt 
            from AssetsDepreciation,Assets  where Assets.assetsId=AssetsDepreciation.assetsId 
            and AssetsDepreciation.fiscalYearId=@fiscalYearId and AssetsDepreciation.fiscalPeriodId=@fiscalPeriodId 
            group by Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId";
            //报废
            sql4 += @" union all ";
            sql4 += @"select Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId,
            count(Assets.assetsId) currentPeriodReduceNum,sum(assetsNetValue) currentPeriodReduceAmt 
            from AssetsScrapDetail,Assets  where Assets.assetsId=AssetsScrapDetail.assetsId 
            and AssetsScrapDetail.scrapDate>=@fromDate and  AssetsScrapDetail.scrapDate<=@toDate 
            group by Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId";
            //已拆分,已合并，盘点丢失
            sql4 += @" union all ";
            sql4 += @"select Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId,
            count(Assets.assetsId) currentPeriodReduceNum,sum(assetsNetValue) currentPeriodReduceAmt 
            from Assets  where  Assets.assetsState in ('S','M','L') 
            and Assets.smDate>=@fromDate and Assets.smDate<=@toDate 
            group by Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId";

            sql4 += @") A
            group by assetsClassId,departmentId,storeSiteId,setBooksId
            order by assetsClassId,departmentId,storeSiteId,setBooksId";
            DataTable dt4 = AppMember.DbHelper.GetDataSet(sql4, paras4, DbUpdate.cmd).Tables[0];

            return dt4;
        }

        protected DataTable GetEndPeriod(DateTime fromDate, DateTime toDate, EntryModel model)
        {
            Dictionary<string, object> paras5 = new Dictionary<string, object>();
            paras5.Add("fromDate", fromDate);
            paras5.Add("toDate", toDate.ToString("yyyy-MM-dd 23:59:59"));
            paras5.Add("fiscalPeriodId", model.FiscalPeriodId);
            paras5.Add("fiscalYearId", model.FiscalYearId);
            string sql5 = @"select Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId,
                    count(Assets.assetsId) endPeriodNum,sum(isnull(AssetsDepreciation.assetsNetValue,Assets.assetsValue)) endPeriodAmt 
                    from Assets left join AssetsDepreciation 
                    on ( Assets.assetsId=AssetsDepreciation.assetsId and AssetsDepreciation.fiscalYearId=@fiscalYearId and AssetsDepreciation.fiscalPeriodId=@fiscalPeriodId )
                    where  Assets.addDate<=@toDate and (Assets.smDate<@fromDate or Assets.smDate is null)
                    and Assets.assetsState not in ('O','S','M','L')
                    group by Assets.assetsClassId,Assets.departmentId,Assets.storeSiteId,Assets.setBooksId";
            DataTable dt5 = AppMember.DbHelper.GetDataSet(sql5, paras5, DbUpdate.cmd).Tables[0];
            return dt5;
        }


    }
}
