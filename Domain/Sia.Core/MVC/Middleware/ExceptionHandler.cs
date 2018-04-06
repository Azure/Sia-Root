using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sia.Core.Exceptions;
using System.Threading.Tasks;

namespace Sia.Core.Middleware
{
    public class ExceptionHandler
    {
        private RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (BaseException ex)
            {
                await HandleExceptionAsync(context, ex)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, BaseException ex)
        {
            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;
            await context
                .Response
                .WriteAsync(result)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
