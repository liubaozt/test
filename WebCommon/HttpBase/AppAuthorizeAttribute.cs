using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using BaseCommon.Data;
using WebCommon.Init;
using System.Data;
using BaseCommon.Basic;

namespace WebCommon.HttpBase
{
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HasAuthorize(httpContext))
                return true;
            else
                return false;
        }

        protected bool HasAuthorize(HttpContextBase httpContext)
        {
            if (DataConvert.ToString(httpContext.User.Identity.Name) == "")
            {
                httpContext.Response.Redirect("~/Home/Error");
            }
            UserInfo sysUser = CacheInit.GetUserInfo(httpContext);
            string url = httpContext.Request.Path;
            string[] urls = url.Split('/');
            if (urls.Length > 4)
            {
                url =  "/" + urls[urls.Length-3]+ "/" + urls[urls.Length-2]+"/" + urls[urls.Length-1];
            }
            string formMode = DataConvert.ToString(httpContext.Request.QueryString["formMode"]);
            if (formMode == "")
                formMode = DataConvert.ToString(httpContext.Request.Form["formMode"]);
            string listMode = DataConvert.ToString(httpContext.Request.QueryString["listMode"]);
            if (listMode == "")
                listMode = DataConvert.ToString(httpContext.Request.Form["listMode"]);
            string sql = "";
            Dictionary<string, object> paras = new Dictionary<string, object>();

            if (listMode != "")
                url += "/?listMode=" + listMode;
            if (formMode != "")
                url += "/?formMode=" + formMode;
            paras.Add("userId", sysUser.UserId);
            paras.Add("url", url);
            sql = @"select A.url
                          from AppAuthority,(select AppMenu.menuId, (case when formMode!='' and formMode is not null then url+'/?formMode='+formMode
                                            else
                                            url
                                            end) url
                                            from AppMenu ) A
                    where AppAuthority.menuId=A.menuId
                    and AppAuthority.userId=@userId
                    and A.url=@url ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dtGrid.Rows.Count > 0)
                return true;
            else
            {
                paras.Add("groupId", sysUser.GroupId);
                sql = @"select A.url
                          from AppAuthority,(select AppMenu.menuId, (case when formMode!='' and formMode is not null then url+'/?formMode='+formMode
                                            else
                                            url
                                            end) url
                                            from AppMenu ) A
                    where AppAuthority.menuId=A.menuId
                    and AppAuthority.groupId=@groupId
                    and A.url=@url ";
                 dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                 if (dtGrid.Rows.Count > 0)
                     return true;
                 else
                     return false;
            }
               
        }
    }
}
