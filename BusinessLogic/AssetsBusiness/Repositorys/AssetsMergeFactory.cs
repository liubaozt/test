using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsMergeFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsMergeRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsMergeRepository();
        }
    }
}
