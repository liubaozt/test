using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class PurchaseTypeFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new PurchaseTypeRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new PurchaseTypeRepository();
        }
    }
}
