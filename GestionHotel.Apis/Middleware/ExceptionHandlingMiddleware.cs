using GestionHotel.Core.Exceptions;
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

            switch (exception)
            {
                case BusinessException businessEx:
                    response.Message = businessEx.Message;
                    response.ErrorCode = businessEx.ErrorCode;
                    response.Details = businessEx.Details;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(businessEx, "Erreur métier: {Message}", businessEx.Message);
                    break;

                case ReservationException reservationEx:
                    response.Message = reservationEx.Message;
                    response.ErrorCode = reservationEx.ErrorCode;
                    response.Details = reservationEx.Details;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(reservationEx, "Erreur de réservation: {Message}", reservationEx.Message);
                    break;

                case PaymentException paymentEx:
                    response.Message = paymentEx.Message;
                    response.ErrorCode = paymentEx.ErrorCode;
                    response.Details = paymentEx.Details;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(paymentEx, "Erreur de paiement: {Message}", paymentEx.Message);
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    response.Message = "Accès non autorisé";
                    response.ErrorCode = "UNAUTHORIZED";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogWarning(unauthorizedEx, "Tentative d'accès non autorisé");
                    break;

                case ArgumentException argEx:
                    response.Message = argEx.Message;
                    response.ErrorCode = "INVALID_ARGUMENT";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(argEx, "Argument invalide: {Message}", argEx.Message);
                    break;

                case ArgumentNullException nullEx:
                    response.Message = "Paramètre requis manquant";
                    response.ErrorCode = "MISSING_PARAMETER";
                    response.Details = new { Parameter = nullEx.ParamName };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(nullEx, "Paramètre manquant: {ParamName}", nullEx.ParamName);
                    break;

                case InvalidOperationException invalidOpEx:
                    response.Message = "Opération invalide";
                    response.ErrorCode = "INVALID_OPERATION";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(invalidOpEx, "Opération invalide: {Message}", invalidOpEx.Message);
                    break;

                case TimeoutException timeoutEx:
                    response.Message = "Délai d'attente dépassé";
                    response.ErrorCode = "TIMEOUT";
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    _logger.LogError(timeoutEx, "Timeout: {Message}", timeoutEx.Message);
                    break;

                default:
                    response.Message = "Une erreur interne s'est produite";
                    response.ErrorCode = "INTERNAL_ERROR";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "Erreur non gérée: {Message}", exception.Message);
                    
                    // En développement, inclure plus de détails
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        response.Details = new
                        {
                            ExceptionType = exception.GetType().Name,
                            StackTrace = exception.StackTrace,
                            InnerException = exception.InnerException?.Message
                        };
                    }
                    break;
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
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public object Details { get; set; }
        public string TraceId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Path { get; set; }
    }
}
