using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class CustomerFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new CustomerRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new CustomerRepository();
        }
    }
}
