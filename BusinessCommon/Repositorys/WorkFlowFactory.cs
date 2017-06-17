using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessCommon.Repositorys
{
    public class WorkFlowFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new WorkFlowRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new WorkFlowRepository();
        }
    }
}
