using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;

namespace BusinessLogic.AssetsBusiness
{
    public class AssetsDepreciation
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsValue { get; set; }
        public string AssetsNetValue { get; set; }
        public string DepreciationType { get; set; }
        public string DepreciationMonth { get; set; }
        public string DepreciationTotal { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsDepreciationRepository : MaintainRepository
    {

        public DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey)
        {
            string sql = string.Format(@"select Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        Assets.assetsValue assetsValue,
                   Assets.assetsNetValue assetsNetValue,
                   Assets.depreciationType depreciationTypeCode,
                   Assets.durableYears durableYears,
                   Assets.remainMonth remainMonth,
                   Assets.remainRate remainRate,
                  (select CodeTable.codeName from CodeTable where Assets.depreciationType=CodeTable.codeNo and CodeTable.codeType='DepreciationType' and CodeTable.languageVer='{0}' ) depreciationType,
                  0 depreciationMonth ,
                  (Assets.assetsValue-Assets.assetsNetValue) depreciationTotal ,
                  '' Remark
                from Assets ", AppMember.AppLanguage.ToString());
            if (formMode == "pickUp")
            {
                sql += " where 1=1 " + WhereSql(paras);
            }
            else
            {
                sql += " where 1<>1  ";
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            foreach (DataRow dr in dtGrid.Rows)
            {
                double assetsValue = DataConvert.ToDouble(dr["assetsValue"]);
                double assetsNetValue = DataConvert.ToDouble(dr["assetsNetValue"]);
                double remainRate = DataConvert.ToDouble(dr["remainRate"]);
                double durableYears = DataConvert.ToDouble(dr["durableYears"]);
                double remainMonth = DataConvert.ToDouble(dr["remainMonth"]);
                if (DataConvert.ToString(dr["depreciationTypeCode"]) == "L")
                {
                    double depreciationMonth = assetsValue * ((1 - remainRate) / (12 * durableYears));
                    dr["depreciationMonth"] = depreciationMonth;
                }
                else if (DataConvert.ToString(dr["depreciationTypeCode"]) == "D")
                {
                    if (remainMonth > 24)
                    {
                        double depreciationMonth = assetsNetValue * (2 / (12 * durableYears));
                        dr["depreciationMonth"] = depreciationMonth;
                    }
                    else
                    {
                        double depreciationMonth = assetsValue * ((1 - remainRate) / (12 * durableYears));
                        dr["depreciationMonth"] = depreciationMonth;
                    }
                }
                else if (DataConvert.ToString(dr["depreciationTypeCode"]) == "Y")
                {
                    double useYear = Math.Floor((durableYears * 12 - remainMonth) / 12);
                    double depreciationMonth = assetsValue * (1 - remainRate) * (((durableYears - useYear) / (durableYears * (durableYears + 1) / 2)) / 12);
                    dr["depreciationMonth"] = depreciationMonth;
                }
            }
            return dtGrid;
        }

        protected string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("departmentId") && DataConvert.ToString(paras["departmentId"]) != "")
                whereSql += @" and Assets.departmentId =@departmentId";
            return whereSql;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            List<AssetsDepreciation> gridData = JsonHelper.JSONStringToList<AssetsDepreciation>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsDepreciation assetsBorrow in gridData)
            {
                AddDetail(objs, assetsBorrow, sysUser, viewTitle);
            }
            return 1;
        }

        protected int AddDetail(Dictionary<string, object> objs, AssetsDepreciation assetsDepreciation, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsDepreciation where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsDepreciation";
            DataRow dr = dt.NewRow();
            dr["assetsId"] = assetsDepreciation.AssetsId;
            dr["fiscalYearId"] = DataConvert.ToString(objs["fiscalYearId"]);
            dr["fiscalPeriodId"] = DataConvert.ToString(objs["fiscalPeriodId"]);
            dr["depreciationMonth"] = assetsDepreciation.DepreciationMonth;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            UpdateAssets(assetsDepreciation,sysUser,viewTitle);
            return 1;
        }

        protected int UpdateAssets( AssetsDepreciation assetsDepreciation, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsDepreciation.AssetsId);
            string sql = @"select * from Assets where assetsId=@assetsId ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras,dbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            dt.Rows[0]["assetsNetValue"] = DataConvert.ToDouble(dt.Rows[0]["assetsNetValue"]) - DataConvert.ToDouble(assetsDepreciation.DepreciationMonth);
            dt.Rows[0]["remainMonth"] = DataConvert.ToInt32(dt.Rows[0]["remainMonth"]) - 1;
            Update5Field(dt, sysUser.UserId, viewTitle);
            return dbUpdate.Update(dt);
        }

    }
}
