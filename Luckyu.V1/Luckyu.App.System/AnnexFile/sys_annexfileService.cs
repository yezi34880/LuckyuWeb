using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class sys_annexfileService : RepositoryFactory<sys_annexfileEntity>
    {
        public void Insert(sys_annexfileEntity entity)
        {
            BaseRepository().Insert(entity);
        }
        public void Delete(sys_annexfileEntity entity)
        {
            BaseRepository().Delete(entity);
        }
    }
}
