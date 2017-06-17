using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;

namespace BusinessLogic.BasicData
{
    public class AccountSubjectRepository : MasterRepository
    {

        public AccountSubjectRepository()
        {
            DefaulteGridSortField = "accountSubjectNo";
        }

       
        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("accountSubjectNo") && DataConvert.ToString(paras["accountSubjectNo"]) != "")
                whereSql += @" and AccountSubject.accountSubjectNo like '%'+@accountSubjectNo+'%'";
            if (paras.ContainsKey("accountSubjectName") && DataConvert.ToString(paras["accountSubjectName"]) != "")
                whereSql += @" and AccountSubject.accountSubjectName like '%'+@accountSubjectName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AccountSubject.accountSubjectId accountSubjectId,
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
                          where 1=1 {1}", DataConvert.ToString(rowSize), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AccountSubject  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAccountSubject(string accountSubjectId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("accountSubjectId", accountSubjectId);
            string sql = @"select * from AccountSubject where accountSubjectId=@accountSubjectId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AccountSubject where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AccountSubject";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["accountSubjectId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("accountSubjectId", pkValue);
            string sql = @"select * from AccountSubject where accountSubjectId=@accountSubjectId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AccountSubject";
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dt.Rows[0][kv.Key] = kv.Value;
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return dbUpdate.Update(dt);
        }

        protected override int DeleteData(Dictionary<string, object> objs,UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("accountSubjectId", pkValue);
            string sql = @"select * from AccountSubject where accountSubjectId=@accountSubjectId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AccountSubject";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public DataTable GetAccountSubjectTree()
        {
            string sql = @"select accountSubjectId,parentId,accountSubjectName,1 isOpen ,'false' checked from AccountSubject ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select accountSubjectId,accountSubjectName,parentId from AccountSubject  order by accountSubjectName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
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
