using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Collections;
using System.Data.OleDb;
using BaseCommon.Repositorys;
using System.Text.RegularExpressions;

namespace BusinessCommon.Repositorys
{
    public class DataImport
    {
        public string seqno { get; set; }
        public string fileName { get; set; }
        public string sortNo { get; set; }
    }

    public class DataImportRepository : BaseRepository
    {
        private DataSet ExcelSqlConnection(string filepath, string tableName)
        {
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
            //string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;data source=" + filepath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
            OleDbConnection ExcelConn = new OleDbConnection(strCon);
            try
            {
                string strCom = "";
                //strCom = string.Format("SELECT * FROM [Sheet1$]");
                ExcelConn.Open();

                DataTable sheetNames = ExcelConn.GetOleDbSchemaTable
                        (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                OleDbDataAdapter myCommand;
                DataSet ds = new DataSet();
                foreach (DataRow sheetnameRow in sheetNames.Rows)
                {
                    string sheetName = DataConvert.ToString(sheetnameRow["TABLE_NAME"]);
                    if (!sheetName.EndsWith("$_") && !sheetName.EndsWith("$'_"))
                    {
                        strCom = string.Format("SELECT * FROM [{0}] ", sheetName);
                        myCommand = new OleDbDataAdapter(strCom, ExcelConn);
                        myCommand.Fill(ds, "[" + sheetName + "]");
                    }
                }
                ExcelConn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                ExcelConn.Close();
                AppLog.WriteLog("system", LogType.Error, "DataImport", ex.Message);
                throw ex;
                //return null;
            }
        }

        private int ImportDepartment(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppDepartment where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppDepartment";
            int i = 2;
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["departmentId"] = id;
                dr["departmentNo"] = drSource[0];
                dr["departmentName"] = drSource[1];
                dr["isCompany"] = "N";
                dr["isHeaderOffice"] = "N";
                Dictionary<string, object> paras = new Dictionary<string, object>();
                string companyName = DataConvert.ToString(drSource[2]);
                paras.Add("companyName", companyName);
                string sqlc = @"select departmentId from AppDepartment where departmentName=@companyName";
                DataTable dtc = AppMember.DbHelper.GetDataSet(sqlc, paras).Tables[0];
                if (dtc.Rows.Count < 1)
                    throw new Exception(AppMember.AppText["DepartmentExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                        AppMember.AppText["ImportCompany"] + "[" + companyName + "]" + AppMember.AppText["NotExsit"]);
                string pDepartmentName = DataConvert.ToString(drSource[3]);
                paras.Add("departmentName", pDepartmentName);
                string companyId = DataConvert.ToString(dtc.Rows[0]["departmentId"]);
                paras.Add("companyId", companyId);
                string sqld = @"select departmentId,departmentPath from AppDepartment where departmentName=@departmentName and companyId=@companyId";
                DataTable dtd = AppMember.DbHelper.GetDataSet(sqld, paras).Tables[0];
                if (dtd.Rows.Count < 1)
                    throw new Exception(AppMember.AppText["DepartmentExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                        AppMember.AppText["ImportParentDepartment"] + "[" + pDepartmentName + "]" + AppMember.AppText["NotExsit"]);
                dr["companyId"] = companyId;
                if (dtd.Rows.Count < 1)
                {
                    DataRow[] drSelect = dt.Select("departmentName='" + pDepartmentName + "' and companyId='" + companyId + "'");
                    if (drSelect.Length > 0)
                    {
                        dr["parentId"] = drSelect[0]["departmentId"];
                        dr["departmentPath"] = DataConvert.ToString(drSelect[0]["departmentPath"]) + "," + id;
                    }
                }
                else
                {
                    dr["parentId"] = DataConvert.ToString(dtd.Rows[0]["departmentId"]);
                    dr["departmentPath"] = DataConvert.ToString(dtd.Rows[0]["departmentPath"]) + "," + id;
                }
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
                i++;
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportCompany(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {

            string sql = @"select * from AppDepartment where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppDepartment";
            string headerOfficeId = "";
            DataTable dtsa = new DataTable();
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName, "Company");
                dr["departmentId"] = id;
                dr["departmentNo"] = drSource[0];
                dr["departmentName"] = drSource[1];
                dr["isCompany"] = "Y";
                dr["companyId"] = id;
                if (DataConvert.ToString(drSource[2]) == DataConvert.ToString(AppMember.AppText["Yes"]))
                {
                    dr["isHeaderOffice"] = "Y";
                    headerOfficeId = id;
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("companyName", DataConvert.ToString(drSource[2]));
                    string sql2 = @"select * from AppUser where userNo='sa' ";
                    dtsa = AppMember.DbHelper.GetDataSet(sql2).Tables[0];
                    dtsa.TableName = "AppUser";
                    dtsa.Rows[0]["departmentId"] = id;

                }
                else
                {
                    dr["isHeaderOffice"] = "N";
                }
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (DataConvert.ToString(dr["isHeaderOffice"]) == "Y")
                {
                    dr["parentId"] = "0";
                    dr["departmentPath"] = dr["departmentId"];
                }
                else
                {
                    if (headerOfficeId == "")
                    {
                        string sqlHeaderOffice = @"select * from AppDepartment where isHeaderOffice='Y' ";
                        DataTable dtHeaderOffice = AppMember.DbHelper.GetDataSet(sqlHeaderOffice).Tables[0];
                        if (dtHeaderOffice.Rows.Count > 0)
                            headerOfficeId = DataConvert.ToString(dtHeaderOffice.Rows[0]["departmentId"]);
                    }
                    dr["parentId"] = headerOfficeId;
                    dr["departmentPath"] = headerOfficeId + "," + dr["departmentId"];
                }

            }
            Update5Field(dtsa, sysUser.UserId, viewTitle);
            DbUpdate = new DataUpdate();
            List<DataTable> dtList = new List<DataTable>();
            if (dt.Rows.Count > 0)
                dtList.Add(dt);
            if (dtsa.Rows.Count > 0)
                dtList.Add(dtsa);
            return DbUpdate.Update(dtList, true);
        }

        private int ImportPurchaseType(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from PurchaseType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "PurchaseType";
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["purchaseTypeId"] = id;
                dr["purchaseTypeNo"] = drSource[0];
                dr["purchaseTypeName"] = drSource[1];
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportEquityOwner(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from EquityOwner where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "EquityOwner";
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["equityOwnerId"] = id;
                dr["equityOwnerNo"] = drSource[0];
                dr["equityOwnerName"] = drSource[1];
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportScrapType(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from ScrapType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "ScrapType";
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["scrapTypeId"] = id;
                dr["scrapTypeNo"] = drSource[0];
                dr["scrapTypeName"] = drSource[1];
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }



        private int ImportAssetsUses(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsUses where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsUses";
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["assetsUsesId"] = id;
                dr["assetsUsesNo"] = drSource[0];
                dr["assetsUsesName"] = drSource[1];
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportSetBooks(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from SetBooks where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "SetBooks";
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["setBooksId"] = id;
                dr["setBooksNo"] = drSource[0];
                dr["setBooksName"] = drSource[1];
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportStoreSite(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from StoreSite where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "StoreSite";
            int i = 2;
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["storeSiteId"] = id;
                dr["storeSiteNo"] = drSource[0];
                dr["storeSiteName"] = drSource[1];
                Dictionary<string, object> paras = new Dictionary<string, object>();
                string companyName = DataConvert.ToString(drSource[2]);
                paras.Add("companyName", companyName);
                string sqlc = @"select departmentId,departmentPath from AppDepartment where departmentName=@companyName";
                DataTable dtc = AppMember.DbHelper.GetDataSet(sqlc, paras).Tables[0];
                if (dtc.Rows.Count < 1)
                    throw new Exception(AppMember.AppText["StoreSiteExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                        AppMember.AppText["ImportCompany"] + "[" + companyName + "]" + AppMember.AppText["NotExsit"]);
                string pStoreSiteName = DataConvert.ToString(drSource[3]);
                paras.Add("storeSiteName", pStoreSiteName);
                string companyId = DataConvert.ToString(dtc.Rows[0]["departmentId"]);
                string companyPath = DataConvert.ToString(dtc.Rows[0]["departmentPath"]);
                paras.Add("companyId", companyId);
                string sqld = @"select storeSiteId,storeSitePath from StoreSite where storeSiteName=@storeSiteName and companyId=@companyId";
                DataTable dtd = AppMember.DbHelper.GetDataSet(sqld, paras).Tables[0];
                if (dtd.Rows.Count < 1 && pStoreSiteName != "")
                    throw new Exception(AppMember.AppText["StoreSiteExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                        AppMember.AppText["ImportParentStoreSite"] + "[" + pStoreSiteName + "]" + AppMember.AppText["NotExsit"]);
                dr["companyId"] = companyId;
                if (pStoreSiteName == "")
                {
                    dr["parentId"] = companyId;
                    dr["storeSitePath"] = companyPath + "," + id;
                }
                else if (dtd.Rows.Count < 1)
                {
                    DataRow[] drSelect = dt.Select("storeSiteName='" + pStoreSiteName + "' and companyId='" + companyId + "'");
                    if (drSelect.Length > 0)
                    {
                        dr["parentId"] = drSelect[0]["storeSiteId"];
                        dr["storeSitePath"] = DataConvert.ToString(drSelect[0]["storeSitePath"]) + "," + id;
                    }
                    else
                    {
                        dr["parentId"] = companyId;
                        dr["storeSitePath"] = companyPath + "," + id;
                    }
                }
                else
                {
                    dr["parentId"] = DataConvert.ToString(dtd.Rows[0]["storeSiteId"]);
                    dr["storeSitePath"] = DataConvert.ToString(dtd.Rows[0]["storeSitePath"]) + "," + id;
                }
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
                i++;
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportUsers(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppUser where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppUser";
            int i = 2;
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["userId"] = id;
                dr["userNo"] = drSource["用户编码"];
                dr["userName"] = drSource["姓名"];
                dr["isSysUser"] = "Y";
                dr["hasApproveAuthority"] = "Y";
                dr["userPwd"] = AppSecurity.Encryption.Encryt("123");
                Dictionary<string, object> paras = new Dictionary<string, object>();
                string companyName = DataConvert.ToString(drSource["所属公司"]);
                paras.Add("companyName", companyName);
                string sqlc = @"select departmentId from AppDepartment where departmentName=@companyName";
                DataTable dtc = AppMember.DbHelper.GetDataSet(sqlc, paras).Tables[0];
                if (dtc.Rows.Count < 1)
                    throw new Exception(AppMember.AppText["UserExcel"] + i.ToString() + AppMember.AppText["ImportRow"]
                        + AppMember.AppText["ImportCompany"] + "[" + companyName + "]" + AppMember.AppText["NotExsit"]);
                string departmentName = DataConvert.ToString(drSource["所属部门"]);
                departmentName = departmentName.Replace("，", ",");
                string[] departments = departmentName.Split(',');
                string departmentNames = "";
                foreach (string d in departments)
                {
                    departmentNames += "'" + d + "',";
                }
                if (departmentNames.Length > 0)
                    departmentNames = departmentNames.Substring(0, departmentNames.Length - 1);
                //paras.Add("departmentName", departmentName);
                string companyId = DataConvert.ToString(dtc.Rows[0]["departmentId"]);

                paras.Add("companyId", companyId);
                string sqld = string.Format(@"select departmentId from AppDepartment where departmentName in({0}) and companyId=@companyId", departmentNames);
                DataTable dtd = AppMember.DbHelper.GetDataSet(sqld, paras).Tables[0];
                if (dtd.Rows.Count < departments.Length)
                    throw new Exception(AppMember.AppText["UserExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                            AppMember.AppText["ImportDepartmentForUser"] + "[" + departmentName + "]" + AppMember.AppText["NotExsit"]);
                string departmentIds = "";
                foreach (DataRow drd in dtd.Rows)
                {
                    departmentIds += DataConvert.ToString(drd["departmentId"]) + ",";
                }
                if (departmentIds.Length > 0)
                    departmentIds = departmentIds.Substring(0, departmentIds.Length - 1);
                dr["departmentId"] = departmentIds;
                dr["departmentDisplay"] = departmentName;

                dr["tel"] = drSource["电话"];
                dr["email"] = drSource["邮箱"];
                dr["address"] = drSource["地址"];
                dr["sex"] = drSource["性别"];

                Dictionary<string, object> parasGroup = new Dictionary<string, object>();
                parasGroup.Add("groupName", DataConvert.ToString(drSource["角色"]));
                string sqlGroup = @"select groupId from AppGroup where groupName=@groupName ";
                DataTable dtGroup = AppMember.DbHelper.GetDataSet(sqlGroup, parasGroup).Tables[0];
                if (dtGroup.Rows.Count < 1)
                    throw new Exception(AppMember.AppText["UserExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                            AppMember.AppText["ImportGroup"] + "[" + departmentName + "]" + AppMember.AppText["NotExsit"]);
                dr["groupId"] = DataConvert.ToString(dtGroup.Rows[0]["groupId"]);
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
                i++;
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportSupplier(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Supplier where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Supplier";
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["supplierId"] = id;
                dr["supplierNo"] = drSource[0];
                dr["supplierName"] = drSource[1];
                dr["tel"] = drSource[2];
                dr["email"] = drSource[3];
                dr["address"] = drSource[4];
                dr["contacts"] = drSource[5];
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }

        private int ImportAssetsClass(DataTable dtSource, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsClass where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsClass";
            foreach (DataRow drSource in dtSource.Rows)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["assetsClassId"] = id;
                dr["assetsClassNo"] = drSource[0];
                dr["assetsClassName"] = drSource[1];
                Dictionary<string, object> paras = new Dictionary<string, object>();
                string pAssetsClassName = DataConvert.ToString(drSource[2]);
                if (pAssetsClassName == "")
                {
                    dr["parentId"] = "0";
                    dr["assetsClassPath"] = id;
                }
                else
                {
                    paras.Add("assetsClassName", pAssetsClassName);
                    string sqlc = @"select assetsClassId,assetsClassPath from AssetsClass where assetsClassName=@assetsClassName";
                    DataTable dtc = AppMember.DbHelper.GetDataSet(sqlc, paras).Tables[0];
                    if (dtc.Rows.Count < 1)
                    {
                        DataRow[] drSelect = dt.Select("assetsClassName='" + pAssetsClassName + "'");
                        if (drSelect.Length > 0)
                        {
                            dr["parentId"] = drSelect[0]["assetsClassId"];
                            dr["assetsClassPath"] = DataConvert.ToString(drSelect[0]["assetsClassPath"]) + "," + id;
                        }
                    }
                    else
                    {
                        dr["parentId"] = DataConvert.ToString(dtc.Rows[0]["assetsClassId"]);
                        dr["assetsClassPath"] = DataConvert.ToString(dtc.Rows[0]["assetsClassPath"]) + "," + id;
                    }
                }
                dr["durableYears"] = drSource[3];
                dr["remainRate"] = drSource[4];
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt, true);
        }


        private int ImportAssets(DataSet dsSource, UserInfo sysUser, string viewTitle)
        {
            DbUpdate = new DataUpdate();
            string sql = @"select * from Assets where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Assets";
            int i = 2;
            foreach (DataTable dtSource in dsSource.Tables)
            {
                i = 2;
                foreach (DataRow drSource in dtSource.Rows)
                {
                    bool isUpdateAssets = false;
                    DataRow dr;
                    string assetsNo = dtSource.Columns.Contains("资产编号") ?
                                     DataConvert.ToString(drSource["资产编号"]) : string.Empty;
                    if (assetsNo == string.Empty)
                    {
                        //throw new Exception(dtSource.TableName+AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                        //   AppMember.AppText["ImportAssetsNo"] + AppMember.AppText["NotNull"]);
                        break;
                    }

                    string sqlNo = @"select * from Assets where assetsNo=@assetsNo ";
                    Dictionary<string, object> parasNo = new Dictionary<string, object>();
                    parasNo.Add("assetsNo", assetsNo);
                    DataTable dtUpdateAssets = AppMember.DbHelper.GetDataSet(sqlNo, parasNo).Tables[0];
                    dtUpdateAssets.TableName = "Assets";
                    if (dtUpdateAssets.Rows.Count > 0)
                    {
                        dr = dtUpdateAssets.Rows[0];
                        isUpdateAssets = true;
                    }
                    else
                    {
                        dr = dt.NewRow();
                        string id = IdGenerator.GetMaxId(dt.TableName);
                        dr["assetsId"] = id;
                    }

                    dr["assetsNo"] = assetsNo;

                    string assetsName = dtSource.Columns.Contains("资产名称") ?
                                   DataConvert.ToString(drSource["资产名称"]) : string.Empty;
                    if (assetsName == string.Empty)
                        throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                           AppMember.AppText["ImportAssetsName"] + AppMember.AppText["NotNull"]);
                    dr["assetsName"] = assetsName;

                    dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;

                    //资产分类
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    string assetsClass = dtSource.Columns.Contains("资产分类") ?
                                         DataConvert.ToString(drSource["资产分类"]) : string.Empty;
                    if (assetsClass == string.Empty)
                        throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                            AppMember.AppText["ImportAssetsClass"] + AppMember.AppText["NotNull"]);
                    paras.Add("assetsClassName", assetsClass);
                    string sql1 = @"select * from AssetsClass where assetsClassName=@assetsClassName";
                    DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras).Tables[0];
                    if (dt1.Rows.Count < 1)
                    {
                        dt1.TableName = "AssetsClass";
                        DataRow dr1 = dt1.NewRow();
                        string assetsClassId = IdGenerator.GetMaxId(dt1.TableName);
                        dr1["assetsClassId"] = assetsClassId;
                        dr1["assetsClassNo"] = AutoNoGenerator.GetMaxNo("AssetsClass", "C", 3);
                        dr1["assetsClassName"] = assetsClass;
                        dr1["parentId"] = "0";
                        dr1["assetsClassPath"] = assetsClassId;
                        dt1.Rows.Add(dr1);
                        Create5Field(dt1, sysUser.UserId, viewTitle);
                        //DbUpdate = new DataUpdate();
                        DbUpdate.Update(dt1);
                        dr["assetsClassId"] = assetsClassId;
                    }
                    else
                    {
                        dr["assetsClassId"] = DataConvert.ToString(dt1.Rows[0]["assetsClassId"]);
                    }

                    //公司
                    string companyName = dtSource.Columns.Contains("所属公司") ?
                                         DataConvert.ToString(drSource["所属公司"]) : string.Empty;
                    if (companyName == string.Empty)
                        throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                           AppMember.AppText["ImportCompany"] + AppMember.AppText["NotNull"]);
                    paras.Add("companyName", companyName);
                    string sqlc = @"select * from AppDepartment where departmentName=@companyName";
                    DataTable dtc = AppMember.DbHelper.GetDataSet(sqlc, paras).Tables[0];
                    if (dtc.Rows.Count < 1)
                        throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                            AppMember.AppText["ImportCompany"] + "[" + companyName + "]" + AppMember.AppText["NotExsit"]);
                    string companyId = DataConvert.ToString(dtc.Rows[0]["departmentId"]);
                    string companyPath = DataConvert.ToString(dtc.Rows[0]["departmentPath"]);
                    paras.Add("companyId", companyId);
                    //部门
                    string departmentName = dtSource.Columns.Contains("使用部门") ?
                                         DataConvert.ToString(drSource["使用部门"]) : string.Empty;
                    if (departmentName == string.Empty)
                        throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                           AppMember.AppText["ImportDepartment"] + AppMember.AppText["NotNull"]);
                    string[] ds = departmentName.Split('/');
                    departmentName = ds[0];
                    paras.Add("departmentName", departmentName);
                    string sqld = @"select * from AppDepartment where departmentName=@departmentName and companyId=@companyId";
                    DataTable dtd = AppMember.DbHelper.GetDataSet(sqld, paras).Tables[0];
                    //if (dtd.Rows.Count < 1)
                    //    throw new Exception(AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                    //        AppMember.AppText["ImportDepartment"] + "[" + departmentName + "]" + AppMember.AppText["NotExsit"]);
                    string departmentId = "";
                    if (dtd.Rows.Count < 1)
                    {
                        dtd.TableName = "AppDepartment";
                        DataRow drc = dtd.NewRow();
                        departmentId = IdGenerator.GetMaxId(dtd.TableName);
                        drc["departmentId"] = departmentId;
                        drc["departmentNo"] = AutoNoGenerator.GetMaxNo("AppDepartment", "DEP", 4);
                        drc["departmentName"] = departmentName;
                        drc["parentId"] = companyId;
                        drc["departmentPath"] = companyPath + "," + departmentId;
                        drc["isCompany"] = "N";
                        drc["isHeaderOffice"] = "N";
                        drc["companyId"] = companyId;
                        dtd.Rows.Add(drc);
                        Create5Field(dtd, sysUser.UserId, viewTitle);
                        //DbUpdate = new DataUpdate();
                        DbUpdate.Update(dtd);
                    }
                    else
                    {
                        departmentId = DataConvert.ToString(dtd.Rows[0]["departmentId"]);
                    }
                    dr["departmentId"] = departmentId;
                    paras.Add("departmentId", departmentId);

                    //存放地点
                    string storeSiteName = dtSource.Columns.Contains("存放地点") ?
                                         DataConvert.ToString(drSource["存放地点"]) : string.Empty;
                    if (storeSiteName == string.Empty)
                        throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                           AppMember.AppText["ImportStoreSite"] + AppMember.AppText["NotNull"]);
                    //string[] ss = storeSiteName.Split('/');
                    //if(ss.Length>1)
                    //    storeSiteName = ss[1];
                    paras.Add("storeSiteName", storeSiteName);
                    string sql2 = @"select * from StoreSite where storeSiteName=@storeSiteName and companyId=@companyId";
                    DataTable dt2 = AppMember.DbHelper.GetDataSet(sql2, paras).Tables[0];
                    //if (dt2.Rows.Count < 1)
                    //    throw new Exception(dtSource.TableName+AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                    //        AppMember.AppText["ImportStoreSite"] + "[" + storeSiteName + "]" + AppMember.AppText["NotExsit"]);
                    if (dt2.Rows.Count < 1)
                    {
                        dt2.TableName = "StoreSite";
                        DataRow drc = dt2.NewRow();
                        string storeSiteId = IdGenerator.GetMaxId(dt2.TableName);
                        drc["storeSiteId"] = storeSiteId;
                        drc["storeSiteNo"] = AutoNoGenerator.GetMaxNo("StoreSite", "S", 4);
                        drc["storeSiteName"] = storeSiteName;
                        drc["parentId"] = companyId;
                        drc["storeSitePath"] = companyPath + "," + storeSiteId;
                        drc["companyId"] = companyId;
                        dt2.Rows.Add(drc);
                        Create5Field(dt2, sysUser.UserId, viewTitle);
                        //DbUpdate = new DataUpdate();
                        DbUpdate.Update(dt2);
                        dr["storeSiteId"] = storeSiteId;
                    }
                    else
                    {
                        dr["storeSiteId"] = DataConvert.ToString(dt2.Rows[0]["storeSiteId"]);
                    }

                    //购置方式
                    string purchaseTypeName = dtSource.Columns.Contains("购置方式") ?
                                         DataConvert.ToString(drSource["购置方式"]) : string.Empty;
                    if (purchaseTypeName != "")
                    {
                        paras.Add("purchaseTypeName", purchaseTypeName);
                        string sql3 = @"select purchaseTypeId from PurchaseType where purchaseTypeName=@purchaseTypeName";
                        DataTable dt3 = AppMember.DbHelper.GetDataSet(sql3, paras).Tables[0];
                        if (dt3.Rows.Count < 1)
                            throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                                AppMember.AppText["ImportPurchaseType"] + "[" + purchaseTypeName + "]" + AppMember.AppText["NotExsit"]);
                        dr["purchaseTypeId"] = DataConvert.ToString(dt3.Rows[0]["purchaseTypeId"]);
                    }

                    string purchaseDate = dtSource.Columns.Contains("入库日期") ?
                                        DataConvert.ToString(drSource["入库日期"]) : string.Empty;
                    //if (purchaseDate == string.Empty)
                    //    throw new Exception(dtSource.TableName+AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                    //       AppMember.AppText["ImportPurchaseDate"] + AppMember.AppText["NotNull"]);
                    dr["purchaseDate"] = dtSource.Columns.Contains("入库日期") ?
                                         drSource["入库日期"] : DBNull.Value;
                    dr["addDate"] = dtSource.Columns.Contains("入库日期") ?
                                         drSource["入库日期"] : DBNull.Value;
                    dr["assetsValue"] = dtSource.Columns.Contains("资产原值") ?
                                           drSource["资产原值"] : DBNull.Value;
                    dr["assetsNetValue"] = dtSource.Columns.Contains("资产原值") ?
                                                drSource["资产原值"] : DBNull.Value;
                    dr["durableYears"] = dtSource.Columns.Contains("可用年限") ?
                                       drSource["可用年限"] : DBNull.Value;
                    dr["remainRate"] = dtSource.Columns.Contains("残值率") ?
                                        drSource["残值率"] : DBNull.Value;
                    if (AppMember.DepreciationRuleOpen)
                    {
                        string depreciationRule2 = dtSource.Columns.Contains("折旧代码") ?
                                         DataConvert.ToString(drSource["折旧代码"]) : string.Empty;
                        if (depreciationRule2 != "")
                        {
                            paras.Add("depreciationRuleNo2", depreciationRule2);
                            string sql20 = @"select * from DepreciationRule where depreciationRuleNo=@depreciationRuleNo2";
                            DataTable dt20 = AppMember.DbHelper.GetDataSet(sql20, paras).Tables[0];
                            if (dt20.Rows.Count > 0)
                            {
                                dr["remainMonth"] = DataConvert.ToInt32(dt20.Rows[0]["totalMonth"]);
                            }
                        }
                    }
                    else
                    {
                        dr["remainMonth"] = DataConvert.ToInt32(dr["durableYears"]) * 12;
                    }

                    string assetsUsesName = dtSource.Columns.Contains("资产用途") ?
                                         DataConvert.ToString(drSource["资产用途"]) : string.Empty;
                    if (assetsUsesName != "")
                    {
                        paras.Add("assetsUsesName", assetsUsesName);
                        string sql6 = @"select assetsUsesId from AssetsUses where assetsUsesName=@assetsUsesName";
                        DataTable dt6 = AppMember.DbHelper.GetDataSet(sql6, paras).Tables[0];
                        if (dt6.Rows.Count < 1)
                            throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                                AppMember.AppText["ImportAssetsUses"] + "[" + assetsUsesName + "]" + AppMember.AppText["NotExsit"]);
                        dr["assetsUsesId"] = DataConvert.ToString(dt6.Rows[0]["assetsUsesId"]);
                    }

                    string usePeople = dtSource.Columns.Contains("使用人") ?
                                         DataConvert.ToString(drSource["使用人"]) : string.Empty;
                    if (usePeople != "")
                    {
                        if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
                        {
                            paras.Add("usePeople", usePeople);
                            string sql7 = @"select userId from AppUser where userName=@usePeople and departmentId=@departmentId ";
                            DataTable dt7 = AppMember.DbHelper.GetDataSet(sql7, paras).Tables[0];
                            if (dt7.Rows.Count < 1 && usePeople != "")
                                throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                                    AppMember.AppText["ImportUsePeople"] + "[" + usePeople + "]" + AppMember.AppText["NotExsit"]);
                            if (dt7.Rows.Count > 0)
                                dr["usePeople"] = DataConvert.ToString(dt7.Rows[0]["userId"]);
                        }
                        else
                        {
                            dr["usePeople"] = usePeople;
                            paras.Add("usePeopleName", usePeople);
                            string sql7 = @"select * from AppUser where userName=@usePeopleName";
                            DataTable dt7 = AppMember.DbHelper.GetDataSet(sql7, paras).Tables[0];
                            if (dt7.Rows.Count < 1)
                            {
                                dt7.TableName = "AppUser";
                                DataRow dr7 = dt7.NewRow();
                                string userId = IdGenerator.GetMaxId(dt7.TableName);
                                dr7["userId"] = userId;
                                dr7["userName"] = usePeople;
                                dr7["isSysUser"] = "N";
                                dt7.Rows.Add(dr7);
                                Create5Field(dt7, sysUser.UserId, viewTitle);
                                //DbUpdate = new DataUpdate();
                                DbUpdate.Update(dt7);
                            }
                        }
                    }

                    string keeper = dtSource.Columns.Contains("保管人") ?
                                         DataConvert.ToString(drSource["保管人"]) : string.Empty;
                    if (keeper != "")
                    {
                        if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.High)
                        {
                            paras.Add("keeper", keeper);
                            string sql8 = @"select userId from AppUser where userName=@keeper and departmentId=@departmentId ";
                            DataTable dt8 = AppMember.DbHelper.GetDataSet(sql8, paras).Tables[0];
                            if (dt8.Rows.Count < 1 && keeper != "")
                                throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                                    AppMember.AppText["ImportKeeper"] + "[" + keeper + "]" + AppMember.AppText["NotExsit"]);
                            if (dt8.Rows.Count > 0)
                                dr["keeper"] = DataConvert.ToString(dt8.Rows[0]["userId"]);
                        }
                        else
                        {
                            dr["keeper"] = keeper;
                            if (keeper != usePeople)
                            {
                                paras.Add("keeperName", keeper);
                                string sql8 = @"select * from AppUser where userName=@keeperName";
                                DataTable dt8 = AppMember.DbHelper.GetDataSet(sql8, paras).Tables[0];
                                if (dt8.Rows.Count < 1)
                                {
                                    dt8.TableName = "AppUser";
                                    DataRow dr8 = dt8.NewRow();
                                    string userId = IdGenerator.GetMaxId(dt8.TableName);
                                    dr8["userId"] = userId;
                                    dr8["userName"] = keeper;
                                    dr8["isSysUser"] = "N";
                                    dt8.Rows.Add(dr8);
                                    Create5Field(dt8, sysUser.UserId, viewTitle);
                                    //DbUpdate = new DataUpdate();
                                    DbUpdate.Update(dt8);
                                }
                            }
                        }
                    }


                    string equityOwnerName = dtSource.Columns.Contains("产权归属") ?
                                         DataConvert.ToString(drSource["产权归属"]) : string.Empty;
                    if (equityOwnerName != "")
                    {
                        paras.Add("equityOwnerName", equityOwnerName);
                        string sql4 = @"select equityOwnerId from EquityOwner where equityOwnerName=@equityOwnerName";
                        DataTable dt4 = AppMember.DbHelper.GetDataSet(sql4, paras).Tables[0];
                        if (dt4.Rows.Count < 1 && equityOwnerName != "")
                            throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                                AppMember.AppText["ImportEquityOwner"] + "[" + equityOwnerName + "]" + AppMember.AppText["NotExsit"]);
                        if (dt4.Rows.Count > 0)
                            dr["equityOwnerId"] = DataConvert.ToString(dt4.Rows[0]["equityOwnerId"]);
                    }

                    //string supplierName = dtSource.Columns.Contains("供应商") ?
                    //                     DataConvert.ToString(drSource["供应商"]) : string.Empty;
                    //dr["supplierName"] = supplierName;
                    //if (supplierName != "")
                    //{
                    //    paras.Add("supplierName", supplierName);
                    //    string sql5 = @"select supplierId from Supplier where supplierName=@supplierName";
                    //    DataTable dt5 = AppMember.DbHelper.GetDataSet(sql5, paras).Tables[0];
                    //    if (dt5.Rows.Count < 1 && supplierName != "")
                    //        throw new Exception(dtSource.TableName+AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                    //            AppMember.AppText["ImportSupplier"] + "[" + supplierName + "]" + AppMember.AppText["NotExsit"]);
                    //    if (dt5.Rows.Count > 0)
                    //        dr["supplierId"] = DataConvert.ToString(dt5.Rows[0]["supplierId"]);
                    //}


                    dr["assetsBarcode"] = dtSource.Columns.Contains("资产条码") ?
                                         DataConvert.ToString(drSource["资产条码"]) : string.Empty;
                    dr["depreciationType"] = dtSource.Columns.Contains("折旧方式") ?
                                         DataConvert.ToString(drSource["折旧方式"]) : string.Empty;
                    dr["spec"] = dtSource.Columns.Contains("规格型号") ?
                                         DataConvert.ToString(drSource["规格型号"]) : string.Empty;

                    string assetsState = dtSource.Columns.Contains("资产状态") ?
                                         DataConvert.ToString(drSource["资产状态"]) : "";
                    if (assetsState != "")
                    {
                        paras.Add("codeName", assetsState);
                        string sql19 = @"select codeNo from CodeTable where codeType='AssetsState' and codeName=@codeName";
                        DataTable dt19 = AppMember.DbHelper.GetDataSet(sql19, paras).Tables[0];
                        if (dt19.Rows.Count < 1 && assetsState != "")
                            throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                                AppMember.AppText["ImportAssetsState"] + "[" + assetsState + "]" + AppMember.AppText["NotExsit"]);
                        if (dt19.Rows.Count > 0)
                            dr["assetsState"] = DataConvert.ToString(dt19.Rows[0]["codeNo"]);
                    }
                    else
                    {
                        dr["assetsState"] = "A";
                    }

                    string depreciationRule = dtSource.Columns.Contains("折旧代码") ?
                                         DataConvert.ToString(drSource["折旧代码"]) : string.Empty;
                    if (depreciationRule != "")
                    {
                        paras.Add("depreciationRuleNo", depreciationRule);
                        string sql20 = @"select depreciationRuleId from DepreciationRule where depreciationRuleNo=@depreciationRuleNo";
                        DataTable dt20 = AppMember.DbHelper.GetDataSet(sql20, paras).Tables[0];
                        if (dt20.Rows.Count < 1 && depreciationRule != "")
                            throw new Exception(dtSource.TableName + AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                                AppMember.AppText["ImportDepreciationRule"] + "[" + depreciationRule + "]" + AppMember.AppText["NotExsit"]);
                        if (dt20.Rows.Count > 0)
                            dr["depreciationRule"] = DataConvert.ToString(dt20.Rows[0]["depreciationRuleId"]);
                    }

                    dr["assetsQty"] = dtSource.Columns.Contains("数量") ?
                                         drSource["数量"] : DBNull.Value;
                    dr["purchaseDate2"] = dtSource.Columns.Contains("购买日期") ?
                                         drSource["购买日期"] : DBNull.Value;
                    dr["checking"] = "N";

                    //dr["CEANo"] = dtSource.Columns.Contains("CEANo") ?
                    //                     DataConvert.ToString(drSource["CEANo"]) : string.Empty;
                    dr["TagMaterial"] = dtSource.Columns.Contains("品牌") ?
                                         DataConvert.ToString(drSource["品牌"]) : string.Empty;
                    dr["remark"] = dtSource.Columns.Contains("备注") ?
                                         DataConvert.ToString(drSource["备注"]) : string.Empty;

                    //dr["scrapDate"] = dtSource.Columns.Contains("报废日期") ?
                    //                     drSource["报废日期"] : DBNull.Value; 
                    dr["markMK"] = dtSource.Columns.Contains("序号") ?
                                         DataConvert.ToString(drSource["序号"]) : string.Empty;
                    //dr["mujuNo"] = dtSource.Columns.Contains("模具-供应商编号") ?
                    //                     DataConvert.ToString(drSource["模具-供应商编号"]) : string.Empty;
                    //dr["shengchanhuopinNo"] = dtSource.Columns.Contains("模具-生产货品货号") ?
                    //                     DataConvert.ToString(drSource["模具-生产货品货号"]) : string.Empty;
                    //dr["mujuxueshu"] = dtSource.Columns.Contains("模具-穴数") ?
                    //                     DataConvert.ToString(drSource["模具-穴数"]) : string.Empty;
                    //dr["mujushouming"] = dtSource.Columns.Contains("模具-使用寿命") ?
                    //                     DataConvert.ToString(drSource["模具-使用寿命"]) : string.Empty;
                    //dr["shejichannenng"] = dtSource.Columns.Contains("模具-设计产能") ?
                    //                     DataConvert.ToString(drSource["模具-设计产能"]) : string.Empty;
                    //dr["hadshejichannneng"] = dtSource.Columns.Contains("已使用产能") ?
                    //                     DataConvert.ToString(drSource["已使用产能"]) : string.Empty;

                    //dr["shiYongDiDian"] = dtSource.Columns.Contains("使用地点") ?
                    //                     DataConvert.ToString(drSource["使用地点"]) : string.Empty;

                    //string panDianHao = dtSource.Columns.Contains("盘点号") ?
                    //                  DataConvert.ToString(drSource["盘点号"]) : string.Empty;
                    //if (panDianHao == string.Empty)
                    //    throw new Exception(dtSource.TableName+AppMember.AppText["AssetsExcel"] + i.ToString() + AppMember.AppText["ImportRow"] +
                    //       AppMember.AppText["ImportPanDianHao"] + AppMember.AppText["NotNull"]);
                    //dr["panDianHao"] = panDianHao;

                    if (isUpdateAssets)
                    {
                        Update5Field(dtUpdateAssets, sysUser.UserId, viewTitle);
                        DbUpdate.Update(dtUpdateAssets);
                    }
                    else
                    {
                        dt.Rows.Add(dr);
                        Create5Field(dt, sysUser.UserId, viewTitle);
                    }

                    //AppLog.WriteLog(sysUser.UserName, LogType.Info, "Assets", string.Format(AppMember.AppText["LogAssetsImport"],  DataConvert.ToString(dt.Rows[0]["assetsNo"]),
                    // DataConvert.ToString(dt.Rows[0]["assetsName"]), DataConvert.ToString(dt.Rows[0]["assetsBarcode"])));

                    i++;

                }
            }

            return DbUpdate.Update(dt);
            //return dt;

        }

        public int Import(string filepath, string fileNames, UserInfo sysUser, string viewTitle)
        {
            int sum = 0;
            List<DataImport> fileList = JsonHelper.JSONStringToList<DataImport>(DataConvert.ToString(fileNames));
            List<DataImport> fileListSort = fileList.OrderBy(s => s.sortNo).ToList();
            foreach (DataImport di in fileListSort)
            {
                if (di.sortNo == "100")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportCompany(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "105")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportDepartment(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "110")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportPurchaseType(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "115")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportSetBooks(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "120")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportEquityOwner(ds.Tables[0], sysUser, viewTitle);

                }
                else if (di.sortNo == "125")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportScrapType(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "130")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportAssetsUses(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "135")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportStoreSite(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "140")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportUsers(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "145")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportSupplier(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "150")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    sum += ImportAssetsClass(ds.Tables[0], sysUser, viewTitle);
                }
                else if (di.sortNo == "155")
                {
                    DataSet ds = ExcelSqlConnection(filepath + di.fileName, "");
                    //DbUpdate = new DataUpdate();
                    //DbUpdate.BeginTransaction();
                    //foreach (DataTable dt in ds.Tables)
                    //{
                    sum += ImportAssets(ds, sysUser, viewTitle);
                    //}
                    //DbUpdate.Commit();
                }
            }
            return sum;
        }


    }
}
