using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BaseCommon.Basic;
using BusinessCommon.Repositorys;
using BaseCommon.Models;

namespace BusinessCommon.Models.Group
{
    public class SelectModel : EntryViewModel 
    {
        public string FieldIdObj { get; set; }

        public string Tree_HideString { get; set; }
        public string TreeId { get; set; }
        public DataTable GroupTree { get; set; }

        public bool ShowCheckBox { get; set; }
        public string GroupId { get; set; }

        public GroupRepository Repository = new GroupRepository();

    }
}