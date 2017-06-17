using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.AssetsBusiness.Models.AssetsCheck;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessCommon.Repositorys;
using System.Data.Common;
using BusinessLogic.AssetsBusiness.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;
using System.IO;
using BusinessLogic.BasicData.Repositorys;

namespace WebApp.Areas.AssetsBusiness.Controllers
{
    public class AssetsCheckController : ApproveMasterController
    {
        AssetsCheckRepository Repository;
        public AssetsCheckController()
        {
            ControllerName = "AssetsCheck";
        }

        protected override IApproveMasterFactory CreateRepository()
        {
            Repository = new AssetsCheckRepository();
            return new ApproveMasterRepositoryFactory<AssetsCheckRepository>(Repository);
        }

        //[AppAuthorize]
        public ActionResult List(string pageId, string viewTitle, string listMode)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, listMode, "AssetsCheck", model);
            model.GridPkField = "assetsCheckId";
            return View(model);
        }

        [HttpPost]
        public override JsonResult EntryGridData(string formVar, string formMode, string primaryKey)
        {
            int pageIndex = DataConvert.ToInt32(Request.Params["page"]);
            int pageRowNum = DataConvert.ToInt32(Request.Params["rows"]);

            Dictionary<string, object> paras = new Dictionary<string, object>();
            int cnt = Repository.GetEntryGridCount(paras, formMode, primaryKey, formVar);
            double aa = (double)cnt / pageRowNum;
            double pageCnt = Math.Ceiling(aa);


            paras.Add("pageIndex", pageIndex);
            paras.Add("pageRowNum", pageRowNum);

            DataTable dt = Repository.GetEntryGridDataTable(paras, formMode, primaryKey, formVar);
            var rows = DataTable2Object.Data(dt, EntryGridLayout(formMode).GridLayouts);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = pageIndex, records = cnt, total = pageCnt, rows = rows };
            return result;
        }

        //[AppAuthorize]
        public ActionResult Entry(string pageId, string primaryKey, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            Repository.SetModel(primaryKey, formMode, model);
            SetParentEntryModel(pageId, primaryKey, formMode, viewTitle, model);
            SetThisEntryModel(model);
            return View(model);
        }

        //[AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model, string approveReturn)
        {
            //if (model.FormMode != "approve")
            //{
            //    if (CheckModelIsValid(model))
            //    {
            //        Update(EntryRepository, model, model.FormMode, model.AssetsCheckId, model.ViewTitle);
            //        SetMustModel(model);
            //    }
            //    if (model.FormMode == "reapply")
            //        return RedirectToAction("List", "AssetsCheck", new { Area = "AssetsBusiness", pageId = model.PageId, viewTitle = model.ViewTitle, approvemode = model.FormMode });
            //    else
            //        return View(model);
            //}
            //else
            //{
            //    return DealApprove(EntryRepository, model, approveReturn);
            //}
            if (Update(Repository, model, model.AssetsCheckId, approveReturn) == 1)
            {
                if (model.FormMode == "approve" || model.FormMode == "reapply")
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle, listMode = model.FormMode });
                if (model.FormMode == "delete")
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle, listMode = "actual" });
                else if (model.FormMode == "new" || model.FormMode == "new2")
                {
                    EntryModel newModel = new EntryModel();
                    SetParentEntryModel(model.PageId, "", model.FormMode, model.ViewTitle, newModel);
                    SetThisEntryModel(newModel);
                    return View(newModel);
                }
                else
                {
                    if (model.FormMode == "actual")
                        return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle, listMode = model.FormMode });
                    else
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


        public ActionResult UpLoad(string pageId, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormId = "EntryForm";
            model.ReturnUrl = Url.Action("List", new { listMode = "actual" });
            model.SaveUrl = Url.Action("UpLoad");
            model.FormMode = "upload";
            return View(model);
        }

        [HttpPost]
        public ActionResult UpLoad(EntryModel model)
        {
            PDACheck pdachk = new PDACheck();
            try
            {
                pdachk.UpLoad(model.UpLoadFileName);
                model.HasError = "false";
            }
            catch (Exception ex)
            {
                model.HasError = "true";
                model.Message = ex.Message;
            }
            return View(model);
        }

        public ActionResult DownLoad(string pageId, string primaryKey)
        {
            PDACheck pdachk = new PDACheck();
            pdachk.DownLoad(primaryKey);
            string fileName = Server.MapPath("~/Content/uploads/sqlite/" + "PDA.db");
            return File(fileName, "text/plain", "PDA.db");
        }

        [HttpPost]
        public ActionResult GetAutoNo()
        {
            string no = AutoNoGenerator.GetMaxNo("AssetsCheck");
            return Content(no, "text/html");
        }

        private void SetThisEntryModel(EntryModel model)
        {
            model.EntryGridId = "EntryGrid";
            model.EntryGridLayout = EntryGridLayout(model.FormMode);
            model.SelectUrl = Url.Action("Select", "AssetsManage", new { Area = "AssetsBusiness" });
            if (model.FormMode == "delete")
                model.ReturnUrl = Url.Action("List", new { listMode = "actual" });

            model.AssetsClassUrl = Url.Action("DropList", "AssetsClass", new { Area = "BasicData", currentId = model.AssetsClassId });
            model.AssetsClassDialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.AssetsClassAddFavoritUrl = Url.Action("AddFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.AssetsClassReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.DepartmentId });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.StoreSiteUrl = Url.Action("DropList", "StoreSite", new { Area = "BasicData", currentId = model.StoreSiteId });
            model.StoreSiteDialogUrl = Url.Action("Select", "StoreSite", new { Area = "BasicData" });
            model.StoreSiteAddFavoritUrl = Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            model.StoreSiteReplaceFavoritUrl = Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });

        }

        public ActionResult SqliteUpload()
        {
            HttpPostedFileBase file = Request.Files["Filedata"];
            string folderpath = RouteData.Values["id"].ToString();
            string path = Server.MapPath("~/Content/uploads/" + folderpath + "/");
            if (file != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(path + file.FileName);
                return Content(file.FileName, "text/html");
            }
            else
                return Content("0", "text/html");
        }

    }

}
