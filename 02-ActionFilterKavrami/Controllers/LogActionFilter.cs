﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace _02_ActionFilterKavrami.Controllers
{
    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.ActionDescriptor.RouteValues["controller"];
            string actionName = context.ActionDescriptor.RouteValues["action"];
            string url = context.HttpContext.Request.Path;

            Debug.WriteLine($"Log (Action çalışmadan önce): Path = {url} - Controller = {controllerName} - Action = {actionName}");

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

            string controllerName = context.ActionDescriptor.RouteValues["controller"];
            string actionName = context.ActionDescriptor.RouteValues["action"];
            string url = context.HttpContext.Request.Path;

            Debug.WriteLine($"Log (Action çalışmadan sonra): Path = {url} - Controller = {controllerName} - Action = {actionName}");


            base.OnActionExecuted(context);
        }
    }
}