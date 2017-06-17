using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class ScrapTypeFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new ScrapTypeRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new ScrapTypeRepository();
        }
    }
}
