using log4net;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Booking.Infrastucture;

namespace Booking.API
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Request body: {context.Request.Body}\n Error: {ex.Message}");
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (ex is ApplicationException)
                {
                    await context.Response.WriteAsync(ex.Message);
                }
            }
        }
    }
}
