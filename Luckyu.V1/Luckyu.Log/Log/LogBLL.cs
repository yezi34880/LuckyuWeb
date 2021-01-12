using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Log
{
    public class LogBLL
    {
        private static sys_logService logService = new sys_logService();

        public static void WriteLog(sys_logEntity entity)
        {
            logService.Insert(entity);
        }

        public static void WriteLog(sys_logEntity entity, Exception ex)
        {
            var msg = LogHelper.ErrorFormat(ex, entity);
            entity.log_content = msg;
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error(entity.log_content);
            logService.Insert(entity);
        }

        public static sys_logEntity GetLog(string keyValue, DateTime date)
        {
            var entity = logService.GetEntity(r => r.log_id == keyValue, date);
            return entity;
        }


    }
}
