using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using System.Data;
using BusinessLogic.AssetsBusiness.Repositorys;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Models.AssetsManage
{
    public class SelectModel : ListModel
    {
      
        public bool IsMultiSelect { get; set; }

        public AssetsRepository Repository = new AssetsRepository();

        public int GridWidth { get; set; }

        public string GridFormId { get; set; }

        [AppDisplayNameAttribute("IsCascadeDelete")]
        public bool IsCascadeDelete { get; set; }
       
    }
}
