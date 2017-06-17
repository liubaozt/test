using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCommon.Common;
using BusinessLogic.Report.Models.AssetsClassChangeReport;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebCommon.Init;
using System.Data;

namespace WebApp.Areas.Report.Controllers
{
    public class AssetsClassChangeReportController : QueryController
    {
        public AssetsClassChangeReportController()
        {
            ControllerName = "AssetsClassChangeReport";
           // Repository = new AssetsClassChangeReportRepository();
        }

        public ActionResult Entry(string pageId, string primaryKey,  string viewTitle)
        {
            EntryModel model = new EntryModel();
            SetParentEntryModel(pageId, viewTitle, model);
            SetThisModel(model);
            return View(model);
        }

      
        private void SetThisModel(EntryModel model)
        {
            model.EntryGridLayout2 = EntryGridLayout2();
            model.AssetsClassUrl = Url.Action("DropList", "AssetsClass", new { Area = "BasicData", filterExpression = "assetsClassId=" + DFT.SQ + model.AssetsClassId + DFT.SQ });
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + model.DepartmentId + DFT.SQ });
            model.AssetsClassDialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.AssetsClassAddFavoritUrl = Url.Action("AddFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.AssetsClassReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.StoreSiteUrl = Url.Action("DropList", "StoreSite", new { Area = "BasicData", currentId = model.StoreSiteId });
            model.StoreSiteDialogUrl = Url.Action("Select", "StoreSite", new { Area = "BasicData" });
            model.StoreSiteAddFavoritUrl = Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            model.StoreSiteReplaceFavoritUrl = Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });

        }

        protected override Dictionary<string, GridInfo> EntryGridLayout()
        {
            if (HttpContext.Cache[ControllerName + "GridLayout"] == null)
            {
                string areaName = RouteData.DataTokens["area"].ToString();
                CacheInit.CreateGridLayoutCache(HttpContext, areaName, ControllerName, "/layout");
            }
            return (Dictionary<string, GridInfo>)HttpContext.Cache[ControllerName + "GridLayout"];
        }

        protected AdvanceGridLayout EntryGridLayout2()
        {
            if (HttpContext.Cache[ControllerName + "GridLayout"] == null)
            {
                string areaName = RouteData.DataTokens["area"].ToString();
                CacheInit.CreateAdvanceGridLayoutCache(HttpContext, areaName, ControllerName, "/gridLayout/columnLayout");
            }
            return (AdvanceGridLayout)HttpContext.Cache[ControllerName + "GridLayout"];
        }

        [HttpPost]
        public override JsonResult EntryGridData(string formVar)
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
                DataTable dt = Repository.GetReportGridDataTable(condition);
                rows = DataTable2Object.Data(dt, EntryGridLayout2().GridLayouts);
            }
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = rows.Length, rows = rows };
            return result;
        }


    }
}
