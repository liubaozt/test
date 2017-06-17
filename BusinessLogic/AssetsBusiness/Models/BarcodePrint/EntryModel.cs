using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BusinessCommon.Repositorys;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Repositorys;
using System.Data;

namespace BusinessLogic.AssetsBusiness.Models.BarcodePrint
{
    public class EntryModel : EntryViewModel
    {
        [AppDisplayNameAttribute("BarcodeStyleId")]
        public string BarcodeStyleId { get; set; }

        public string SelectUrl { get; set; }

        public string DefaultAssetsId { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }
        public string EntryGridString { get; set; }

        public BarcodePrintRepository BarcodePrintRepository = new BarcodePrintRepository(); 

        public AssetsRepository AssetsRepository = new AssetsRepository();

        public BarcodeStyleRepository BarcodeStyleRepository = new BarcodeStyleRepository();

        public AssetsPrint CurrentAssets { get; set; }

        public List<AssetsPrint> AllAssets { get; set; }

        public string AssetsCount { get; set; }

        public DataTable BarcodeStyle { get; set; }
    
    }
}