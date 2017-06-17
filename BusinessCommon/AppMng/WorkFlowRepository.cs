using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace BusinessCommon.AppMng
{
    public class WorkFlowRepository : MasterRepository
    {

        public WorkFlowRepository()
        {
            DefaulteGridSortField = "wfName";
        }

       

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("wfName") && DataConvert.ToString(paras["wfName"]) != "")
                whereSql += @" and AppWorkFlow.wfName like '%'+@workFlowName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AppWorkFlow.approveTable approveTable,
                                 AppWorkFlow.wfName wfName,
                                 (select CodeTable.codeName from CodeTable where AppWorkFlow.approveTable=CodeTable.codeNo and CodeTable.codeType='ApproveTable' and CodeTable.languageVer='{1}' ) approveTableName,
                                 AppWorkFlow.remark remark,
                                 U1.userName createId ,
                                 AppWorkFlow.createTime createTime ,
                                 U2.userName updateId ,
                                 AppWorkFlow.updateTime updateTime ,
                                 AppWorkFlow.updatePro updatePro
                          from AppWorkFlow left join AppUser U1 on AppWorkFlow.createId=U1.userId
                                       left join AppUser U2 on AppWorkFlow.updateId=U2.userId
                          where 1=1 {2}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AppWorkFlow  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetWorkFlow(string approveTable)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", approveTable);
            string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppWorkFlow where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppWorkFlow";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            string pk = DataConvert.ToString(objs["approveTable"]);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            string workFlowJson = objs["wfjson"].ToString();
            dbUpdate.Update(dt);
            SaveDetail(workFlowJson, pk);
            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", pkValue);
            string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppWorkFlow";
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dt.Rows[0][kv.Key] = kv.Value;
            }
            string workFlowJson = objs["wfjson"].ToString();
            SaveDetail(workFlowJson, pkValue);
            Update5Field(dt, sysUser.UserId, viewTitle);
            dbUpdate.Update(dt);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", pkValue);
            string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppWorkFlow";
            dt.Rows[0].Delete();
            string workFlowJson = objs["wfjson"].ToString();
            DeleteDetail(pkValue);
            DeletePath(pkValue);
            dbUpdate.Update(dt);
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
            dbUpdate.Update(dt);
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
            dbUpdate.Update(dt);
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
            return dbUpdate.Update(dt);
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
            return dbUpdate.Update(dt);
        }

    }
}
