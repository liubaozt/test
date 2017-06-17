using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.AssetsUses
{
    public class ListModel : ListViewModel
    {
 
        [AppDisplayNameAttribute("AssetsUsesNo")]
        public string AssetsUsesNo { get; set; }
        [AppDisplayNameAttribute("AssetsUsesName")]
        public string AssetsUsesName { get; set; }

    }
}