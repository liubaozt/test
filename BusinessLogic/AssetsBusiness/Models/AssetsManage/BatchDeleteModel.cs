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
    public class BatchDeleteModel : SelectModel
    {
        public string FormMode { get; set; }
        public string BatchGridString { get; set; }
       
    }
}
