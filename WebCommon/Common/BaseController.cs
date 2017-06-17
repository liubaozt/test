using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using BaseCommon.Data;
using System.Collections;
using BaseCommon.Basic;
using System.Data;
using System.Web.Caching;
using BaseCommon.Models;

namespace WebCommon.Common
{
    public abstract class BaseController : Controller
    {
        //protected DataUpdate dbUpdate;

        protected string ControllerName;


        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //if (RouteData.DataTokens.Count > 0)
            //    AreaName = RouteData.DataTokens["area"].ToString();
            InitCssAndJs();

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            TempData["Exception"] = filterContext.Exception.Message;
        }

        private void InitCssAndJs()
        {
            string layoutContentPath = "~/Content/css/";

            string[] layoutCss = new string[]{
                "redmond/jquery-ui-1.8.18.custom.css",
                "redmond/ui.jqgrid.css",
                "tree/zTreeStyle/zTreeStyle.css",
                //"button/Button.css",
                "uploadify/uploadify.css",
                "Site.css",
                "pagecss.css"
                };
            StringBuilder sb = new StringBuilder();
            foreach (var item in layoutCss)
            {
                sb.Append(@"<link type=""text/css"" rel=""stylesheet"" href=""");
                sb.Append(Url.Content(layoutContentPath) + item);
                sb.AppendLine(@"""/>");
            }

            TempData["CssBlock"] = sb.ToString();
            sb.Clear();
            string jsPath = "~/Scripts/used/";
            string[] jqScript = new string[]{
                "jquery-1.7.1.min.js",
                "jquery-ui-1.8.18.custom.min.js",
                "jquery.ui.datepicker-zh-CN.js",
                "jquery.layout.js",
                "grid.locale-cn.js",
                "jquery.jqGrid.min.js",
                "tree/jquery.ztree.core-3.0.js",
                "tree/jquery.ztree.excheck-3.0.js",
                "json2.js",
                //"btn.js",
                "jquery.uploadify.min.js",
                "app.customer.js",
            };
            foreach (var item in jqScript)
            {
                sb.Append(@"<script type=""text/javascript"" src=""");
                sb.Append(Url.Content(jsPath + item));
                sb.AppendLine(@" ""></script>");
            }
            TempData["ScriptBlock"] = sb.ToString();
        }

        protected bool CheckModelIsValid(BaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.HasError = "false";
                return true;
            }
            else
            {
                model.HasError = "true";
                return false;
            }
        }


        public JsonResult DropListJson(List<DropListSource> dropList, string filter)
        {
            if (DataConvert.ToString(filter) != "")
            {
                var selectList = dropList.Where(m => m.Filter == filter).Select(a => new SelectListItem
                {
                    Text = a.Text,
                    Value = a.Value
                });
                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var selectList = dropList.Select(a => new SelectListItem
                {
                    Text = a.Text,
                    Value = a.Value
                });
                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DropListJson(List<DropListSource> dropList)
        {
            var selectList = dropList.Select(a => new SelectListItem
            {
                Text = a.Text,
                Value = a.Value
            });
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        protected  void ClearClientPageCache(HttpResponseBase response)
        {
            response.Buffer = true; response.Expires = 0; response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            response.AddHeader("pragma", "no-cache");
            //response.AddHeader("cache-control", "private");
            response.AddHeader("cache-control", "no-store");
            response.CacheControl = "no-cache";
        }

    }
}