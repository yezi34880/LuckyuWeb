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
        /// 根据时间段构建sql条件  
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="data">值 格式为 2019-1-1 - 2020-1-1，必须是以【 - 】空格-空格作为分隔符</param>
        /// <param name="data">分隔符 默认分隔符为【 - 】空格-空格</param>
        public static List<IConditionalModel> GetDateCondition(string field, string data, string split = " - ")
        {
            var modelCondition = new List<IConditionalModel>();
            if (data.IsEmpty())
            {
                return null;
            }
            var timespans = data.SplitWithoutEmpty(split);
            if (timespans.Length > 1)
            {
                modelCondition.Add(new ConditionalModel
                {
                    FieldName = field,
                    FieldValue = timespans[0],
                    ConditionalType = ConditionalType.GreaterThanOrEqual,
                    FieldValueConvertFunc = (val) => { return timespans[0].ToDate(); }
                });
                var nextDay = timespans[1].ToDate().AddDays(1);
                modelCondition.Add(new ConditionalModel
                {
                    FieldName = field,
                    FieldValue = nextDay.ToString("yyyy-MM-dd"),
                    ConditionalType = ConditionalType.LessThan,
                    FieldValueConvertFunc = val => nextDay
                });
            }
            return modelCondition;
        }

        /// <summary>
        /// 根据数字范围构建sql条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data">值 格式为空 或 1 - 999，必须是以【 - 】空格-空格作为分隔符</param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static List<IConditionalModel> GetNumberCondition(string field, string data, string split = " - ")
        {
            var modelCondition = new List<IConditionalModel>();
            if (data.IsEmpty())
            {
                return null;
            }
            var nums = data.SplitWithoutEmpty(split);
            if (nums.Length > 1)
            {
                var num1 = nums[0].ToDecimalOrNull();
                if (num1.HasValue)
                {
                    modelCondition.Add(new ConditionalModel
                    {
                        FieldName = field,
                        FieldValue = num1.Value.ToString(),
                        ConditionalType = ConditionalType.GreaterThanOrEqual,
                        FieldValueConvertFunc = val => num1.Value
                    });
                }
                var num2 = nums[1].ToDecimalOrNull();
                if (num2.HasValue)
                {
                    modelCondition.Add(new ConditionalModel
                    {
                        FieldName = field,
                        FieldValue = num2.Value.ToString(),
                        ConditionalType = ConditionalType.LessThanOrEqual,
                        FieldValueConvertFunc = val => num2.Value
                    });
                }
            }
            return modelCondition;
        }

        /// <summary>
        /// 字符 Like 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IConditionalModel GetStringLikeCondition(string field, string data)
        {
            var modelCondition = new ConditionalModel
            {
                FieldName = field,
                FieldValue = $"%{data}%",
                ConditionalType = ConditionalType.Like
            };
            return modelCondition;
        }

        /// <summary>
        /// 字符 = 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IConditionalModel GetStringEqualCondition(string field, string data, string notSearch = "")
        {
            if (data == notSearch)
            {
                return null;
            }
            var modelCondition = new ConditionalModel
            {
                FieldName = field,
                FieldValue = data,
                ConditionalType = ConditionalType.Equal
            };
            return modelCondition;
        }

        public static IConditionalModel GetStringContainCondition(string field, string data, string split = ",")
        {
            var modelCondition = new ConditionalModel
            {
                FieldName = field,
                FieldValue = data,
                ConditionalType = ConditionalType.In,
                FieldValueConvertFunc = (val) => { return data.SplitWithoutEmpty(split); }
            };
            return modelCondition;
        }
        #endregion

        #region 直接在当前条件上扩展
        public static void AddDateCondition(this List<IConditionalModel> condition, string field, string data, string split = " ~ ")
        {
            var newCondition = SearchConditionHelper.GetDateCondition(field, data, split);
            if (!newCondition.IsEmpty())
            {
                condition.AddRange(newCondition);
            }
        }

        public static void AddStringLikeCondition(this List<IConditionalModel> condition, string field, string data)
        {
            var newCondition = SearchConditionHelper.GetStringLikeCondition(field, data);
            if (!newCondition.IsEmpty())
            {
                condition.Add(newCondition);
            }
        }

        /// <summary>
        /// 字符 = 条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void AddStringEqualCondition(this List<IConditionalModel> condition, string field, string data, string notSearch = "")
        {
            var newCondition = SearchConditionHelper.GetStringEqualCondition(field, data, notSearch);
            if (!newCondition.IsEmpty())
            {
                condition.Add(newCondition);
            }
        }

        public static void AddStringContainCondition(this List<IConditionalModel> condition, string field, string data, string split = ",")
        {
            var newCondition = SearchConditionHelper.GetStringContainCondition(field, data, split);
            if (!newCondition.IsEmpty())
            {
                condition.Add(newCondition);
            }
        }

        #endregion
    }
}
