using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.EquityOwner;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class EquityOwnerRepository : MasterRepository
    {

        public EquityOwnerRepository()
        {
            DefaulteGridSortField = "equityOwnerNo";
            MasterTable = "EquityOwner";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.EquityOwnerNo) != "")
            {
                wcd.Sql += @" and EquityOwner.equityOwnerNo like '%'+@equityOwnerNo+'%'";
                wcd.DBPara.Add("equityOwnerNo", model.EquityOwnerNo);
            }
            if (DataConvert.ToString(model.EquityOwnerName) != "")
            {
                wcd.Sql += @" and EquityOwner.equityOwnerName like '%'+@equityOwnerName+'%'";
                wcd.DBPara.Add("equityOwnerName", model.EquityOwnerName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  EquityOwner.equityOwnerId equityOwnerId,
                                 EquityOwner.equityOwnerNo equityOwnerNo,
                                 EquityOwner.equityOwnerName equityOwnerName,
                                 U1.userName createId ,
                                 EquityOwner.createTime createTime ,
                                 U2.userName updateId ,
                                 EquityOwner.updateTime updateTime ,
                                 EquityOwner.updatePro updatePro
                          from EquityOwner left join AppUser U1 on EquityOwner.createId=U1.userId
                                    left join AppUser U2 on EquityOwner.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("equityOwnerId", primaryKey);
                string sql = @"select * from EquityOwner where equityOwnerId=@equityOwnerId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.EquityOwnerId = primaryKey;
                model.EquityOwnerNo = DataConvert.ToString(dr["equityOwnerNo"]);
                model.EquityOwnerName = DataConvert.ToString(dr["equityOwnerName"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from EquityOwner where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "EquityOwner";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["equityOwnerId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("equityOwnerId", pkValue);
            string sql = @"select * from EquityOwner where equityOwnerId=@equityOwnerId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "EquityOwner";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("equityOwnerId", pkValue);
            string sql = @"select * from EquityOwner where equityOwnerId=@equityOwnerId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "EquityOwner";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["equityOwnerNo"] = model.EquityOwnerNo;
            dr["equityOwnerName"] = model.EquityOwnerName;
        }


        public override DataTable GetDropListSource()
        {
            string sql = @"select * from EquityOwner  order by equityOwnerName ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["equityOwnerId"]);
                dropList.Text = DataConvert.ToString(dr["equityOwnerName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
