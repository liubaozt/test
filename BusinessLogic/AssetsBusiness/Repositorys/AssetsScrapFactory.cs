using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsScrapFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsScrapRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsScrapRepository();
        }
    }
}
