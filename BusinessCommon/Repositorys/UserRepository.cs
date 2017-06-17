using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessCommon.Models.User;
using BaseCommon.Models;


namespace BusinessCommon.Repositorys
{
    public class UserRepository : MasterRepository
    {
        public UserRepository()
        {
            DefaulteGridSortField = "userNo";
            MasterTable = "AppUser";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.Low)
            {
                wcd.Sql += @" and AppUser.isSysUser='Y'";
            }
            if (condition.SysUser.UserNo != "sa" && condition.SysUser.IsHeaderOffice != "Y")
            {
                wcd.Sql += @" and D.companyId=@companyId";
                wcd.DBPara.Add("companyId", condition.SysUser.CompanyId);
            }
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.UserNo) != "")
            {
                wcd.Sql += @" and AppUser.userNo like '%'+@userNo+'%'";
                wcd.DBPara.Add("userNo", model.UserNo);
            }
            if (DataConvert.ToString(model.UserName) != "")
            {
                wcd.Sql += @" and AppUser.userName like '%'+@userName+'%'";
                wcd.DBPara.Add("userName", model.UserName);
            }
            if (DataConvert.ToString(model.GroupId) != "")
            {
                wcd.Sql += @" and AppUser.groupId=@groupId ";
                wcd.DBPara.Add("groupId", model.GroupId);
            }
            if (DataConvert.ToString(model.DepartmentId) != "")
            {
                wcd.Sql += @" and AppUser.departmentId=@departmentId ";
                wcd.DBPara.Add("departmentId", model.DepartmentId);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  AppUser.userId userId,
                                 AppUser.userNo userNo,
                                 AppUser.userName userName,
                                 AppUser.groupIdDisplay groupId ,  
                                 AppUser.departmentDisplay departmentId,
                                (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                                 AppPost.postName postId,
                                 (select CodeTable.codeName from CodeTable where AppUser.sex=CodeTable.codeNo and CodeTable.codeType='Sex' and CodeTable.languageVer='{0}' ) sex,
                                 AppUser.tel tel,
                                 AppUser.email email,
                                 AppUser.address address,
                                (select CodeTable.codeName from CodeTable where AppUser.isFixed=CodeTable.codeNo and CodeTable.codeType='BoolVal' and CodeTable.languageVer='{0}' ) isFixed,
                                 U1.userName createId,
                                 AppUser.createTime,
                                 U2.userName updateId,
                                 AppUser.updateTime  updateTime,
                                 AppUser.updatePro updatePro
                              from AppUser left join AppGroup on AppUser.groupId=AppGroup.groupId   
                              left join AppDepartment D on AppUser.departmentId=D.departmentId 
                              left join AppPost on AppUser.postId=AppPost.postId 
                              left join AppUser U1 on AppUser.createId=U1.userId
                              left join AppUser U2 on AppUser.updateId=U2.userId  
                          where 1=1 ", AppMember.AppLanguage.ToString());
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("userId", primaryKey);
                string sql = @"select * from AppUser where userId=@userId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.UserId = primaryKey;
                model.UserNo = DataConvert.ToString(dr["userNo"]);
                model.UserName = DataConvert.ToString(dr["userName"]);
                model.GroupId = DataConvert.ToString(dr["groupId"]);
                model.GroupIdDisplay = DataConvert.ToString(dr["groupIdDisplay"]);
                model.DepartmentId = DataConvert.ToString(dr["departmentId"]);
                model.DepartmentIdDisplay = DataConvert.ToString(dr["departmentDisplay"]);
                model.PostId = DataConvert.ToString(dr["postId"]);
                model.Tel = DataConvert.ToString(dr["tel"]);
                model.Email = DataConvert.ToString(dr["email"]);
                model.Address = DataConvert.ToString(dr["address"]);
                model.Sex = DataConvert.ToString(dr["sex"]);
                model.IsSysUser = DataConvert.ToString(dr["isSysUser"]) == "Y" ? true : false;
                model.HasApproveAuthority = DataConvert.ToString(dr["hasApproveAuthority"]) == "Y" ? true : false;
                model.IsFixed = DataConvert.ToString(dr["isFixed"]);
                model.AccessLevel = DataConvert.ToString(dr["accessLevel"]);
            }
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sqlcnt = @"select * from AppUser where isSysUser='Y'";
            DataTable dtcnt = AppMember.DbHelper.GetDataSet(sqlcnt).Tables[0];
            //if (dtcnt.Rows.Count >= 5)
            //    throw new Exception(AppMember.AppText["MaxUserCount"]);
            string sql = @"select * from AppUser where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppUser";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["userPwd"] = AppSecurity.Encryption.Encryt("123");
            dr["userId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", pkValue);
            string sql = @"select * from AppUser where userId=@userId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppUser";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", pkValue);
            string sql = @"select * from AppUser where userId=@userId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppUser";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["userNo"] = model.UserNo;
            dr["userName"] = model.UserName;
            dr["groupId"] = model.GroupId;
            dr["groupIdDisplay"] = model.GroupIdDisplay;
            dr["departmentId"] = model.DepartmentId;
            dr["departmentDisplay"] = model.DepartmentIdDisplay;
            dr["postId"] = model.PostId;
            dr["tel"] = model.Tel;
            dr["email"] = model.Email;
            dr["address"] = model.Address;
            dr["sex"] = model.Sex;
            if (model.IsSysUser == true)
                dr["isSysUser"] = "Y";
            else
                dr["isSysUser"] = "N";
            if (model.HasApproveAuthority == true)
                dr["hasApproveAuthority"] = "Y";
            else
                dr["hasApproveAuthority"] = "N";
            dr["accessLevel"] = model.AccessLevel;
        }

        public bool ValidLogin(string userName, string password)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userNo", userName);
            paras.Add("userPwd", AppSecurity.Encryption.Encryt(password));
            string sql = @"select * from AppUser where userNo=@userNo and userPwd=@userPwd ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from AppUser  order by isFixed desc,userName ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public static string GetUserName(string userNo)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userNo", userNo);
            string sql = @"select * from AppUser where userNo=@userNo";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToString(dtGrid.Rows[0]["userName"]);
        }

        public DataTable GetUserInfo()
        {
            string sql = @"select AppGroup.groupNo,
                        AppGroup.groupName, 
                        AppUser.userId,
                        AppUser.userNo,
                        AppUser.userName,
                        AppUser.groupId ,
                        AppUser.isSysUser,
                        AppUser.accessLevel,
                        D.departmentId departmentId,
                        D.departmentNo departmentNo,
                        D.departmentName departmentName,
	                    D.companyId companyId,
	                    C.departmentNo companyNo,
	                    C.departmentName companyName ,
                        C.isHeaderOffice isHeaderOffice
                        from AppUser left join AppGroup on AppGroup.groupId=AppUser.groupId 
	                    left join AppDepartment D on AppUser.departmentId like '%'+D.departmentId +'%'  
	                    left join AppDepartment C on C.departmentId=D.companyId 
                        where AppUser.isSysUser='Y'
                        order by AppUser.userName  ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["userId"]);
                dropList.Text = DataConvert.ToString(dr["userName"]);
                dropList.Filter = DataConvert.ToString(dr["groupId"]);
                list.Add(dropList);
            }
            return list;
        }

        public string UserAutoCompleteSource()
        {
            string sql = @"select   AppUser.userName userName
                        from AppUser where isSysUser<>'Y' or isSysUser is null ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            string source = "";
            foreach (DataRow dr in dtGrid.Rows)
            {
                source +="'"+ DataConvert.ToString(dr["userName"])+"',";
            }
            if (source != "")
            {
                source = source.Substring(0, source.Length - 1);
                source = "[" + source + "]";
            }
            return source;
        }

    }
}
