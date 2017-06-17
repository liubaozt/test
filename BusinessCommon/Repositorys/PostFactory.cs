using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BusinessCommon.Repositorys
{
    public class PostFactory : IMasterFactory
    {
        public ILoadList CreateListRepository()
        {
            return new PostRepository();
        }

        public IEntry CreateEntryRepository()
        {
            return new PostRepository();
        }
    }
}
