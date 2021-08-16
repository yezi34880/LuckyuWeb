using FreeSql.DatabaseModel;
using Luckyu.DataAccess;
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
            var maindb = db.Ado.MasterPool.Get();
            //var databases = db.DbFirst.GetDatabases();
            var dbtables = db.DbFirst.GetTablesByDatabase(maindb.Value.Database);
            return dbtables;
        }

        public DbTableInfo GetTableInfoByName(string tablename)
        {
            var db = BaseRepository().db;
            var dbtable = db.DbFirst.GetTableByName(tablename);
            return dbtable;
        }

    }
}
