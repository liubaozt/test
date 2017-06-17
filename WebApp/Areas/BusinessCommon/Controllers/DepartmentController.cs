using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessCommon.Models.Department;
using BusinessCommon.Repositorys;
using WebCommon.Data;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;
using BaseControl.HtmlHelpers;

namespace WebApp.Areas.BusinessCommon.Controllers
{
    public class DepartmentController : MasterController
    {
        DepartmentRepository Repository;
        public DepartmentController()
        {
            ControllerName = "Department";
            CacheExpiryMinute = 5;
            CachePriority = CacheItemPriority.High;
        }

        protected override IMasterFactory CreateRepository()
        {
            Repository = new DepartmentRepository();
            return new MasterRepositoryFactory<DepartmentRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, model);
            SetThisListModel(model);
            model.GridPkField = "departmentId";
            return View(model);
        }


        [AppAuthorize]
        public ActionResult Entry(string pageId, string primaryKey, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            Repository.SetModel(primaryKey, formMode, model);
            SetParentEntryModel(pageId, formMode, viewTitle, model);
            SetThisEntryModel(model);
            return View(model);
        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            //if (CheckModelIsValid(model))
            //{
            //    Update(EntryRepository, model, model.FormMode, model.DepartmentId, model.ViewTitle);
            //}
            //SetThisEntryModel(model);
            //return View(model);
            if (Update(Repository, model, model.DepartmentId) == 1)
            {
                if (model.FormMode == "new")
                {
                    SetThisEntryModel(model);
                    return View(model);
                }
                else
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle });
            }
            else
            {
                SetThisEntryModel(model);
                return View(model);
            }
        }

        public ActionResult Select(string pageId,string showCheckbox,string departmentId)
        {
            SelectModel model = new SelectModel();
            model.PageId = pageId;
            model.TreeId =TreeId.DepartmentTreeId ;
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            model.DepartmentTree = model.Repository.GetDepartmentTree(sysUser);
            if (showCheckbox == "true")
                model.ShowCheckBox = true;
            model.DepartmentId = departmentId;
            return PartialView("DepartmentSelect", model);
        }

        [HttpPost]
        public ActionResult SearchTree(string pageId, string pySearch)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DepartmentRepository urep = new DepartmentRepository();
            DataTable list = new DataTable();
            if (HttpContext.Cache["DepartmentTree"] == null)
            {
                list = urep.GetDepartmentTree(sysUser);
                DataColumn col = new DataColumn("PY");
                list.Columns.Add(col);
                foreach (DataRow dr in list.Rows)
                {
                    dr["PY"] = PinYin.GetFirstPinyin(DataConvert.ToString(dr["departmentName"]));
                }
                HttpContext.Cache.Add("DepartmentTree", list, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            else
            {
                list = (DataTable)HttpContext.Cache["DepartmentTree"];
            }
            DataRow[] drs = list.Select(" PY like '%" + pySearch.ToUpper() + "%'");
            if (drs.Length > 0)
            {
                DataTable dt = drs.CopyToDataTable();
                string treeString = AppTreeView.TreeViewString(pageId, TreeId.DepartmentTreeId, dt, "", false);
                return Content(treeString, "text/html");
            }
            else
            {
                return Content("0", "text/html");
            }
           
           
        }

        private void SetThisEntryModel(EntryModel model)
        {
            //model.ParentUrl = Url.Action("DepartmentDropList", "DropList", new { Area = "", filterExpression = "departmentId=" + DFT.SQ + model.ParentId + DFT.SQ });
            model.ParentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.ParentId });
            model.DialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.AddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.ReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
        }

        private void SetThisListModel(ListModel model)
        {
            //model.ParentUrl = Url.Action("DepartmentDropList", "DropList", new { Area = "", filterExpression = "departmentId=" + DFT.SQ + model.ParentId + DFT.SQ });
            model.ParentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.ParentId });
            model.DialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.AddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.ReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
        }

        public override JsonResult DropList(string currentId, string pySearch)
        {
            ClearClientPageCache(Response);
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DepartmentRepository rep = new DepartmentRepository();
            DataTable source = rep.GetDropListSource(sysUser.UserId, currentId);
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

        public JsonResult AllDropList()
        {
            ClearClientPageCache(Response);
            DepartmentRepository rep = new DepartmentRepository();
            DataTable source = rep.GetDropListSource();
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

    }
}
