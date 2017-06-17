using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Data.Common;

namespace BaseCommon.Repositorys
{
    public class ApproveRepository : BaseRepository
    {

        public ApproveRepository()
        {

        }

        public DataTable GetGridDataTable(Dictionary<string, object> paras)
        {
            string sql = string.Format(@" select tableName,refId,
                    (select AppUser.userName from AppUser where AppUser.userId=AppApprove.approver) approver,
                    seqno,
                    (select CodeTable.codeName from CodeTable where AppApprove.approveState=CodeTable.codeNo and CodeTable.codeType='ApproveState' and CodeTable.languageVer='{0}' ) approveState ,
                    approveMind,approveLevel,approveTime  
                    from AppApprove 
                    where tableName=@tableName and refId=@refId and AppApprove.approveState<>'O'
                    order by AppApprove.seqno , AppApprove.approveLevel,AppApprove.approveState ", AppMember.AppLanguage.ToString());
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public DataRow GetApproveData(string tableName, string pkValue, string userId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            paras.Add("refId", pkValue);
            paras.Add("approver", userId);
            string sql = string.Format(@" select tableName,refId,approver,approveState,approveMind,approveLevel,approveNode  from AppApprove 
            where tableName=@tableName and refId=@refId  and approver=@approver  and isValid='Y' ");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle, string tableName, string pkField, string pkValue, string approveState, bool isInit = false)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            paras.Add("refId", pkValue);
            paras.Add("approver", sysUser.UserId);
            string sql = @"select isnull(max(seqno),0) seqno  from AppApprove where tableName=@tableName and refId=@refId and approver=@approver ";
            DataTable dtSeqno = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            int seqno = DataConvert.ToInt32(dtSeqno.Rows[0]["seqno"]);
            sql = @"select * from AppApprove where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AppApprove";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            if (approveState == "A" && seqno == 0)
                dr["seqno"] = seqno + 2;
            else
                dr["seqno"] = seqno + 1;
            //dr["approveId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            if (!isInit)
                OverrideRefTable(tableName, pkField, pkValue, sysUser, viewTitle, approveState);
            return 1;
        }

        /// <summary>
        /// 更新审批状态，如果为R(退回)，将这条工作流的历史审批信息设为无效（isValid='N'）
        /// 如果为非R,同一角色下有一人审批过，将其他人的审批信息设为无效（isValid='N'）
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="viewTitle"></param>
        /// <param name="tableName"></param>
        /// <param name="pkField"></param>
        /// <param name="pkValue"></param>
        /// <param name="approveState"></param>
        /// <param name="approveMind"></param>
        /// <param name="approveNode"></param>
        /// <param name="isEndNode"></param>
        /// <returns></returns>
        public int EditData(UserInfo sysUser, string viewTitle, string tableName, string pkField, string pkValue, string approveState, string approveMind, string approveNode, bool isEndNode = false)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();

            string sql = "";
            if (approveState == "R")
            {
                paras.Add("tableName", tableName);
                paras.Add("refId", pkValue);
                sql = @"select * from AppApprove where tableName=@tableName and refId=@refId ";
            }
            else
            {
                paras.Add("tableName", tableName);
                paras.Add("refId", pkValue);
                paras.Add("approver", sysUser.UserId);
                sql = @"select * from AppApprove where tableName=@tableName and refId=@refId and approver=@approver and isValid='Y'";
            }
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AppApprove";
            foreach (DataRow dr in dt.Rows)
            {
                if (DataConvert.ToString(dr["approver"]) == sysUser.UserId && DataConvert.ToString(dr["isValid"]) == "Y")
                {
                    dr["approveMind"] = approveMind;
                    dr["approveTime"] = DateTime.Now;
                    if (approveState == "R")
                        dr["approveState"] = "R";
                    else
                        dr["approveState"] = "P";
                }
                if (approveState == "R")
                    dr["isValid"] = "N";
                else
                    dr["isValid"] = "Y";

            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            if (approveState != "R")
                UpdateSameNodeOtherApprove(sysUser, viewTitle, tableName, pkField, pkValue, approveState, approveMind, approveNode);
            OverrideRefTable(tableName, pkField, pkValue, sysUser, viewTitle, approveState, isEndNode);
            return 1;
        }

        /// <summary>
        /// 同一个节点，如果有多个审批人，有一个人审批了，其他人的审批信息作废，设置isValid='N'
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="viewTitle"></param>
        /// <param name="tableName"></param>
        /// <param name="pkField"></param>
        /// <param name="pkValue"></param>
        /// <param name="approveState"></param>
        /// <param name="approveMind"></param>
        /// <param name="approveNode"></param>
        /// <returns></returns>
        private int UpdateSameNodeOtherApprove(UserInfo sysUser, string viewTitle, string tableName, string pkField, string pkValue, string approveState, string approveMind, string approveNode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            paras.Add("refId", pkValue);
            paras.Add("approveNode", approveNode);
            paras.Add("approver", sysUser.UserId);
            string sql = @"select * from AppApprove where tableName=@tableName and refId=@refId and approver<>@approver and approveNode=@approveNode and isValid='Y'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AppApprove";
            foreach (DataRow dr in dt.Rows)
            {
                dr["isValid"] = "N";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            return 1;
        }

        /// <summary>
        /// 判断同一节点，是否有同一角色的其他人审批过，如审批过，当前审批人无法再审批。
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="viewTitle"></param>
        /// <param name="tableName"></param>
        /// <param name="pkField"></param>
        /// <param name="pkValue"></param>
        /// <param name="approveNode"></param>
        /// <returns></returns>
        public bool CheckSameNodeOtherHasApprove(UserInfo sysUser, string tableName, string pkField, string pkValue,  string approveNode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            paras.Add("refId", pkValue);
            paras.Add("approveNode", approveNode);
            paras.Add("approver", sysUser.UserId);
            string sql = @"select count(1) cnt from AppApprove where tableName=@tableName and refId=@refId and approver<>@approver and approveNode=@approveNode and isValid='Y' and approveState<>'O'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            if (DataConvert.ToInt32(dt.Rows[0]["cnt"]) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 回写被审批的表的信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pkField"></param>
        /// <param name="pkValue"></param>
        /// <param name="sysUser"></param>
        /// <param name="viewTitle"></param>
        /// <param name="approveState"></param>
        /// <param name="isEndNode"></param>
        /// <returns></returns>
        protected int OverrideRefTable(string tableName, string pkField, string pkValue, UserInfo sysUser, string viewTitle, string approveState, bool isEndNode = false)
        {
            UpdateRefTable(tableName, pkField, pkValue, sysUser, viewTitle, approveState, isEndNode);
            UpdateRefTable(tableName + "Detail", pkField, pkValue, sysUser, viewTitle, approveState, isEndNode);
            return 1;
        }

        protected int UpdateRefTable(string tableName, string pkField, string pkValue, UserInfo sysUser, string viewTitle, string approveState, bool isEndNode = false)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            string sql = @"SELECT  * FROM SysObjects where name=@tableName";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            if (dt.Rows.Count < 1)
                return 0;
            paras.Clear();
            paras.Add(pkField, pkValue);
            sql = @"select * from " + tableName + " where " + pkField + "=@" + pkField;
            dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = tableName;
            foreach (DataRow dr in dt.Rows)
            {
                if (approveState != "R")
                {
                    if (isEndNode)
                        dr["approveState"] = "E";
                    else
                        dr["approveState"] = "I";
                }
                else
                {
                    dr["approveState"] = "R";
                }
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }
    }
}
