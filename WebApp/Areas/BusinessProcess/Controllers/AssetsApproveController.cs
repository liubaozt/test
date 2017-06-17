using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using BusinessLogic.BusinessProcess;
using System.Data;
using WebApp.Areas.BusinessProcess.Models.AssetsApprove;
using WebApp.BaseWeb.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebApp.BaseWeb.Init;

namespace WebApp.Areas.BusinessProcess.Controllers
{
    public class AssetsApproveController : ApproveController
    {
        [Authorize]
        public ActionResult List(string pageId, string viewTitle, string approveMode)
        {
            ListModel model = new ListModel();
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormId = "assetsApproveLF";
            model.GridId = "list";
            model.GridLayout = GridLayout();
            model.AuthorityGridButton = GetAuthorityGridButton(pageId, model.GridId);
            model.ApproveMode = approveMode;
            return View(model);
        }

        protected override Dictionary<string, GridInfo> GridLayout()
        {
            if (HttpContext.Cache["AssetsApproveGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "AssetsApprove", "/layout");
            }
            return (Dictionary<string, GridInfo>)HttpContext.Cache["AssetsApproveGridLayout"];
        }

        protected override DataTable GridDataTable(string formVar, string approveMode, string sortField, string sortType, int pageIndex, int rowNum, int totalRownum)
        {
            AssetsApproveRepository rep = new AssetsApproveRepository();
            Dictionary<string, object> paras = SetParas(formVar, approveMode);
            SetGridParas(paras, sortField, sortType, pageIndex, rowNum, totalRownum);
            return rep.GetGridDataTable(paras);
        }

        protected override int GridDataCount(string formVar, string approveMode)
        {
            AssetsApproveRepository rep = new AssetsApproveRepository();
            Dictionary<string, object> paras = SetParas(formVar, approveMode);
            return rep.GetGridDataCount(paras);
        }

        protected Dictionary<string, object> SetParas(string formVar, string approveMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            if (DataConvert.ToString(formVar) != "")
            {
                ListModel model = JsonHelper.Deserialize<ListModel>(formVar);
                paras.Add("assetsNo", model.AssetsNo);
                paras.Add("assetsName", model.AssetsName);
            }
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            paras.Add("approver", sysUser.UserId);
            paras.Add("approveMode", approveMode);
            return paras;
        }

    }
}
