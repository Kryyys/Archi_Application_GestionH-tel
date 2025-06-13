namespace GestionHotel.Models;

public class Room
{
    public int Id { get; set; }
    public string Type { get; set; } // Simple, Double, Suite
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomStatus Status { get; set; }
}

public enum RoomStatus
{
    Neuf,
    Refaite,
    ARefaire,
    RienASignaler,
    GrosDegats
}