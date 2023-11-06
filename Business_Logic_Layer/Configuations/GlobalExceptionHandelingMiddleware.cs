using Business_Logic_Layer.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Business_Logic_Layer.Configuations;

public class GlobalExceptionHandelingMiddleware
{
    private readonly RequestDelegate _next;

    public  GlobalExceptionHandelingMiddleware(RequestDelegate next)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode status;
        var stackTrace = string.Empty;
        string message = "";

        var exceptionType = ex.GetType();

        if(exceptionType == typeof(NotFoundException)) 
        {
            message = ex.Message;
            status = HttpStatusCode.NotFound;
            stackTrace = ex.StackTrace;
        }
        else if(exceptionType == typeof(BadRequestException)) 
        { 
            message = ex.Message;
            status = HttpStatusCode.BadRequest;
            stackTrace = ex.StackTrace;
        }
        else if(exceptionType == typeof(ConflictException)) 
        {
            message = ex.Message;
            status = HttpStatusCode.Conflict;
            stackTrace = ex.StackTrace;
        }
        else if (exceptionType == typeof(ForbiddenException)) 
        {
            message = ex.Message;
            status = HttpStatusCode.Forbidden;
            stackTrace = ex.StackTrace;
        }
        //else if (exceptionType == typeof(UnauthorizedException)) //Role level
        //{
        //    message = ex.Message;
        //    status = HttpStatusCode.Unauthorized;
        //    stackTrace = ex.StackTrace;
        //}
        else
        {
            message = ex.Message;
            status = HttpStatusCode.InternalServerError;
            stackTrace = ex.StackTrace;
        }

        var exceptionResult = JsonSerializer.Serialize(new { error = message, status, stackTrace});
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(exceptionResult);
    }
}

