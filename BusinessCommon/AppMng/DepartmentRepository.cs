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
    public class DepartmentRepository : MasterRepository
    {

        public DepartmentRepository()
        {
            DefaulteGridSortField = "departmentName";
        }

    

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("departmentName") && DataConvert.ToString(paras["departmentName"]) != "")
                whereSql += @" and AppDepartment.departmentName like '%'+@departmentName+'%'";
            if (paras.ContainsKey("parentId") && DataConvert.ToString(paras["parentId"]) != "")
                whereSql += @" and AppDepartment.parentId=@parentId ";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AppDepartment.departmentId departmentId,
                                 AppDepartment.departmentName departmentName,
                                 D.departmentName parentId,
                                 U1.userName createId ,
                                 AppDepartment.createTime createTime ,
                                 U2.userName updateId ,
                                 AppDepartment.updateTime updateTime ,
                                 AppDepartment.updatePro updatePro
                          from AppDepartment left join AppUser U1 on AppDepartment.createId=U1.userId
                                       left join AppUser U2 on AppDepartment.updateId=U2.userId
                                       left join AppDepartment D on AppDepartment.parentId=D.departmentId 
                          where 1=1 {1}", DataConvert.ToString(rowSize), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AppDepartment  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetDepartment(string departmentId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("departmentId", departmentId);
            string sql = @"select * from AppDepartment where departmentId=@departmentId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppDepartment where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppDepartment";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["departmentId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("departmentId", pkValue);
            string sql = @"select * from AppDepartment where departmentId=@departmentId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppDepartment";
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
            paras.Add("departmentId", pkValue);
            string sql = @"select * from AppDepartment where departmentId=@departmentId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppDepartment";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public DataTable GetDepartmentTree()
        {
            string sql = @"select departmentId,parentId,departmentName,1 isOpen ,'false' checked from AppDepartment ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select departmentId ,departmentName,parentId from AppDepartment  order by departmentName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["departmentId"]);
                dropList.Text = DataConvert.ToString(dr["departmentName"]);
                dropList.Filter = DataConvert.ToString(dr["parentId"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}
