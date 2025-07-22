using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionHotel.Apis.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var result = new
                {
                    Error = "Données invalides",
                    Message = "Veuillez corriger les erreurs suivantes",
                    ValidationErrors = errors,
                    Timestamp = DateTime.UtcNow
                };

                context.Result = new BadRequestObjectResult(result);
                return;
            }

            base.OnActionExecuting(context);
        }
    }

    // Attribut spécialisé pour les réservations
    public class ValidateReservationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Validation personnalisée pour les réservations
            var reservationData = context.ActionArguments.Values.FirstOrDefault();
            
            if (reservationData != null)
            {
                // Exemple : vérifier que la date de début n'est pas dans le passé
                var dateDebutProperty = reservationData.GetType().GetProperty("DateDebut");
                if (dateDebutProperty != null)
                {
                    var dateDebut = (DateTime?)dateDebutProperty.GetValue(reservationData);
                    if (dateDebut.HasValue && dateDebut.Value.Date < DateTime.Today)
                    {
                        context.Result = new BadRequestObjectResult(new
                        {
                            Error = "Date invalide",
                            Message = "La date de début ne peut pas être dans le passé"
                        });
                        return;
                    }
                }

                // Validation de la cohérence des dates
                var dateFinProperty = reservationData.GetType().GetProperty("DateFin");
                if (dateFinProperty != null && dateDebutProperty != null)
                {
                    var dateDebut = (DateTime?)dateDebutProperty.GetValue(reservationData);
                    var dateFin = (DateTime?)dateFinProperty.GetValue(reservationData);
                    
                    if (dateDebut.HasValue && dateFin.HasValue && dateFin.Value <= dateDebut.Value)
                    {
                        context.Result = new BadRequestObjectResult(new
                        {
                            Error = "Dates invalides",
                            Message = "La date de fin doit être postérieure à la date de début"
                        });
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
