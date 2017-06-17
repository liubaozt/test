using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.BasicData.Models.AccountSubject;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessLogic.BasicData.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.BasicData.Controllers
{
    public class AccountSubjectController : MasterController
    {
        AccountSubjectRepository Repository;
        public AccountSubjectController()
        {
            ControllerName = "AccountSubject";
            CacheExpiryMinute = 5;
            CachePriority = CacheItemPriority.High;
        }

        protected override IMasterFactory CreateRepository()
        {
            Repository = new AccountSubjectRepository();
            return new MasterRepositoryFactory<AccountSubjectRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle,  model);
            model.GridPkField = "accountSubjectId";
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
            //    Update(EntryRepository, model, model.FormMode, model.AccountSubjectId, model.ViewTitle);
            //}
            //SetThisEntryModel(model);
            //return View(model);
            if (Update(Repository, model, model.AccountSubjectId) == 1)
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

        public ActionResult OpenAccountSubjectSelect(string pageId)
        {
            SelectModel model = new SelectModel();
            model.PageId = pageId;
            model.TreeId = "tree";
            model.AccountSubjectTree = model.Repository.GetAccountSubjectTree();
            return PartialView("AccountSubjectSelect", model);
        }

        private void SetThisEntryModel(EntryModel model)
        {
            model.ParentUrl = Url.Action("DropList", "AccountSubject", new { Area = "BasicData", value = model.ParentId });
            model.DialogUrl = Url.Action("OpenAccountSubjectSelect", "AccountSubject", new { Area = "BasicData" });
            model.AddFavoritUrl = Url.Action("AddFavorit", "AccountSubject", new { Area = "BasicData", tableName = "AccountSubject" });
            model.ReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AccountSubject", new { Area = "BasicData", tableName = "AccountSubject" });
        }


       

    }
}
