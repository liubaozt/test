using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessCommon.Repositorys
{
    public class GroupFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new GroupRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new GroupRepository();
        }
    }
}
