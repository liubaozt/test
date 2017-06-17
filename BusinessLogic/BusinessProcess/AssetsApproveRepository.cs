using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;

namespace BusinessLogic.BusinessProcess
{
    public class AssetsApproveRepository : ListRepository
    {

        public AssetsApproveRepository()
        {
            DefaulteGridSortField = "assetsNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsNo") && DataConvert.ToString(paras["assetsNo"]) != "")
                whereSql += @" and Assets.assetsNo like '%'+@assetsNo+'%'";
            if (paras.ContainsKey("assetsName") && DataConvert.ToString(paras["assetsName"]) != "")
                whereSql += @" and Assets.assetsName like '%'+@assetsName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string lsql = "";
            if (paras.ContainsKey("approveMode"))
            {
                if (DataConvert.ToString(paras["approveMode"]) == "approve")
                {
                    lsql = @",AppApprove
                     where AppApprove.tableName='Assets' and AppApprove.approveState='O'
                      and Assets.assetsId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where Assets.createId=@approver and Assets.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                       (select CodeTable.codeName from CodeTable where Assets.approveState=CodeTable.codeNo and CodeTable.codeType='ApproveState' and CodeTable.languageVer='{1}' ) approveState,
                       (select userNo from AppUser where Assets.createId=AppUser.userId) createId,
                        Assets.createTime createTime ,
                        (select userNo from AppUser where Assets.updateId=AppUser.userId) updateId,
                        Assets.updateTime updateTime ,
                        Assets.updatePro updatePro
                from Assets {2} {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }

        public int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from Assets  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }
    }
}
