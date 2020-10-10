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
            //var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            //var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;

            var typeController = context.Controller.GetType();
            var typeAction = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetType();
            if (typeController.HasAttribute<AllowAnonymousAttribute>() || typeAction.HasAttribute<AllowAnonymousAttribute>())
            {
                return;
            }

            var isOn = LoginUserInfo.Instance.IsOnLine(context.HttpContext);
            if (!isOn)
            {
                context.Result = new RedirectResult("~/Login/Index");
            }
        }
    }
}
