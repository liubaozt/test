using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class EquityOwnerFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new EquityOwnerRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new EquityOwnerRepository();
        }
    }
}
