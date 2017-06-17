using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WebApp.BaseWeb.Common;
using BusinessLogic.BasicData;

namespace WebApp.Areas.BasicData.Models.AssetsClass
{
    public class SelectModel : EntryViewModel
    {
        public string Tree_HideString { get; set; }
        public string TreeId { get; set; }
        public DataTable AssetsClassTree { get; set; }

        public AssetsClassRepository Repository = new AssetsClassRepository();
    }
}