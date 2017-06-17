using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Models.AssetsType
{
    public class ListModel : ListViewModel
    {
       
        [AppDisplayNameAttribute("AssetsTypeNo")]
        public string AssetsTypeNo { get; set; }
        [AppDisplayNameAttribute("AssetsTypeName")]
        public string AssetsTypeName { get; set; }

    }
}