using GestionHotel.Core.Enums;

namespace GestionHotel.Apis.Endpoints.Booking;

public class BookingView
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; }
    public string ClientEmail { get; set; }
    public string ClientPhone { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public ReservationStatus Status { get; set; }
    public string StatusDisplay => Status switch
    {
        ReservationStatus.Pending => "En attente de paiement",
        ReservationStatus.Confirmed => "Confirmée",
        ReservationStatus.CheckedIn => "En cours",
        ReservationStatus.CheckedOut => "Terminée",
        ReservationStatus.Cancelled => "Annulée",
        ReservationStatus.NoShow => "Non présenté",
        _ => Status.ToString()
    };
    
    public int NumberOfGuests { get; set; }
    public int NumberOfNights { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceDue { get; set; }
    public bool IsPaid => BalanceDue <= 0;
    
    public List<RoomView> Rooms { get; set; } = new();
    public List<PaymentView> Payments { get; set; } = new();
    
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    
    // Règles métier
    public bool CanBeCancelled => Status == ReservationStatus.Pending || Status == ReservationStatus.Confirmed;
    public bool CanBeCancelledWithRefund { get; set; }
    public bool CanCheckIn => Status == ReservationStatus.Confirmed && DateDebut.Date <= DateTime.Now.Date;
    public bool CanCheckOut => Status == ReservationStatus.CheckedIn;
    public bool RequiresPayment => Status == ReservationStatus.Pending && BalanceDue > 0;
}

public class RoomView
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string Type { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomStatus Status { get; set; }
    public string StatusDisplay => Status switch
    {
        RoomStatus.Neuf => "Neuf",
        RoomStatus.Refaite => "Refaite",
        RoomStatus.ARefaire => "À refaire",
        RoomStatus.RienASignaler => "Rien à signaler",
        RoomStatus.GrosDegats => "Gros dégâts",
        _ => Status.ToString()
    };
}

public class PaymentView
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public string MethodDisplay => Method switch
    {
        PaymentMethod.CreditCard => "Carte de crédit",
        PaymentMethod.DebitCard => "Carte de débit",
        PaymentMethod.Cash => "Espèces",
        PaymentMethod.BankTransfer => "Virement bancaire",
        _ => Method.ToString()
    };
    
    public PaymentStatus Status { get; set; }
    public string StatusDisplay => Status switch
    {
        PaymentStatus.Pending => "En attente",
        PaymentStatus.Processing => "En traitement",
        PaymentStatus.Completed => "Complété",
        PaymentStatus.Failed => "Échoué",
        PaymentStatus.Cancelled => "Annulé",
        PaymentStatus.Refunded => "Remboursé",
        PaymentStatus.PartiallyRefunded => "Partiellement remboursé",
        _ => Status.ToString()
    };
    
    public string? CardNumber { get; set; } // Masqué
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? FailureReason { get; set; }
}

public class AvailableRoomView
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string Type { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal TotalPrice { get; set; } // Prix pour toute la durée
    public RoomStatus Status { get; set; }
    public bool IsAvailable { get; set; }
    public string Description { get; set; }
    public List<string> Amenities { get; set; } = new();
}

public class BookingListView
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public ReservationStatus Status { get; set; }
    public string StatusDisplay { get; set; }
    public int NumberOfRooms { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }
}