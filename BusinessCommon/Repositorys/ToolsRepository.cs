using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Collections;

namespace BusinessCommon.Repositorys
{
    public class ToolsRepository
    {
        public DataTable GetAuthorityTreeId(string condition, bool isGroup)
        {
            string sql = "";
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("condition", condition);
            if (isGroup)
                sql = @"select menuId from AppAuthority where groupId=@condition";
            else
                sql = @"select menuId from AppAuthority where userId=@condition";
            DataTable dtAuthority = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtAuthority;
        }
    }
}
