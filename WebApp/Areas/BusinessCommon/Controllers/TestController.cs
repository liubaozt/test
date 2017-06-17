using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCommon.Common;
using BaseCommon.Data;
using BusinessCommon.Repositorys;
using System.Data;
using BaseCommon.Basic;
using WebCommon.Init;
using BusinessCommon.Models.Test;

namespace WebApp.Areas.BusinessCommon.Controllers
{
    public class TestController : BaseController
    {
        public ActionResult Entry(string pageId, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormId = "testEF";
            model.GridLayout = GridLayout();
            model.GridId = "grid";
            model.SaveUrl = Url.Action("Entry", "Test", new { Area = "BusinessCommon" });
            return View(model);
        }

        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            if (ModelState.IsValid)
            {
                string gridHidStr = model.GridId + model.PageId + AppMember.HideString;
                List<Test> gridData = JsonHelper.JSONStringToList<Test>(Request.Form[gridHidStr]);
                string delPkHidStr = model.GridId + model.PageId +AppMember.DeletePK ;
                string delPks = Request.Form[delPkHidStr];
                UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
                DataUpdate dbUpdate = new DataUpdate();
                dbUpdate.BeginTransaction();
                TestRepository rep = new TestRepository(dbUpdate);
                Dictionary<string, object> objs = new Dictionary<string, object>();
                objs.Add("gridData", gridData);
                objs.Add("deletePks", delPks);
                try
                {
                    rep.Save(objs, sysUser, model.ViewTitle);
                    dbUpdate.Commit();
                }
                catch (Exception ex)
                {
                    dbUpdate.Rollback();
                    throw new Exception(ex.Message);
                }
                model.HasError = "false";
            }
            else
            {
                model.HasError = "true";
            }
            model.GridLayout = GridLayout();
            return View(model);
        }

        [HttpPost]
        public JsonResult GridData(string formVar)
        {
            var rows = DataTable2Object.Data(GridDataTable(formVar), GridLayout());
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = rows.Length, rows = rows };
            return result;
        }

        protected DataTable GridDataTable(string formVar)
        {
            TestRepository rep = new TestRepository();
            Dictionary<string, object> paras = new Dictionary<string, object>();
            return rep.GetGridDataTable(paras);
        }

        protected Dictionary<string, GridInfo> GridLayout()
        {
            if (HttpContext.Cache["TestGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "BusinessCommon", "Test");
            }
            return (Dictionary<string, GridInfo>)HttpContext.Cache["TestGridLayout"];
        }

    }
}
