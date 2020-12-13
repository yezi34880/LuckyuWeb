using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Luckyu.Utility
{
    /// <summary>
    /// 描 述：扩展.json序列反序列化
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 转成json对象
        /// </summary>
        /// <param name="Json">json字串</param>
        /// <returns></returns>
        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
        /// <summary>
        /// 转成json字串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        /// <summary>
        /// 转成json字串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="datetimeformats">时间格式化</param>
        /// <returns></returns>
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        /// <summary>
        /// 字串反序列化成指定对象实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string Json)
        {
            var ret = Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json
                , new BadDateFixingConverter()
                , new BadNumberFixingConverter()
                , new NullStringFixingConverter()
                , new BadBoolFixingConverter()
                );

            if (ret != null && typeof(T).IsSubclassOf(typeof(ExtensionEntityBase)))
            {
                JObject jo = Json.ToJObject();
                JObject njo = new JObject();
                foreach (var ji in jo)
                {
                    if (ji.Key.StartsWith("ext_"))
                    {
                        njo.Add(ji.Key, ji.Value);
                    }
                }

                if (njo.Count > 0)
                {
                    ret.GetType().GetProperties().First(n => n.Name == "ExtObject").SetValue(ret, njo);
                }
            }

            return ret;
        }
        /// <summary>
        /// 字串反序列化成指定对象实体(列表)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
        /// <summary>
        /// 字串反序列化成DataTable
        /// </summary>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
        /// <summary>
        /// 字串反序列化成linq对象
        /// </summary>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }

    public class BadDateFixingConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var rawDate = reader.Value.ToDateOrNull();
            return rawDate ?? DateTime.Today;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class BadNumberFixingConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(int) || objectType == typeof(long) || objectType == typeof(decimal) || objectType == typeof(float) || objectType == typeof(double) || objectType == typeof(int?) || objectType == typeof(long?) || objectType == typeof(decimal?) || objectType == typeof(float?) || objectType == typeof(double?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (reader.Value == null)
                    {
                        return null;
                    }
                    var nullableConverter = new System.ComponentModel.NullableConverter(objectType);
                    objectType = nullableConverter.UnderlyingType;
                }
                return Convert.ChangeType(reader.Value, objectType);
            }
            catch (Exception ex)
            {
                return existingValue;
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class NullStringFixingConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return reader.Value ?? "";
            }
            catch (Exception ex)
            {
                return existingValue;
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class BadBoolFixingConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var rawDate = reader.Value.ToBoolOrNull();
            return rawDate ?? false;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }

}
