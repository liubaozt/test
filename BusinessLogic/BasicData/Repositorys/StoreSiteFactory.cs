using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class StoreSiteFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new StoreSiteRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new StoreSiteRepository();
        }
    }
}
