using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BaseCommon.Basic
{
     public class AppDisplayNameAttribute:DisplayNameAttribute
    {
        private string _defaultName;
        public AppDisplayNameAttribute(string defaultName)
        {
            _defaultName = defaultName;
        }

        public override string DisplayName
        {
            get
            {
                string s = AppMember.AppText[_defaultName];
                return s;
            }
        }
    }
}
