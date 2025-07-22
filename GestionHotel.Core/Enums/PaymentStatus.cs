namespace GestionHotel.Core.Enums
{
    public enum PaymentStatus
    {
        EnAttente,              // En attente de paiement
        Reussie,               // Paiement réussi
        Echouee,               // Paiement échoué
        Remboursee,            // Complètement remboursée
        PartielleRemboursee,   // Partiellement remboursée
        Contestee              // Paiement contesté
    }
}