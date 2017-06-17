using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessCommon.Repositorys
{
    public class UserFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new UserRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new UserRepository();
        }
    }
}
