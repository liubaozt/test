using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebApp.BaseWeb.Common;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.Post
{
    public class ListModel : ListViewModel
    {
        public ListModel()
        {
            Repository = new PostRepository();
        }

        [AppDisplayNameAttribute("PostNo")]
        public string PostNo { get; set; }
        [AppDisplayNameAttribute("PostName")]
        public string PostName { get; set; }

    }
}