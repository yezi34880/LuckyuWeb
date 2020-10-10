using Luckyu.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Organization
{
    public class sys_authorizeService : RepositoryFactory<sys_authorizeEntity>
    {
        public void SaveForm(int objectType, string objectId, List<sys_authorizeEntity> list)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.Delete<sys_authorizeEntity>(r => r.object_id == objectId && r.objecttype == objectType);
                trans.Insert(list);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

    }
}
