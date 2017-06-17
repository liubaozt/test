using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsCheckFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsCheckRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsCheckRepository();
        }
    }
}
