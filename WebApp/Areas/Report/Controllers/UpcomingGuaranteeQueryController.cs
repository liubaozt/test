using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCommon.Common;
using BusinessLogic.Report.Models.UpcomingGuaranteeQuery;
using BaseCommon.Data;
using WebCommon.Init;
using BaseCommon.Basic;
using System.Data;
using BusinessLogic.Report.Repositorys;
using System.Text;
using WebCommon.HttpBase;

namespace WebApp.Areas.Report.Controllers
{
    public class UpcomingGuaranteeQueryController : QueryController
    {

        public UpcomingGuaranteeQueryController()
        {
            ControllerName = "UpcomingGuaranteeQuery";
            Repository = new UpcomingGuaranteeQueryRepository();
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
            model.GridHeight = 370;
            model.RemindDays = "30";
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon" });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });

            model.StoreSiteUrl = Url.Action("DropList", "StoreSite", new { Area = "BasicData", currentId = model.StoreSiteId });
            model.StoreSiteDialogUrl = Url.Action("Select", "StoreSite", new { Area = "BasicData" });
            model.StoreSiteAddFavoritUrl = Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            model.StoreSiteReplaceFavoritUrl = Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });


        }
    }
}
