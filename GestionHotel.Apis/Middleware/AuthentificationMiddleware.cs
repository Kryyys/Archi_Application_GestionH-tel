using GestionHotel.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GestionHotel.Apis.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            try
            {
                var token = ExtractTokenFromHeader(context.Request);

                if (!string.IsNullOrEmpty(token))
                {
                    var isValid = await authService.ValidateTokenAsync(token);
                    
                    if (isValid)
                    {
                        await AttachUserToContext(context, authService, token);
                    }
                    else
                    {
                        _logger.LogWarning("Token invalide détecté pour {Path}", context.Request.Path);
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans AuthenticationMiddleware");
                await _next(context);
            }
        }

        private string ExtractTokenFromHeader(HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length).Trim();
            }

            return null;
        }

        private async Task AttachUserToContext(HttpContext context, IAuthService authService, string token)
        {
            try
            {
                var user = await authService.GetCurrentUserAsync(token);
                
                if (user != null)
                {
                    // Ajouter les informations utilisateur au contexte
                    context.Items["User"] = user;
                    context.Items["UserId"] = user.Id;
                    context.Items["UserRole"] = user.Role;
                    context.Items["UserEmail"] = user.Email;

                    // Créer les claims pour l'authentification ASP.NET Core
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.NomUtilisateur),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var identity = new ClaimsIdentity(claims, "jwt");
                    context.User = new ClaimsPrincipal(identity);

                    _logger.LogDebug("Utilisateur {UserId} authentifié avec succès", user.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'attachement de l'utilisateur au contexte");
            }
        }
    }
}
