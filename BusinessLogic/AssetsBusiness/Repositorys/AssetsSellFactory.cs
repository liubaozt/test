using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsSellFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsSellRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsSellRepository();
        }
    }
}
