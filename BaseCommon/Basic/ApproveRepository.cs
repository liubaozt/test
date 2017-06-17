using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Data.Common;

namespace BaseCommon.Basic
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
                    approveMind,approveLevel  
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
            if (approveState == "A" && seqno==0)
                dr["seqno"] = seqno + 2;
            else
                dr["seqno"] = seqno + 1;
            //dr["approveId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            DbUpdate.Update(dt);
            if (!isInit)
                OverrideRefTable(tableName, pkField, pkValue, sysUser, viewTitle, approveState);
            return 1;
        }

        public int EditData(UserInfo sysUser, string viewTitle, string tableName, string pkField, string pkValue, string approveState, string approveMind, bool isEndNode = false)
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
            OverrideRefTable(tableName, pkField, pkValue, sysUser, viewTitle, approveState, isEndNode);
            return 1;
        }

        protected int OverrideRefTable(string tableName, string pkField, string pkValue, UserInfo sysUser, string viewTitle, string approveState, bool isEndNode = false)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add(pkField, pkValue);
            string sql = @"select * from " + tableName + " where " + pkField + "=@" + pkField;
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = tableName;
            if (approveState != "R")
            {
                if (isEndNode)
                    dt.Rows[0]["approveState"] = "E";
                else
                    dt.Rows[0]["approveState"] = "I";
            }
            else
            {
                dt.Rows[0]["approveState"] = "R";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

    }
}
