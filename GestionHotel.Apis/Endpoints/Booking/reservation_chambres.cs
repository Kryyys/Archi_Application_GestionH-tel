namespace GestionHotel.Models;

public class ReservationChambre
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int ChambreId { get; set; }
}
