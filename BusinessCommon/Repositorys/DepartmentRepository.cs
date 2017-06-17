using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessCommon.Models.Department;
using BaseCommon.Models;
using System.Text.RegularExpressions;

namespace BusinessCommon.Repositorys
{
    public class DepartmentRepository : MasterRepository
    {

        public DepartmentRepository()
        {
            DefaulteGridSortField = "departmentNo";
            MasterTable = "AppDepartment";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            if (condition.SysUser.UserNo != "sa" && condition.SysUser.IsHeaderOffice != "Y")
            {
                wcd.Sql += @" and AppDepartment.companyId=@companyId";
                wcd.DBPara.Add("companyId", condition.SysUser.CompanyId);
            }
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.DepartmentNo) != "")
            {
                wcd.Sql += @" and AppDepartment.departmentNo like '%'+@departmentNo+'%'";
                wcd.DBPara.Add("departmentNo", model.DepartmentNo);
            }
            if (DataConvert.ToString(model.DepartmentName) != "")
            {
                wcd.Sql += @" and AppDepartment.departmentName like '%'+@departmentName+'%'";
                wcd.DBPara.Add("departmentName", model.DepartmentName);
            }
            if (DataConvert.ToString(model.ParentId) != "")
            {
                wcd.Sql += @" and AppDepartment.parentId=@parentId ";
                wcd.DBPara.Add("parentId", model.ParentId);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  AppDepartment.departmentId departmentId,
                                 AppDepartment.departmentName departmentName,
                                 AppDepartment.departmentNo departmentNo,
                                 D.departmentName parentId,
                                 C.departmentName companyId,
                                 U1.userName createId ,
                                 AppDepartment.createTime createTime ,
                                 U2.userName updateId ,
                                 AppDepartment.updateTime updateTime ,
                                 AppDepartment.updatePro updatePro
                          from AppDepartment left join AppUser U1 on AppDepartment.createId=U1.userId
                                       left join AppUser U2 on AppDepartment.updateId=U2.userId
                                       left join AppDepartment D on AppDepartment.parentId=D.departmentId 
                                       left join AppDepartment C on AppDepartment.companyId=C.departmentId 
                          where (AppDepartment.isCompany<>'Y' or AppDepartment.isCompany is null) ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("departmentId", primaryKey);
                string sql = @"select * from AppDepartment where departmentId=@departmentId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.DepartmentId = primaryKey;
                model.DepartmentNo = DataConvert.ToString(dr["departmentNo"]);
                model.DepartmentName = DataConvert.ToString(dr["departmentName"]);
                model.ParentId = DataConvert.ToString(dr["parentId"]);
            }
        }



        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppDepartment where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppDepartment";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            string pk = IdGenerator.GetMaxId(dt.TableName);
            dr["departmentId"] = pk;
            myModel.DepartmentId = pk;
            SetDataRow(myModel, dr,sysUser,viewTitle);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("departmentId", pkValue);
            string sql = @"select * from AppDepartment where departmentId=@departmentId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppDepartment";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0],sysUser,viewTitle);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("departmentId", pkValue);
            string sql = @"select * from AppDepartment where departmentId=@departmentId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppDepartment";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr ,UserInfo sysUser, string viewTitle)
        {
            dr["departmentNo"] = model.DepartmentNo;
            dr["departmentName"] = model.DepartmentName;
            dr["isCompany"] = "N";
            dr["isHeaderOffice"] = "N";
            dr["parentId"] = DataConvert.ToString(model.ParentId);

            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("departmentId", model.ParentId);
            string sql = @"select departmentPath from AppDepartment where departmentId=@departmentId";
            DataTable dtp = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            string departmentParentPath = DataConvert.ToString(dtp.Rows[0]["departmentPath"]);
            string departmentPath = departmentParentPath + "," + model.DepartmentId;
            dr["departmentPath"] = departmentPath;

            string[] dpps=departmentParentPath.Split(',');
            for (int i = dpps.Length - 1; i >= 0 ; i--)
            {
                Dictionary<string, object> paras2 = new Dictionary<string, object>();
                paras2.Add("departmentId", dpps[i]);
                string sql2 = @"select departmentId,isCompany from AppDepartment where departmentId=@departmentId";
                DataTable dtCompany = AppMember.DbHelper.GetDataSet(sql2, paras2).Tables[0];
                if (DataConvert.ToString(dtCompany.Rows[0]["isCompany"]) == "Y")
                {
                    dr["companyId"] = DataConvert.ToString(dtCompany.Rows[0]["departmentId"]);
                    break;
                }
            }
            SetChildDepartment(model.DepartmentId, departmentPath, sysUser, viewTitle);
        }

        private void SetChildDepartment(string departmentId, string departmentPath ,UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("departmentId", departmentId);
            string sql = @"select * from AppDepartment where departmentId<>@departmentId and departmentPath like '%'+@departmentId+'%'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string departmentIdCur = DataConvert.ToString(dr["departmentId"]);
                string departmentPathCur = DataConvert.ToString(dr["departmentPath"]);
                string[] departmentPathCurArr = Regex.Split(departmentPathCur, departmentId);
                string departmentPathNew=departmentPath + departmentPathCurArr[1];
                dr["departmentPath"] = departmentPathNew;

                string[] dpps = departmentPathNew.Split(',');
                for (int i = dpps.Length - 1; i >= 0; i--)
                {
                    Dictionary<string, object> paras2 = new Dictionary<string, object>();
                    paras2.Add("departmentId", dpps[i]);
                    string sql2 = @"select departmentId,isCompany,companyId from AppDepartment where departmentId=@departmentId";
                    DataTable dtCompany = AppMember.DbHelper.GetDataSet(sql2, paras2).Tables[0];
                    if (DataConvert.ToString(dtCompany.Rows[0]["isCompany"]) == "Y")
                    {
                        dr["companyId"] = DataConvert.ToString(dtCompany.Rows[0]["departmentId"]);
                        break;
                    }
                }
            }
            dt.TableName = "AppDepartment";
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
        }



        public DataTable GetDepartmentTree(UserInfo sysUser)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            string sql = @"select departmentId,parentId,departmentName, case isCompany when 'Y' then 1 else 0 end  as isOpen ,'false' checked from AppDepartment where 1=1  ";
            if (sysUser.UserNo!="sa" && sysUser.IsHeaderOffice != "Y")
            {
                paras.Add("companyId", sysUser.CompanyId);
                sql += @" and companyId=@companyId";
            }
            sql += " order by  departmentName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql,paras).Tables[0];
            return dt;
        }

        public DataTable GetDropListSource(string userId, string currentId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            paras.Add("departmentId", DataConvert.ToString(currentId));
            string sql = @"select AppDepartment.* from AppDepartment,AppCache 
                  where AppCache.tableName='AppDepartment' 
                  and AppCache.userId=@userId 
                  and AppCache.pkValue=AppDepartment.departmentId
                  union 
                  select AppDepartment.* from AppDepartment
                  where AppDepartment.departmentId=@departmentId
                  order by AppDepartment.departmentName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select AppDepartment.* from AppDepartment order by AppDepartment.departmentName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["departmentId"]);
                dropList.Text = DataConvert.ToString(dr["departmentName"]);
                list.Add(dropList);
            }
            return list;
        }



    }
}
