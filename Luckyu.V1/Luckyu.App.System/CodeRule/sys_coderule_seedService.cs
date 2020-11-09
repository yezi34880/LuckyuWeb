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
    public class sys_coderule_seedService : RepositoryFactory<sys_coderule_seedEntity>
    {
        public void SaveForm(string keyValue, sys_coderule_seedEntity entity)
        {
            if (keyValue.IsEmpty())
            {
                BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                BaseRepository().Update(entity);
            }
        }
    }
}
