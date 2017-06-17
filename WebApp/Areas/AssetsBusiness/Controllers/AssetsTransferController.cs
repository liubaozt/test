using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.AssetsBusiness.Models.AssetsTransfer;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessCommon.Repositorys;
using System.Data.Common;
using BusinessLogic.AssetsBusiness.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.AssetsBusiness.Controllers
{
    public class AssetsTransferController : ApproveMasterController
    {
        AssetsTransferRepository Repository;
        public AssetsTransferController()
        {
            ControllerName = "AssetsTransfer";
        }

        protected override IApproveMasterFactory CreateRepository()
        {
            Repository = new AssetsTransferRepository();
            return new ApproveMasterRepositoryFactory<AssetsTransferRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle, string listMode)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, listMode, "AssetsTransfer", model);
            model.GridPkField = "assetsTransferId";
            return View(model);
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
            //        Update(EntryRepository, model, model.FormMode, model.AssetsTransferId, model.ViewTitle);
            //    if (model.FormMode == "reapply")
            //        return RedirectToAction("List", "AssetsTransfer", new { Area = "AssetsBusiness", pageId = model.PageId, viewTitle = model.ViewTitle, approvemode = model.FormMode });
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
            if (Update(Repository, model, model.AssetsTransferId, approveReturn) == 1)
            {
                if (model.FormMode == "approve" || model.FormMode == "reapply")
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle, listMode = model.FormMode });
                else if (model.FormMode == "new" || model.FormMode == "new2")
                {
                    EntryModel newModel = new EntryModel();
                    SetParentEntryModel(model.PageId, "", model.FormMode, model.ViewTitle, newModel);
                    SetThisEntryModel(newModel);
                    //SetThisEntryModel(model);
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
            string no = AutoNoGenerator.GetMaxNo("AssetsTransfer");
            return Content(no, "text/html");
        }

        private void SetThisEntryModel(EntryModel model)
        {
            model.EntryGridId = "EntryGrid";
            model.EntryGridLayout = EntryGridLayout(model.FormMode);
            model.SelectUrl = Url.Action("Select", "AssetsManage", new { Area = "AssetsBusiness" });
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.DepartmentId });
            model.StoreSiteUrl = Url.Action("DropList", "StoreSite", new { Area = "BasicData", currentId = model.StoreSiteId });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.StoreSiteDialogUrl = Url.Action("Select", "StoreSite", new { Area = "BasicData" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.StoreSiteAddFavoritUrl = Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            model.StoreSiteReplaceFavoritUrl = Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            UserRepository user = new UserRepository();
            model.UserSource = user.UserAutoCompleteSource();
        }

    }

}
