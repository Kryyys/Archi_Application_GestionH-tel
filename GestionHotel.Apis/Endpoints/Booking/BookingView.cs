using System;
using System.Collections.Generic;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class BookingView
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal MontantTotal { get; set; }
        public bool PaiementEffectue { get; set; }
        public List<RoomBookingView> Rooms { get; set; } = new();
        public DateTime DateCreation { get; set; }
        public string? Commentaires { get; set; }
    }

    public class RoomBookingView
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Tarif { get; set; }
        public int Capacite { get; set; }
        public string Status { get; set; } = "Disponible";
        public bool IsAvailable { get; set; } = true;
        public string? Notes { get; set; }
    }

    public class PaymentBookingView
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public decimal Montant { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public DateTime DatePaiement { get; set; }
        public string? DetailsCarte { get; set; }
    }

    public class AvailableRoomsBookingView
    {
        public List<RoomBookingView> Rooms { get; set; } = new();
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NombreNuits { get; set; }
        public decimal PrixMinimum { get; set; }
        public decimal PrixMaximum { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class CancellationBookingView
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool RemboursementApplique { get; set; }
        public decimal MontantRembourse { get; set; }
        public decimal FraisAppliques { get; set; }
        public string? RaisonRefusRemboursement { get; set; }
        public DateTime DateLimiteRemboursement { get; set; }
    }

    // Classes de constantes pour remplacer les enums
    public static class BookingConstants
    {
        public static class RoomStatus
        {
            public const string Disponible = "Disponible";
            public const string Occupee = "Occupee";
            public const string EnNettoyage = "EnNettoyage";
            public const string HorsService = "HorsService";
            public const string Maintenance = "Maintenance";
            public const string Neuf = "Neuf";
            public const string Refaite = "Refaite";
            public const string ARefaire = "ARefaire";
            public const string RienASignaler = "RienASignaler";
            public const string GrosDegats = "GrosDegats";
        }

        public static class PaymentMethod
        {
            public const string CarteBancaire = "CarteBancaire";
            public const string Especes = "Especes";
            public const string Virement = "Virement";
            public const string PayPal = "PayPal";
            public const string ApplePay = "ApplePay";
            public const string GooglePay = "GooglePay";
        }

        public static class ReservationStatus
        {
            public const string EnAttente = "EnAttente";
            public const string Confirmee = "Confirmee";
            public const string CheckedIn = "CheckedIn";
            public const string CheckedOut = "CheckedOut";
            public const string Annulee = "Annulee";
            public const string NoShow = "NoShow";
        }

        public static class PaymentStatus
        {
            public const string EnAttente = "EnAttente";
            public const string Reussie = "Reussie";
            public const string Echouee = "Echouee";
            public const string Remboursee = "Remboursee";
            public const string PartielleRemboursee = "PartielleRemboursee";
            public const string Contestee = "Contestee";
        }
    }

    // Modèles pour les réponses d'API
    public class BookingResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BookingView? Data { get; set; }
        public string? ErrorCode { get; set; }
    }

    public class AvailableRoomsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AvailableRoomsBookingView? Data { get; set; }
        public string? ErrorCode { get; set; }
    }

    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PaymentBookingView? Data { get; set; }
        public string? ErrorCode { get; set; }
    }

    public class CancellationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public CancellationBookingView? Data { get; set; }
        public string? ErrorCode { get; set; }
    }
}