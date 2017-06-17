using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class FiscalYearFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new FiscalYearRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new FiscalYearRepository();
        }
    }
}
