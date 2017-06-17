using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.AssetsBusiness.Models.AssetsDepreciation;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessLogic.AssetsBusiness.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.AssetsBusiness.Controllers
{
    public class AssetsDepreciationController : MaintainController
    {
        protected AssetsDepreciationRepository Repository;
        public AssetsDepreciationController()
        {
            ControllerName = "AssetsDepreciation";
            Repository = new AssetsDepreciationRepository();
        }

        protected GridLayout EntryGridLayout()
        {
            if (HttpContext.Cache["DepreciationEntryGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "AssetsBusiness", "DepreciationEntry");
            }
            return (GridLayout)HttpContext.Cache["DepreciationEntryGridLayout"];
        }

        [HttpPost]
        public JsonResult EntryGridData(string formVar, string formMode, string primaryKey)
        {
            int pageIndex = DataConvert.ToInt32(Request.Params["page"]);
            int pageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            Dictionary<string, object> paras = SetParas(formVar);
            int cnt= Repository.GetGridCount(paras, formMode);
            double aa = (double)cnt / pageRowNum;
            double pageCnt = Math.Ceiling(aa);
            var rows = DataTable2Object.Data(EntryGridDataTable(formVar, formMode, primaryKey), EntryGridLayout().GridLayouts);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = pageIndex, records = cnt, total = pageCnt, rows = rows };
            return result;
        }

        protected DataTable EntryGridDataTable(string formVar, string formMode, string primaryKey)
        {
            
            EntryModel model = new EntryModel();
            Dictionary<string, object> paras = SetParas(formVar);
            string sortField = DataConvert.ToString(Request.Form["sidx"]);
            string sortType = DataConvert.ToString(Request.Params["sord"]);
            int pageIndex = DataConvert.ToInt32(Request.Params["page"]);
            int pageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            paras.Add("sortField", sortField);
            paras.Add("sortType", sortType);
            paras.Add("pageIndex", pageIndex);
            paras.Add("pageRowNum", pageRowNum);
            return Repository.GetEntryGridDataTable(paras, formMode, primaryKey);
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string primaryKey, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            SetParentEntryModel(pageId, formMode, viewTitle, model);
            model.EntryGridLayout = EntryGridLayout();
            model.EntryGridId = "EntryGrid";
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + model.DepartmentId + DFT.SQ });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            DataRow dr = Repository.GetModel();
            if (dr != null)
            {
                model.FiscalYearId = DataConvert.ToString(dr["fiscalYearIdNext"]);
                model.FiscalPeriodId = DataConvert.ToString(dr["fiscalPeriodIdNext"]);
            }
            return View(model);
        }

        protected Dictionary<string, object> SetParas(string formVar)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            if (DataConvert.ToString(formVar) != "")
            {
                EntryModel model = JsonHelper.Deserialize<EntryModel>(formVar);
                paras.Add("fiscalYearId", model.FiscalYearId);
                paras.Add("fiscalPeriodId", model.FiscalPeriodId);
            }
            return paras;
        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            if (CheckModelIsValid(model))
            {
                //string gridHidStr = model.EntryGridId + model.PageId + AppMember.HideString;
                //Dictionary<string, object> objs = new Dictionary<string, object>();
                //objs.Add("fiscalYearId", model.FiscalYearId);
                //objs.Add("fiscalPeriodId", model.FiscalPeriodId);
                //objs.Add("gridData", Request.Form[gridHidStr]);
               
                Update(Repository, model, model.ViewTitle);
                
            }
            model.EntryGridLayout = EntryGridLayout();
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckPeriod(string fiscalYearId, string fiscalPeriodId)
        {
            DataRow dr = Repository.GetModel();
            if (dr==null ||(DataConvert.ToString(dr["fiscalYearIdNext"]) == fiscalYearId
                && DataConvert.ToString(dr["fiscalPeriodIdNext"]) == fiscalPeriodId))
            {
                return Content("1", "text/html");
            }
            else
            {
                string msg = AppMember.AppText["DepreciationPeriod"] + DataConvert.ToString(dr["fiscalYearIdNextName"]) + "-" + DataConvert.ToString(dr["fiscalPeriodIdNextName"]);
                return Content(msg, "text/html");
            }
        }

    }
}
