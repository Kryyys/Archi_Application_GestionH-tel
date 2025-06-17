using GestionHotel.Models;

namespace GestionHotel.Apis.Endpoints.Booking;

public class BookingView
{
    public List<Room> AvailableRooms { get; set; } = new();
    public SearchPeriod SearchPeriod { get; set; } = new();
    public int TotalAvailableRooms => AvailableRooms.Count;
}

public class SearchPeriod
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfNights => (EndDate - StartDate).Days;
}