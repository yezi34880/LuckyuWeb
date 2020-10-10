using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.Log
{
    public class sys_logService
    {
        LogDBConnection connection = new LogDBConnection();

        public void Insert(sys_logEntity entity)
        {
            var db = connection.dbClient;
            entity.Create();
            db.Insertable(entity).ExecuteCommand();
        }
    }
}
