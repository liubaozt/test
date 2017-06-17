using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsTransferFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsTransferRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsTransferRepository();
        }
    }
}
