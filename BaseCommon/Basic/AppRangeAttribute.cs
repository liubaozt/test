using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BaseCommon.Basic
{
    public class AppRangeAttribute : RangeAttribute
    {
        public AppRangeAttribute(int minimum, int maximum):base( minimum,  maximum)
        {
            ErrorMessage = AppMember.AppText["Ranger"];
        }

        public AppRangeAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        {
            ErrorMessage = AppMember.AppText["Ranger"];
        }
    }
}
