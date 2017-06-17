using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class FiscalPeriodFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new FiscalPeriodRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new FiscalPeriodRepository();
        }
    }
}
