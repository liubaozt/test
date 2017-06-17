using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Models;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.Report.Models.StoreSiteAssetsPlot
{
    public class EntryModel : PlotViewModel  
    {

        [AppDisplayNameAttribute("CompanyId")]
        public string CompanyId { get; set; }
    }
}
