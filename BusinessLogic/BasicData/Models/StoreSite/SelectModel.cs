﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BusinessLogic.BasicData;
using BaseCommon.Models;
using BusinessLogic.BasicData.Repositorys;

namespace BusinessLogic.BasicData.Models.StoreSite
{
    public class SelectModel : EntryViewModel
    {
        public string Tree_HideString { get; set; }
        public string TreeId { get; set; }
        public DataTable StoreSiteTree { get; set; }

        public StoreSiteRepository Repository = new StoreSiteRepository();

        public string PYStoreSiteSearch { get; set; }
    }
}