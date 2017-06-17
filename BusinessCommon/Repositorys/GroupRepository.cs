using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BusinessCommon.Models.Group;
using BaseCommon.Repositorys;
using BaseCommon.Models;

namespace BusinessCommon.Repositorys
{
    public class GroupRepository : MasterRepository
    {

        public GroupRepository()
        {
            DefaulteGridSortField = "groupNo";
            MasterTable = "AppGroup";
        }

        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.GroupNo) != "")
            {
                wcd.Sql += @" and AppGroup.groupNo like '%'+@groupNo+'%'";
                wcd.DBPara.Add("groupNo", model.GroupNo);
            }
            if (DataConvert.ToString(model.GroupName) != "")
            {
                wcd.Sql += @" and AppGroup.groupName like '%'+@groupName+'%'";
                wcd.DBPara.Add("groupName", model.GroupName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  AppGroup.groupId groupId,
                                 AppGroup.groupNo groupNo,
                                 AppGroup.groupName groupName,
                                 AppGroup.remark remark,
                                 (select CodeTable.codeName from CodeTable where AppGroup.isFixed=CodeTable.codeNo and CodeTable.codeType='BoolVal' and CodeTable.languageVer='{0}' ) isFixed,
                                 U1.userName createId ,
                                 AppGroup.createTime createTime ,
                                 U2.userName updateId ,
                                 AppGroup.updateTime updateTime ,
                                 AppGroup.updatePro updatePro
                          from AppGroup left join AppUser U1 on AppGroup.createId=U1.userId
                                       left join AppUser U2 on AppGroup.updateId=U2.userId
                          where 1=1 ", AppMember.AppLanguage.ToString());
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("groupId", primaryKey);
                string sql = @"select * from AppGroup where groupId=@groupId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.GroupId = primaryKey;
                model.GroupNo = DataConvert.ToString(dr["groupNo"]);
                model.GroupName = DataConvert.ToString(dr["groupName"]);
                model.Remark = DataConvert.ToString(dr["remark"]);
                model.IsFixed = DataConvert.ToString(dr["isFixed"]); 
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppGroup where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppGroup";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["groupId"] = IdGenerator.GetMaxId(dt.TableName);
            dr["isFixed"] = "N";
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("groupId", pkValue);
            string sql = @"select * from AppGroup where groupId=@groupId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppGroup";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("groupId", pkValue);
            string sql = @"select * from AppGroup where groupId=@groupId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppGroup";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model,DataRow dr)
        {
            dr["groupNo"] = model.GroupNo;
            dr["groupName"] = model.GroupName;
            dr["remark"] = model.Remark;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from AppGroup  order by isFixed desc,groupNo ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["groupId"]);
                dropList.Text = DataConvert.ToString(dr["groupName"]);
                list.Add(dropList);
            }
            return list;
        }

        public DataTable GetGroupTree(UserInfo sysUser)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            string sql = @"select groupId,0 parentId,groupName, 1 isOpen ,'false' checked from AppGroup where 1=1  ";
            sql += " order by  groupName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dt;
        }

    }
}
