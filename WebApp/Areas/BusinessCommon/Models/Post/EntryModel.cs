using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BusinessCommon.AppMng;

namespace WebApp.Areas.BusinessCommon.Models.Post
{
    public class EntryModel : EntryViewModel
    {
        public string PostId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("PostNo")]
        public string PostNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("PostName")]
        public string PostName { get; set; }

        public PostRepository Repository = new PostRepository();
    }
}