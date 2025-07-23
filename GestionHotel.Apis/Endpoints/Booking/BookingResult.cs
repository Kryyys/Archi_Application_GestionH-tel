using System;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class BookingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? ReservationId { get; set; }
        public object? Reservation { get; set; }
        public object? AvailableRooms { get; set; }
        public object? CancellationResult { get; set; }
        public object? PaymentResult { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}