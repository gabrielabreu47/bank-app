using System.Net;
using System.Text.Json;
using ClientDirectory.Domain.Common;

namespace Api
{
    /// <summary>
    /// Middleware for global exception handling and error response formatting.
    /// </summary>
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ClientNotFoundException ex)
            {
                logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (AccountNotFoundException ex)
            {
                logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (InsufficientFundsException ex)
            {
                logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (DailyLimitExceededException ex)
            {
                logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, "An unexpected error occurred.", HttpStatusCode.InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var response = new { error = message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

