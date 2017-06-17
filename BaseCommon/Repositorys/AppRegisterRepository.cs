using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;


namespace BaseCommon.Repositorys
{
    public class AppRegisterRepository : BaseRepository
    {
        public int AddRegister(string registerNo, string companyName)
        {
            string sql = @"select * from AppRegister";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppRegister";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            DataRow dr2 = dt.NewRow();
            dr2["registerNo"] = registerNo;
            dr2["companyName"] = companyName;
            dr2["registerid"] = IdGenerator.GetMaxId(dt.TableName);
            dr2["createDate"] = IdGenerator.GetServerDate();
            dt.Rows.Add(dr2);
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dt);

        }

        public string GetMachineInfo()
        {
           return AppSecurity.MacIpAddress.GetMacAddress();
        }
    }
}
