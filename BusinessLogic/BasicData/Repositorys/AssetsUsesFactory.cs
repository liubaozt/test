using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AssetsUsesFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsUsesRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new AssetsUsesRepository();
        }
    }
}
