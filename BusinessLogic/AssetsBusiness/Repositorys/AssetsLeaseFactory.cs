using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsLeaseFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsLeaseRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsLeaseRepository();
        }
    }
}
