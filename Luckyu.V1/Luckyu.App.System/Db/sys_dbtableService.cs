using Luckyu.DataAccess;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class sys_dbtableService : RepositoryFactory<sys_dbtableEntity>
    {

        public List<DbTableInfo> GetAllDBTable()
        {
            var db = BaseRepository().db;
            var dbtables = db.DbMaintenance.GetTableInfoList();
            return dbtables;
        }

        public DbTableInfo GetTableInfoByName(string tablename)
        {
            var db = BaseRepository().db;
            var dbtables = db.DbMaintenance.GetTableInfoList();
            var dbtable = dbtables.Where(r => r.Name == tablename).FirstOrDefault();
            return dbtable;
        }

    }
}
