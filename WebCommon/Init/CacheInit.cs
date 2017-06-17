using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BusinessCommon.Repositorys;
using System.Web.Caching;
using BaseCommon.Data;
using BaseCommon.Basic;

namespace WebCommon.Init
{
    public class CacheInit
    {

        public static UserInfo GetUserInfo(HttpContextBase HttpContext,bool isLogin=false)
        {
            if (DataConvert.ToString(HttpContext.User.Identity.Name) == "")
            {
                HttpContext.Response.Redirect("~/Home/Error");
            }
            if (HttpContext.Session["CurSetBooks"] == null)
            {
                HttpContext.Response.Redirect("~/Home/Error");
            }
            DataTable dtUsers = new DataTable();
            if (HttpContext.Cache["UserInfoList"] == null || isLogin)
            {
                UserRepository rep = new UserRepository();
                DataTable list = rep.GetUserInfo();
                HttpContext.Cache.Remove("UserInfoList");
                HttpContext.Cache.Add("UserInfoList", list, null, DateTime.Now.AddHours(5), TimeSpan.Zero, CacheItemPriority.High, null);
                dtUsers = list;
            }
            else
            {
                 dtUsers = (DataTable)HttpContext.Cache["UserInfoList"];
            }
            DataRow[] drs = dtUsers.Select("userNo='" + HttpContext.User.Identity.Name + "'");
            UserInfo cuuruser = new UserInfo();
            cuuruser.UserId = DataConvert.ToString(drs[0]["userId"]);
            cuuruser.UserNo = HttpContext.User.Identity.Name;
            cuuruser.UserName = DataConvert.ToString(drs[0]["userName"]);
            cuuruser.GroupId = DataConvert.ToString(drs[0]["groupId"]);
            cuuruser.GroupNo = DataConvert.ToString(drs[0]["groupNo"]);
            cuuruser.GroupName = DataConvert.ToString(drs[0]["groupName"]);
            cuuruser.CompanyId = DataConvert.ToString(drs[0]["companyId"]);
            cuuruser.CompanyNo = DataConvert.ToString(drs[0]["companyNo"]);
            cuuruser.CompanyName = DataConvert.ToString(drs[0]["companyName"]);
            cuuruser.IsHeaderOffice = DataConvert.ToString(drs[0]["isHeaderOffice"]);
            cuuruser.IsSysUser = DataConvert.ToString(drs[0]["isSysUser"]);
            //cuuruser.DepartmentId = DataConvert.ToString(drs[0]["departmentId"]);
            //cuuruser.DepartmentNo = DataConvert.ToString(drs[0]["departmentNo"]);
            //cuuruser.DepartmentName = DataConvert.ToString(drs[0]["departmentName"]);
            cuuruser.AccessLevel = DataConvert.ToString(drs[0]["accessLevel"]);
            cuuruser.MySetBooks = (CurSetBooks)HttpContext.Session["CurSetBooks"];
            cuuruser.Departments = new List<DepartmentInfo>();
            foreach (DataRow dr in drs)
            {
                if (!DataConvert.ToString(dr["departmentId"]).StartsWith("CMY"))
                {
                    cuuruser.Departments.Add(new DepartmentInfo
                    {
                        DepartmentId = DataConvert.ToString(dr["departmentId"]),
                        DepartmentName = DataConvert.ToString(dr["departmentName"]),
                        DepartmentNo = DataConvert.ToString(dr["departmentNo"])
                    });
                }
            }
            return cuuruser;
        }

        public static void CreateGridLayoutCache(HttpContextBase HttpContext,string areaName, string layoutName)
        {
            GridLayout layout = XmlHelper.GetGridLayout(areaName, layoutName);
            HttpContext.Cache.Add(layoutName + "GridLayout", layout, null, DateTime.Now.AddHours(5), TimeSpan.Zero, CacheItemPriority.High, null);
        }

        public static void CreateAdvanceGridLayoutCache(HttpContextBase HttpContext, string areaName, string layoutName)
        {
            AdvanceGridLayout layout = XmlHelper.GetAdvanceGridLayout(areaName, layoutName);
            HttpContext.Cache.Add(layoutName + "GridLayout", layout, null, DateTime.Now.AddHours(5), TimeSpan.Zero, CacheItemPriority.High, null);
        }

        public static void CreateGridLayoutCache(HttpContextBase HttpContext, string areaName, string layoutName, string formMode,string  cacheName)
        {
            GridLayout layout = new GridLayout();
            layout.GridTitle = XmlHelper.GetGridTitle(areaName, layoutName);
            layout.GridLayouts = XmlHelper.GetGridLayout(areaName, layoutName,  formMode);
            HttpContext.Cache.Add(cacheName, layout, null, DateTime.Now.AddHours(5), TimeSpan.Zero, CacheItemPriority.High, null);
        }

        public static void RefreshCache(HttpContextBase HttpContext, IEntry rep, string cacheName, DateTime ctime, CacheItemPriority priority, CacheCreateType createType)
        {

            if (createType == CacheCreateType.IfNoThenGenerated)
            {
                if (HttpContext.Cache[cacheName] == null)
                {
                    DataTable list = rep.GetDropListSource();
                    HttpContext.Cache.Add(cacheName, list, null, ctime, TimeSpan.Zero, priority, null);
                }
            }
            else if (createType == CacheCreateType.Immediately)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Remove(cacheName);
                HttpContext.Cache.Add(cacheName, list, null, ctime, TimeSpan.Zero, priority, null);
            }


        }

    }
}