using log4net;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Booking.API
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                log.Error($"Request body: {context.Request.Body}\n Error: {ex.Message}");
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
