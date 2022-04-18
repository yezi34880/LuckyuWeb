using Luckyu.Utility;
using SqlSugar;
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
        /// 字符 通用查询 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ConditionalModel> GetCustomCondition(string field, string op, string data)
        {
            var filters = new List<ConditionalModel>();
            if (data.IsEmpty())
            {
                return filters;
            }
            var filter = new ConditionalModel();
            switch (op)
            {
                case "cn":
                    filter.ConditionalType = ConditionalType.Like;
                    break;
                case "nc":
                    filter.ConditionalType = ConditionalType.NoLike;
                    break;
                case "eq":
                    filter.ConditionalType = ConditionalType.Equal;
                    break;
                case "ne":
                    filter.ConditionalType = ConditionalType.NoEqual;
                    break;
                case "bw":
                    filter.ConditionalType = ConditionalType.LikeLeft;
                    break;
                case "ew":
                    filter.ConditionalType = ConditionalType.LikeRight;
                    break;
                default:
                    filter.ConditionalType = ConditionalType.Like;
                    break;
            }
            filters.Add(filter);
            return filters;
        }


        /// <summary>
        /// 根据时间段构建sql条件  
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="data">值 格式为 2019-1-1 - 2020-1-1，必须是以【 - 】空格-空格作为分隔符</param>
        /// <param name="data">分隔符 默认分隔符为【 - 】空格-空格</param>
        public static List<ConditionalModel> GetDateCondition(string field, string data, string split = " - ")
        {
            var filters = new List<ConditionalModel>();
            var timespans = data.SplitNoEmpty(split);
            if (timespans.Length > 0)
            {
                filters.Add(new ConditionalModel
                {
                    FieldName = field,
                    FieldValue = timespans[0],
                    ConditionalType = ConditionalType.GreaterThanOrEqual,
                    FieldValueConvertFunc = (val) => { return timespans[0].ToDate(); }
                });
                var nextDay = timespans[1].ToDate().AddDays(1).ToString("yyyy-MM-dd");
                filters.Add(new ConditionalModel
                {
                    FieldName = field,
                    FieldValue = nextDay,
                    ConditionalType = ConditionalType.LessThan,
                    FieldValueConvertFunc = (val) => { return nextDay; }
                });
            }
            else
            {
                throw new Exception("日期段格式不正确");
            }
            return filters;
        }

        /// <summary>
        /// 根据数字范围构建sql条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data">值 格式为空 或 1 - 999，必须是以【 - 】空格-空格作为分隔符</param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static List<ConditionalModel> GetNumberCondition(string field, string data, string split = " - ")
        {
            var filters = new List<ConditionalModel>();
            var timespans = data.SplitNoEmpty(split);
            if (timespans.Length > 0)
            {
                filters.Add(new ConditionalModel
                {
                    FieldName = field,
                    FieldValue = timespans[0],
                    ConditionalType = ConditionalType.GreaterThanOrEqual,
                    FieldValueConvertFunc = (val) => { return timespans[0].ToDecimal(); }
                });
                filters.Add(new ConditionalModel
                {
                    FieldName = field,
                    FieldValue = timespans[1],
                    ConditionalType = ConditionalType.LessThanOrEqual,
                    FieldValueConvertFunc = (val) => { return timespans[1].ToDecimal(); }
                });
            }
            else
            {
                throw new Exception("数字格式不正确");
            }
            return filters;
        }

        /// <summary>
        /// 字符 Like 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ConditionalModel> GetStringLikeCondition(string field, string data)
        {
            var filters = new List<ConditionalModel>();
            if (data.IsEmpty())
            {
                return filters;
            }
            var filter = new ConditionalModel()
            {
                FieldName = field,
                FieldValue = $"%{data}%",
                ConditionalType = ConditionalType.Like
            };
            filters.Add(filter);
            return filters;
        }

        /// <summary>
        /// 字符 = 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ConditionalModel> GetStringEqualCondition(string field, string data, string notSearch = "")
        {
            var filters = new List<ConditionalModel>();
            if (data.IsEmpty())
            {
                return filters;
            }
            if (data == notSearch)
            {
                return filters;
            }
            var filter = new ConditionalModel()
            {
                FieldName = field,
                FieldValue = $"{data}",
                ConditionalType = ConditionalType.Equal
            };
            filters.Add(filter);
            return filters;
        }

        public static List<ConditionalModel> GetStringContainCondition(string field, string data, string split = ",")
        {
            var filters = new List<ConditionalModel>();
            if (data.IsEmpty())
            {
                return filters;
            }
            var filter = new ConditionalModel()
            {
                FieldName = field,
                FieldValue = data,
                ConditionalType = ConditionalType.In,
                FieldValueConvertFunc = (val) => { return data.SplitNoEmpty(split); }
            };
            filters.Add(filter);

            return filters;
        }
        #endregion

        #region 分页构造条件
        public static List<IConditionalModel> ContructJQCondition(JqgridPageRequest jqPage)
        {
            var filterss = new List<IConditionalModel>();
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
                        filterss.AddRange(SearchConditionHelper.GetCustomCondition(rule.field, rule.op, rule.data));
                    }
                    else
                    {
                        switch (rule.ltype)
                        {
                            case "user_id":
                            case "department_id":
                            case "company_id":
                                filterss.AddRange(SearchConditionHelper.GetStringContainCondition(rule.field, rule.data));
                                break;
                            case "datasource":
                            case "dataitem":
                                filterss.AddRange(SearchConditionHelper.GetStringEqualCondition(rule.field, rule.data, "-1"));
                                break;
                            case "datasources":
                            case "dataitems":
                                {
                                    if (rule.data != "-1")
                                    {
                                        filterss.AddRange(SearchConditionHelper.GetStringLikeCondition(rule.field, rule.data));
                                    }
                                    break;
                                }
                            case "daterange":
                                filterss.AddRange(SearchConditionHelper.GetDateCondition(rule.field, rule.data));
                                break;
                            case "numberrange":
                                filterss.AddRange(SearchConditionHelper.GetNumberCondition(rule.field, rule.data));
                                break;
                        }
                    }
                }
            }
            return filterss;
        }

        #endregion
    }
}
