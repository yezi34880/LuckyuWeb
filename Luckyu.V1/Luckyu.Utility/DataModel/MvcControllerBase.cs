using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Luckyu.Utility
{
    public class MvcControllerBase : Controller
    {

        #region 返回数据
        protected IActionResult Success(object data = null)
        {
            var res = new ResponseResult
            {
                code = 200,
                info = "操作成功",
                data = data
            };
            return Json(res);
        }
        protected IActionResult Success<T>(T data = default(T))
        {
            var res = new ResponseResult
            {
                code = 200,
                info = "操作成功",
                data = data
            };
            return Json(res);
        }
        protected IActionResult Success(string info, object data = null)
        {
            var res = new ResponseResult
            {
                code = 200,
                info = info,
                data = data
            };
            return Json(res);
        }

        protected IActionResult Success<T>(string info, T data = default(T))
        {
            var res = new ResponseResult
            {
                code = 200,
                info = info,
                data = data
            };
            return Json(res);
        }

        protected IActionResult Fail(string info = "操作失败", object data = null)
        {
            var res = new ResponseResult
            {
                code = 500,
                info = info,
                data = data
            };
            return Json(res);
        }

        protected IActionResult Fail<T>(string info = "操作失败", T data = default(T))
        {
            var res = new ResponseResult
            {
                code = 500,
                info = info,
                data = data
            };
            return Json(res);
        }
        #endregion

    }
}
