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
using BusinessLogic.AssetsBusiness.Models.AssetsDepreciation;

namespace BusinessLogic.AssetsBusiness.Repositorys
{

    public class AutoDepreciation
    {
        static bool TaskExcuting = false;
        public static void ExcuteAutoUpdateByThread()
        {
            if (!TaskExcuting)
            {
                TaskExcuting = true;
                //AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", string.Format(AppMember.AppText["AutoDepreciationProcess"]));
                AssetsDepreciationRepository rep = new AssetsDepreciationRepository();
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
                    AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Error, "AutoTask", ex.Message);
                    throw ex;
                }
                finally
                {
                    dbUpdate.Close();
                    TaskExcuting = false;
                   
                }
            }
        }


        static System.Timers.Timer myTimer;
        public static void ExcuteAutoUpdate()
        {
            myTimer = new System.Timers.Timer(60000 * 1); //每3分钟进行一次检查
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            //myTimer.Interval = 60000 * 3;
            myTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            myTimer.Stop();
            AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", string.Format(AppMember.AppText["AutoDepreciationProcess"]));
            AssetsDepreciationRepository rep = new AssetsDepreciationRepository();
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
                myTimer.Start();
            }
        }
    }


    public class AssetsDepreciation
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsValue { get; set; }
        public string AssetsNetValue { get; set; }
        public string DepreciationType { get; set; }
        public string PurchaseDate { get; set; }
        public string DepreciationAmount { get; set; }
        public string DepreciationTotal { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsDepreciationRepository : MaintainRepository
    {

        public DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey)
        {
            string sqlWhere = "";
            if (formMode == "pickUp")
            {
                sqlWhere += " where 1=1 " + WhereSql(paras);
            }
            else
            {
                sqlWhere += " where 1<>1  ";
            }
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int pageRowNum = DataConvert.ToInt32(paras["pageRowNum"]);
            int start = 1 + pageRowNum * (pageIndex - 1);
            int end = pageRowNum * pageIndex;
            string sql = string.Format(@"select Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        Assets.assetsValue assetsValue,
                   Assets.assetsNetValue assetsNetValue,
                   Assets.depreciationType depreciationTypeCode,
                   Assets.durableYears durableYears,
                   Assets.remainMonth remainMonth,
                   Assets.remainRate remainRate,
                   Assets.purchaseDate purchaseDate,
                   Assets.depreciationRule depreciationRule,
                  (select CodeTable.codeName from CodeTable where Assets.depreciationType=CodeTable.codeNo and CodeTable.codeType='DepreciationType' and CodeTable.languageVer='{0}' ) depreciationType,
                  0 depreciationAmount ,
                  (Assets.assetsValue-Assets.assetsNetValue) depreciationTotal ,
                  '' Remark
                from (select *,Row_Number() OVER ( ORDER BY assetsno ) rnum from Assets {1}) Assets where rnum between {2} and {3}  ", AppMember.AppLanguage.ToString(), sqlWhere, start, end);

            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            SetData(dtGrid);
            //foreach (DataRow dr in dtGrid.Rows)
            //{
            //    double assetsValue = DataConvert.ToDouble(dr["assetsValue"]);
            //    double assetsNetValue = DataConvert.ToDouble(dr["assetsNetValue"]);
            //    double remainRate = DataConvert.ToDouble(dr["remainRate"]);
            //    double durableYears = DataConvert.ToDouble(dr["durableYears"]);
            //    double remainMonth = DataConvert.ToDouble(dr["remainMonth"]);
            //    DateTime purchaseDate = DataConvert.ToDateTime(dr["purchaseDate"]);
            //    if (DataConvert.ToString(dr["depreciationTypeCode"]) == "L")
            //    {
            //        double depreciationAmount = durableYears==0?0:(assetsValue * ((1 - remainRate) / (12 * durableYears)));
            //        int totalMonth = DateTime.Now.Year * 12 + DateTime.Now.Month - purchaseDate.Year * 12 - purchaseDate.Month;
            //        if (assetsValue == assetsNetValue && totalMonth > 0)
            //            depreciationAmount = depreciationAmount * totalMonth;
            //        dr["depreciationAmount"] = depreciationAmount;
            //    }
            //    else if (DataConvert.ToString(dr["depreciationTypeCode"]) == "D")
            //    {
            //        if (remainMonth > 24)
            //        {
            //            double depreciationAmount = assetsNetValue * (2 / (12 * durableYears));
            //            dr["depreciationAmount"] = depreciationAmount;
            //        }
            //        else
            //        {
            //            double depreciationAmount = assetsValue * ((1 - remainRate) / (12 * durableYears));
            //            dr["depreciationAmount"] = depreciationAmount;
            //        }
            //    }
            //    else if (DataConvert.ToString(dr["depreciationTypeCode"]) == "Y")
            //    {
            //        double useYear = Math.Floor((durableYears * 12 - remainMonth) / 12);
            //        double depreciationAmount = assetsValue * (1 - remainRate) * (((durableYears - useYear) / (durableYears * (durableYears + 1) / 2)) / 12);
            //        dr["depreciationAmount"] = depreciationAmount;
            //    }
            //}
            return dtGrid;
        }

        private void SetData(DataTable dtGrid)
        {
            foreach (DataRow dr in dtGrid.Rows)
            {
                double assetsValue = DataConvert.ToDouble(dr["assetsValue"]);
                double assetsNetValue = DataConvert.ToDouble(dr["assetsNetValue"]);
                double remainMonth = DataConvert.ToDouble(dr["remainMonth"]);
                DateTime purchaseDate = DataConvert.ToDateTime(dr["purchaseDate"]);
                double remainRate = DataConvert.ToDouble(dr["remainRate"]);
                double durableYears = DataConvert.ToDouble(dr["durableYears"]);
                double totalMonth = durableYears * 12;
                string depreciationTypeCode = DataConvert.ToString(dr["depreciationTypeCode"]);
                if (AppMember.DepreciationRuleOpen)
                {
                    string depreciationRule = DataConvert.ToString(dr["depreciationRule"]);
                    Dictionary<string, object> paras1 = new Dictionary<string, object>();
                    paras1.Add("depreciationRuleId", depreciationRule);
                    string sql1 = @"select * from DepreciationRule  where depreciationRuleId=@depreciationRuleId ";
                    DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras1).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        remainRate = DataConvert.ToDouble(dt1.Rows[0]["remainRate"]);
                        totalMonth = DataConvert.ToDouble(dt1.Rows[0]["totalMonth"]);
                        depreciationTypeCode = DataConvert.ToString(dt1.Rows[0]["depreciationType"]);
                        if (depreciationTypeCode == "")
                            depreciationTypeCode = "L";
                    }
                }

                if (depreciationTypeCode == "L")
                {
                    double depreciationAmount = totalMonth == 0 ? 0 : (assetsValue * ((1 - remainRate) / (totalMonth)));
                    DateTime depreciationTime = IdGenerator.GetServerDate().AddMonths(-1);
                    int totalDepreciationMonth = depreciationTime.Year * 12 + depreciationTime.Month - purchaseDate.Year * 12 - purchaseDate.Month;
                    if (DataConvert.ToInt32(totalDepreciationMonth) >= DataConvert.ToInt32(totalMonth))
                    {
                        dr["remainMonth"] = 0;
                        dr["depreciationAmount"] = assetsNetValue - assetsValue * remainRate;
                    }
                    else
                    {
                        if (assetsValue == assetsNetValue && totalDepreciationMonth > 0)
                        {
                            dr["remainMonth"] = DataConvert.ToInt32(dr["remainMonth"]) - totalDepreciationMonth;
                            if (DataConvert.ToInt32(dr["remainMonth"]) == 0)
                                depreciationAmount = assetsValue;
                            else
                                depreciationAmount = depreciationAmount * totalDepreciationMonth;
                        }
                        else
                        {
                            dr["remainMonth"] = DataConvert.ToInt32(dr["remainMonth"]) - 1;
                        }
                        dr["depreciationAmount"] = depreciationAmount;
                    }
                }
                else if (depreciationTypeCode == "D")
                {
                    if (remainMonth > 24)
                    {
                        double depreciationAmount = assetsNetValue * (2 / (12 * durableYears));
                        dr["depreciationAmount"] = depreciationAmount;
                    }
                    else
                    {
                        double depreciationAmount = assetsValue * ((1 - remainRate) / (12 * durableYears));
                        dr["depreciationAmount"] = depreciationAmount;
                    }
                }
                else if (depreciationTypeCode == "Y")
                {
                    double useYear = Math.Floor((durableYears * 12 - remainMonth) / 12);
                    double depreciationAmount = assetsValue * (1 - remainRate) * (((durableYears - useYear) / (durableYears * (durableYears + 1) / 2)) / 12);
                    dr["depreciationAmount"] = depreciationAmount;
                }
            }
        }

        public int GetGridCount(Dictionary<string, object> paras, string formMode)
        {
            if (formMode != "pickUp")
            {
                return 1;
            }
            string sql = string.Format(@"select 1 from Assets where 1=1 {0}  ", WhereSql(paras));

            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];

            return dtGrid.Rows.Count;
        }


        private DataTable GetGridDataSource(Dictionary<string, object> paras)
        {
            string sql = string.Format(@"select Assets.assetsId AssetsId,
                        Assets.assetsNo AssetsNo,
                        Assets.assetsName AssetsName,
                        Assets.assetsValue AssetsValue,
                   Assets.assetsNetValue AssetsNetValue,
                   Assets.depreciationType DepreciationTypeCode,
                   Assets.durableYears DurableYears,
                   Assets.remainMonth RemainMonth,
                   Assets.remainRate RemainRate,
                  Assets.purchaseDate2 purchaseDate,
                  Assets.depreciationRule depreciationRule,
                  (select CodeTable.codeName from CodeTable where Assets.depreciationType=CodeTable.codeNo and CodeTable.codeType='DepreciationType' and CodeTable.languageVer='{0}' ) DepreciationType,
                  0.00 DepreciationAmount ,
                  (Assets.assetsValue-Assets.assetsNetValue) DepreciationTotal ,
                  '' Remark
                from Assets where 1=1 {1} ", AppMember.AppLanguage.ToString(), WhereSql(paras));

            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            SetData(dtGrid);
            //foreach (DataRow dr in dtGrid.Rows)
            //{
            //    double assetsValue = DataConvert.ToDouble(dr["assetsValue"]);
            //    double assetsNetValue = DataConvert.ToDouble(dr["assetsNetValue"]);
            //    double remainRate = DataConvert.ToDouble(dr["remainRate"]);
            //    double durableYears = DataConvert.ToDouble(dr["durableYears"]);
            //    double remainMonth = DataConvert.ToDouble(dr["remainMonth"]);
            //    if (DataConvert.ToString(dr["depreciationTypeCode"]) == "L")
            //    {
            //        double depreciationAmount = assetsValue * ((1 - remainRate) / (12 * durableYears));
            //        dr["depreciationAmount"] = depreciationAmount;
            //    }
            //    else if (DataConvert.ToString(dr["depreciationTypeCode"]) == "D")
            //    {
            //        if (remainMonth > 24)
            //        {
            //            double depreciationAmount = assetsNetValue * (2 / (12 * durableYears));
            //            dr["depreciationAmount"] = depreciationAmount;
            //        }
            //        else
            //        {
            //            double depreciationAmount = assetsValue * ((1 - remainRate) / (12 * durableYears));
            //            dr["depreciationAmount"] = depreciationAmount;
            //        }
            //    }
            //    else if (DataConvert.ToString(dr["depreciationTypeCode"]) == "Y")
            //    {
            //        double useYear = Math.Floor((durableYears * 12 - remainMonth) / 12);
            //        double depreciationAmount = assetsValue * (1 - remainRate) * (((durableYears - useYear) / (durableYears * (durableYears + 1) / 2)) / 12);
            //        dr["depreciationAmount"] = depreciationAmount;
            //    }
            //}
            return dtGrid;
        }


        protected string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            //whereSql += " and assetsState in ('A','B','X') ";
            whereSql += " and (remainMonth>0 and remainMonth is not null) ";
            if (AppMember.DepreciationRuleOpen)
            {
                whereSql += " and (depreciationRule is not null and depreciationRule<>'') ";
            }
            else
            {
                whereSql += " and  (depreciationType is not null and depreciationType<>'') ";
            }
            if (paras.ContainsKey("fiscalYearId") && DataConvert.ToString(paras["fiscalYearId"]) != "")
            {
                //Dictionary<string, object> paras1 = new Dictionary<string, object>();
                //paras1.Add("fiscalYearId", DataConvert.ToString(paras["fiscalYearId"]));
                //string sql = @"select fiscalYearName from FiscalYear where fiscalYearId=@fiscalYearId";
                //DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras1).Tables[0];
                //string fiscalYearName = DataConvert.ToString(dtGrid.Rows[0]["fiscalYearName"]);

                //Dictionary<string, object> paras2 = new Dictionary<string, object>();
                //paras2.Add("fiscalPeriodId", DataConvert.ToString(paras["fiscalPeriodId"]));
                //sql = @"select fiscalPeriodName from FiscalPeriod where fiscalPeriodId=@fiscalPeriodId";
                //DataTable dtGrid2 = AppMember.DbHelper.GetDataSet(sql, paras2).Tables[0];
                //string fiscalPeriodName = DataConvert.ToString(dtGrid2.Rows[0]["fiscalPeriodName"]);
                //DateTime dtTime = DataConvert.ToDateTime(fiscalYearName + "-" + fiscalPeriodName + "-1");

                DateTime nowTime = IdGenerator.GetServerDate().AddMonths(-1);
             
                whereSql += string.Format(" and (purchaseDate2 is not null and purchaseDate2<'{0}')", nowTime.ToString("yyyy-MM-dd"));

                whereSql += string.Format(@" and  not exists ( select 1 from AssetsDepreciation where AssetsDepreciation.assetsId=Assets.assetsId and  fiscalYearId=@fiscalYearId and fiscalPeriodId=@fiscalPeriodId)");
            }

            return whereSql;
        }

        public override DataRow GetModel()
        {
            string sql = @"select DepreciationTrack.fiscalYearId,
                (select fiscalYearName from FiscalYear where DepreciationTrack.fiscalYearId=FiscalYear.fiscalYearId) fiscalYearName,
                DepreciationTrack.fiscalPeriodId,
                (select fiscalPeriodName from FiscalPeriod where DepreciationTrack.fiscalPeriodId=FiscalPeriod.fiscalPeriodId and DepreciationTrack.fiscalYearId=FiscalPeriod.fiscalYearId) fiscalPeriodName,
                DepreciationTrack.fiscalYearIdNext,
                (select fiscalYearName from FiscalYear where DepreciationTrack.fiscalYearIdNext=FiscalYear.fiscalYearId) fiscalYearIdNextName,
                DepreciationTrack.fiscalPeriodIdNext,
                (select fiscalPeriodName from FiscalPeriod where DepreciationTrack.fiscalPeriodIdNext=FiscalPeriod.fiscalPeriodId and DepreciationTrack.fiscalYearId=FiscalPeriod.fiscalYearId) fiscalPeriodIdNextName
                from DepreciationTrack ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            if (dtGrid.Rows.Count > 0)
                return dtGrid.Rows[0];
            else
                return null;
        }

        public override int Update(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            EntryModel myModel = model as EntryModel;
            Dictionary<string, object> objs = new Dictionary<string, object>();
            objs.Add("fiscalYearId", myModel.FiscalYearId);
            objs.Add("fiscalPeriodId", myModel.FiscalPeriodId);
            DataTable dt = GetGridDataSource(objs);
            foreach (DataRow dr in dt.Rows)
            {
                AddDetail(myModel, dr, sysUser, viewTitle);
            }
            //List<AssetsDepreciation> gridData = JsonHelper.JSONStringToList<AssetsDepreciation>(DataConvert.ToString(myModel.EntryGridString));
            //foreach (AssetsDepreciation assetsBorrow in gridData)
            //{
            //    AddDetail(myModel, assetsBorrow, sysUser, viewTitle);
            //}
            UpdateDepreciationTrack(myModel, sysUser, viewTitle);
            return 1;
        }

        public int UpdateAuto(string viewTitle)
        {
            DateTime nowTime = IdGenerator.GetServerDate();
            //if (nowTime.Hour <= 1 || nowTime.Hour >= 5)
            //    return 0;
            //找到本月对应期间
            Dictionary<string, object> paras1 = new Dictionary<string, object>();
            paras1.Add("nowTime", nowTime.AddMonths(-1).ToString("yyyy-MM-dd"));
            string sql1 = @"select fiscalYearId,fiscalPeriodId from FiscalPeriod 
            where fromDate<=@nowTime and toDate>=@nowTime";
            DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras1, DbUpdate.cmd).Tables[0];

            ////找到本月已经折旧的资产集合
            //Dictionary<string, object> paras2 = new Dictionary<string, object>();
            //paras2.Add("fiscalPeriodId", DataConvert.ToString(dt1.Rows[0]["fiscalPeriodId"]));
            //paras2.Add("fiscalYearId", DataConvert.ToString(dt1.Rows[0]["fiscalYearId"]));
            //string sql2 = @"select * from AssetsDepreciation where fiscalYearId=@fiscalYearId and fiscalPeriodId=@fiscalPeriodId ";
            //DataTable dt2 = AppMember.DbHelper.GetDataSet(sql2, paras2, DbUpdate.cmd).Tables[0];
            //dt2.TableName = "AssetsDepreciation";

            //找到本月所有需要折旧的集合，如该资产本月已经折旧就不需要再折旧了。
            Dictionary<string, object> paras2 = new Dictionary<string, object>();
            paras2.Add("fiscalPeriodId", DataConvert.ToString(dt1.Rows[0]["fiscalPeriodId"]));
            paras2.Add("fiscalYearId", DataConvert.ToString(dt1.Rows[0]["fiscalYearId"]));
            EntryModel model = new EntryModel();
            model.FiscalPeriodId = DataConvert.ToString(dt1.Rows[0]["fiscalPeriodId"]);
            model.FiscalYearId = DataConvert.ToString(dt1.Rows[0]["fiscalYearId"]);
            DataTable dt = GetGridDataSource(paras2);
            UserInfo sysUser = new UserInfo();
            sysUser.MySetBooks = new CurSetBooks();
            sysUser.MySetBooks.SetBooksId = "BK1503010001";
            foreach (DataRow dr in dt.Rows)
            {
                //if (dt2.Select(string.Format(" fiscalYearId='{0}' and fiscalPeriodId='{1}' and assetsId='{2}' ", model.FiscalYearId, model.FiscalPeriodId, DataConvert.ToString(dr["assetsId"]))).Length > 0)
                //    continue;
                AddDetail(model, dr, sysUser, AppMember.AppText["AutoDepreciation"]);
            }
            //AddDetail(model, null, AppMember.AppText["AutoDepreciation"], paras2);
            UpdateDepreciationTrack(model, null, viewTitle);
            return 1;
        }

        //protected int AddDetail(EntryModel model, AssetsDepreciation assetsDepreciation, UserInfo sysUser, string viewTitle)
        //{
        //    string sql = @"select * from AssetsDepreciation where 1<>1 ";
        //    DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
        //    dt.TableName = "AssetsDepreciation";
        //    DataRow dr = dt.NewRow();
        //    dr["assetsId"] = assetsDepreciation.AssetsId;
        //    dr["fiscalYearId"] = model.FiscalYearId;
        //    dr["fiscalPeriodId"] = model.FiscalPeriodId;
        //    dr["depreciationAmount"] = assetsDepreciation.DepreciationAmount;
        //    dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
        //    UpdateAssets(assetsDepreciation, dr, sysUser, viewTitle);
        //    dt.Rows.Add(dr);
        //    Create5Field(dt, sysUser.UserId, viewTitle);
        //    DbUpdate.Update(dt);
        //    return 1;
        //}

        protected int AddDetail(EntryModel model, DataRow drAssetsDepreciation, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsDepreciation where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsDepreciation";
            DataRow dr = dt.NewRow();
            dr["assetsId"] = drAssetsDepreciation["AssetsId"];
            dr["fiscalYearId"] = model.FiscalYearId;
            dr["fiscalPeriodId"] = model.FiscalPeriodId;
            dr["depreciationAmount"] = drAssetsDepreciation["DepreciationAmount"];
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dr["remainMonth"] = DataConvert.ToInt32(drAssetsDepreciation["remainMonth"]);
            UpdateAssets(dr, sysUser, viewTitle);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            return 1;
        }

        //protected int AddDetail(EntryModel model, UserInfo sysUser, string viewTitle, Dictionary<string, object> paras)
        //{
        //    DataTable dtAssets = GetGridDataSource(paras);
        //    string sql = @"select * from AssetsDepreciation where 1<>1 ";
        //    DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
        //    dt.TableName = "AssetsDepreciation";
        //    foreach (DataRow drAssets in dtAssets.Rows)
        //    {
        //        AssetsDepreciation assetsDepreciation = new AssetsDepreciation();
        //        assetsDepreciation.AssetsId = DataConvert.ToString(drAssets["assetsId"]);
        //        assetsDepreciation.DepreciationAmount = DataConvert.ToString(drAssets["depreciationAmount"]);
        //        DataRow dr = dt.NewRow();
        //        dr["assetsId"] = assetsDepreciation.AssetsId;
        //        dr["fiscalYearId"] = model.FiscalYearId;
        //        dr["fiscalPeriodId"] = model.FiscalPeriodId;
        //        dr["depreciationAmount"] = assetsDepreciation.DepreciationAmount;
        //        dr["setBooksId"] = "BK1503010001";
        //        UpdateAssets(assetsDepreciation, dr, sysUser, viewTitle);
        //        dt.Rows.Add(dr);
        //    }
        //    Create5Field(dt, sysUser == null ? "" : sysUser.UserId, viewTitle);
        //    DbUpdate.Update(dt);
        //    return 1;
        //}

        //protected int UpdateAssets(AssetsDepreciation assetsDepreciation, DataRow drAssetsDepreciation, UserInfo sysUser, string viewTitle)
        //{
        //    Dictionary<string, object> paras = new Dictionary<string, object>();
        //    paras.Add("assetsId", assetsDepreciation.AssetsId);
        //    string sql = @"select * from Assets where assetsId=@assetsId ";
        //    DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
        //    dt.TableName = "Assets";
        //    double assetsNetValue = DataConvert.ToDouble(dt.Rows[0]["assetsNetValue"]) - DataConvert.ToDouble(assetsDepreciation.DepreciationAmount);
        //    int remainMonth = DataConvert.ToInt32(dt.Rows[0]["remainMonth"]) - 1;
        //    dt.Rows[0]["assetsNetValue"] = assetsNetValue;
        //    dt.Rows[0]["remainMonth"] = remainMonth;
        //    drAssetsDepreciation["assetsNetValue"] = assetsNetValue;
        //    drAssetsDepreciation["remainMonth"] = remainMonth;
        //    Update5Field(dt, sysUser.UserId, viewTitle);
        //    return DbUpdate.Update(dt);
        //}

        protected int UpdateAssets(DataRow drAssetsDepreciation, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", DataConvert.ToString(drAssetsDepreciation["AssetsId"]));
            string sql = @"select * from Assets where assetsId=@assetsId ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            int remainMonth = DataConvert.ToInt32(drAssetsDepreciation["remainMonth"]);
            double assetsNetValue = DataConvert.ToDouble(dt.Rows[0]["assetsNetValue"]) - DataConvert.ToDouble(drAssetsDepreciation["DepreciationAmount"]);
            dt.Rows[0]["assetsNetValue"] = assetsNetValue;
            dt.Rows[0]["remainMonth"] = remainMonth;
            drAssetsDepreciation["assetsNetValue"] = assetsNetValue;
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected int UpdateDepreciationTrack(EntryModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select FiscalYear.fiscalYearName,FiscalPeriod.fiscalPeriodName ,FiscalPeriod.fiscalYearId, FiscalPeriod.fiscalPeriodId 
                    from FiscalPeriod,FiscalYear
                     where FiscalYear.fiscalYearId=FiscalPeriod.fiscalYearId
                    order by  FiscalYear.fiscalYearName,FiscalPeriod.fiscalPeriodName";
            DataTable dtFiscalPeriod = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            string fiscalYearIdNext = null;
            string fiscalPeriodIdNext = null;
            for (int i = 0; i < dtFiscalPeriod.Rows.Count; i++)
            {
                if (DataConvert.ToString(dtFiscalPeriod.Rows[i]["fiscalYearId"]) == model.FiscalYearId
                    && DataConvert.ToString(dtFiscalPeriod.Rows[i]["fiscalPeriodId"]) == model.FiscalPeriodId)
                {
                    if ((i + 1) < dtFiscalPeriod.Rows.Count)
                    {
                        fiscalYearIdNext = DataConvert.ToString(dtFiscalPeriod.Rows[i + 1]["fiscalYearId"]);
                        fiscalPeriodIdNext = DataConvert.ToString(dtFiscalPeriod.Rows[i + 1]["fiscalPeriodId"]);
                        break;
                    }
                }
            }

            Dictionary<string, object> paras = new Dictionary<string, object>();
            sql = @"select * from DepreciationTrack ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "DepreciationTrack";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["fiscalYearId"] = model.FiscalYearId;
                    dr["fiscalPeriodId"] = model.FiscalPeriodId;
                    dr["fiscalYearIdNext"] = fiscalYearIdNext;
                    dr["fiscalPeriodIdNext"] = fiscalPeriodIdNext;
                }
                Update5Field(dt, sysUser == null ? "" : sysUser.UserId, viewTitle);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["depreciationTrackId"] = IdGenerator.GetMaxId(dt.TableName);
                dr["fiscalYearId"] = model.FiscalYearId;
                dr["fiscalPeriodId"] = model.FiscalPeriodId;
                dr["fiscalYearIdNext"] = fiscalYearIdNext;
                dr["fiscalPeriodIdNext"] = fiscalPeriodIdNext;
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser == null ? "" : sysUser.UserId, viewTitle);
            }
            return DbUpdate.Update(dt);
        }


    }
}