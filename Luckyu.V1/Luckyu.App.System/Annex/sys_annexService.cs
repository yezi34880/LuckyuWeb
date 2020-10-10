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
    public class sys_annexService : RepositoryFactory<sys_annexEntity>
    {
        public void Insert(sys_annexEntity entity)
        {
            BaseRepository().Insert(entity);
        }
        public void Delete(sys_annexEntity entity)
        {
            BaseRepository().Delete(entity);
        }
    }
}
