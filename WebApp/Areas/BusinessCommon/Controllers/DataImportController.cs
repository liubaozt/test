using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCommon.Common;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Basic;
using WebCommon.Init;
using BusinessCommon.Models.DataImport;
using System.Collections;
using WebCommon.HttpBase;
using BusinessCommon.Repositorys;

namespace WebApp.Areas.BusinessCommon.Controllers
{
    public class DataImportController : BaseController
    {
        public DataImportController()
        {
            ControllerName = "DataImport";
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormId = "EntryForm";
            model.SaveUrl = Url.Action("Entry");
            model.EntryGridLayout = EntryGridLayout();
            model.CustomClick = false;
            return View(model);

        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            DataImportRepository rep = new DataImportRepository();
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            string path = Server.MapPath("~/Content/uploads/excel/");
            string fileNames = Request.Form["listExcelFileString"].ToString();
            DataUpdate dbUpdate = new DataUpdate();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                rep.Import(path, fileNames, sysUser, model.ViewTitle);
                dbUpdate.Commit();
                model.HasError = "false";
            }
            catch (Exception ex)
            {
                dbUpdate.Rollback();
                model.Message = ex.Message;
                model.HasError = "true";
                AppLog.WriteLog(sysUser.UserName, LogType.Error, "UpdateError", ex.Message);
            }
            finally
            {
                dbUpdate.Close();
            }
            model.EntryGridLayout = EntryGridLayout();
            if (model.HasError == "false")
                return RedirectToAction("Entry", new { pageId = model.PageId, viewTitle = model.ViewTitle });
            else
                return View(model);

        }

        protected GridLayout EntryGridLayout()
        {
            if (HttpContext.Cache["DataImportGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "BusinessCommon", "DataImport");
            }
            return (GridLayout)HttpContext.Cache["DataImportGridLayout"];
        }

        [HttpPost]
        public JsonResult EntryGridData()
        {
       
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //result.Data = new { page = 1, total = rows.Length, rows = rows };
            return result;
        }
    }
}
