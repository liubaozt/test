using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;

namespace BusinessCommon.AppMng
{
    public class GroupRepository : MasterRepository
    {

        public GroupRepository()
        {
            DefaulteGridSortField = "groupNo";
        }

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("groupNo") && DataConvert.ToString(paras["groupNo"]) != "")
                whereSql += @" and AppGroup.groupNo like '%'+@groupNo+'%'";
            if (paras.ContainsKey("groupName") && DataConvert.ToString(paras["groupName"]) != "")
                whereSql += @" and AppGroup.groupName like '%'+@groupName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AppGroup.groupId groupId,
                                 AppGroup.groupNo groupNo,
                                 AppGroup.groupName groupName,
                                 AppGroup.remark remark,
                                 U1.userName createId ,
                                 AppGroup.createTime createTime ,
                                 U2.userName updateId ,
                                 AppGroup.updateTime updateTime ,
                                 AppGroup.updatePro updatePro
                          from AppGroup left join AppUser U1 on AppGroup.createId=U1.userId
                                       left join AppUser U2 on AppGroup.updateId=U2.userId
                          where 1=1 {1}", DataConvert.ToString(rowSize), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AppGroup  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetGroup(string groupId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("groupId", groupId);
            string sql = @"select * from AppGroup where groupId=@groupId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppGroup where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppGroup";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["groupId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("groupId", pkValue);
            string sql = @"select * from AppGroup where groupId=@groupId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppGroup";
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
            paras.Add("groupId", pkValue);
            string sql = @"select * from AppGroup where groupId=@groupId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppGroup";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public  List<DropListSource> DropList()
        {
            string sql = @"select groupId,groupNo,groupName from AppGroup  order by groupNo ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["groupId"]);
                dropList.Text = DataConvert.ToString(dr["groupName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
