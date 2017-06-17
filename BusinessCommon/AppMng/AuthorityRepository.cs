using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Collections;

namespace BusinessCommon.AppMng
{
    public class AuthorityRepository
    {

        public DataTable GetUserMenu(string userId)
        {
            
            string sql = @"select AppMenu.menuId,
                        AppMenu.menuName,
                        AppMenu.url,
                        AppMenu.appLevel,
                        AppMenu.fromId,
                        AppMenu.toId,
                        AppMenu.isA,
                        AppMenu.isB
                         from AppAuthority,AppMenu
                        where AppAuthority.menuId=AppMenu.menuId
                        and AppMenu.isBtnMenu=0
                        and AppAuthority.userId=@userId
                        and AppMenu.languageVer=@languageVer";
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            paras.Add("languageVer", AppMember.AppLanguage.ToString());
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dtGrid.Rows.Count <= 0)
            {
                sql = @"select groupId from AppUser
                         where userId=@userId";
                dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                string groupId = DataConvert.ToString(dtGrid.Rows[0]["groupId"]);
                paras.Add("groupId", groupId);
                sql = @"select AppMenu.menuId,
                        AppMenu.menuName,
                        AppMenu.url,
                        AppMenu.appLevel,
                        AppMenu.fromId,
                        AppMenu.toId,
                        AppMenu.isA,
                        AppMenu.isB
                         from AppAuthority,AppMenu
                        where AppAuthority.menuId=AppMenu.menuId
                        and AppMenu.isBtnMenu=0
                        and AppAuthority.groupId=@groupId
                        and AppMenu.languageVer=@languageVer";
                dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];

            }
            return dtGrid;
        }

        public DataTable GetUserGridButton(string userId, string parentId, string gridId)
        {
            string sql = @"select AppMenu.menuId ,
                        AppMenu.menuName text,
                        AppMenu.url url,
                        AppMenu.btnId btnId,
                        AppMenu.gridId gridId,
                        AppMenu.parentId pageId,
                        AppMenu.formMode formMode
                         from AppAuthority,AppMenu,AppUser
                        where AppAuthority.menuId=AppMenu.menuId
                        and AppAuthority.userId=AppUser.userId 
                        and AppMenu.isBtnMenu=1
                        and AppUser.userId=@userId
                        and AppMenu.parentId=@parentId
                        and AppMenu.languageVer=@languageVer";
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            paras.Add("parentId", parentId);
            paras.Add("languageVer", AppMember.AppLanguage.ToString());
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dtGrid.Rows.Count < 1)
            {
                sql = @"select AppMenu.menuId ,
                        AppMenu.menuName text,
                        AppMenu.url url,
                        AppMenu.btnId btnId,
                        AppMenu.gridId gridId,
                        AppMenu.parentId pageId,
                        AppMenu.formMode formMode
                         from AppAuthority,AppMenu,AppUser
                        where AppAuthority.menuId=AppMenu.menuId
                        and AppAuthority.groupId=AppUser.groupId 
                        and AppMenu.isBtnMenu=1
                        and AppUser.userId=@userId
                        and AppMenu.parentId=@parentId
                        and AppMenu.languageVer=@languageVer";
                dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            }
            return dtGrid;
        }

        public DataTable GetAuthorityTree(string condition, bool isGroup)
        {
            string sql = "";
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("condition", condition);
            if (isGroup)
                sql = @"select menuId from AppAuthority where groupId=@condition";
            else
                sql = @"select menuId from AppAuthority where userId=@condition";
            DataTable dtAuthority = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            paras.Clear();
            paras.Add("languageVer", AppMember.AppLanguage.ToString());
            sql = @"select menuId,parentId,menuName,isOpen,'false' checked from AppMenu where  languageVer=@languageVer";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dt.Rows.Count > 0 && dtAuthority.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow dr2 in dtAuthority.Rows)
                    {
                        if (DataConvert.ToString(dr["menuId"]) == DataConvert.ToString(dr2["menuId"]))
                        {
                            dr["checked"] = "true";
                        }
                    }

                }
            }
            return dt;
        }

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

        public int SaveForGroup(string groupId, string authorityTree, UserInfo sysUser)
        {
            string menustr = authorityTree.Substring(0, authorityTree.Length - 1);
            string[] treeData = menustr.Split(',');
            ArrayList sql = new ArrayList();
            sql.Add("delete from AppAuthority where groupId='" + groupId + "'");
            for (int i = 0; i < treeData.Length; i++)
            {
                sql.Add("insert into AppAuthority(authId,groupId,menuId,createId,createTime)" +
                " values ('" + IdGenerator.GetMaxId("AppAuthority") + "','" + groupId + "','" + treeData[i] + "','" + sysUser.UserId + "','" + IdGenerator.GetServerDate() + "')");
            }
            return AppMember.DbHelper.ExecuteSqlTran(sql);
        }

        public int SaveForUser(string userId, string authorityTree, UserInfo sysUser)
        {
            string menustr = authorityTree.Substring(0, authorityTree.Length - 1);
            string[] treeData = menustr.Split(',');
            ArrayList sql = new ArrayList();
            sql.Add("delete from AppAuthority where userId='" + userId + "'");
            for (int i = 0; i < treeData.Length; i++)
            {
                sql.Add("insert into AppAuthority(authId,userId,menuId,createId,createTime)" +
                " values ('" + IdGenerator.GetMaxId("AppAuthority") + "','" + userId + "','" + treeData[i] + "','" + sysUser.UserId + "','" + IdGenerator.GetServerDate() + "')");
            }
            return AppMember.DbHelper.ExecuteSqlTran(sql);
        }

    }
}
