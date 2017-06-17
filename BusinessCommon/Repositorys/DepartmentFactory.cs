using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessCommon.Repositorys
{
    public class DepartmentFactory : IMasterFactory
    {

        public ILoadList CreateListRepository()
        {
            return new DepartmentRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new DepartmentRepository();
        }

    }
}
