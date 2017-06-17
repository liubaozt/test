using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.AssetsClass
{
    public class ListModel : ListViewModel
    {

        [AppDisplayNameAttribute("AssetsClassNo")]
        public string AssetsClassNo { get; set; }
        [AppDisplayNameAttribute("AssetsClassName")]
        public string AssetsClassName { get; set; }
        [AppDisplayNameAttribute("AssetsClassParentId")]
        public string ParentId { get; set; }

    }
}