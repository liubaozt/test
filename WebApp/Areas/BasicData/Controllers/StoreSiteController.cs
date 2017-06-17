using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.BasicData.Models.StoreSite;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessLogic.BasicData.Repositorys;
using BusinessCommon.Repositorys;
using WebCommon.Data;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;
using BaseControl.HtmlHelpers;

namespace WebApp.Areas.BasicData.Controllers
{
    public class StoreSiteController : MasterController
    {
        StoreSiteRepository Repository;
        public StoreSiteController()
        {
            NotNeedCache = true;
            ControllerName = "StoreSite";
            CacheExpiryMinute = 5;
            CachePriority = CacheItemPriority.High;

        }

        protected override IMasterFactory CreateRepository()
        {
            Repository = new StoreSiteRepository();
            return new MasterRepositoryFactory<StoreSiteRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle )
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle,  model);
            model.GridPkField = "storeSiteId";
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
            //    Update(EntryRepository, model, model.FormMode, model.StoreSiteId, model.ViewTitle);
            //}
            //SetThisEntryModel(model);
            //return View(model);
            if (Update(Repository, model, model.StoreSiteId) == 1)
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

        public ActionResult Select(string pageId)
        {
            SelectModel model = new SelectModel();
            model.PageId = pageId;
            model.TreeId = TreeId.StoreSiteTreeId;
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            model.StoreSiteTree = model.Repository.GetStoreSiteTree(sysUser);
            return PartialView("StoreSiteSelect", model);
        }

        [HttpPost]
        public ActionResult SearchTree(string pageId, string pySearch)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            StoreSiteRepository urep = new StoreSiteRepository();
            DataTable list = new DataTable();
            if (HttpContext.Cache["StoreSiteTree"] == null)
            {
                list = urep.GetStoreSiteTree(sysUser);
                DataColumn col = new DataColumn("PY");
                list.Columns.Add(col);
                foreach (DataRow dr in list.Rows)
                {
                    dr["PY"] = PinYin.GetFirstPinyin(DataConvert.ToString(dr["storeSiteName"]));
                }
                HttpContext.Cache.Add("StoreSiteTree", list, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            else
            {
                list = (DataTable)HttpContext.Cache["StoreSiteTree"];
            }
            DataRow[] drs = list.Select(" PY like '%" + pySearch.ToUpper() + "%'");
            if (drs.Length > 0)
            {
                DataTable dt = drs.CopyToDataTable();
                string treeString = AppTreeView.TreeViewString(pageId, TreeId.StoreSiteTreeId, dt, "", false);
                return Content(treeString, "text/html");
            }
            else
            {
                return Content("0", "text/html");
            }


        }


        private void SetThisEntryModel(EntryModel model)
        {
            model.ParentUrl = Url.Action("DropList", "StoreSite", new { Area = "BasicData", currentId = model.ParentId });
            model.DialogUrl = Url.Action("Select", "StoreSite", new { Area = "BasicData" });
            model.AddFavoritUrl = Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            model.ReplaceFavoritUrl = Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
        }

        public override JsonResult DropList(string currentId, string pySearch)
        {
            ClearClientPageCache(Response);
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            StoreSiteRepository rep = new StoreSiteRepository();
            DataTable source = rep.GetDropListSource(sysUser.UserId, currentId,sysUser);
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

    }
}
