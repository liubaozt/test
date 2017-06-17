using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BaseCommon.Basic;
using System.Data.Common;
using System.Configuration;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.AssetsBusiness.Repositorys;
using BusinessLogic.AssetsBusiness.Models.BarcodePrint;
using BaseCommon.Init;

namespace WebApp.WebService
{
    /// <summary>
    /// DataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {
        /// <summary>
        /// 资产列头
        /// </summary>
        /// <param name="_space"></param>
        /// <returns></returns>
        [WebMethod]
        public int fun1(out string _assetheader)
        {
            _assetheader = "资产编号|资产名称|资产分类|资产条码|部门|存放地点|序列号|入库日期|到期日期|使用人|保管人|公司";
            return _assetheader.Split("|".ToCharArray()).Length;
        }

        /// <summary>
        /// 资产数据
        /// </summary>
        /// <param name="_expression"></param>
        /// <param name="_table"></param>
        /// <returns></returns>
        [WebMethod]
        public int fun2(string _assetExpression, out DataTable _table)
        {
            AppInit.Init();
            string sql = string.Format(@"select  Assets.assetsBarcode,
                                        Assets.assetsName,
                                        AssetsClass.assetsClassName assetsClassName,
                                        Assets.assetsNo,  
                                        D.departmentName departmentName,  
                                         StoreSite.storeSiteName storeSiteName,  
                                        Assets.spec,
                                        Assets.purchaseDate,
                                        {1} DuetoDate,
                                        A.userName usePeopleName,  
                                        B.userName keeperName,  
                                       (select departmentName from AppDepartment C where D.companyId=C.departmentId ) CompanyName
                                        from Assets
                                        left join AppDepartment D on D.departmentId=Assets.departmentId
                                        left join StoreSite on StoreSite.storeSiteId=Assets.storeSiteId 
                                        left join AssetsUses on AssetsUses.assetsUsesId=Assets.assetsUsesId 
                                        left join AssetsClass on AssetsClass.assetsClassId=Assets.assetsClassId
                                        left join EquityOwner on EquityOwner.equityOwnerId=Assets.equityOwnerId
                                        left join ProjectManage on ProjectManage.projectManageId=Assets.projectManageId
                                        left join AppUser A on A.userId=Assets.usePeople 
                                        left join AppUser B on B.userId=Assets.keeper  where 1=1 {0}", _assetExpression, DuetoDateSql());
            _table = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            _table.TableName = "Assets";
            return 1;
        }

        /// <summary>
        /// 存放地点列头
        /// </summary>
        /// <param name="_space"></param>
        /// <returns></returns>
        [WebMethod]
        public int fun3(out string _storeSiteheader)
        {
            _storeSiteheader = "存放地点编码|存放地点名称|上级地点|公司";
            return 1;
        }

        /// <summary>
        /// 存放地点数据
        /// </summary>
        /// <param name="_expression"></param>
        /// <param name="_table"></param>
        /// <returns></returns>
        [WebMethod]
        public int fun4(string _storeSiteExpression, out DataTable _table)
        {
            AppInit.Init();
            string sql = string.Format(@"select 
                                 StoreSite.storeSiteNo storeSiteNo,
                                 StoreSite.storeSiteName storeSiteName,
                                 T.storeSiteName parentStoreSiteName,
                                 D.departmentName companyName
                          from StoreSite left join AppUser U1 on StoreSite.createId=U1.userId
                                    left join AppUser U2 on StoreSite.updateId=U2.userId 
                                    left join StoreSite T on StoreSite.parentId=T.storeSiteId
                                    left join AppDepartment D on StoreSite.companyId=D.departmentId
                          where 1=1 {0}", _storeSiteExpression);
            _table = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return 1;
        }

        /// <summary>
        /// 获取标签样式
        /// </summary>
        /// <param name="_class"></param>
        /// <param name="_labelStyle"></param>
        /// <returns></returns>
        [WebMethod]
        public int fun5(int _class, out string _labelStyle)
        {
            AppInit.Init();
            BarcodePrintRepository repository = new BarcodePrintRepository();
            if (_class == 0)
            {
                _labelStyle = repository.GetLabelStyleByType("Z");
            }
            else
            {
                _labelStyle = repository.GetLabelStyleByType("C");
            }
            return 1;
        }

        /// <summary>
        /// 更新标签样式,已废弃。
        /// </summary>
        /// <param name="_class"></param>
        /// <param name="_labelStyle"></param>
        /// <returns></returns>
        [WebMethod]
        public int fun6(int _class, string _labelStyle)
        {
            AppInit.Init();
            DataUpdate dbUpdate = new DataUpdate();
            BarcodePrintRepository rep = new BarcodePrintRepository();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                string labelType = "";
                if (_class == 0)
                    labelType = "Z";
                else
                    labelType = "C";
                rep.UpdateByPrintTools(labelType, _labelStyle, "Print Tools Update");
                dbUpdate.Commit();
                return 1;
            }
            catch (Exception)
            {
                dbUpdate.Rollback();
                return 0;
            }
            finally
            {
                dbUpdate.Close();
            }
        }

        [WebMethod] // 取得所有模板数据.LabelId, LabelName, LabelContent, IsDefault
        public int fun7(int _class, out DataTable _table)
        {
            AppInit.Init();
            BarcodePrintRepository repository = new BarcodePrintRepository();
            if (_class == 0)
            {
                _table = repository.GetLabelStyleByPrintTools("Z");
            }
            else
            {
                _table = repository.GetLabelStyleByPrintTools("C");
            }
            return 1;
        }

        [WebMethod]//保存模板
        public int fun8(int _class, string _LabelId, string _LabelName, string _LabelContent)
        {
            AppInit.Init();
            DataUpdate dbUpdate = new DataUpdate();
            BarcodePrintRepository rep = new BarcodePrintRepository();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                string labelType = "";
                if (_class == 0)
                    labelType = "Z";
                else
                    labelType = "C";
                rep.UpdateByPrintTools(labelType, _LabelId, _LabelName, _LabelContent, "Print Tools Update");
                dbUpdate.Commit();
                return 1;
            }
            catch (Exception)
            {
                dbUpdate.Rollback();
                return 0;
            }
            finally
            {
                dbUpdate.Close();
            }
        }


        [WebMethod] //设置指定模板为默认模板
        public int fun9(int _class, string _LabelId)
        {
            AppInit.Init();
            DataUpdate dbUpdate = new DataUpdate();
            BarcodePrintRepository rep = new BarcodePrintRepository();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                string labelType = "";
                if (_class == 0)
                    labelType = "Z";
                else
                    labelType = "C";
                rep.UpdateDefaultStyle(labelType, _LabelId, "Print Tools Update");
                dbUpdate.Commit();
                return 1;
            }
            catch (Exception)
            {
                dbUpdate.Rollback();
                return 0;
            }
            finally
            {
                dbUpdate.Close();
            }
        }

        [WebMethod] // 删除指定模板
        public int fun10(int _class, string _LabelId)
        {
            AppInit.Init();
            DataUpdate dbUpdate = new DataUpdate();
            BarcodePrintRepository rep = new BarcodePrintRepository();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                string labelType = "";
                if (_class == 0)
                    labelType = "Z";
                else
                    labelType = "C";
                rep.DeleteStyle(labelType, _LabelId, "Print Tools Update");
                dbUpdate.Commit();
                return 1;
            }
            catch (Exception)
            {
                dbUpdate.Rollback();
                return 0;
            }
            finally
            {
                dbUpdate.Close();
            }
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


    }
}
