using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Leaf.Filters
{
    public class ObjectResultExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ObjectResultExceptionFilterAttribute(IHostingEnvironment hosting)
        {
            HostingEnvironment = hosting;
        }

        private IHostingEnvironment HostingEnvironment { get; }

        public override void OnException(ExceptionContext context)
        {
            string message;

            if (context.Exception is ApplicationException)
                message = context.Exception.Message;
            else
                message = "서버에서 처리되지 않은 오류가 발생하였습니다.";

            dynamic obj = new ExpandoObject();
            obj.message = message;

            if (HostingEnvironment.IsDevelopment()) obj.error = context.Exception;

            var result = new ObjectResult(obj);
            result.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Result = result;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);

            return Task.CompletedTask;
        }
    }
}