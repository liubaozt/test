using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCommon.Basic
{
    public interface IApproveMasterFactory
    {
        ILoadList CreateListRepository();
        IApproveEntry CreateApproveEntryRepository();
    }
}
