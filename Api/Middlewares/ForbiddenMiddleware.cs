using Api.Application.Common.BaseResponse;
using Newtonsoft.Json;
using System.Net;

namespace Api.Middlewares;

public class ForbiddenMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            var errorMessage = "You dont have permission to use this endpoint";
            var forbiddenResponse = BaseResponse.Forbidden(errorMessage);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(forbiddenResponse));
        }
    }
}
