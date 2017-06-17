using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessCommon.Repositorys
{
    public class BarcodeStyleFactory : IMasterFactory
    {

        public ILoadList CreateListRepository()
        {
            return new BarcodeStyleRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new BarcodeStyleRepository();
        }

    }
}
