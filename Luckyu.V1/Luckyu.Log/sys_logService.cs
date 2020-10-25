using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.Log
{
    public class sys_logService
    {
        public void Insert(sys_logEntity entity)
        {
            var db = LogDBConnection.InitDatabase();
            entity.Create();
            db.Insert(entity).ExecuteAffrows();
        }
    }
}
