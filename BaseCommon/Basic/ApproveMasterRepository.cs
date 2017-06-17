using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using log4net;
using System.Reflection;
using System.Data;
using System.Data.Common;

namespace BaseCommon.Basic
{
    public abstract class ApproveMasterRepository : MasterRepository, IApproveEntry
    {
        string approveLevel;
        string firstLevel;

     /// <summary>
     /// 
     /// </summary>
     /// <param name="tableName"></param>
     /// <param name="pkFiled"></param>
     /// <param name="refId"></param>
     /// <param name="viewTitle"></param>
     /// <param name="formMode"></param>
     /// <param name="createUser"></param>
     /// <returns>0:不需进行审批；1：需进行审批</returns>
        public virtual int InitFirstApproveTask(string tableName, string pkFiled, string refId, string viewTitle, string formMode = "", string createUser = "")
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", tableName);
            string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];

            if (dt.Rows.Count > 0)
            {
                if (formMode == "reapply")
                {
                    ApproveRepository rep = new ApproveRepository();
                    rep.DbUpdate = DbUpdate;
                    Dictionary<string, object> objs = new Dictionary<string, object>();
                    objs.Add("refId", refId);
                    objs.Add("tableName", tableName);
                    UserInfo sysUser = new UserInfo();
                    sysUser.UserId = createUser;
                    objs.Add("approver", createUser);
                    objs.Add("isValid", "Y");
                    objs.Add("approveState", "A");
                    rep.AddData(objs, sysUser, viewTitle, tableName, pkFiled, refId, "A", true);
                }


                paras.Clear();
                paras.Add("tableName", tableName);
                approveLevel = "1";
                sql = @"select AppWorkFlowPath.pathTo  from AppWorkFolwDetail,AppWorkFlowPath
                        where AppWorkFolwDetail.nodeType='start' and AppWorkFolwDetail.tableName=AppWorkFlowPath.tableName
                        and AppWorkFolwDetail.nodeId=AppWorkFlowPath.pathFrom
                        and AppWorkFolwDetail.tableName=@tableName ";
                DataTable dtStart = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                string node = DataConvert.ToString(dtStart.Rows[0]["pathTo"]);
                InitFirstApprove(node, tableName, pkFiled, refId, viewTitle);
            }
            else
            {
                return 0;
            }
            return 1;
        }

        private void InitFirstApprove(string nodeId, string tableName, string pkFiled, string refId, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            paras.Add("nodeId", nodeId);
            string sql = @"select * from AppWorkFolwDetail where tableName=@tableName and nodeId=@nodeId";
            DataTable dtNode = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            string nodeType = DataConvert.ToString(dtNode.Rows[0]["nodeType"]);
            if (nodeType != "task")
            {
                if (nodeType == "fork")
                {
                    approveLevel = "1.1";
                }
                sql = @"select AppWorkFlowPath.pathTo from AppWorkFolwDetail,AppWorkFlowPath
                            where  AppWorkFolwDetail.tableName=AppWorkFlowPath.tableName
                            and AppWorkFolwDetail.nodeId=AppWorkFlowPath.pathFrom
                            and AppWorkFolwDetail.tableName=@tableName
                            and AppWorkFolwDetail.nodeId=@nodeId";
                DataTable dtNodeTo = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                if (dtNodeTo.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNodeTo.Rows)
                    {
                        string node = DataConvert.ToString(dr["pathTo"]);
                        InitFirstApprove(node, tableName, pkFiled, refId, viewTitle);
                    }
                }
            }
            else
            {

                string departmentId = DataConvert.ToString(dtNode.Rows[0]["departmentId"]);
                string postId = DataConvert.ToString(dtNode.Rows[0]["postId"]);
                paras.Clear();
                paras.Add("departmentId", departmentId);
                paras.Add("postId", postId);
                sql = @"select * from AppUser where departmentId=@departmentId and postId=@postId";
                DataTable dtUser = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                string userId = DataConvert.ToString(dtUser.Rows[0]["userId"]);
                ApproveRepository rep = new ApproveRepository();
                rep.DbUpdate = DbUpdate;
                Dictionary<string, object> objs = new Dictionary<string, object>();
                objs.Add("refId", refId);
                objs.Add("tableName", tableName);
                UserInfo sysUser = new UserInfo();
                sysUser.UserId = userId;
                objs.Add("approver", userId);
                objs.Add("approveNode", nodeId);
                objs.Add("approveLevel", approveLevel);
                objs.Add("isValid", "Y");
                objs.Add("approveState", "O");
                rep.AddData(objs, sysUser, viewTitle, tableName, pkFiled, refId, "O", true);
            }
        }

        protected virtual int DeleteApproveData(string tableName, string refId, string userId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            paras.Add("refId", refId);
            paras.Add("approver", userId);
            string sql = @"select * from AppApprove where tableName=@tableName and refId=@refId and approver=@approver";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AppApprove";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            DbUpdate.Update(dt);
            return 1;
        }

        public virtual int InitNextApproveTask(string tableName, string nodeId, string currentApproveLevel, string currentApprover, string pkFiled, string refId, string viewTitle, ref bool isEndNode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("approveTable", tableName);
            string sql = @"select * from AppWorkFlow where approveTable=@approveTable";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (currentApproveLevel.Contains("."))
                {
                    string[] levels = currentApproveLevel.Split('.');
                    string lindex = DataConvert.ToString(DataConvert.ToInt32(levels[levels.Length - 1]) + 1);
                    string lsindex = DataConvert.ToString(DataConvert.ToInt32(levels[levels.Length - 2]) + 1);
                    for (int i = 0; i < levels.Length - 1; i++)
                    {
                        if (i < levels.Length - 2)
                            firstLevel = levels[i] + ".";
                        approveLevel = levels[i] + ".";
                    }
                    approveLevel += lindex;
                    firstLevel += lsindex;
                }
                else
                {
                    approveLevel = DataConvert.ToString(DataConvert.ToInt32(currentApproveLevel) + 1);
                    firstLevel = approveLevel;
                }
                paras.Clear();
                paras.Add("tableName", tableName);
                paras.Add("pathFrom", nodeId);
                sql = @"select pathTo  from AppWorkFlowPath
                        where pathFrom=@pathFrom and tableName=@tableName ";
                DataTable dtStart = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                string node = DataConvert.ToString(dtStart.Rows[0]["pathTo"]);
                InitNextApprove(node, currentApproveLevel, currentApprover, tableName, pkFiled, refId, viewTitle, ref isEndNode);
            }
            return 1;
        }

        private void InitNextApprove(string nodeId, string currentApproveLevel, string currentApprover, string tableName, string pkFiled, string refId, string viewTitle, ref bool isEndNode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            paras.Add("nodeId", nodeId);
            paras.Add("refId", refId);
            string sql = @"select * from AppWorkFolwDetail where tableName=@tableName and nodeId=@nodeId";
            DataTable dtNode = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            string nodeType = DataConvert.ToString(dtNode.Rows[0]["nodeType"]);
            if (nodeType == "task")
            {
                string departmentId = DataConvert.ToString(dtNode.Rows[0]["departmentId"]);
                string postId = DataConvert.ToString(dtNode.Rows[0]["postId"]);
                paras.Clear();
                paras.Add("departmentId", departmentId);
                paras.Add("postId", postId);
                sql = @"select * from AppUser where departmentId=@departmentId and postId=@postId";
                DataTable dtUser = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                string userId = DataConvert.ToString(dtUser.Rows[0]["userId"]);
                ApproveRepository rep = new ApproveRepository();
                rep.DbUpdate = DbUpdate;
                Dictionary<string, object> objs = new Dictionary<string, object>();
                objs.Add("refId", refId);
                objs.Add("tableName", tableName);
                UserInfo sysUser = new UserInfo();
                sysUser.UserId = userId;
                objs.Add("approver", userId);
                objs.Add("preApprover", currentApprover);
                objs.Add("approveNode", nodeId);
                objs.Add("approveLevel", approveLevel);
                objs.Add("isValid", "Y");
                objs.Add("approveState", "O");
                rep.AddData(objs, sysUser, viewTitle, tableName, pkFiled, refId, "O", true);
            }
            else
            {
                if (nodeType == "fork")
                {
                    approveLevel += ".1";
                }
                else if (nodeType == "join")
                {
                    sql = @"select AppApprove.*  from AppWorkFlowPath,AppApprove
                            where AppApprove.tableName=@tableName and AppApprove.refId=@refId 
                            and AppWorkFlowPath.pathTo=@nodeId 
                            and AppWorkFlowPath.tableName=AppApprove.tableName
                            and AppWorkFlowPath.pathFrom=AppApprove.approveNode
                            and AppApprove.approveState<>'P'
                            and AppApprove.approveState<>'A'
                            and AppApprove.isValid='Y' ";
                    DataTable dtNotApprove = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                    if (dtNotApprove.Rows.Count - 1 > 0)
                        return;
                    else
                    {
                        approveLevel = firstLevel;
                    }
                }
                else if (nodeType == "end")
                {
                    isEndNode = true;
                }
                sql = @"select AppWorkFlowPath.pathTo from AppWorkFolwDetail,AppWorkFlowPath
                            where  AppWorkFolwDetail.tableName=AppWorkFlowPath.tableName
                            and AppWorkFolwDetail.nodeId=AppWorkFlowPath.pathFrom
                            and AppWorkFolwDetail.tableName=@tableName
                            and AppWorkFolwDetail.nodeId=@nodeId";
                DataTable dtNodeTo = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                if (dtNodeTo.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNodeTo.Rows)
                    {
                        string node = DataConvert.ToString(dr["pathTo"]);
                        InitNextApprove(node, currentApproveLevel, currentApprover, tableName, pkFiled, refId, viewTitle, ref isEndNode);
                    }
                }
            }
        }

        /// <summary>
        /// 审批完成时处理
        /// </summary>
        /// <param name="approvePkValue"></param>
        /// <param name="sysUser"></param>
        /// <param name="viewTitle"></param>
        /// <returns></returns>
        public virtual int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            return 1;
        }

        protected virtual int UpdateAssetsState(string assetsId,string assetsState, string sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            dtAssets.Rows[0]["assetsState"] = assetsState;
            Update5Field(dtAssets, sysUser, viewTitle);
            return DbUpdate.Update(dtAssets);
        }


        #region IApproveEntry 成员

        public virtual DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey)
        {
            throw new NotImplementedException();
        }

        public virtual DataRow GetModel(string primaryKey)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
