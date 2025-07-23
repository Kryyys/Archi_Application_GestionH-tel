using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace GestionHotel.Apis.Filters
{
    public class RoleAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public RoleAuthorizationAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Vérifier si l'utilisateur est authentifié
            if (context.HttpContext.User?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Error = "Non authentifié",
                    Message = "Vous devez être connecté pour accéder à cette ressource"
                });
                return;
            }

            // Récupérer le rôle de l'utilisateur
            var userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new ForbidResult("Aucun rôle défini pour cet utilisateur");
                return;
            }

            // Vérifier si le rôle est autorisé
            if (!_allowedRoles.Contains(userRole))
            {
                context.Result = new ForbidResult($"Rôle '{userRole}' non autorisé");
                return;
            }

            // Ajouter des informations utilisateur au contexte
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            context.HttpContext.Items["UserId"] = userId;
            context.HttpContext.Items["UserName"] = userName;
            context.HttpContext.Items["UserRole"] = userRole;
        }
    }
}