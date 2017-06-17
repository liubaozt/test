using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.StoreSite;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class StoreSiteRepository : MasterRepository
    {

        public StoreSiteRepository()
        {
            DefaulteGridSortField = "storeSiteNo";
            MasterTable = "StoreSite";
        }

        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            if (condition.SysUser.UserNo!="sa" && condition.SysUser.IsHeaderOffice != "Y")
            {
                wcd.Sql += @" and D.companyId=@companyId";
                wcd.DBPara.Add("companyId", condition.SysUser.CompanyId);
            }
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.StoreSiteNo) != "")
            {
                wcd.Sql += @" and StoreSite.storeSiteNo like '%'+@storeSiteNo+'%'";
                wcd.DBPara.Add("storeSiteNo", model.StoreSiteNo);
            }
            if (DataConvert.ToString(model.StoreSiteName) != "")
            {
                wcd.Sql += @" and StoreSite.storeSiteName like '%'+@storeSiteName+'%'";
                wcd.DBPara.Add("storeSiteName", model.StoreSiteName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  StoreSite.storeSiteId storeSiteId,
                                 StoreSite.storeSiteNo storeSiteNo,
                                 StoreSite.storeSiteName storeSiteName,
                                 T.storeSiteName parentId,
                                 D.departmentName companyId,
                                 U1.userName createId ,
                                 StoreSite.createTime createTime ,
                                 U2.userName updateId ,
                                 StoreSite.updateTime updateTime ,
                                 StoreSite.updatePro updatePro
                          from StoreSite left join AppUser U1 on StoreSite.createId=U1.userId
                                    left join AppUser U2 on StoreSite.updateId=U2.userId 
                                    left join StoreSite T on StoreSite.parentId=T.storeSiteId
                                    left join AppDepartment D on StoreSite.companyId=D.departmentId
                          where 1=1 ");
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("storeSiteId", primaryKey);
                string sql = @"select * from StoreSite where storeSiteId=@storeSiteId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.StoreSiteId = primaryKey;
                model.StoreSiteNo = DataConvert.ToString(dr["storeSiteNo"]);
                model.StoreSiteName = DataConvert.ToString(dr["storeSiteName"]);
                model.CompanyId = DataConvert.ToString(dr["companyId"]); 
                model.ParentId = DataConvert.ToString(dr["parentId"]); 
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from StoreSite where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "StoreSite";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            string pk = IdGenerator.GetMaxId(dt.TableName);
            dr["storeSiteId"] = pk;
            myModel.StoreSiteId = pk;
            SetDataRow(myModel, dr);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("storeSiteId", pkValue);
            string sql = @"select * from StoreSite where storeSiteId=@storeSiteId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "StoreSite";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("storeSiteId", pkValue);
            string sql = @"select * from StoreSite where storeSiteId=@storeSiteId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "StoreSite";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["storeSiteNo"] = model.StoreSiteNo;
            dr["storeSiteName"] = model.StoreSiteName;
            dr["companyId"] = model.CompanyId;
            dr["parentId"] = DataConvert.ToString(model.ParentId) == "" ? model.CompanyId : DataConvert.ToString(model.ParentId);
            if (DataConvert.ToString(model.ParentId) == "")
            {
                dr["storeSitePath"] =model.CompanyId+","+ model.StoreSiteId;
            }
            else
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("storeSiteId", model.ParentId);
                string sql = @"select storeSitePath from StoreSite where storeSiteId=@storeSiteId";
                DataTable dtp = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                string path = "";
                if (dtp.Rows.Count > 0)
                    path = DataConvert.ToString(dtp.Rows[0]["storeSitePath"]);
                else
                {
                    //Dictionary<string, object> paras2 = new Dictionary<string, object>();
                    //paras2.Add("storeSiteId", model.ParentId);
                    //string sql2 = @"select storeSitePath from StoreSite where storeSiteId=@storeSiteId";
                    //DataTable dtp2 = AppMember.DbHelper.GetDataSet(sql2, paras2).Tables[0];
                    //if (dtp2.Rows.Count > 0)
                    //    path = DataConvert.ToString(dtp2.Rows[0]["storeSitePath"]);
                }
                dr["storeSitePath"] = path == "" ? (model.CompanyId) : path + "," + model.StoreSiteId;
            }
        }


        public DataTable GetStoreSiteTree(UserInfo sysUser)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            string sql = @"select departmentId storeSiteId,parentId,departmentName storeSiteName,1 isOpen ,'false' checked from AppDepartment where isCompany='Y' ";
            paras.Add("companyId", sysUser.CompanyId);
            if (sysUser.UserNo!="sa" && sysUser.IsHeaderOffice != "Y")
            {             
                sql += @" and companyId=@companyId";
            }
            sql += " union all ";
            sql += @"select storeSiteId,parentId,storeSiteName,0 isOpen ,'false' checked from StoreSite where 1=1  ";
            if (sysUser.UserNo!="sa" && sysUser.IsHeaderOffice != "Y")
            {
                sql += @" and companyId=@companyId";
            }
            DataTable dt = AppMember.DbHelper.GetDataSet(sql,paras).Tables[0];
            return dt;
        }

        public DataTable GetDropListSource(string userId, string currentId, UserInfo sysUser)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            paras.Add("storeSiteId", DataConvert.ToString(currentId));
            paras.Add("companyId", DataConvert.ToString(currentId));//有可能是公司有可能是存放地
            string sql = @"select StoreSite.storeSiteId,StoreSite.storeSiteName from StoreSite,AppCache 
                            where AppCache.tableName='StoreSite' 
                            and AppCache.userId=@userId 
                            and AppCache.pkValue=StoreSite.storeSiteId  
                            union 
                            select StoreSite.storeSiteId,StoreSite.storeSiteName from StoreSite
                            where StoreSite.storeSiteId=@storeSiteId
                            union 
                            select AppDepartment.departmentId,AppDepartment.departmentName from AppDepartment
                            where AppDepartment.departmentId=@companyId ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql,paras).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["storeSiteId"]);
                dropList.Text = DataConvert.ToString(dr["storeSiteName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}
