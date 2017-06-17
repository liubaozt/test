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
    public class EquityOwnerRepository : MasterRepository
    {

        public EquityOwnerRepository()
        {
            DefaulteGridSortField = "equityOwnerNo";
        }

       

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("equityOwnerNo") && DataConvert.ToString(paras["equityOwnerNo"]) != "")
                whereSql += @" and EquityOwner.equityOwnerNo like '%'+@equityOwnerNo+'%'";
            if (paras.ContainsKey("equityOwnerName") && DataConvert.ToString(paras["equityOwnerName"]) != "")
                whereSql += @" and EquityOwner.equityOwnerName like '%'+@equityOwnerName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} EquityOwner.equityOwnerId equityOwnerId,
                                 EquityOwner.equityOwnerNo equityOwnerNo,
                                 EquityOwner.equityOwnerName equityOwnerName,
                                 U1.userName createId ,
                                 EquityOwner.createTime createTime ,
                                 U2.userName updateId ,
                                 EquityOwner.updateTime updateTime ,
                                 EquityOwner.updatePro updatePro
                          from EquityOwner left join AppUser U1 on EquityOwner.createId=U1.userId
                                    left join AppUser U2 on EquityOwner.updateId=U2.userId 
                          where 1=1 {1}", DataConvert.ToString(rowSize),WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from EquityOwner  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetEquityOwner(string equityOwnerId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("equityOwnerId", equityOwnerId);
            string sql = @"select * from EquityOwner where equityOwnerId=@equityOwnerId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from EquityOwner where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "EquityOwner";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["equityOwnerId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("equityOwnerId", pkValue);
            string sql = @"select * from EquityOwner where equityOwnerId=@equityOwnerId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "EquityOwner";
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
            paras.Add("equityOwnerId", pkValue);
            string sql = @"select * from EquityOwner where equityOwnerId=@equityOwnerId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "EquityOwner";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select equityOwnerId,equityOwnerName from EquityOwner  order by equityOwnerName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["equityOwnerId"]);
                dropList.Text = DataConvert.ToString(dr["equityOwnerName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
