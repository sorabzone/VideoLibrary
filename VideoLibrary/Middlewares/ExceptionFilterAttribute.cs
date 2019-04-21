using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using VideoLibrary.Logger;
using VideoLibrary.ResponseHelpers;

namespace VideoLibrary.Middlewares
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            CommonLogger.LogError(context.Exception);
            context.HttpContext.Response.StatusCode = 500;

            var response = new CustomResponse<bool>
            {
                Code = ResponseHelpers.StatusCode.Exception,
                Message = $@"{context.Exception.Message},{context.Exception.StackTrace}",
                Data = false
            };

            context.Result = new JsonResult(response);

            context.ExceptionHandled = true;
            base.OnException(context);
        }
    }
}
