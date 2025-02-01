using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TBC.ROPP.Application.Middlewares;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;

            switch (ex)
            {
                case UnauthorizedAccessException:
                    context.RequestServices.GetRequiredService<ILoggerFactory>()
                        .CreateLogger<ErrorHandlerMiddleware>()
                        .LogError(ex, ex.Message);
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                default:
                    context.RequestServices.GetRequiredService<ILoggerFactory>()
                        .CreateLogger<ErrorHandlerMiddleware>()
                        .LogError(ex, ex.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }
}