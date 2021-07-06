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
            BaseRepository().db.Insert(entity).ExecuteAffrows();
        }
        public void Delete(sys_annexfileEntity entity)
        {
            BaseRepository().db.Update<sys_annexfileEntity>().SetSource(entity).Set(r => r.is_delete == 1).ExecuteAffrows();
        }

        public void Update(sys_annexfileEntity entity)
        {
            var db = BaseRepository().db;
            db.Update<sys_annexfileEntity>().SetSource(entity).ExecuteAffrows();
        }
        public void UpdateSort(List<sys_annexfileEntity> list)
        {
            var db = BaseRepository().db;
            db.Transaction(() =>
            {
                foreach (var item in list)
                {
                    db.Update<sys_annexfileEntity>().SetSource(item).UpdateColumns(r => new
                    {
                        r.sort,
                    }).ExecuteAffrows();
                }
            });
        }

    }
}
