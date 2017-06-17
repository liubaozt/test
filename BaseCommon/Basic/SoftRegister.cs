using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCommon.Basic
{
    public class SoftRegister
    {
        public static bool CheckValid()
        {
           return AppSecurity.AppRegister.CheckValid();
        }
    }
}
