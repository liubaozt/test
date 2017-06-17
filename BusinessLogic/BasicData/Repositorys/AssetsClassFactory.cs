using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AssetsClassFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsClassRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new AssetsClassRepository();
        }
    }
}
