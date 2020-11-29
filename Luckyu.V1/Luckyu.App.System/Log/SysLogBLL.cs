using FreeSql.Internal.Model;
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
            var db = LogDBConnection.InitDatabase();
            Expression<Func<sys_logEntity, bool>> exp = r => r.is_enable == 1 && r.log_type == logtype;
            var query = db.Select<sys_logEntity>().Where(exp);
            var filters = new List<DynamicFilterInfo>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                }
            }
            if (!filters.IsEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.WhereDynamicFilter(filter);
                }
            }
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            long total;
            var list = logService.GetPage(jqPage.page, jqPage.rows, query, out total);
            var page = new JqgridPageResponse<sys_logEntity>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;

        }

    }
}
