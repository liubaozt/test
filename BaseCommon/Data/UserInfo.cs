using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCommon.Data
{
    public class UserInfo
    {
        public string UserId { get; set; }

        public string UserNo { get; set; }

        public string UserName { get; set; }

        public string GroupId { get; set; }

        public string GroupNo { get; set; }

        public string GroupName { get; set; }

        public string CompanyId { get; set; }

        public string CompanyNo { get; set; }

        public string CompanyName { get; set; }

        public string IsHeaderOffice { get; set; }

        public string IsSysUser { get; set; }

        //public string DepartmentId { get; set; }

        //public string DepartmentNo { get; set; }

        //public string DepartmentName { get; set; }

        public List<DepartmentInfo> Departments{ get; set; }

        public string AccessLevel { get; set; }

        public CurSetBooks MySetBooks { get; set; }
    }
}
