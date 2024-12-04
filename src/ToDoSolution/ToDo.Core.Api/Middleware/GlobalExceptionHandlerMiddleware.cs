namespace ToDo.Core.Api.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Proceed with the request
                await _next(httpContext);
            }
            catch (ApplicationException appEx)
            {
                // Handle specific ApplicationException and return 400
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(new { error = appEx.Message }.ToString());
            }
            catch (Exception ex)
            {
                // Handle other exceptions and return 500
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(new { error = "Something went wrong" }.ToString());
            }
        }
    }

}