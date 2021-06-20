using FreeSql.Internal.Model;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.DataAccess
{
    /// <summary>
    /// 统一构建查询语句的地方
    /// </summary>
    public static class SearchConditionHelper
    {
        #region 返回当前条件
        /// <summary>
        /// 根据时间段构建sql条件  
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="data">值 格式为 2019-1-1 - 2020-1-1，必须是以【 - 】空格-空格作为分隔符</param>
        /// <param name="data">分隔符 默认分隔符为【 - 】空格-空格</param>
        public static DynamicFilterInfo GetDateCondition(string field, string data, string split = " - ")
        {
            var filter = new DynamicFilterInfo();
            filter.Logic = DynamicFilterLogic.And;
            filter.Filters = new List<DynamicFilterInfo>();
            if (data.IsEmpty())
            {
                return null;
            }
            var timespans = data.SplitNoEmpty(split);
            if (timespans.Length > 1)
            {
                filter.Filters.Add(new DynamicFilterInfo
                {
                    Field = field,
                    Value = timespans,
                    Operator = DynamicFilterOperator.DateRange,
                });
            }
            return filter;
        }

        /// <summary>
        /// 根据数字范围构建sql条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data">值 格式为空 或 1 - 999，必须是以【 - 】空格-空格作为分隔符</param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static DynamicFilterInfo GetNumberCondition(string field, string data, string split = " - ")
        {
            if (data.IsEmpty())
            {
                return null;
            }
            var filter = new DynamicFilterInfo();
            filter.Logic = DynamicFilterLogic.And;
            filter.Filters = new List<DynamicFilterInfo>();
            var nums = data.SplitNoEmpty(split);
            if (nums.Length > 1)
            {
                var num1 = nums[0].ToDecimalOrNull();
                if (num1.HasValue)
                {
                    filter.Filters.Add(new DynamicFilterInfo
                    {
                        Field = field,
                        Value = num1.Value,
                        Operator = DynamicFilterOperator.GreaterThanOrEqual,
                    });
                }
                var num2 = nums[1].ToDecimalOrNull();
                if (num2.HasValue)
                {
                    filter.Filters.Add(new DynamicFilterInfo
                    {
                        Field = field,
                        Value = num2.Value,
                        Operator = DynamicFilterOperator.LessThan,
                    });
                }
            }
            return filter;
        }

        /// <summary>
        /// 字符 Like 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DynamicFilterInfo GetStringLikeCondition(string field, string data)
        {
            if (data.IsEmpty())
            {
                return null;
            }
            var filter = new DynamicFilterInfo();
            filter.Logic = DynamicFilterLogic.And;
            filter.Field = field;
            filter.Value = data;
            filter.Operator = DynamicFilterOperator.Contains;
            return filter;
        }

        /// <summary>
        /// 字符 = 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DynamicFilterInfo GetStringEqualCondition(string field, string data, string notSearch = "")
        {
            if (data.IsEmpty())
            {
                return null;
            }
            if (data == notSearch)
            {
                return null;
            }
            var filter = new DynamicFilterInfo();
            filter.Logic = DynamicFilterLogic.And;
            filter.Field = field;
            filter.Value = data;
            filter.Operator = DynamicFilterOperator.Equal;
            return filter;
        }

        public static DynamicFilterInfo GetStringContainCondition(string field, string data, string split = ",")
        {
            if (data.IsEmpty())
            {
                return null;
            }
            var datas = data.SplitNoEmpty(split);
            var filter = new DynamicFilterInfo();
            filter.Logic = DynamicFilterLogic.And;
            filter.Field = field;
            filter.Value = datas;
            filter.Operator = DynamicFilterOperator.Any;
            return filter;
        }
        #endregion

        #region 分页构造条件
        public static List<DynamicFilterInfo> ContructJQCondition(JqgridPageRequest jqPage)
        {
            var filters = new List<DynamicFilterInfo>();
            if (jqPage.isSearch)
            {
                foreach (var rule in jqPage.fitersObj.rules)
                {
                    if (rule.field.IsEmpty() || rule.data.IsEmpty())
                    {
                        continue;
                    }
                    if (rule.ltype.IsEmpty() || rule.ltype == "text")
                    {
                        filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                    }
                    else
                    {
                        switch (rule.ltype)
                        {
                            case "user_id":
                            case "department_id":
                            case "company_id":
                                filters.Add(SearchConditionHelper.GetStringContainCondition(rule.field, rule.data));
                                break;
                            case "datasource":
                            case "dataitem":
                                filters.Add(SearchConditionHelper.GetStringEqualCondition(rule.field, rule.data, "-1"));
                                break;
                            case "datasources":
                            case "dataitems":
                                {
                                    if (rule.data != "-1")
                                    {
                                        filters.Add(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                                    }
                                    break;
                                }
                            case "daterange":
                                filters.Add(SearchConditionHelper.GetDateCondition(rule.field, rule.data));
                                break;
                            case "numberrange":
                                filters.Add(SearchConditionHelper.GetNumberCondition(rule.field, rule.data));
                                break;
                        }
                    }
                }
            }
            return filters;
        }

        #endregion
    }
}
