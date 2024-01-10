using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> _logger, RequestDelegate _next) 
        {
            this.logger = _logger;
            this.next = _next;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex) 
            {
                var errId = Guid.NewGuid();
                logger.LogError(ex, $"{errId} : {ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errId,
                    ErrorMessage = "Something went wrong!"

                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }

        }
    }
}
