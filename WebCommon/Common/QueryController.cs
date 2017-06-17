using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessCommon.Repositorys;
using WebCommon.Init;
using BaseCommon.Basic;
using BaseCommon.Models;
using BaseCommon.Repositorys;
using System.Text;

namespace WebCommon.Common
{
    public abstract class QueryController : BaseController
    {
        public IQuery Repository { get; set; }

        protected string ExportFileName = "EXP";

        protected virtual AdvanceGridLayout EntryGridLayout(string formMode)
        {
            if (HttpContext.Cache[ControllerName + "GridLayout"] == null)
            {
                string areaName = RouteData.DataTokens["area"].ToString();
                CacheInit.CreateAdvanceGridLayoutCache(HttpContext, areaName, ControllerName);
            }
            return (AdvanceGridLayout)HttpContext.Cache[ControllerName + "GridLayout"];
        }

        [HttpPost]
        public virtual JsonResult EntryGridData(string formVar)
        {
            ListCondition condition = new ListCondition();
            condition.SortField = DataConvert.ToString(Request.Form["sidx"]);
            condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            condition.ListModelString = formVar;
            var rows = new object[0];
            if (DataConvert.ToString(formVar) != "")
            {
                QueryEntryViewModel model = JsonHelper.Deserialize<QueryEntryViewModel>(formVar);
                DataTable dt = Repository.GetReportGridDataTable(condition);
                rows = DataTable2Object.Data(dt, EntryGridLayout(model.FormMode).GridLayouts);
            }
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = 1, rows = rows };
            return result;
        }

        public ActionResult ExcelExport(string formvar)
        {
            ListCondition condition = new ListCondition();
            condition.SortField = DataConvert.ToString(Request.Params["sidx"]);
            condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            condition.ListModelString = formvar;
            DataTable dt = Repository.GetReportGridDataTable(condition);
            QueryEntryViewModel model = JsonHelper.Deserialize<QueryEntryViewModel>(formvar);
            StringBuilder sbHtml = ExcelHelper.CreateExcel(dt, EntryGridLayout(model.FormMode));
            //string fileName = Server.MapPath("~/Content/uploads/output.xls");
            //ExcelHelper.CreateExcel(dt, EntryGridLayout(), "main", "报表", fileName);
            //return File(fileName, "application/ms-excel", IdGenerator.GetMaxId("ExcelExport") + ".xls");
            byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            return File(fileContents, "application/ms-excel", IdGenerator.GetMaxId(ExportFileName) + ".xls");
        }

        [HttpPost]
        public ActionResult ExcelPrint(string formvar)
        {
            //ListCondition condition = new ListCondition();
            //condition.SortField = DataConvert.ToString(Request.Params["sidx"]);
            //condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            //condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            //condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            //condition.ListModelString = formvar;
            //DataTable dt = Repository.GetReportGridDataTable(condition);
            //QueryEntryViewModel model = JsonHelper.Deserialize<QueryEntryViewModel>(formvar);
            //ExcelHelper.PrintExcel(dt, EntryGridLayout(model.FormMode), "main");
            //return Content("1", "text/html");
            ListCondition condition = new ListCondition();
            condition.SortField = DataConvert.ToString(Request.Params["sidx"]);
            condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            condition.ListModelString = formvar;
            DataTable dt = Repository.GetReportGridDataTable(condition);
            QueryEntryViewModel model = JsonHelper.Deserialize<QueryEntryViewModel>(formvar);
            model.EntryGridData = dt;
            model.EntryGridLayout = EntryGridLayout(model.FormMode);
            model.EntryGridId = "EntryGrid";
            model.FormId = "PrintForm";
            model.GridHeight = 375;
            return View("ReportPrint", model);
        }

        protected void SetParentEntryModel(string pageId, string viewTitle,string formMode, QueryEntryViewModel model)
        {
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.ExportUrl = Url.Action("ExcelExport");
            model.FormId = "QueryForm";
            model.GridHeight = 375;
            model.EntryGridLayout = EntryGridLayout(formMode);
        }

    }
}