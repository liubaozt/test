using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.Unit;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class UnitRepository : MasterRepository
    {

        public UnitRepository()
        {
            DefaulteGridSortField = "unitNo";
            MasterTable = "Unit";
        }

        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.UnitNo) != "")
            {
                wcd.Sql += @" and Unit.unitNo like '%'+@unitNo+'%'";
                wcd.DBPara.Add("unitNo", model.UnitNo);
            }
            if (DataConvert.ToString(model.UnitName) != "")
            {
                wcd.Sql += @" and Unit.unitName like '%'+@unitName+'%'";
                wcd.DBPara.Add("unitName", model.UnitName);
            }
            if (DataConvert.ToString(model.UnitType) != "")
            {
                wcd.Sql += @" and Unit.unitType =@unitType ";
                wcd.DBPara.Add("unitType", model.UnitType);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  Unit.unitId unitId,
                                 Unit.unitNo unitNo,
                                 Unit.unitName unitName,
                                 CodeTable.codeName unitType,
                                 U1.userName createId ,
                                 Unit.createTime createTime ,
                                 U2.userName updateId ,
                                 Unit.updateTime updateTime ,
                                 Unit.updatePro updatePro
                          from Unit left join AppUser U1 on Unit.createId=U1.userId
                                    left join AppUser U2 on Unit.updateId=U2.userId
                                    left join CodeTable on (CodeTable.codeNo=Unit.unitType and CodeTable.codeType='{0}' and languageVer='{1}')
                          where 1=1 ",  "UnitType", AppMember.AppLanguage.ToString());
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("unitId", primaryKey);
                string sql = @"select * from Unit where unitId=@unitId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.UnitId = primaryKey;
                model.UnitNo = DataConvert.ToString(dr["unitNo"]);
                model.UnitName = DataConvert.ToString(dr["unitName"]);
                model.UnitType = DataConvert.ToString(dr["unitType"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Unit where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Unit";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["unitId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("unitId", pkValue);
            string sql = @"select * from Unit where unitId=@unitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Unit";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("unitId", pkValue);
            string sql = @"select * from Unit where unitId=@unitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Unit";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["unitNo"] = model.UnitNo;
            dr["unitName"] = model.UnitName;
            dr["unitType"] = model.UnitType;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from Unit  order by unitName ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }


        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["unitId"]);
                dropList.Text = DataConvert.ToString(dr["unitName"]);
                dropList.Filter = DataConvert.ToString(dr["unitType"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
