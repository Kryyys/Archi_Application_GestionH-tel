namespace GestionHotel.Core.Enums
{
    public enum CleaningStatus
    {
        EnAttente,    // En attente d'être prise en charge
        EnCours,      // En cours de nettoyage
        Terminee,     // Nettoyage terminé
        ARefaire,     // À refaire (problème détecté)
        Suspendue     // Suspendue (problème technique/matériel)
    }
}