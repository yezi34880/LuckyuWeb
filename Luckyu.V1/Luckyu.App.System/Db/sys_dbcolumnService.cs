using Luckyu.DataAccess;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class sys_dbcolumnService : RepositoryFactory<sys_dbcolumnEntity>
    {
        public List<DbColumnInfo> GetColumnInfoByTableName(string tablename)
        {
            var db = BaseRepository().db;
            var dtcolumns = db.DbMaintenance.GetColumnInfosByTableName(tablename);
            return dtcolumns;
        }

    }
}
