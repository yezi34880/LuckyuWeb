using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Luckyu.Web
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class AuthorizeFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            var action = descriptor.MethodInfo;
            var controller = descriptor.ControllerTypeInfo;
            if (controller.HasAttribute<AllowAnonymousAttribute>() || action.HasAttribute<AllowAnonymousAttribute>())
            {
                return;
            }

            var isOnLine = LoginUserInfo.Instance.IsOnLine(context.HttpContext);
            if (!isOnLine)
            {
                context.Result = new RedirectResult("~/Login/Index");
            }
        }
    }
}
