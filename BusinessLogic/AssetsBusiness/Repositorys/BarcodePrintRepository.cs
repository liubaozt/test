using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.BarcodePrint;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsPrint
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsClassName { get; set; }
        public string ImgDefault { get; set; }
        public string AssetsBarcode { get; set; }
        public string AssetsUsesName { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string StoreSiteName { get; set; }
        public string UsePeopleName { get; set; }
        public string KeeperName { get; set; }
        public string AssetsValue { get; set; }
        public string PurchaseDate { get; set; }
        public string EquityOwnerName { get; set; }
        public string Spec { get; set; }
        public string Remark { get; set; }
        public string ProjectManageName { get; set; }

    }


    public class BarcodePrintRepository : BaseRepository
    {

        protected WhereConditon EntryGridWhereSql(string formVar)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            if (formVar == null)
                wcd.Sql += " and 1<>1 ";
            Entry2Model model = JsonHelper.Deserialize<Entry2Model>(formVar);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsNo) != "")
            {
                wcd.Sql += @" and Assets.assetsNo like '%'+@assetsNo+'%'";
                wcd.DBPara.Add("assetsNo", model.AssetsNo);
            }
            if (DataConvert.ToString(model.AssetsName) != "")
            {
                wcd.Sql += @" and Assets.assetsName like '%'+@assetsName+'%'";
                wcd.DBPara.Add("assetsName", model.AssetsName);
            }
            if (DataConvert.ToString(model.AssetsBarcode) != "")
            {
                wcd.Sql += @" and Assets.assetsBarcode like '%'+@assetsBarcode+'%'";
                wcd.DBPara.Add("assetsBarcode", model.AssetsBarcode);
            }
            if (DataConvert.ToString(model.AssetsClassId) != "")
            {
                wcd.Sql += @" and Assets.assetsClassId=@assetsClassId";
                wcd.DBPara.Add("assetsClassId", model.AssetsClassId);
            }
            if (DataConvert.ToString(model.DepartmentId) != "")
            {
                wcd.Sql += @" and Assets.departmentId=@departmentId";
                wcd.DBPara.Add("departmentId", model.DepartmentId);
            }
            if (DataConvert.ToString(model.StoreSiteId) != "")
            {
                wcd.Sql += @" and Assets.storeSiteId=@storeSiteId";
                wcd.DBPara.Add("storeSiteId", model.StoreSiteId);
            }
            if (DataConvert.ToString(model.AssetsState) != "")
            {
                wcd.Sql += @" and Assets.assetsState=@assetsState";
                wcd.DBPara.Add("assetsState", model.AssetsState);
            }
            if (DataConvert.ToString(model.UsePeople) != "")
            {
                wcd.Sql += @" and  Assets.usePeople like '%'+@usePeople+'%'";
                wcd.DBPara.Add("usePeople", model.UsePeople);
            }
            if (DataConvert.ToString(model.Keeper) != "")
            {
                wcd.Sql += @" and  Assets.keeper like '%'+@keeper+'%'";
                wcd.DBPara.Add("keeper", model.Keeper);
            }
            if (DataConvert.ToString(model.ProjectManageId) != "")
            {
                wcd.Sql += @" and Assets.projectManageId=@projectManageId";
                wcd.DBPara.Add("projectManageId", model.ProjectManageId);
            }
            if (DataConvert.ToString(model.PurchaseDateFrom) != "")
            {
                wcd.Sql += @" and Assets.purchaseDate>=@purchaseDateFrom";
                wcd.DBPara.Add("purchaseDateFrom", DataConvert.ToString(model.PurchaseDateFrom) + " 00:00:00");
            }
            if (DataConvert.ToString(model.PurchaseDateTo) != "")
            {
                wcd.Sql += @" and Assets.purchaseDate<=@purchaseDateTo";
                wcd.DBPara.Add("purchaseDateTo", DataConvert.ToString(model.PurchaseDateTo) + " 23:59:59");
            }
            if (DataConvert.ToString(model.CompanyId) != "")
            {
                wcd.Sql += @" and D.companyId=@companyId";
                wcd.DBPara.Add("companyId", model.CompanyId);
            }
            return wcd;
        }

        private string DuetoDateSql()
        {
            if (AppMember.DepreciationRuleOpen)
            {
                return " (SELECT DATEADD(month,DepreciationRule.totalmonth,purchasedate)  from DepreciationRule where assets.depreciationRule=DepreciationRule.depreciationRuleId ) ";
            }
            else
            {
                return " (SELECT DATEADD(year,durableyears,purchasedate)) ";
            }
        }

        public DataTable GetEntryGridDataTable(string formVar, string formMode, string primaryKey)
        {
            //            string sql = string.Format(@"select Assets.*,
            //                                        AppDepartment.departmentName departmentName,  
            //                                        StoreSite.storeSiteName storeSiteName,  
            //                                        A.userName usePeopleName,  
            //                                        B.userName keeperName,  
            //                                        AssetsUses.assetsUsesName assetsUsesName,  
            //                                        AssetsClass.assetsClassName assetsClassName,
            //                                        EquityOwner.equityOwnerName equityOwnerName,
            //                                        ProjectManage.projectManageName projectManageName
            //                                        from Assets
            //                                        left join AppDepartment on AppDepartment.departmentId=Assets.departmentId
            //                                        left join StoreSite on StoreSite.storeSiteId=Assets.storeSiteId 
            //                                        left join AssetsUses on AssetsUses.assetsUsesId=Assets.assetsUsesId 
            //                                        left join AssetsClass on AssetsClass.assetsClassId=Assets.assetsClassId
            //                                        left join EquityOwner on EquityOwner.equityOwnerId=Assets.equityOwnerId
            //                                        left join ProjectManage on ProjectManage.projectManageId=Assets.projectManageId
            //                                        left join AppUser A on A.userId=Assets.usePeople 
            //                                        left join AppUser B on B.userId=Assets.keeper  where 1=1 ");
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsBarcode AssetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) AssetsClassName,
	                (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
	                (select assetsUsesName from AssetsUses where Assets.assetsUsesId=AssetsUses.assetsUsesId) AssetsUsesName,
                   (select departmentName from AppDepartment C where D.companyId=C.departmentId ) CompanyName,
                    Assets.departmentId departmentId,
	                 D.departmentName DepartmentName,
	                Assets.storeSiteId storeSiteId,
                   (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) StoreSiteName,
                    Assets.usePeople usePeople,
	                isnull((select userName from AppUser where Assets.usePeople=AppUser.userId),Assets.usePeople) UsePeopleName,
                    Assets.keeper keeper,
	                isnull((select userName from AppUser where Assets.keeper=AppUser.userId),Assets.keeper) keeperName,
                    Assets.assetsValue assetsValue,
                    Assets.assetsNetValue assetsNetValue,
                    Assets.imgDefault ImgDefault,
                    Assets.AssetsNo AssetsBarcode,
                   convert(nvarchar(100),Assets.purchaseDate,23)   purchaseDate,
                      convert(nvarchar(100),{1},23) DuetoDate,
	                (select CodeTable.codeName from CodeTable where Assets.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{0}' ) assetsState,
                     (select equityOwnerName from EquityOwner where Assets.equityOwnerId=EquityOwner.equityOwnerId) equityOwnerName,
                     (select projectManageName from ProjectManage where Assets.projectManageId=ProjectManage.projectManageId) projectManageName,
                      Assets.spec spec,
                      Assets.remark remark
                from Assets 
               left join AppDepartment D on Assets.departmentId=D.departmentId 
                where 1=1  ", AppMember.AppLanguage.ToString(), DuetoDateSql());
            WhereConditon wcd = EntryGridWhereSql(formVar);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql + wcd.Sql, wcd.DBPara).Tables[0];
            return dtGrid;
        }

        public DataTable GetStoreSiteGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey)
        {
            string sql = string.Format(@"select  StoreSite.storeSiteId storeSiteId,
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
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public int Update(Entry2Model model, UserInfo sysUser, string viewTitle)
        {
            if (model.FormMode == "new")
            {
                return Add(model, sysUser, viewTitle);
            }
            else if (model.FormMode == "modified")
            {
                return Modified(model, sysUser, model.BarcodeStyleId, viewTitle);
            }
            else if (model.FormMode == "delete")
            {
                return Delete(model, sysUser, model.BarcodeStyleId, viewTitle);
            }
            else
                return 0;

        }

        protected int Add(Entry2Model model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppLabelStyle where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppLabelStyle";
            DataRow dr = dt.NewRow();
            dr["styleId"] = IdGenerator.GetMaxId(dt.TableName);
            dr["styleName"] = model.BarcodeStyleName;
            dr["xmlString"] = model.XmlString;
            dr["labelType"] = model.LabelType;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }
        protected int Modified(Entry2Model model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("styleId", pkValue);
            string sql = @"select * from AppLabelStyle where styleId=@styleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppLabelStyle";
            //dt.Rows[0]["styleName"] = model.BarcodeStyleName;
            dt.Rows[0]["xmlString"] = model.XmlString;
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        public int UpdateByPrintTools(string labelType, string XmlString, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("labelType", labelType);
            string sql = @"select * from AppLabelStyle where labelType=@labelType";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppLabelStyle";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["xmlString"] = XmlString;
                }
                Update5Field(dt, "", viewTitle);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["styleId"] = IdGenerator.GetMaxId(dt.TableName);
                dr["styleName"] = labelType == "Z" ? "AssetsLabel" : "StoreSiteLabel";
                dr["xmlString"] = XmlString;
                dr["labelType"] = labelType;
                dt.Rows.Add(dr);
                Create5Field(dt, "", viewTitle);
            }
            return DbUpdate.Update(dt);
        }

        public int UpdateByPrintTools(string labelType, string _LabelId, string _LabelName, string XmlString, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("labelType", labelType);
            paras.Add("LabelId", _LabelId);
            string sql = @"select * from AppLabelStyle where labelType=@labelType and LabelId=@LabelId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppLabelStyle";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["styleName"] = _LabelName;
                    dr["xmlString"] = XmlString;
                }
                Update5Field(dt, "", viewTitle);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["styleId"] = IdGenerator.GetMaxId(dt.TableName);
                dr["LabelId"] = _LabelId;
                dr["styleName"] = _LabelName;
                dr["xmlString"] = XmlString;
                dr["labelType"] = labelType;
                dt.Rows.Add(dr);
                Create5Field(dt, "", viewTitle);
            }
            return DbUpdate.Update(dt);
        }

        public int UpdateDefaultStyle(string labelType, string _LabelId, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("labelType", labelType);
            string sql = @"select * from AppLabelStyle where labelType=@labelType ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppLabelStyle";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (DataConvert.ToString(dr["LabelId"]) == _LabelId)
                        dr["isDefault"] = 1;
                    else
                        dr["isDefault"] = 0;
                }
                Update5Field(dt, "", viewTitle);
            }
            return DbUpdate.Update(dt);
        }

        public int DeleteStyle(string labelType, string _LabelId, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("labelType", labelType);
            paras.Add("LabelId", _LabelId);
            string sql = @"select * from AppLabelStyle where labelType=@labelType and  LabelId=@LabelId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppLabelStyle";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr.Delete();
                }
            }
            return DbUpdate.Update(dt);
        }


        protected int Delete(Entry2Model model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("styleId", pkValue);
            string sql = @"select * from AppLabelStyle where styleId=@styleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppLabelStyle";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        public DataTable GetDropListSource()
        {
            string sql = @"select * from AppLabelStyle  order by styleId ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public string GetLabelStyle(string styleId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("styleId", styleId);
            string sql = @"select * from AppLabelStyle where styleId=@styleId ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToString(dtGrid.Rows[0]["xmlString"]);
        }

        public string GetLabelStyleByType(string labelType)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("labelType", labelType);
            string sql = @"select * from AppLabelStyle where labelType=@labelType and isDefault=1 ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dtGrid.Rows.Count > 0)
                return DataConvert.ToString(dtGrid.Rows[0]["xmlString"]);
            else
            {
                sql = @"select top 1 * from AppLabelStyle where labelType=@labelType";
                DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                if (dt.Rows.Count > 0)
                    return DataConvert.ToString(dt.Rows[0]["xmlString"]);
                else
                    return "";
            }
        }

        public DataTable GetLabelStyleByPrintTools(string labelType)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("labelType", labelType);
            string sql = @"select LabelId,styleName LabelName,xmlString LabelContent,isDefault from AppLabelStyle where labelType=@labelType ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public string GetDefaultLabelStyle(string formMode)
        {
            string sql = @"select min(styleId) styleId from AppLabelStyle where 1=1 ";
            if (formMode == "new2")
                sql += @" and labelType='A'";
            else if (formMode == "storeSite")
                sql += @" and labelType='S'";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            if (dtGrid.Rows.Count > 0)
            {
                string styleId = DataConvert.ToString(dtGrid.Rows[0]["styleId"]);
                Dictionary<string, object> paras2 = new Dictionary<string, object>();
                paras2.Add("styleId", styleId);
                string sql2 = @"select * from AppLabelStyle where styleId=@styleId ";
                DataTable dtGrid2 = AppMember.DbHelper.GetDataSet(sql2, paras2).Tables[0];
                if (dtGrid2.Rows.Count > 0)
                    return DataConvert.ToString(dtGrid2.Rows[0]["xmlString"]);
                else
                    return "";
            }
            else
            {
                return "";
            }

        }

        public List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["styleId"]);
                dropList.Text = DataConvert.ToString(dr["styleName"]);
                dropList.Filter = DataConvert.ToString(dr["styleName"]);
                list.Add(dropList);
            }
            return list;
        }
    }
}
