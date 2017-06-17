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
using BusinessCommon.Models.Group;
using BusinessCommon.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;
using WebCommon.Data;

namespace WebApp.Areas.BusinessCommon.Controllers
{
    public class GroupController : MasterController
    {
        GroupRepository Repository;
        public GroupController()
        {
            ControllerName = "Group";
            CacheExpiryMinute = 5;
            CachePriority = CacheItemPriority.High;
        }

        protected override IMasterFactory CreateRepository()
        {
            Repository = new GroupRepository();
            return new MasterRepositoryFactory<GroupRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle)
        {
            ClearClientPageCache(Response);
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, model);
            model.GridPkField = "groupId";
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
            //    Update(EntryRepository, model, model.FormMode, model.GroupId, model.ViewTitle);

            //}
            //return View(model);
            if (model.IsFixed == "Y")
            {
                ModelState.AddModelError("GroupNo", AppMember.AppText["IsFixed"]);
                return View(model);
            }
            if (Update(Repository, model, model.GroupId) == 1)
            {
                if (model.FormMode == "new")
                    return View(model);
                else
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle});
            }
            else
                return View(model);

        }

        public ActionResult Select(string pageId, string showCheckbox, string selectVal, string fieldIdObj)
        {
            SelectModel model = new SelectModel();
            model.PageId = pageId;
            model.TreeId = TreeId.DepartmentTreeId;
            model.FieldIdObj = fieldIdObj;
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            model.GroupTree = model.Repository.GetGroupTree(sysUser);
            if (showCheckbox == "true")
                model.ShowCheckBox = true;
            model.GroupId = selectVal;
            return PartialView("GroupSelect", model); 
        }


    }
}
