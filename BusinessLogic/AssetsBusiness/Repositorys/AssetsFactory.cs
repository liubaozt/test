using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsRepository();
        }
    }
}
