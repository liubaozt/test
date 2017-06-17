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
using BusinessCommon.Models.SetBooks;
using BusinessCommon.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.BusinessCommon.Controllers
{
    public class SetBooksController : MasterController
    {
        SetBooksRepository Repository;
        public SetBooksController()
        {
            ControllerName = "SetBooks";
            CacheExpiryMinute = 5;
            CachePriority = CacheItemPriority.High;
        }

        protected override IMasterFactory CreateRepository()
        {
            Repository = new SetBooksRepository();
            return new MasterRepositoryFactory<SetBooksRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, model);
            model.GridPkField = "setBooksId";
            return View(model);
        }

         [AppAuthorize]
        public ActionResult Entry(string pageId, string primaryKey, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            Repository.SetModel(primaryKey, formMode, model);
            SetParentEntryModel(pageId, formMode, viewTitle, model);
            return View(model);
        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            //if (CheckModelIsValid(model))
            //{
            //    Update(EntryRepository, model, model.FormMode, model.SetBooksId, model.ViewTitle);

            //}
            //return View(model);
            if (model.IsFixed == "Y")
            {
                ModelState.AddModelError("SetBooksNo", AppMember.AppText["IsFixed"]);
                return View(model);
            }
            if (Update(Repository, model, model.SetBooksId) == 1)
            {
                if (model.FormMode == "new")
                    return View(model);
                else
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle});
            }
            else
                return View(model);

        }


    }
}
