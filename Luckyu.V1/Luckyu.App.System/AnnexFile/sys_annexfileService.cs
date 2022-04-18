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
            BaseRepository().db.Insertable(entity).ExecuteCommand();
        }
        public void Delete(sys_annexfileEntity entity)
        {
            BaseRepository().db.Updateable<sys_annexfileEntity>(entity).SetColumns(r => r.is_delete == 1).ExecuteCommand();
        }

        public void Update(sys_annexfileEntity entity)
        {
            var db = BaseRepository().db;
            db.Updateable<sys_annexfileEntity>(entity).ExecuteCommand();
        }
        public void UpdateSort(List<sys_annexfileEntity> list)
        {
            var db = BaseRepository().db;
            db.UseTran(() =>
            {
                foreach (var item in list)
                {
                    db.Updateable<sys_annexfileEntity>(item).UpdateColumns(r => new
                    {
                        r.sort,
                    }).ExecuteCommand();
                }
            });
        }

        public void DownLoad(sys_annexfileEntity entity)
        {
            BaseRepository().db.Updateable<sys_annexfileEntity>(entity).SetColumns(r => r.downloadcount == r.downloadcount + 1).ExecuteCommand();
        }

    }
}
