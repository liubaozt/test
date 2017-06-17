using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.AssetsBusiness.Models.AssetsSplit;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessCommon.Repositorys;
using System.Data.Common;
using BusinessLogic.AssetsBusiness;
using BusinessLogic.AssetsBusiness.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.AssetsBusiness.Controllers
{
    public class AssetsSplitController : ApproveMasterController
    {
        AssetsSplitRepository Repository;
        public AssetsSplitController()
        {
            ControllerName = "AssetsSplit";
        }

        protected override IApproveMasterFactory CreateRepository()
        {
            Repository = new AssetsSplitRepository();
            return new ApproveMasterRepositoryFactory<AssetsSplitRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle, string listMode)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, listMode,"AssetsSplit", model);
            model.GridPkField = "assetsSplitId";
            return View(model);
        }



        protected GridLayout UpEntryGridLayout()
        {
            if (HttpContext.Cache["SplitUpEntryGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "AssetsBusiness", "SplitUpEntry");
            }
            return (GridLayout)HttpContext.Cache["SplitUpEntryGridLayout"];
        }


        [HttpPost]
        public JsonResult UpEntryGridData(string formVar, string formMode, string primaryKey)
        {
            var rows = DataTable2Object.Data(UpEntryGridDataTable(formVar, formMode, primaryKey), UpEntryGridLayout().GridLayouts);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = rows.Length, rows = rows };
            return result;
        }

        protected DataTable UpEntryGridDataTable(string formVar, string formMode, string primaryKey)
        {
            EntryModel model = new EntryModel();
            Dictionary<string, object> paras = new Dictionary<string, object>();
            return Repository.GetUpEntryGridDataTable(paras, formMode, primaryKey);
        }


        [AppAuthorize]
        public ActionResult Entry(string pageId, string primaryKey, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            Repository.SetModel(primaryKey, formMode, model);
            SetParentEntryModel(pageId, primaryKey, formMode, viewTitle, model);
            SetThisEntryModel(model);
            return View(model);
        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model, string approveReturn)
        {
            //if (model.FormMode != "approve")
            //{
            //    if (CheckModelIsValid(model))
            //    {
            //        Update(EntryRepository, model, model.FormMode, model.AssetsSplitId, model.ViewTitle);
            //        model.UpEntryGridLayout = UpEntryGridLayout();
            //        model.EntryGridLayout = EntryGridLayout(model.FormMode);
            //    }
            //    if (model.FormMode == "reapply")
            //        return RedirectToAction("List", "AssetsSplit", new { Area = "AssetsBusiness", pageId = model.PageId, viewTitle = model.ViewTitle, approvemode = model.FormMode });
            //    else
            //    {
            //        SetMustModel(model);
            //        return View(model);
            //    }
            //}
            //else
            //{
            //    return DealApprove(EntryRepository, model, approveReturn);
            //}

            if (Update(Repository, model, model.AssetsSplitId, approveReturn) == 1)
            {
                if (model.FormMode == "approve" || model.FormMode == "reapply")
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle, listMode = model.FormMode });
                else if (model.FormMode == "new" || model.FormMode == "new2")
                {
                    EntryModel newModel = new EntryModel();
                    SetParentEntryModel(model.PageId, "", model.FormMode, model.ViewTitle, newModel);
                    SetThisEntryModel(newModel);
                    return View(newModel);
                }
                else
                {
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle });
                }
            }
            else
            {
                if (model.FormMode == "approve")
                {
                    Repository.SetModel(model.ApprovePkValue, model.FormMode, model);
                    SetParentEntryModel(model.PageId, model.ApprovePkValue, model.FormMode, model.ViewTitle, model);
                }
                SetThisEntryModel(model);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult GetAutoNo()
        {
            string no = AutoNoGenerator.GetMaxNo("AssetsSplit");
            return Content(no, "text/html");
        }

        private void SetThisEntryModel(EntryModel model)
        {
            model.UpEntryGridId = "UpEntryGrid";
            model.EntryGridId = "EntryGrid";
            model.EntryGridLayout = EntryGridLayout(model.FormMode);
            model.UpEntryGridLayout = UpEntryGridLayout();
            model.SelectUrl = Url.Action("SingleSelect", "AssetsManage", new { Area = "AssetsBusiness" });
        }


    }

}
