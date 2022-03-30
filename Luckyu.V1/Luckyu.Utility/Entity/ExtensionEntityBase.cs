using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Luckyu.Utility
{
    /// <summary>
    /// 扩展类，用扩展表的形式对现有类进行扩展（扩展表规则为原表名 + 【_extension】，并且主键名与原表相同，其他字段以【ex_】开头）
    /// </summary>
    public class ExtensionEntityBase
    {
        [NotMapped]
        public JObject ExtObject { get; set; }

        [NotMapped]
        public string ExtJson
        {
            get
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExtObject);
            }
            set
            {
                ExtObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(value);
            }
        }
    }
}
