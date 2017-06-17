using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;


namespace BusinessLogic.BasicData.Models.ScrapType
{
    public class ListModel : ListViewModel
    {
     
        [AppDisplayNameAttribute("ScrapTypeNo")]
        public string ScrapTypeNo { get; set; }
        [AppDisplayNameAttribute("ScrapTypeName")]
        public string ScrapTypeName { get; set; }

    }
}