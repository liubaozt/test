using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using WebCommon.Common;
using BaseCommon.Basic;
using System.IO;
using WebCommon.Init;
using BusinessCommon.Models.User;
using BusinessCommon.Repositorys;
using System.Web.Caching;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;
using BaseCommon.Models;

namespace WebApp.Areas.BusinessCommon.Controllers
{
    public class UserController : MasterController
    {
        UserRepository Repository;
        public UserController()
        {
            ControllerName = "User";
            CacheExpiryMinute = 5;
            CachePriority = CacheItemPriority.High;
        }

        protected override IMasterFactory CreateRepository()
        {
            Repository = new UserRepository();
            return new MasterRepositoryFactory<UserRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle,  model);
            model.GridPkField = "userId";
            model.GridHeight = 345;
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.DepartmentId });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
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
            ClearClientPageCache(Response);
            //if (model.IsFixed == "Y")
            //{
            //    ModelState.AddModelError("UserNo", AppMember.AppText["IsFixed"]);
            //    return View(model);
            //}
            if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.Low)
            {
                model.IsSysUser = true;
            }
            if (Update(Repository, model, model.UserId) == 1)
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

        protected virtual void SetThisEntryModel(EntryModel model)
        {
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.DepartmentIdDisplay });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon", showCheckbox = "true", departmentId = model.DepartmentId });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.Low)
            {
                model.IsSysUser = true;
            }
        }

        protected virtual GridLayout EntryGridLayout(string formMode)
        {
            string cacheName = "DropDownGridLayout";
            string layoutName = "DropDownGrid";
            if (HttpContext.Cache[cacheName] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "Common", layoutName, formMode, cacheName);
            }
            return (GridLayout)HttpContext.Cache[cacheName];
        }


        protected override int Update(IEntry rep, EntryViewModel model, string pkValue)
        {
            base.Update(rep, model, pkValue);
            HttpContext.Cache.Remove("UserInfoList");
            UserRepository repUser = new UserRepository();
            DataTable list = repUser.GetUserInfo();
            HttpContext.Cache.Add("UserInfoList", list, null, DateTime.Now.AddHours(5), TimeSpan.Zero, CacheItemPriority.High, null);
            return 1;
        }
       
    }
}
