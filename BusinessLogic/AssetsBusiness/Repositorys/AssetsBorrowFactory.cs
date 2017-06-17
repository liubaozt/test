using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsBorrowFactory : IApproveMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AssetsBorrowRepository();
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return new AssetsBorrowRepository();
        }
    }
}
