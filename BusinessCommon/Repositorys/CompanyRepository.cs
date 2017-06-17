using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessCommon.Models.Company;
using BaseCommon.Models;

namespace BusinessCommon.Repositorys
{
    public class CompanyRepository : MasterRepository
    {

        public CompanyRepository()
        {
            DefaulteGridSortField = "departmentNo";
            MasterTable = "AppDepartment";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.DepartmentName) != "")
            {
                wcd.Sql += @" and AppDepartment.departmentName like '%'+@departmentName+'%'";
                wcd.DBPara.Add("departmentName", model.DepartmentName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  AppDepartment.departmentId departmentId,
                                 AppDepartment.departmentName departmentName,
                                 AppDepartment.departmentNo departmentNo,
                                   (select CodeTable.codeName from CodeTable where AppDepartment.isHeaderOffice=CodeTable.codeNo and CodeTable.codeType='BoolVal' and CodeTable.languageVer='{0}' ) isHeaderOffice,
                                 U1.userName createId ,
                                 AppDepartment.createTime createTime ,
                                 U2.userName updateId ,
                                 AppDepartment.updateTime updateTime ,
                                 AppDepartment.updatePro updatePro
                          from AppDepartment left join AppUser U1 on AppDepartment.createId=U1.userId
                                       left join AppUser U2 on AppDepartment.updateId=U2.userId
                                       left join AppDepartment D on AppDepartment.parentId=D.departmentId 
                          where AppDepartment.isCompany='Y' ", AppMember.AppLanguage.ToString());
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
                model.IsHeaderOffice = DataConvert.ToString(dr["isHeaderOffice"])=="Y"?true:false;
            }
        }



        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppDepartment where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppDepartment";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            string pk = IdGenerator.GetMaxId(dt.TableName, "Company");
            dr["departmentId"] = pk;
            myModel.DepartmentId = pk;
            SetDataRow(myModel, dr);
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
            SetDataRow(myModel, dt.Rows[0]);
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

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["departmentNo"] = model.DepartmentNo;
            dr["departmentName"] = model.DepartmentName;
            dr["isCompany"] ="Y";
            dr["companyId"] = model.DepartmentId;
            if (model.IsHeaderOffice==true)
            {
                dr["parentId"] = "0";
                dr["departmentPath"] = model.DepartmentId;
                dr["isHeaderOffice"] = "Y";
            }
            else
            {
                string sqlIsHeaderOffice = @"select departmentId from AppDepartment where isHeaderOffice='Y'";
                DataTable dtIsHeaderOffice = AppMember.DbHelper.GetDataSet(sqlIsHeaderOffice).Tables[0];
                if (dtIsHeaderOffice.Rows.Count > 0)
                {
                    dr["parentId"] = dtIsHeaderOffice.Rows[0]["departmentId"];
                    dr["departmentPath"] = DataConvert.ToString(dtIsHeaderOffice.Rows[0]["departmentId"]) + "," + model.DepartmentId;
                }
                dr["isHeaderOffice"] = "N";
            }
        }


        public DataTable GetDepartmentTree()
        {
            string sql = @"select departmentId,parentId,departmentName,0 isOpen ,'false' checked from AppDepartment ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        public DataTable GetDropListSource(UserInfo sysUser)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            string sql = @"select AppDepartment.* from AppDepartment
                  where isCompany='Y'";
            if (sysUser.UserNo!="sa" && sysUser.IsHeaderOffice != "Y")
            {
                paras.Add("companyId", sysUser.CompanyId);
                sql += " and departmentId=@companyId";
            }
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
