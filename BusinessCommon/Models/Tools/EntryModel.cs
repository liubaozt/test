using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.Data;
using BusinessCommon.Repositorys;
using BaseCommon.Models;

namespace BusinessCommon.Models.Tools
{
    public class EntryModel : EntryViewModel
    {
        public string CssMergeUrl { get; set; }

        public ToolsRepository Repository = new ToolsRepository();
    }
}