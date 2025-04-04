using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace Api.ExceptionHandlers
{
    public class DbExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DbException e)
                return false;

            httpContext.Response.StatusCode = e.ErrorCode;
            httpContext.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = (int)e.ErrorCode,
                Title = "Database Error",   //есть что-то лучше записать сюда?
                Detail = e.Message,
                Instance = httpContext.Request.Path,
                Type = e.GetType().Name,
            };

            var problemDetailsContext = new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = e
            };
            await problemDetailsService.WriteAsync(problemDetailsContext);
            return true;
        }
    }
}
