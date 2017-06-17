using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using System.Reflection;
using System.Data;
using System.Data.Common;
using BaseCommon.Basic;

namespace BaseCommon.Repositorys
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
                InitFirstApprove(node, tableName, pkFiled, refId, viewTitle,createUser);
            }
            else
            {
                return 0;
            }
            return 1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="tableName"></param>
        /// <param name="pkFiled"></param>
        /// <param name="refId"></param>
        /// <param name="viewTitle"></param>
        private void InitFirstApprove(string nodeId, string tableName, string pkFiled, string refId, string viewTitle, string createUser)
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
                sql = @"select AppWorkFlowPath.pathTo,AppWorkFlowPath.pathConditon from AppWorkFolwDetail,AppWorkFlowPath
                            where  AppWorkFolwDetail.tableName=AppWorkFlowPath.tableName
                            and AppWorkFolwDetail.nodeId=AppWorkFlowPath.pathFrom
                            and AppWorkFolwDetail.tableName=@tableName
                            and AppWorkFolwDetail.nodeId=@nodeId";
                DataTable dtNodeTo = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                if (dtNodeTo.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNodeTo.Rows)
                    {
                        if (nodeType == "fork")
                        {
                            string pathConditon = DataConvert.ToString(dr["pathConditon"]);
                            if (PassCondition(tableName, pkFiled, refId, pathConditon))
                            {
                                string node = DataConvert.ToString(dr["pathTo"]);
                                InitFirstApprove(node, tableName, pkFiled, refId, viewTitle,createUser);
                                break;
                            }
                        }
                        else
                        {
                            string node = DataConvert.ToString(dr["pathTo"]);
                            InitFirstApprove(node, tableName, pkFiled, refId, viewTitle,createUser);
                            break;
                        }
                    }
                }
            }
            else
            {

                string groupId = DataConvert.ToString(dtNode.Rows[0]["departmentId"]);
                string approveRange = DataConvert.ToString(dtNode.Rows[0]["postId"]); 
                //paras.Clear();
                //paras.Add("departmentId", departmentId);
                ////paras.Add("postId", postId);
                //if(approveRange=="2")
                //    sql = @"select * from AppUser where groupId=@departmentId and hasApproveAuthority='Y' ";
                //else
                //    sql = @"select * from AppUser where groupId=@departmentId and hasApproveAuthority='Y' ";
                //DataTable dtUser = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                DataTable dtUser = GetApprover(tableName, pkFiled, refId, groupId, approveRange,createUser);
                foreach (DataRow dr in dtUser.Rows)
                {
                    string userId = DataConvert.ToString(dr["userId"]);
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
        }

        private string GetGroupIdWhereInSql(string groupId)
        {
            var groupIds = groupId.Split('[');
            string groupIdWhereIn = "";
            string groupIdArea = "";
            if (groupIds.Length > 1)
                groupIdArea = groupIds[1].Substring(0, groupIds[1].Length - 1);
            else
                groupIdArea = groupIds[0].Substring(0, groupIds[0].Length - 1);
            var inGroupIds = groupIdArea.Split(',');
            foreach (var id in inGroupIds)
            {
                groupIdWhereIn += "'" + id + "',";
            }
            groupIdWhereIn = groupIdWhereIn.Substring(0, groupIdWhereIn.Length - 1);
            return groupIdWhereIn;
        }

        /// <summary>
        /// 获取审批人，如果审批范围为1（所有），则找到审批角色下的所有人，如果审批范围为2（自己部门），则找到申请人所在部门下的这个角色的审批人。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pkFiled"></param>
        /// <param name="refId"></param>
        /// <param name="groupId"></param>
        /// <param name="approveRange"></param>
        /// <param name="createUser">first时需转入，next时根据主键可以查createid</param>
        /// <returns></returns>
        private DataTable GetApprover(string tableName, string pkFiled, string refId, string groupId, string approveRange,string createUser="")
        {
            string sql = "";
            if (approveRange == "2")
            {
                string sql2 ="";
                if(createUser=="")
                    sql2 = string.Format(@"select AppUser.departmentId from {0} a,AppUser where a.{1}='{2}' and a.createId=AppUser.userId", tableName, pkFiled, refId);
                else
                    sql2 = string.Format(@"select departmentId from AppUser where userId='{0}'",createUser);
                DataTable dtCreateUserDeparment = AppMember.DbHelper.GetDataSet(sql2, DbUpdate.cmd).Tables[0];
                if (dtCreateUserDeparment.Rows.Count > 0)
                {
                    string departmentIds =DataConvert.ToString( dtCreateUserDeparment.Rows[0]["departmentId"]);
                    string[] ids = departmentIds.Split(',');
                    if(ids.Length<1)
                        throw new Exception(AppMember.AppText["ApplyerHasnotDepartment"]);
                    string likes = " and (";
                    foreach (string id in ids)
                    {
                        if (DataConvert.ToString(id).StartsWith("CMY"))
                            continue;
                        likes += string.Format(" departmentId like '%{0}%' or", DataConvert.ToString(id));
                    }
                    likes =likes.Substring(0,likes.Length-2)+ ")";
                    sql = @"select * from AppUser where groupId in ({0}) and hasApproveAuthority='Y' "+likes;
                }
            }
            else
                sql = @"select * from AppUser where groupId in ({0}) and hasApproveAuthority='Y' ";
            string groupIdWhereIn = GetGroupIdWhereInSql(groupId);
            sql = string.Format(sql, groupIdWhereIn);
            DataTable dtUser = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            return dtUser;
        }

        private bool PassCondition(string tableName, string pkFiled, string refId, string pathConditon)
        {
            pathConditon=DataConvert.ToString(pathConditon);
            if (pathConditon.StartsWith(">=") || pathConditon.StartsWith(">") ||
                pathConditon.StartsWith("<=") || pathConditon.StartsWith("<") ||
                pathConditon.StartsWith("="))
            {
                string sql = "";
                if (DataConvert.ToString(tableName).ToLower() == "AssetsPurchase".ToLower())//资产采购
                {
                    sql = string.Format(@"select count(1) cnt from AssetsPurchaseDetail
                        where assetsPurchaseId ='{0}' 
                        and assetsValue{1}", DataConvert.ToString(refId), DataConvert.ToString(pathConditon));
                }
                else
                {
                    sql = string.Format(@"select count(1) cnt from {0} s,{0}Detail d,Assets a
                        where s.{1} =d.{1} 
                        and d.assetsId=a.assetsId
                        and s.{1}='{2}'
                        and a.assetsValue{3}", DataConvert.ToString(tableName), DataConvert.ToString(pkFiled),
                                                    DataConvert.ToString(refId), DataConvert.ToString(pathConditon));
                }
                DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
                if (DataConvert.ToInt32(dt.Rows[0]["cnt"]) > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
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

        protected virtual int DeleteBatchApproveData(string tableName, string refId, string userId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            //paras.Add("refId", refId);
            paras.Add("approver", userId);
            string sql = @"select * from AppApprove where tableName=@tableName and refId in (" + refId + ") and approver=@approver";
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
                string groupId = DataConvert.ToString(dtNode.Rows[0]["departmentId"]);
                string approveRange = DataConvert.ToString(dtNode.Rows[0]["postId"]);
                //paras.Clear();
                //paras.Add("departmentId", departmentId);
                ////paras.Add("postId", postId);
                //sql = @"select * from AppUser where groupId=@departmentId and hasApproveAuthority='Y'";
                //DataTable dtUser = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                DataTable dtUser = GetApprover(tableName, pkFiled, refId, groupId, approveRange);
                foreach (DataRow dr in dtUser.Rows)
                {
                    string userId = DataConvert.ToString(dr["userId"]);
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
                sql = @"select AppWorkFlowPath.pathTo,AppWorkFlowPath.pathConditon from AppWorkFolwDetail,AppWorkFlowPath
                            where  AppWorkFolwDetail.tableName=AppWorkFlowPath.tableName
                            and AppWorkFolwDetail.nodeId=AppWorkFlowPath.pathFrom
                            and AppWorkFolwDetail.tableName=@tableName
                            and AppWorkFolwDetail.nodeId=@nodeId";
                DataTable dtNodeTo = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                if (dtNodeTo.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNodeTo.Rows)
                    {
                        if (nodeType == "fork")
                        {
                            string pathConditon = DataConvert.ToString(dr["pathConditon"]);
                            if (PassCondition(tableName, pkFiled, refId, pathConditon))
                            {
                                string node = DataConvert.ToString(dr["pathTo"]);
                                InitNextApprove(node, currentApproveLevel, currentApprover, tableName, pkFiled, refId, viewTitle, ref isEndNode);
                                break;
                            }
                        }
                        else
                        {
                            string node = DataConvert.ToString(dr["pathTo"]);
                            InitNextApprove(node, currentApproveLevel, currentApprover, tableName, pkFiled, refId, viewTitle, ref isEndNode);
                            break;
                        }
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

        protected virtual int UpdateAssetsState(string assetsId, string assetsState, string sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            dtAssets.Rows[0]["assetsState"] = assetsState;
            if (assetsState == "S" || assetsState == "M" || assetsState == "L")
            {
                dtAssets.Rows[0]["smDate"] = IdGenerator.GetServerDate();
            }
            Update5Field(dtAssets, sysUser, viewTitle);
            return DbUpdate.Update(dtAssets);
        }

        protected virtual int UpdateAssetsState(List<string> assetsIdList, string assetsState, string sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            string sql = @"select * from Assets where assetsId in (";
            if (assetsIdList.Count > 2100)
                throw new Exception();
            for (int i = 0; i < assetsIdList.Count; i++)
            {
                sql += "@assetsId" + i.ToString() + ",";
                paras.Add("assetsId" + i.ToString(), assetsIdList[i]);
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql += ")";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            DateTime smDate=IdGenerator.GetServerDate();
            foreach (DataRow dr in dtAssets.Rows)
            {
                dr["assetsState"] = assetsState;
                if (assetsState == "S" || assetsState == "M" || assetsState == "L")
                {
                    dr["smDate"] = smDate;
                }
            }
            Update5Field(dtAssets, sysUser, viewTitle);
            return DbUpdate.Update(dtAssets);
        }

        

        #region IApproveEntry 成员

        public virtual DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey, string formVar)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
