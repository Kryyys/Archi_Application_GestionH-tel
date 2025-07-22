using System.Diagnostics;
using System.Text;

namespace GestionHotel.Apis.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();
            
            // Ajouter l'ID de requête au contexte
            context.Items["RequestId"] = requestId;

            try
            {
                // Log de la requête entrante
                await LogRequestAsync(context, requestId);

                // Capturer la réponse
                var originalBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context);

                // Log de la réponse
                await LogResponseAsync(context, requestId, stopwatch.ElapsedMilliseconds);

                // Restaurer le stream original et copier la réponse
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement de la requête {RequestId}", requestId);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task LogRequestAsync(HttpContext context, string requestId)
        {
            var request = context.Request;
            
            var logData = new
            {
                RequestId = requestId,
                Method = request.Method,
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                UserAgent = request.Headers["User-Agent"].ToString(),
                RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow
            };

            _logger.LogInformation("Requête entrante: {RequestLog}", 
                System.Text.Json.JsonSerializer.Serialize(logData));

            // Log du corps de la requête pour POST/PUT (si pas trop volumineux)
            if ((request.Method == "POST" || request.Method == "PUT") && 
                request.ContentLength.HasValue && 
                request.ContentLength.Value < 10000) // Limite à 10KB
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;

                _logger.LogDebug("Corps de la requête {RequestId}: {RequestBody}", requestId, bodyAsText);
            }
        }

        private async Task LogResponseAsync(HttpContext context, string requestId, long elapsedMs)
        {
            var response = context.Response;
            
            var logData = new
            {
                RequestId = requestId,
                StatusCode = response.StatusCode,
                ContentLength = response.ContentLength,
                ContentType = response.ContentType,
                ElapsedMilliseconds = elapsedMs,
                Timestamp = DateTime.UtcNow
            };

            var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            _logger.Log(logLevel, "Réponse sortante: {ResponseLog}", 
                System.Text.Json.JsonSerializer.Serialize(logData));

            // Log du corps de la réponse en cas d'erreur (si pas trop volumineux)
            if (response.StatusCode >= 400 && 
                response.Body.CanSeek && 
                response.Body.Length < 10000)
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogDebug("Corps de la réponse d'erreur {RequestId}: {ResponseBody}", requestId, responseBody);
            }
        }
    }
}