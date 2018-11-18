using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RabbitMQ.Client.Exceptions;

namespace Efforteo.Common.Exceptions
{
    public class BasicExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorCode = "error";
            var statusCode = HttpStatusCode.BadRequest;
            var exceptionType = exception.GetType();
            switch (exception)
            {
                // TODO: uncomment and test later
//                case Exception e when exceptionType == typeof(EfforteoException):
//                    errorCode = ((EfforteoException) e).Code;
//                    statusCode = HttpStatusCode.Unauthorized;
//                    break;
                case Exception e when exceptionType == typeof(UnauthorizedAccessException):
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new {code = errorCode, message = exception.Message};
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}