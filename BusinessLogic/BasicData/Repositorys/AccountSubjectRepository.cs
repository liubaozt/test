using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.AccountSubject;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AccountSubjectRepository : MasterRepository
    {

        public AccountSubjectRepository()
        {
            DefaulteGridSortField = "accountSubjectNo";
            MasterTable = "AccountSubject";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AccountSubjectNo) != "")
            {
                wcd.Sql += @" and AccountSubject.accountSubjectNo like '%'+@accountSubjectNo+'%'";
                wcd.DBPara.Add("accountSubjectNo", model.AccountSubjectNo);
            }
            if (DataConvert.ToString(model.AccountSubjectName) != "")
            {
                wcd.Sql += @" and AccountSubject.accountSubjectName like '%'+@accountSubjectName+'%'";
                wcd.DBPara.Add("accountSubjectName", model.AccountSubjectName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  AccountSubject.accountSubjectId accountSubjectId,
                                 AccountSubject.accountSubjectNo accountSubjectNo,
                                 AccountSubject.accountSubjectName accountSubjectName,
                                 AccountSubject.parentId parentId,
                                 U1.userName createId ,
                                 AccountSubject.createTime createTime ,
                                 U2.userName updateId ,
                                 AccountSubject.updateTime updateTime ,
                                 AccountSubject.updatePro updatePro
                          from AccountSubject left join AppUser U1 on AccountSubject.createId=U1.userId
                                       left join AppUser U2 on AccountSubject.updateId=U2.userId
                          where 1=1 ");
            return subViewSql;
        }




        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("accountSubjectId", primaryKey);
                string sql = @"select * from AccountSubject where accountSubjectId=@accountSubjectId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AccountSubjectId = primaryKey;
                model.AccountSubjectNo = DataConvert.ToString(dr["accountSubjectNo"]);
                model.AccountSubjectName = DataConvert.ToString(dr["accountSubjectName"]);
                model.ParentId = DataConvert.ToString(dr["parentId"]);     
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AccountSubject where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AccountSubject";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["accountSubjectId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("accountSubjectId", pkValue);
            string sql = @"select * from AccountSubject where accountSubjectId=@accountSubjectId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AccountSubject";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("accountSubjectId", pkValue);
            string sql = @"select * from AccountSubject where accountSubjectId=@accountSubjectId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AccountSubject";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["accountSubjectNo"] = model.AccountSubjectNo;
            dr["accountSubjectName"] = model.AccountSubjectName;
            dr["parentId"] = model.ParentId;
        }


        public DataTable GetAccountSubjectTree()
        {
            string sql = @"select accountSubjectId,parentId,accountSubjectName,1 isOpen ,'false' checked from AccountSubject ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from AccountSubject  order by accountSubjectName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["accountSubjectId"]);
                dropList.Text = DataConvert.ToString(dr["accountSubjectName"]);
                dropList.Filter = DataConvert.ToString(dr["parentId"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
