using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Collections;
using BusinessCommon.Models.PasswordModifiy;
using BaseCommon.Repositorys;

namespace BusinessCommon.Repositorys
{
    public class PasswordModifiyRepository : BaseRepository
    {
        public int ModifiyPassword(EntryModel model, string userId)
        {
            if (DataConvert.ToString(model.NewPassword) != DataConvert.ToString(model.NewPassword2))
                return 0;
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            string sql = @"select * from AppUser where userId=@userId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppUser";
            dt.Rows[0]["userPwd"] = AppSecurity.Encryption.Encryt(model.NewPassword);
            Update5Field(dt, userId, model.ViewTitle);
            return DbUpdate.Update(dt);
        }

        public int CheckOldPassword(string oldPassword)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("oldPassword", AppSecurity.Encryption.Encryt(oldPassword));
            string sql = @"select * from AppUser where userPwd=@oldPassword";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dt.Rows.Count > 0)
                return 1;
            else
                return 0;
        }
    }
}
