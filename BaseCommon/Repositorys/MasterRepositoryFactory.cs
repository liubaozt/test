using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BaseCommon.Repositorys
{
    public class MasterRepositoryFactory<T> : IMasterFactory where T : MasterRepository
    {
        public T obj;
        public MasterRepositoryFactory(T _obj)
        {
            this.obj = _obj;
        }

        public ILoadList CreateListRepository()
        {
            return obj;
        }

        public IEntry CreateEntryRepository()
        {
            return obj;
        }

    }
}
