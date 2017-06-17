using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Data.Common;

namespace BusinessCommon.AppMng
{
    public class ApproveRepository : BaseRepository
    {
        protected DataUpdate dbUpdate = new DataUpdate();

        protected DbCommand cmd;

        public DataTable GetGridDataTable(Dictionary<string, object> paras)
        {
            string sql = string.Format(@" select approveId,tableName,refId,approver,approveState,approveMind  from AppApprove 
           where tableName=@tableName and refId=@refId ");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle,string tableName, string pkField, string pkValue,bool isReturn)
        {
            string sql = @"select * from AppApprove where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppApprove";
            DataRow dr = dt.NewRow();
            cmd = dbUpdate.BeginTransaction();
            try
            {
                foreach (KeyValuePair<string, object> kv in objs)
                {
                    dr[kv.Key] = kv.Value;
                }
                dr["approveId"] = IdGenerator.GetMaxId(dt.TableName);
                Create5Field(dr, sysUser.UserId, viewTitle);
                dt.Rows.Add(dr);
                dbUpdate.Update(dt, cmd);
                OverrideRefTable(tableName, pkField, pkValue, sysUser, viewTitle, isReturn);
                dbUpdate.Commit(cmd);
            }
            catch (Exception ex)
            {
                dbUpdate.Rollback(cmd);
                throw new Exception(ex.Message);
            }
            return 1;
        }

        //public int EditData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle, string tableName, string pkField, string pkValue)
        //{
        //    Dictionary<string, object> paras = new Dictionary<string, object>();
        //    paras.Add("approveId", pkValue);
        //    string sql = @"select * from AppApprove where userId=@userId";
        //    DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
        //    dt.TableName = "AppApprove";
        //    cmd = dbUpdate.BeginTransaction();
        //    try
        //    {
        //        foreach (KeyValuePair<string, object> kv in objs)
        //        {
        //            dt.Rows[0][kv.Key] = kv.Value;
        //        }
        //        Update5Field(dt, sysUser.UserId, viewTitle);
        //        dbUpdate.Update(dt, cmd);
        //        OverrideRefTable(tableName, pkField, pkValue, sysUser, viewTitle,true);
        //        dbUpdate.Commit(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        dbUpdate.Rollback(cmd);
        //        throw new Exception(ex.Message);
        //    }
        //    return 1;
        //}

        protected int OverrideRefTable(string tableName, string pkField, string pkValue, UserInfo sysUser, string viewTitle,bool isReturn)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add(pkField, pkValue);
            string sql = @"select * from " + tableName + " where " + pkField + "=@" + pkField;
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = tableName;
            if (!isReturn)
            {
                dt.Rows[0]["approveState"] = "I";
                dt.Rows[0]["approveLevel"] = DataConvert.ToInt32(dt.Rows[0]["approveLevel"]) + 1;
                dt.Rows[0]["approveNode"] = "ssss";
            }
            else
            {
                dt.Rows[0]["approveState"] = "R";
                dt.Rows[0]["approveLevel"] = 0;
                dt.Rows[0]["approveNode"] = "return";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return dbUpdate.Update(dt, cmd);
        }

    }
}
