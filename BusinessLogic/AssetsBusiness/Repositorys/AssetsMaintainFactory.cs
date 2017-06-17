using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsMaintainFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsMaintainRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsMaintainRepository();
        }
    }
}
