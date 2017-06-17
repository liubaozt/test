using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using log4net;
using System.Reflection;

namespace BusinessCommon.AppMng
{
    public class UserRepository : MasterRepository
    {
        public UserRepository()
        {
            DefaulteGridSortField = "userNo";
        }

        

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("userNo") && DataConvert.ToString(paras["userNo"]) != "")
                whereSql += @" and AppUser.userNo like '%'+@userNo+'%'";
            if (paras.ContainsKey("userName") && DataConvert.ToString(paras["userName"]) != "")
                whereSql += @" and AppUser.userName like '%'+@userName+'%'";
            if (paras.ContainsKey("groupId") && DataConvert.ToString(paras["groupId"]) != "")
                whereSql += @" and AppUser.groupId=@groupId ";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AppUser.userId userId,
                                 AppUser.userNo userNo,
                                 AppUser.userName userName,
                                 AppGroup.groupName groupId ,  
                                 AppDepartment.departmentName departmentId,
                                 AppPost.postName postId,
                                 U1.userName createId,
                                 AppUser.createTime,
                                 U2.userName updateId,
                                 AppUser.updateTime  updateTime,
                                 AppUser.updatePro updatePro
                              from AppUser inner join AppGroup on AppUser.groupId=AppGroup.groupId   
                              left join AppDepartment on AppUser.departmentId=AppDepartment.departmentId 
                              left join AppPost on AppUser.postId=AppPost.postId 
                              left join AppUser U1 on AppUser.createId=U1.userId
                              left join AppUser U2 on AppUser.updateId=U2.userId  
                          where 1=1 {1} ", DataConvert.ToString(rowSize), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AppUser  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetUser(string userId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            string sql = @"select * from AppUser where userId=@userId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetUsers()
        {
            string sql = @"select userId,userNo,userName,groupId,hasApprove from AppUser ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppUser where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppUser";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["userPwd"] ="123";
            dr["userId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle,string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", pkValue);
            string sql = @"select * from AppUser where userId=@userId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppUser";
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dt.Rows[0][kv.Key] = kv.Value;
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return dbUpdate.Update(dt);
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", pkValue);
            string sql = @"select * from AppUser where userId=@userId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppUser";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public bool ValidLogin(string userName, string password)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userNo", userName);
            paras.Add("userPwd", password);
            string sql = @"select * from AppUser where userNo=@userNo and userPwd=@userPwd ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select userId,userName,groupId from AppUser  order by userName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["userId"]);
                dropList.Text = DataConvert.ToString(dr["userName"]);
                dropList.Filter = DataConvert.ToString(dr["groupId"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
