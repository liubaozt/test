using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AccountSubjectFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new AccountSubjectRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new AccountSubjectRepository();
        }
    }
}
