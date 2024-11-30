﻿using Api.Application.Common.BaseResponse;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Middlewares;

public static class UnauthorizedHelper
{
    public static async Task HandleUnauthorizedResponse(JwtBearerChallengeContext context, string message)
    {
        var httpContext = context.HttpContext;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        var response = BaseResponse.Unauthorized(message);

        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
