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

namespace WebCommon.Common
{
    public abstract class ReportController : BaseController
    {

        protected void SetEntryModel(string pageId, string viewTitle, ReportEntryViewModel model)
        {
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormId = "ReportForm";
        }

    }
}