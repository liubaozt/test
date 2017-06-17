using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.ProjectManage;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Repositorys
{
    public class ProjectManageRepository : MasterRepository
    {

        public ProjectManageRepository()
        {
            DefaulteGridSortField = "projectManageNo";
            MasterTable = "ProjectManage";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.ProjectManageNo) != "")
            {
                wcd.Sql += @" and ProjectManage.projectManageNo like '%'+@projectManageNo+'%'";
                wcd.DBPara.Add("projectManageNo", model.ProjectManageNo);
            }
            if (DataConvert.ToString(model.ProjectManageName) != "")
            {
                wcd.Sql += @" and ProjectManage.projectManageName like '%'+@projectManageName+'%'";
                wcd.DBPara.Add("projectManageName", model.ProjectManageName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  ProjectManage.projectManageId projectManageId,
                                 ProjectManage.projectManageNo projectManageNo,
                                 ProjectManage.projectManageName projectManageName,
                                 U1.userName createId ,
                                 ProjectManage.createTime createTime ,
                                 U2.userName updateId ,
                                 ProjectManage.updateTime updateTime ,
                                 ProjectManage.updatePro updatePro
                          from ProjectManage left join AppUser U1 on ProjectManage.createId=U1.userId
                                    left join AppUser U2 on ProjectManage.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("projectManageId", primaryKey);
                string sql = @"select * from ProjectManage where projectManageId=@projectManageId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.ProjectManageId = primaryKey;
                model.ProjectManageNo = DataConvert.ToString(dr["projectManageNo"]);
                model.ProjectManageName = DataConvert.ToString(dr["projectManageName"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from ProjectManage where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "ProjectManage";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["projectManageId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("projectManageId", pkValue);
            string sql = @"select * from ProjectManage where projectManageId=@projectManageId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "ProjectManage";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("projectManageId", pkValue);
            string sql = @"select * from ProjectManage where projectManageId=@projectManageId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "ProjectManage";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["projectManageNo"] = model.ProjectManageNo;
            dr["projectManageName"] = model.ProjectManageName;
        }


        public override DataTable GetDropListSource()
        {
            string sql = @"select * from ProjectManage  order by projectManageName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["projectManageId"]);
                dropList.Text = DataConvert.ToString(dr["projectManageName"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
