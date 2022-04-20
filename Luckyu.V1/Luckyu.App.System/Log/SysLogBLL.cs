using Luckyu.DataAccess;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class SysLogBLL
    {
        private sys_logService logService = new sys_logService();

        public JqgridPageResponse<sys_logEntity> Page(JqgridPageRequest jqPage, int logtype)
        {
            var db = LogDBConnection.db;
            Expression<Func<sys_logEntity, bool>> exp = r => r.is_enable == 1 && r.log_type == logtype;
            var date = DateTime.Today;
            if (jqPage.isSearch)
            {
                var log_time = jqPage.fitersObj.rules.Where(r => r.field == "log_time").FirstOrDefault();
                if (log_time != null&& !log_time.data.IsEmpty())
                {
                    date = log_time.data.ToDate();
                }
            }
            var total = 0;
            var list = logService.GetPage(jqPage.page, jqPage.rows, exp, date, ref total);
            var page = new JqgridPageResponse<sys_logEntity>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }

    }
}
