using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessCommon.Repositorys;
using WebApp.BaseWeb.Common;
using WebApp.BaseWeb.Init;
using BaseCommon.Data;
using System.Web.Caching;
using BusinessLogic.BasicData;
using BusinessLogic.BasicData.Repositorys;
using System.Data;
using BaseCommon.Basic;

namespace WebApp.Controllers
{
    public class DropListController : BaseController
    {
        public JsonResult DropListJson( string filter, List<DropListSource> dropList)
        {
            if (DataConvert.ToString(filter) != "")
            {
                var selectList = dropList.Where(m => m.Filter == filter).Select(a => new SelectListItem
                {
                    Text = a.Text,
                    Value = a.Value
                });
                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var selectList = dropList.Select(a => new SelectListItem
                {
                    Text = a.Text,
                    Value = a.Value
                });
                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DropListJson(List<DropListSource> dropList)
        {
            var selectList = dropList.Select(a => new SelectListItem
             {
                 Text = a.Text,
                 Value = a.Value
             });
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GroupDropList(string filterExpression)
        {
            GroupRepository rep = new GroupRepository();
            if (HttpContext.Cache["GroupDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("GroupDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["GroupDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult UserDropList(string filterExpression)
        {
            UserRepository rep = new UserRepository();
            if (HttpContext.Cache["UserDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("UserDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["UserDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult DepartmentDropList(string currentId)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DepartmentRepository rep = new DepartmentRepository();
            DataTable source = rep.GetDropListSource(sysUser.UserId, currentId);
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

        public JsonResult DepartmentAllDropList()
        {
            DepartmentRepository rep = new DepartmentRepository();
            DataTable source = rep.GetDropListSource();
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

        public JsonResult FiscalYearDropList(string filterExpression)
        {
            FiscalYearRepository rep = new FiscalYearRepository();
            if (HttpContext.Cache["FiscalYearDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("FiscalYearDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["FiscalYearDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult AccountSubjectDropList(string filterExpression)
        {
            AccountSubjectRepository rep = new AccountSubjectRepository();
            if (HttpContext.Cache["AccountSubjectDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("AccountSubjectDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["AccountSubjectDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult CodeTableDropList(string filter)
        {
            if (HttpContext.Cache["CodeTableDropList"] == null)
            {
                CodeTableRepository rep = new CodeTableRepository();
                List<DropListSource> list = rep.DropList();
                HttpContext.Cache.Add("CodeTableDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            List<DropListSource> dropList = (List<DropListSource>)HttpContext.Cache["CodeTableDropList"];
            return DropListJson(filter, dropList);
        }


        public JsonResult AssetsClassDropList(string currentId)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            AssetsClassRepository rep = new AssetsClassRepository();
            DataTable source = rep.GetDropListSource(sysUser.UserId, currentId);
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

        public JsonResult StoreSiteDropList(string currentId)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            StoreSiteRepository rep = new StoreSiteRepository();
            DataTable source = rep.GetDropListSource(sysUser.UserId, currentId);
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

        public JsonResult AssetsTypeDropList(string filterExpression)
        {
            AssetsTypeRepository rep = new AssetsTypeRepository();
            if (HttpContext.Cache["AssetsTypeDropList"] == null)
            {
               DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("AssetsTypeDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["AssetsTypeDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult AssetsUsesDropList(string filterExpression)
        {
            AssetsUsesRepository rep = new AssetsUsesRepository();
            if (HttpContext.Cache["AssetsUsesDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("AssetsUsesDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["AssetsUsesDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult EquityOwnerDropList(string filterExpression)
        {
            EquityOwnerRepository rep = new EquityOwnerRepository();
            if (HttpContext.Cache["EquityOwnerDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("EquityOwnerDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["EquityOwnerDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult PurchaseTypeDropList(string filterExpression)
        {
            PurchaseTypeRepository rep = new PurchaseTypeRepository();
            if (HttpContext.Cache["PurchaseTypeDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("PurchaseTypeDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["PurchaseTypeDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult ProjectManageDropList(string filterExpression)
        {
            ProjectManageRepository rep = new ProjectManageRepository();
            if (HttpContext.Cache["ProjectManageDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("ProjectManageDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["ProjectManageDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult ScrapTypeDropList(string filterExpression)
        {
            ScrapTypeRepository rep = new ScrapTypeRepository();
            if (HttpContext.Cache["ScrapTypeDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("ScrapTypeDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["ScrapTypeDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult SupplierDropList(string filterExpression)
        {
            SupplierRepository rep = new SupplierRepository();
            if (HttpContext.Cache["SupplierDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("SupplierDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["SupplierDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult CustomerDropList(string filterExpression)
        {
            CustomerRepository rep = new CustomerRepository();
            if (HttpContext.Cache["CustomerDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("CustomerDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["CustomerDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult UnitDropList(string filterExpression)
        {
            UnitRepository rep = new UnitRepository();
            if (HttpContext.Cache["UnitDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("UnitDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["UnitDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult FiscalPeriodDropList(string filterExpression)
        {
            FiscalPeriodRepository rep = new FiscalPeriodRepository();
            if (HttpContext.Cache["FiscalPeriodDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("FiscalPeriodDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["FiscalPeriodDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult PostDropList(string filterExpression)
        {
            PostRepository rep = new PostRepository();
            if (HttpContext.Cache["PostDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("PostDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["PostDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        public JsonResult BarcodeStyleDropList(string filterExpression)
        {
            BarcodeStyleRepository rep = new BarcodeStyleRepository();
            if (HttpContext.Cache["BarcodeStyleDropList"] == null)
            {
                DataTable list = rep.GetDropListSource();
                HttpContext.Cache.Add("BarcodeStyleDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            DataTable source = (DataTable)HttpContext.Cache["BarcodeStyleDropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }
    }
}
