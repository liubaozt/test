using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.DepreciationRule;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Repositorys
{
    public class DepreciationRuleRepository : MasterRepository
    {

        public DepreciationRuleRepository()
        {
            DefaulteGridSortField = "depreciationRuleNo";
            MasterTable = "DepreciationRule";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.DepreciationRuleNo) != "")
            {
                wcd.Sql += @" and DepreciationRule.depreciationRuleNo like '%'+@depreciationRuleNo+'%'";
                wcd.DBPara.Add("depreciationRuleNo", model.DepreciationRuleNo);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  DepreciationRule.depreciationRuleId depreciationRuleId,
                                 DepreciationRule.depreciationRuleNo depreciationRuleNo,
                                 DepreciationRule.totalMonth totalMonth,
                                  DepreciationRule.remainRate remainRate,
                                  (select CodeTable.codeName from CodeTable where DepreciationRule.depreciationType=CodeTable.codeNo and CodeTable.codeType='DepreciationType' and CodeTable.languageVer='{0}' ) depreciationType,
                                 U1.userName createId ,
                                 DepreciationRule.createTime createTime ,
                                 U2.userName updateId ,
                                 DepreciationRule.updateTime updateTime ,
                                 DepreciationRule.updatePro updatePro
                          from DepreciationRule left join AppUser U1 on DepreciationRule.createId=U1.userId
                                    left join AppUser U2 on DepreciationRule.updateId=U2.userId 
                          where 1=1 ", AppMember.AppLanguage.ToString());
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("depreciationRuleId", primaryKey);
                string sql = @"select * from DepreciationRule where depreciationRuleId=@depreciationRuleId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.DepreciationRuleId = primaryKey;
                model.DepreciationRuleNo = DataConvert.ToString(dr["depreciationRuleNo"]);
                model.TotalMonth = DataConvert.ToInt32(dr["totalMonth"]);
                model.RemainRate = DataConvert.ToDouble(dr["remainRate"]);
                model.DepreciationType = DataConvert.ToString(dr["depreciationType"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from DepreciationRule where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "DepreciationRule";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["depreciationRuleId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("depreciationRuleId", pkValue);
            string sql = @"select * from DepreciationRule where depreciationRuleId=@depreciationRuleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "DepreciationRule";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("depreciationRuleId", pkValue);
            string sql = @"select * from DepreciationRule where depreciationRuleId=@depreciationRuleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "DepreciationRule";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["depreciationRuleNo"] = model.DepreciationRuleNo;
            dr["totalMonth"] = model.TotalMonth;
            dr["remainRate"] = model.RemainRate;
            dr["depreciationType"] = model.DepreciationType;
        }


        public override DataTable GetDropListSource()
        {
            string sql = @"select * from DepreciationRule  order by depreciationRuleNo";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["depreciationRuleId"]);
                dropList.Text = DataConvert.ToString(dr["depreciationRuleNo"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
