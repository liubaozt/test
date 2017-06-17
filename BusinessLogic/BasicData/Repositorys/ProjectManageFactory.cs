using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessLogic.BasicData.Repositorys
{
    public class ProjectManageFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new ProjectManageRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new ProjectManageRepository();
        }
    }
}
