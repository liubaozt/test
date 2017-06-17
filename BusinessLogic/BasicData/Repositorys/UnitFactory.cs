using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class UnitFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new UnitRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new UnitRepository();
        }
    }
}
