using System.Collections.Generic;

namespace Luckyu.Utility
{
    public class JqgridPageRequest
    {
        public bool _search { get; set; }

        public string nd { get; set; }

        /// <summary>
        /// 一页行数
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 排序字段字段名
        /// </summary>
        //public string sidx { get; set; }
        private string _sidx;
        public string sidx
        {
            get
            {
                return _sidx;
            }
            set
            {
                if (!value.IsEmpty())  // 增加验证 防止 SQL注入
                {
                    if (value.Contains("-"))
                    {
                        value = value.Replace("-", "");
                    }
                    if (value.Contains("\'"))
                    {
                        value = value.Replace("\'", "");
                    }
                    if (value.Contains("\""))
                    {
                        value = value.Replace("\"", "");
                    }
                }

                _sidx = value;
            }
        }

        /// <summary>
        /// 正序倒序 asc / desc
        /// </summary>
        //public string sord { get; set; }
        private string _sord;
        public string sord
        {
            get
            {
                return _sord;
            }
            set
            {
                if (!value.IsEmpty())  // 增加验证 防止 SQL注入
                {
                    if (value.Contains("-"))
                    {
                        value = value.Replace("-", "");
                    }
                    if (value.Contains("\'"))
                    {
                        value = value.Replace("\'", "");
                    }
                    if (value.Contains("\""))
                    {
                        value = value.Replace("\"", "");
                    }
                }

                _sord = value;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        public string filters { get; set; }

        public JqGridFiters fitersObj
        {
            get
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<JqGridFiters>(filters);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool isSearch
        {
            get
            {
                return this != null && this._search && this.fitersObj != null && this.fitersObj.rules != null && this.fitersObj.rules.Count > 0;
            }
        }

    }

    public class JqGridFiters
    {
        public string groupOp { get; set; }

        public List<JqGridFiterRule> rules { get; set; }

        public List<JqGridFiters> groups { get; set; }
    }

    public class JqGridFiterRule
    {
        public string field { get; set; }

        public string op { get; set; }

        public string data { get; set; }

        public string ltype { get; set; }
    }
}
