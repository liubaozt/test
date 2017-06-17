using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.BasicData.Models.AssetsClass;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessLogic.BasicData.Repositorys;
using BusinessCommon.Repositorys;
using WebCommon.Data;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;
using BusinessLogic.AssetsBusiness.Repositorys;

namespace WebApp.Areas.BasicData.Controllers
{
    public class AssetsClassController : MasterController
    {
        AssetsClassRepository Repository;
        public AssetsClassController()
        {
            ControllerName = "AssetsClass";
            NotNeedCache = true;
            CacheExpiryMinute = 5;
            CachePriority = CacheItemPriority.High;
        }
        protected override IMasterFactory CreateRepository()
        {
            Repository = new AssetsClassRepository();
            return new MasterRepositoryFactory<AssetsClassRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, model);
            model.GridPkField = "assetsClassId";
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
            //    Update(EntryRepository, model, model.FormMode, model.AssetsClassId, model.ViewTitle);
            //}
            //SetThisEntryModel(model);
            //return View(model);
            if (Update(Repository, model, model.AssetsClassId) == 1)
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
            model.TreeId = TreeId.AssetsClassTreeId;
            model.AssetsClassTree = model.Repository.GetAssetsClassTree();
            return PartialView("AssetsClassSelect", model);
        }

       

        private void SetThisEntryModel(EntryModel model)
        {
            model.ParentUrl = Url.Action("DropList", "AssetsClass", new { Area = "BasicData", currentId = model.ParentId });
            model.DialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.AddFavoritUrl = Url.Action("AddFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.ReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
        }

        public JsonResult GetDefaultByAssetsClass(string assetsClassId)
        {
            ClearClientPageCache(Response);
            DataRow dr = Repository.GetModel(assetsClassId);
            string assetsClassNo = DataConvert.ToString(dr["assetsClassNo"]);
            AssetsRepository arep = new AssetsRepository();
            var selectList = new { remainRate = DataConvert.ToDouble(dr["RemainRate"]),
                                   durableYears = DataConvert.ToInt32(dr["durableYears"]),
                                   unitId = DataConvert.ToString(dr["unitId"]),
                                   depreciationType = DataConvert.ToString(dr["depreciationType"]),
                                   assetsBarcode = arep.GetAssetsBarcode(assetsClassNo)
            };
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }


        public override JsonResult DropList(string currentId, string pySearch)
        {
            ClearClientPageCache(Response);
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            AssetsClassRepository rep = new AssetsClassRepository();
            DataTable source = rep.GetDropListSource(sysUser.UserId, currentId);
            List<DropListSource> dropList = rep.DropList(source, "");
            return DropListJson(dropList);
        }

    }
}
