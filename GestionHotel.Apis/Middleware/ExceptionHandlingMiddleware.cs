// ===============================================
// GestionHotel.Apis/Middleware/ExceptionHandlingMiddleware.cs (Version finale)
// ===============================================

using System.Net;
using System.Text.Json;

namespace GestionHotel.Apis.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path
            };

            // Gestion simple des exceptions - UN SEUL CAS PAR TYPE
            if (exception is UnauthorizedAccessException)
            {
                response.Message = "Accès non autorisé";
                response.ErrorCode = "UNAUTHORIZED";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                _logger.LogWarning(exception, "Accès non autorisé");
            }
            else if (exception is ArgumentException argEx)
            {
                response.Message = argEx.Message;
                response.ErrorCode = "INVALID_ARGUMENT";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogWarning(argEx, "Argument invalide");
            }
            else if (exception is ArgumentNullException nullEx)
            {
                response.Message = "Paramètre requis manquant";
                response.ErrorCode = "MISSING_PARAMETER";
                response.Details = new { Parameter = nullEx.ParamName };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogWarning(nullEx, "Paramètre manquant");
            }
            else if (exception is TimeoutException)
            {
                response.Message = "Délai d'attente dépassé";
                response.ErrorCode = "TIMEOUT";
                context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                _logger.LogError(exception, "Timeout");
            }
            else
            {
                // Cas par défaut pour toutes les autres exceptions
                response.Message = "Une erreur interne s'est produite";
                response.ErrorCode = "INTERNAL_ERROR";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, "Erreur non gérée");
                
                // Détails supplémentaires en développement
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    response.Details = new
                    {
                        ExceptionType = exception.GetType().Name,
                        Message = exception.Message,
                        StackTrace = exception.StackTrace
                    };
                }
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public object? Details { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}