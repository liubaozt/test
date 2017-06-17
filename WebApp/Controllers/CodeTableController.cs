using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessCommon.Repositorys;
using WebCommon.Common;
using WebCommon.Init;
using BaseCommon.Data;
using System.Web.Caching;
using BusinessLogic.BasicData;
using BusinessLogic.BasicData.Repositorys;
using System.Data;
using BaseCommon.Basic;

namespace WebApp.Controllers
{
    public class CodeTableController : BaseController
    {
        public JsonResult DropList(string filter)
        {
            if (HttpContext.Cache["CodeTableDropList"] == null)
            {
                CodeTableRepository rep = new CodeTableRepository();
                List<DropListSource> list = rep.DropList();
                HttpContext.Cache.Add("CodeTableDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            List<DropListSource> dropList = (List<DropListSource>)HttpContext.Cache["CodeTableDropList"];
            return DropListJson(dropList, filter);
        }

        public JsonResult AssetsIdleStateDropList()
        {
            string filter = "AssetsState";
            if (HttpContext.Cache["CodeTableDropList"] == null)
            {
                CodeTableRepository rep = new CodeTableRepository();
                List<DropListSource> list = rep.DropList();
                HttpContext.Cache.Add("CodeTableDropList", list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            List<DropListSource> dropList = (List<DropListSource>)HttpContext.Cache["CodeTableDropList"];
            List<DropListSource> dropList2 = dropList.Where(m => m.Filter == filter && (m.Value=="A" ||m.Value=="X" )).ToList();

            return DropListJson(dropList2, "");
        }

    }
}
