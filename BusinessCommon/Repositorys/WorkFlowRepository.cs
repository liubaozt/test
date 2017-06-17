using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.Common;
using BaseCommon.Repositorys;
using BusinessCommon.Models.WorkFlow;
using BaseCommon.Models;

namespace BusinessCommon.Repositorys
{
    public class WorkFlowRepository : MasterRepository
    {

        public WorkFlowRepository()
        {
            DefaulteGridSortField = "wfName";
            MasterTable = "AppWorkFlow";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.WfName) != "")
            {
                wcd.Sql += @" and AppWorkFlow.wfName like '%'+@workFlowName+'%'";
                wcd.DBPara.Add("wfName", model.WfName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select AppWorkFlow.approveTable approveTable,
                                 AppWorkFlow.wfName wfName,
                                 (select CodeTable.codeName from CodeTable where AppWorkFlow.approveTable=CodeTable.codeNo and CodeTable.codeType='ApproveTable' and CodeTable.languageVer='{0}' ) approveTableName,
                                 AppWorkFlow.remark remark,
                                 U1.userName createId ,
                                 AppWorkFlow.createTime createTime ,
                                 U2.userName updateId ,
                                 AppWorkFlow.updateTime updateTime ,
                                 AppWorkFlow.updatePro updatePro
                          from AppWorkFlow left join AppUser U1 on AppWorkFlow.createId=U1.userId
                                       left join AppUser U2 on AppWorkFlow.updateId=U2.userId
                          where 1=1 " ,AppMember.AppLanguage.ToString());
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("approveTable", primaryKey);
                string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.ApproveTable = primaryKey;
                model.WfName = DataConvert.ToString(dr["wfName"]);
                model.Remark = DataConvert.ToString(dr["remark"]);
                model.WorkFlowJson = DataConvert.ToString(dr["wfjson"]);
            }
            else
            {
                model.WorkFlowJson = "1";
            }
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppWorkFlow where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppWorkFlow";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            dr["wfName"] = myModel.WfName;
            dr["approveTable"] = myModel.ApproveTable;
            dr["remark"] = myModel.Remark;
            dr["wfjson"] = myModel.WorkFlowJson;
            string pk = DataConvert.ToString(myModel.ApproveTable);
            dt.Rows.Add(dr);
            string workFlowJson = myModel.WorkFlowJson.ToString();
            SaveDetail(workFlowJson, pk);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
        
            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", pkValue);
            string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppWorkFlow";
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["wfName"] = myModel.WfName;
            dt.Rows[0]["approveTable"] = myModel.ApproveTable;
            dt.Rows[0]["remark"] = myModel.Remark;
            dt.Rows[0]["wfjson"] = myModel.WorkFlowJson;
            string workFlowJson = myModel.WorkFlowJson.ToString();
            SaveDetail(workFlowJson, pkValue);
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", pkValue);
            string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppWorkFlow";
            dt.Rows[0].Delete();
            DeleteDetail(pkValue);
            DeletePath(pkValue);
            DbUpdate.Update(dt);
            return 1;
        }

        protected int SaveDetail(string workFlowJson, string pkValue)
        {
            DeleteDetail(pkValue);
            DeletePath(pkValue);
            string[] sArray = Regex.Split(workFlowJson, "}}}},", RegexOptions.IgnoreCase);
            string[] statesArray = Regex.Split(sArray[0], "}}},", RegexOptions.IgnoreCase);
            string[] pathsArray = Regex.Split(sArray[1], "}}},", RegexOptions.IgnoreCase);

            foreach (string states in statesArray)
            {
                string[] states_propsArray = Regex.Split(states, "props:{", RegexOptions.IgnoreCase);
                string[] states1Array = Regex.Split(states_propsArray[0], ",text:{text:", RegexOptions.IgnoreCase);
                string[] states11Array = Regex.Split(states1Array[0], ":{type:'", RegexOptions.IgnoreCase);
                string nodeId = states11Array[0].Replace("{states:{", "");
                string nodeType = states11Array[1].Replace("'", "");

                string[] states2Array = Regex.Split(states_propsArray[1], "'},assignee:{value:'", RegexOptions.IgnoreCase);
                string[] states21Array = Regex.Split(states2Array[0], "'},department:{value:'", RegexOptions.IgnoreCase);
                string nodeText = states21Array[0].Replace("text:{value:'", "");
                nodeText = nodeText.Replace("'", "");
                string departmentId = "";
                if (states21Array.Length > 1)
                    departmentId = states21Array[1].Replace("'", "");
                string nodePost = "";
                if (states2Array.Length > 1)
                    nodePost = states2Array[1].Replace("'", "");
                AddDetail(pkValue, nodeId, nodeType, nodeText, departmentId, nodePost);
            }

            foreach (string paths in pathsArray)
            {
                string[] paths_textArray = Regex.Split(paths, "text:{text:", RegexOptions.IgnoreCase);
                string[] paths1Array = Regex.Split(paths_textArray[0], ",to:", RegexOptions.IgnoreCase);
                string[] paths11Array = Regex.Split(paths1Array[0], ":{from:", RegexOptions.IgnoreCase);
                if (paths1Array.Length < 2 || paths11Array.Length < 1)
                    continue;
                string pathId = paths11Array[0].Replace("paths:{", "");
                string pathFrom = paths11Array[1].Replace("paths:{", "");
                pathFrom = pathFrom.Replace("'", "");
                string[] paths12Array = Regex.Split(paths1Array[1], ", dots:", RegexOptions.IgnoreCase);
                string pathTo = paths12Array[0].Replace("'", "");

                string[] paths2Array = Regex.Split(paths_textArray[1], "},textPos:", RegexOptions.IgnoreCase);
                string pathText = paths2Array[0].Replace("'", "");
                AddPath(pkValue, pathId, pathText, pathFrom, pathTo);
            }
            return 1;
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", pkValue);
            string sql = @"select * from AppWorkFolwDetail where tableName=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppWorkFolwDetail";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            DbUpdate.Update(dt);
            return 1;
        }

        protected int DeletePath(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", pkValue);
            string sql = @"select * from AppWorkFlowPath where tableName=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppWorkFlowPath";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            DbUpdate.Update(dt);
            return 1;
        }

        protected int AddDetail(string pkValue, string nodeId, string nodeType, string nodeText, string departmentId, string nodePost)
        {
            string sql = @"select * from AppWorkFolwDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppWorkFolwDetail";
            DataRow dr = dt.NewRow();
            dr["tableName"] = pkValue;
            dr["nodeId"] = nodeId;
            dr["nodeType"] = nodeType;
            dr["nodeText"] = nodeText;
            dr["departmentId"] = departmentId;
            dr["postId"] = nodePost;
            dt.Rows.Add(dr);
            return DbUpdate.Update(dt);
        }

        protected int AddPath(string pkValue, string pathId, string pathText, string pathFrom, string pathTo)
        {
            string sql = @"select * from AppWorkFlowPath where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppWorkFlowPath";
            DataRow dr = dt.NewRow();
            dr["tableName"] = pkValue;
            dr["pathId"] = pathId;
            dr["pathConditon"] = pathText;
            dr["pathFrom"] = pathFrom;
            dr["pathTo"] = pathTo;
            dt.Rows.Add(dr);
            return DbUpdate.Update(dt);
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from AppWorkFlow  order by wfName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["approveTable"]);
                dropList.Text = DataConvert.ToString(dr["wfName"]);
                list.Add(dropList);
            }
            return list;
        }

        public bool HasApprovingFlow(EntryModel model)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", model.ApproveTable);
            string sql = @"select count(1) cnt from appapprove where tablename=@approveTable and isvalid='Y' and approvestate in('O')";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (DataConvert.ToInt32(dt.Rows[0]["cnt"]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }          
        }

        public bool HasWorkFlowForTableName(EntryModel model)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", model.ApproveTable);
            string sql = @"select count(1) cnt from AppWorkFlow where approveTable=@approveTable ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (DataConvert.ToInt32(dt.Rows[0]["cnt"]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
