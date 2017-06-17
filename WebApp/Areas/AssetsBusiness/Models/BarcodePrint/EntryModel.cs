using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BusinessCommon.AppMng;

namespace WebApp.Areas.AssetsBusiness.Models.BarcodePrint
{
    public class EntryModel : EntryViewModel
    {
        [AppDisplayNameAttribute("BarcodeStyleId")]
        public string BarcodeStyleId { get; set; }

        public string SelectUrl { get; set; }

        public string DefaultAssetsId { get; set; }

        public string EntryGridId { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }

        public BarcodePrintRepository BarcodePrintRepository = new BarcodePrintRepository();

        public AssetsRepository AssetsRepository = new AssetsRepository();

        public BarcodeStyleRepository BarcodeStyleRepository = new BarcodeStyleRepository();
    }
}