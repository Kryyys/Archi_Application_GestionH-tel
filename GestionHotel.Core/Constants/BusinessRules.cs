namespace GestionHotel.Core.Constants
{
    public static class BusinessRules
    {
        // Règles d'annulation
        public const int HeuresAnnulationGratuite = 48;
        public const decimal FraisAnnulationTardive = 50.00m;
        
        // Règles de notification
        public const int HeuresNotificationPreSejour = 24;
        public const int HeuresNotificationPostSejour = 6;
        
        // Règles de nettoyage
        public const int HeuresMaxNettoyage = 4;
        public const int MinutesNettoyageStandard = 30;
        public const int MinutesNettoyageProfond = 90;
        
        // Règles de paiement
        public const int DelaiPaiementJours = 7;
        public const decimal TauxRemboursementStandard = 1.0m; // 100%
        public const decimal TauxRemboursementTardif = 0.0m;   // 0%
        
        // États des chambres
        public static class EtatsChambre
        {
            public const string Neuf = "Neuf";
            public const string Refaite = "Refaite";
            public const string ARefaire = "A refaire";
            public const string RienASignaler = "Rien a signaler";
            public const string GrosDegats = "Gros dégats";
            public const string Disponible = "Disponible";
            public const string Occupee = "Occupee";
            public const string HorsService = "Hors service";
            public const string EnNettoyage = "En nettoyage";
        }
        
        // Types de paiement
        public static class TypesPaiement
        {
            public const string CarteBancaire = "Carte bancaire";
            public const string Especes = "Especes";
            public const string Virement = "Virement";
            public const string PayPal = "PayPal";
        }
        
        // Types de nettoyage
        public static class TypesNettoyage
        {
            public const string Standard = "Standard";
            public const string Profond = "Profond";
            public const string Maintenance = "Maintenance";
            public const string Urgent = "Urgent";
        }
        
        // Messages d'erreur
        public static class ErrorMessages
        {
            public const string ReservationNotFound = "Réservation introuvable";
            public const string RoomNotAvailable = "Chambre non disponible pour ces dates";
            public const string PaymentFailed = "Échec du paiement";
            public const string CancellationTooLate = "Annulation trop tardive pour un remboursement complet";
            public const string InvalidDateRange = "Plage de dates invalide";
            public const string UserNotAuthorized = "Utilisateur non autorisé";
            public const string InvalidCredentials = "Identifiants invalides";
        }
        
        // Codes d'erreur
        public static class ErrorCodes
        {
            public const string RESERVATION_001 = "RESERVATION_001"; // Réservation introuvable
            public const string RESERVATION_002 = "RESERVATION_002"; // Chambre non disponible
            public const string RESERVATION_003 = "RESERVATION_003"; // Dates invalides
            public const string PAYMENT_001 = "PAYMENT_001";         // Paiement échoué
            public const string PAYMENT_002 = "PAYMENT_002";         // Remboursement impossible
            public const string AUTH_001 = "AUTH_001";               // Identifiants invalides
            public const string AUTH_002 = "AUTH_002";               // Non autorisé
            public const string VALIDATION_001 = "VALIDATION_001";   // Données invalides
        }
        
        // Templates de messages
        public static class MessageTemplates
        {
            public const string ConfirmationReservation = 
                "Bonjour {clientName},\n\nVotre réservation du {dateDebut} au {dateFin} pour la chambre {roomNumber} est confirmée.\n\nMontant total: {montant}€\n\nCordialement,\nL'équipe de l'hôtel";
                
            public const string RappelArrivee = 
                "Bonjour {clientName},\n\nNous vous rappelons que votre séjour commence demain ({dateDebut}) dans notre chambre {roomNumber}.\n\nNous vous attendons avec plaisir!\n\nCordialement,\nL'équipe de l'hôtel";
                
            public const string DemandeAvis = 
                "Bonjour {clientName},\n\nNous espérons que votre séjour s'est bien passé. Nous serions ravis de connaître votre avis sur votre expérience.\n\nMerci de prendre quelques minutes pour nous laisser votre feedback.\n\nCordialement,\nL'équipe de l'hôtel";
        }
    }
}