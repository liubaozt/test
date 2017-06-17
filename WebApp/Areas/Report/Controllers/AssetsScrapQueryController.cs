using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCommon.Common;
using BusinessLogic.Report.Models.AssetsScrapQuery;
using BaseCommon.Data;
using WebCommon.Init;
using BaseCommon.Basic;
using System.Data;
using BusinessLogic.Report.Repositorys;
using System.Text;
using WebCommon.HttpBase;

namespace WebApp.Areas.Report.Controllers
{
    public class AssetsScrapQueryController : QueryController
    {

        public AssetsScrapQueryController()
        {
            ControllerName = "AssetsScrapQuery";
            Repository = new AssetsScrapQueryRepository();
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string formMode, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.EntryGridId = "EntryGrid";
            SetParentEntryModel(pageId, viewTitle,formMode, model);
            SetThisModel(model);
            return View(model);
        }

        private void SetThisModel(EntryModel model)
        {
            model.GridHeight = 380;
            model.AssetsClassUrl = Url.Action("DropList", "AssetsClass", new { Area = "BasicData" });
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon" });
            model.AssetsClassDialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.AssetsClassAddFavoritUrl = Url.Action("AddFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.AssetsClassReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });

        }

    }
}
