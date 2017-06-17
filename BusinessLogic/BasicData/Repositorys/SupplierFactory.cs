using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class SupplierFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new SupplierRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new SupplierRepository();
        }
    }
}
