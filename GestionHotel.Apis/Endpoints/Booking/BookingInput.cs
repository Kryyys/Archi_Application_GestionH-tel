namespace GestionHotel.Apis.Endpoints.Booking;

public class BookingInput
{
    public int ClientId { get; set; }
    public List<int> RoomIds { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}