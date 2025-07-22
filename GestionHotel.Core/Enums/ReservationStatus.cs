namespace GestionHotel.Core.Enums
{
    public enum ReservationStatus
    {
        EnAttente,      // En attente de confirmation
        Confirmee,      // Confirmée et payée
        CheckedIn,      // Client arrivé
        CheckedOut,     // Client parti
        Annulee,        // Annulée
        NoShow          // Client ne s'est pas présenté
    }
}