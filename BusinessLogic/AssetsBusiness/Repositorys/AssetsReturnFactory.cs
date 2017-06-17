using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsReturnFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsReturnRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsReturnRepository();
        }
    }
}
