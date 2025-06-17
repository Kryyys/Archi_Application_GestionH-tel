namespace GestionHotel.Apis.Endpoints.Booking;

public class GetAvailableRoomsInput
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? Capacity { get; set; } // Optionnel : pour filtrer par capacité
    public string? RoomType { get; set; } // Optionnel : pour filtrer par type
}