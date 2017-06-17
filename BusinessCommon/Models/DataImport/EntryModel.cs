using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.Data;
using BusinessCommon.Repositorys;
using BaseCommon.Models;
using BaseCommon.Data;

namespace BusinessCommon.Models.DataImport
{
    public class EntryModel : EntryViewModel
    {
          public GridLayout EntryGridLayout { get; set; }
    }
}