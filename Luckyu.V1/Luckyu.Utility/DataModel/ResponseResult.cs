using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class ResponseResultBase
    {
        public int code { get; set; }

        public string info { get; set; }
    }

    public class ResponseResult : ResponseResultBase
    {
        public static ResponseResult Success(string info, object data = null)
        {
            var res = new ResponseResult
            {
                code = 200,
                info = info,
                data = data
            };
            return res;
        }

        public static ResponseResult Success(object data = null)
        {
            var res = new ResponseResult
            {
                code = 200,
                info = "操作成功",
                data = data
            };
            return res;
        }

        public static ResponseResult Fail(string info = "操作失败", object data = null)
        {
            var res = new ResponseResult
            {
                code = 500,
                info = info,
                data = data
            };
            return res;
        }

        #region 泛型转换
        public static ResponseResult<T> Fail<T>(string info = "操作失败", T data = default(T))
        {
            var res = new ResponseResult<T>
            {
                code = 500,
                info = info,
                data = data
            };
            return res;
        }

        public static ResponseResult<T> Success<T>(T data = default(T))
        {
            var res = new ResponseResult<T>
            {
                code = 200,
                info = "操作成功",
                data = data
            };
            return res;
        }

        public static ResponseResult<T> Success<T>(string info, T data = default(T))
        {
            var res = new ResponseResult<T>
            {
                code = 200,
                info = info,
                data = data
            };
            return res;
        }

        public static ResponseResult<T> Create<T>(ResponseResult res)
        {
            var resT = new ResponseResult<T>
            {
                code = res.code,
                info = res.info,
            };
            return resT;
        }
        #endregion

        public object data { get; set; }
    }

    public class ResponseResult<T> : ResponseResultBase
    {
        public T data { get; set; }
    }

    public enum ResponseCode
    {
        Success = 200,
        Fail = 500,
    }

}
