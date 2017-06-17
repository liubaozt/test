using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BaseCommon.Repositorys
{
    public class ApproveMasterRepositoryFactory<T> : IApproveMasterFactory where T : ApproveMasterRepository
    {
        public T obj;
        public ApproveMasterRepositoryFactory(T _obj)
        {
            this.obj = _obj;
        }

        public ILoadList CreateListRepository()
        {
            return obj;
        }

        public IApproveEntry CreateApproveEntryRepository()
        {
            return obj;
        }
    }
}
