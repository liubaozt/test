using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AssetsTypeFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsTypeRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new AssetsTypeRepository();
        }
    }
}
